using FCSP.Services.VoucherService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FCSP.WebAPI
{
    public class VoucherExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public VoucherExpirationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var voucherService = scope.ServiceProvider.GetRequiredService<IVoucherService>();
                await voucherService.UpdateExpiredVouchers();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Check every hour
            }
        }
    }
}