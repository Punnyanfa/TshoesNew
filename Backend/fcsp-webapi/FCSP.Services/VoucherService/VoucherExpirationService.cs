using FCSP.Services.VoucherService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCSP.WebAPI
{
    public class VoucherExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VoucherExpirationService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(1); // Configurable interval

        public VoucherExpirationService(IServiceProvider serviceProvider, ILogger<VoucherExpirationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("VoucherExpirationService started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Checking for expired vouchers at {Time}", DateTime.UtcNow);
                    using var scope = _serviceProvider.CreateScope();
                    var voucherService = scope.ServiceProvider.GetRequiredService<IVoucherService>();
                    var updatedCount = await voucherService.UpdateExpiredVouchers();
                    _logger.LogInformation("Updated {Count} expired vouchers.", updatedCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating expired vouchers.");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("VoucherExpirationService stopped.");
        }
    }
}