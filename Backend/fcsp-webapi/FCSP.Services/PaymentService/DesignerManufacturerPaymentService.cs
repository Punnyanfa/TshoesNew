using FCSP.Common.Enums;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FCSP.Services.PaymentService
{
    public class DesignerManufacturerPaymentService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DesignerManufacturerPaymentService> _logger;
        private readonly TimeSpan _checkInterval;
        private readonly int _daysAfterOrderToPayOut;

        public DesignerManufacturerPaymentService(
            IServiceProvider serviceProvider,
            ILogger<DesignerManufacturerPaymentService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var intervalMinutes = configuration.GetValue<int>("PaymentProcessing:PaymentProcessingCheckIntervalMinutes", 1); // Default 1 minute
            _checkInterval = TimeSpan.FromMinutes(intervalMinutes);
            _daysAfterOrderToPayOut = configuration.GetValue<int>("PaymentProcessing:DaysAfterOrderToPayOut", 0); // Default 30 days
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("DesignerManufacturerPaymentService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Processing payments for designers and manufacturers at {Time}", DateTime.Now);
                    
                    using var scope = _serviceProvider.CreateScope();
                    
                    // Get all necessary repositories
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                    var orderDetailRepository = scope.ServiceProvider.GetRequiredService<IOrderDetailRepository>();
                    var customShoeDesignRepository = scope.ServiceProvider.GetRequiredService<ICustomShoeDesignRepository>();
                    var manufacturerRepository = scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
                    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                    var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                    var designerRepository = scope.ServiceProvider.GetRequiredService<IDesignerRepository>();

                    // Find completed orders ready for payout (status completed and completed for 30 days)
                    var cutoffDate = DateTime.Now.AddDays(_daysAfterOrderToPayOut);
                    
                    var eligibleOrders = await orderRepository.GetAll()
                        .Where(o => o.Status == OrderStatus.Completed && 
                               o.UpdatedAt <= cutoffDate &&
                               o.Payments.Any(p => p.PaymentStatus == PaymentStatus.Received))
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.CustomShoeDesign)
                                .ThenInclude(csd => csd.User) // Designer
                        .Include(o => o.OrderDetails)
                            .ThenInclude(od => od.Manufacturer)
                                .ThenInclude(m => m.User) // Manufacturer
                        .Include(o => o.Payments.Where(p => p.PaymentStatus == PaymentStatus.Received))
                        .ToListAsync(stoppingToken);

                    _logger.LogInformation("Found {Count} eligible orders for payout processing", eligibleOrders.Count);

                    foreach (var order in eligibleOrders)
                    {
                        // Verify payment status
                        var payment = order.Payments.FirstOrDefault(p => p.PaymentStatus == PaymentStatus.Received);
                        if (payment == null)
                        {
                            _logger.LogWarning("Order {OrderId} has no successful payment, skipping payout", order.Id);
                            continue;
                        }

                        // Additional payment validation
                        if (payment.Amount <= 0)
                        {
                            _logger.LogWarning("Order {OrderId} has invalid payment amount {Amount}, skipping payout", order.Id, payment.Amount);
                            continue;
                        }

                        // Check for existing transactions for all order details
                        var orderDetailIds = order.OrderDetails.Select(od => od.Id).ToList();
                        var existingTransactions = await transactionRepository.GetAll()
                            .Where(t => orderDetailIds.Contains(t.OrderDetailId))
                            .ToListAsync(stoppingToken);

                        if (existingTransactions.Any())
                        {
                            _logger.LogInformation("Order {OrderId} already has transactions for some order details, skipping", order.Id);
                            continue;
                        }

                        foreach (var orderDetail in order.OrderDetails)
                        {
                            try
                            {
                                // Process designer payment
                                var designerId = orderDetail.CustomShoeDesign.UserId; // The user who created the design
                                var designerUser = await userRepository.FindAsync(designerId);
                                
                                if (designerUser != null)
                                {
                                    // Get designer entity to access commission rate
                                    var designer = await designerRepository.GetDesignerByUserIdAsync(designerId);
                                    float designerCommissionRate = designer?.CommissionRate ?? 1.0f; // Default to 100% if not found
                                    
                                    // Calculate payment with commission rate applied
                                    var designerMarkup = orderDetail.DesignerMarkup * orderDetail.Quantity;
                                    var designerAmount = (int)(designerMarkup * (designerCommissionRate / 100.0f));
                                    
                                    _logger.LogInformation("Designer {DesignerId} has commission rate {CommissionRate}%, " +
                                        "receiving {ActualAmount} of {TotalAmount}",
                                        designerId, designerCommissionRate, designerAmount, designerMarkup);
                                    
                                    // Create transaction for designer
                                    var designerTransaction = new Transaction
                                    {
                                        ReceiverId = designerId,
                                        OrderDetailId = orderDetail.Id,
                                        PaymentId = payment.Id,
                                        Amount = designerAmount,
                                        CreatedAt = DateTime.Now,
                                        UpdatedAt = DateTime.Now
                                    };
                                    await transactionRepository.AddAsync(designerTransaction);
                                    
                                    // Update designer's balance
                                    designerUser.Balance = (designerUser.Balance ?? 0) + designerAmount;
                                    await userRepository.UpdateAsync(designerUser);
                                    
                                    _logger.LogInformation("Paid {Amount} to designer {DesignerId} for order detail {OrderDetailId}", 
                                        designerAmount, designerId, orderDetail.Id);
                                }
                                else
                                {
                                    _logger.LogWarning("Designer {DesignerId} not found for order detail {OrderDetailId}", 
                                        designerId, orderDetail.Id);
                                }
                                
                                // Process manufacturer payment if available
                                if (orderDetail.ManufacturerId.HasValue)
                                {
                                    var manufacturerId = orderDetail.ManufacturerId.Value;
                                    var manufacturer = await manufacturerRepository.FindAsync(manufacturerId);
                                    
                                    if (manufacturer != null && manufacturer.UserId > 0)
                                    {
                                        // Apply commission rate for manufacturer
                                        float manufacturerCommissionRate = manufacturer.CommissionRate;
                                        var serviceAmount = orderDetail.ServicePrice * orderDetail.Quantity;
                                        var manufacturerAmount = (int)(serviceAmount * (manufacturerCommissionRate / 100.0f));
                                        
                                        _logger.LogInformation("Manufacturer {ManufacturerId} has commission rate {CommissionRate}%, " +
                                            "receiving {ActualAmount} of {TotalAmount}",
                                            manufacturerId, manufacturerCommissionRate, manufacturerAmount, serviceAmount);
                                        
                                        // Create transaction for manufacturer
                                        var manufacturerTransaction = new Transaction
                                        {
                                            ReceiverId = manufacturer.UserId,
                                            OrderDetailId = orderDetail.Id,
                                            PaymentId = payment.Id,
                                            Amount = manufacturerAmount,
                                            CreatedAt = DateTime.Now,
                                            UpdatedAt = DateTime.Now
                                        };
                                        await transactionRepository.AddAsync(manufacturerTransaction);
                                        
                                        // Update manufacturer user's balance
                                        var manufacturerUser = await userRepository.FindAsync(manufacturer.UserId);
                                        if (manufacturerUser != null)
                                        {
                                            manufacturerUser.Balance = (manufacturerUser.Balance ?? 0) + manufacturerAmount;
                                            await userRepository.UpdateAsync(manufacturerUser);
                                            
                                            _logger.LogInformation("Paid {Amount} to manufacturer {ManufacturerId} for order detail {OrderDetailId}",
                                                manufacturerAmount, manufacturerId, orderDetail.Id);
                                        }
                                        else
                                        {
                                            _logger.LogWarning("Manufacturer user {UserId} not found for manufacturer {ManufacturerId}",
                                                manufacturer.UserId, manufacturerId);
                                        }
                                    }
                                    else
                                    {
                                        _logger.LogWarning("Manufacturer {ManufacturerId} not found or has no user for order detail {OrderDetailId}",
                                            manufacturerId, orderDetail.Id);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                // Log error but continue processing other order details
                                _logger.LogError(ex, "Error processing payment for order detail {OrderDetailId}", orderDetail.Id);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing payments at {Time}", DateTime.Now);
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("DesignerManufacturerPaymentService stopped.");
        }
    }
} 