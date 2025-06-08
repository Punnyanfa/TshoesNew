using FCSP.Services.VoucherService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FCSP.Common.Utils;

public class VoucherExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<VoucherExpirationService> _logger;
    private readonly TimeSpan _checkInterval;

    public VoucherExpirationService(
        IServiceProvider serviceProvider,
        ILogger<VoucherExpirationService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        var intervalMinutes = configuration.GetValue<int>("PaymentProcessing:VoucherExpirationCheckIntervalMinutes", 1);
        _checkInterval = TimeSpan.FromMinutes(intervalMinutes);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("VoucherExpirationService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Checking for expired vouchers at {Time}", DateTimeUtils.GetCurrentGmtPlus7());
                using var scope = _serviceProvider.CreateScope();
                var voucherService = scope.ServiceProvider.GetRequiredService<IVoucherService>();
                var response = await voucherService.UpdateExpiredVouchers();
                if (response.Code == 200)
                {
                    _logger.LogInformation("Updated {Count} expired vouchers.", response.Data);
                }
                else
                {
                    _logger.LogWarning("Failed to update expired vouchers: {Message}", response.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating expired vouchers at {Time}.", DateTimeUtils.GetCurrentGmtPlus7());
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

       
    }
}