using FCSP.Common.Enums;
using FCSP.DTOs;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using FCSP.Services.PaymentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCSP.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public AdminController(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        [HttpPost("process-payments")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseResponseModel<string>>> ProcessDesignerManufacturerPayments()
        {
            try
            {
                // Create a manual payment processor that will run once
                var processor = new ManualPaymentProcessor(_serviceProvider, _configuration);
                var result = await processor.ProcessPaymentsAsync();

                return Ok(new BaseResponseModel<string>
                {
                    Code = 200,
                    Message = "Payment processing completed",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponseModel<string>
                {
                    Code = 500,
                    Message = "Error processing payments",
                    Data = ex.Message
                });
            }
        }
    }

    // Helper class for manual payment processing
    public class ManualPaymentProcessor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly int _daysAfterOrderToPayOut;

        public ManualPaymentProcessor(
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _daysAfterOrderToPayOut = configuration.GetValue<int>("PaymentProcessing:DaysAfterOrderToPayOut", 30);
        }

        public async Task<string> ProcessPaymentsAsync()
        {
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

            // Use a much shorter time frame for manual testing (1 hour instead of days)
            var cutoffDate = DateTime.UtcNow.AddHours(-1);
            
            var eligibleOrders = await orderRepository.GetAll()
                .Where(o => o.Status == OrderStatus.Completed && o.UpdatedAt <= cutoffDate)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.CustomShoeDesign)
                        .ThenInclude(csd => csd.User) // Designer
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Manufacturer)
                        .ThenInclude(m => m.User) // Manufacturer
                .Include(o => o.Payments.Where(p => p.PaymentStatus == PaymentStatus.Received))
                .ToListAsync();

            int processedOrdersCount = 0;
            int processedTransactionsCount = 0;
            
            foreach (var order in eligibleOrders)
            {
                // Only process if there's a successful payment for this order
                var payment = order.Payments.FirstOrDefault(p => p.PaymentStatus == PaymentStatus.Received);
                if (payment == null)
                {
                    continue;
                }

                bool orderProcessed = false;
                
                foreach (var orderDetail in order.OrderDetails)
                {
                    // Check if transaction already exists for this order detail
                    var existingTransactions = await transactionRepository.GetByOrderDetailIdAsync(orderDetail.Id);
                    
                    if (existingTransactions.Any())
                    {
                        continue;
                    }

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
                            
                            // Create transaction for designer
                            var designerTransaction = new Models.Entities.Transaction
                            {
                                ReceiverId = designerId,
                                OrderDetailId = orderDetail.Id,
                                PaymentId = payment.Id,
                                Amount = designerAmount,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            await transactionRepository.AddAsync(designerTransaction);
                            
                            // Update designer's balance
                            designerUser.Balance = (designerUser.Balance ?? 0) + designerAmount;
                            await userRepository.UpdateAsync(designerUser);
                            
                            processedTransactionsCount++;
                            orderProcessed = true;
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
                                
                                // Create transaction for manufacturer
                                var manufacturerTransaction = new Models.Entities.Transaction
                                {
                                    ReceiverId = manufacturer.UserId,
                                    OrderDetailId = orderDetail.Id,
                                    PaymentId = payment.Id,
                                    Amount = manufacturerAmount,
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                };
                                await transactionRepository.AddAsync(manufacturerTransaction);
                                
                                // Update manufacturer user's balance
                                var manufacturerUser = await userRepository.FindAsync(manufacturer.UserId);
                                if (manufacturerUser != null)
                                {
                                    manufacturerUser.Balance = (manufacturerUser.Balance ?? 0) + manufacturerAmount;
                                    await userRepository.UpdateAsync(manufacturerUser);
                                    
                                    processedTransactionsCount++;
                                    orderProcessed = true;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue processing other order details
                        throw new Exception($"Error processing order detail {orderDetail.Id}: {ex.Message}", ex);
                    }
                }

                if (orderProcessed)
                {
                    processedOrdersCount++;
                }
            }

            return $"Processed {processedOrdersCount} orders with {processedTransactionsCount} transactions. Cutoff date: {cutoffDate}";
        }
    }
} 