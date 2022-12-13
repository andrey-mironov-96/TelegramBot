using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace AppBot.Abstract
{
    public abstract class PollingServiceBase<TReceiverService> : BackgroundService
        where TReceiverService : IReceiverService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        internal PollingServiceBase(
            IServiceProvider sp,
            ILogger<PollingServiceBase<TReceiverService>> logger
        )
        {
            _serviceProvider = sp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting polling service");
            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                     using var scope = _serviceProvider.CreateScope();
                     var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();
                     await receiver.ReceiveAsync(stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogError("Polling failed with exception: {Exception}", ex);
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
                }
        }
    }
}