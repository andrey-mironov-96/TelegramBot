using AppBot.Abstract;
using Microsoft.Extensions.Logging;

namespace AppBot.Services
{
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider sp,ILogger<PollingService> logger)
            : base(sp, logger)
        {
        }
    }
}