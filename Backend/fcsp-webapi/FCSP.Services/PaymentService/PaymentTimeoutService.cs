using FCSP.Common.Enums;
using FCSP.Models.Entities;
using FCSP.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Net.payOS;
using FCSP.Common.Utils;

namespace FCSP.Services.PaymentService
{
    public class PaymentTimeoutService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PaymentTimeoutService> _logger;
        private readonly TimeSpan _checkInterval;
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;

        public PaymentTimeoutService(
            IServiceProvider serviceProvider,
            ILogger<PaymentTimeoutService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            var intervalMinutes = configuration.GetValue<int>("PaymentProcessing:PaymentTimeoutCheckIntervalMinutes", 1); // Default 1 minute
            _checkInterval = TimeSpan.FromMinutes(intervalMinutes);
            _clientId = configuration["PayOS:ClientId"] ?? string.Empty;
            _apiKey = configuration["PayOS:ApiKey"] ?? string.Empty;
            _checksumKey = configuration["PayOS:ChecksumKey"] ?? string.Empty;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PaymentTimeoutService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Checking for expired payments at {DateTimeUtils.GetCurrentGmtPlus7()}");
                    
                    using var scope = _serviceProvider.CreateScope();
                    var paymentRepository = scope.ServiceProvider.GetRequiredService<IPaymentRepository>();
                    var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                    // Get all pending payments that are older than 5 minutes
                    var expiredPayments = await paymentRepository.GetAll()
                        .Where(p => p.PaymentStatus == PaymentStatus.Pending && 
                                  p.CreatedAt < DateTimeUtils.GetCurrentGmtPlus7().AddMinutes(-5) && p.PaymentMethod == PaymentMethod.PayOS)
                        .ToListAsync();

                    foreach (var payment in expiredPayments)
                    {
                        try
                        {
                            // Cancel payment in PayOS
                            var payOS = new PayOS(_clientId, _apiKey, _checksumKey);
                            await payOS.cancelPaymentLink(payment.Id);

                            // Update payment status
                            payment.PaymentStatus = PaymentStatus.Cancelled;
                            payment.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
                            await paymentRepository.UpdateAsync(payment);

                            // Update order status
                            var order = await orderRepository.FindAsync(payment.OrderId);
                            if (order != null)
                            {
                                order.Status = OrderStatus.Cancelled;
                                order.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();
                                await orderRepository.UpdateAsync(order);
                            }

                            _logger.LogInformation($"Successfully cancelled expired payment {payment.Id}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error cancelling payment {payment.Id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while checking for expired payments at {DateTimeUtils.GetCurrentGmtPlus7()}");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("PaymentTimeoutService stopped.");
        }
    }
} 