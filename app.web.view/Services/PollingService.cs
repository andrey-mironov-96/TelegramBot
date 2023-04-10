using app.web.view.ClientBot;

namespace app.web.view.Services
{
    public class PollingService : PollingServiceBase<ReceiverService>
    {
        public PollingService(IServiceProvider sp, ILogger<PollingService> logger)
            : base(sp, logger)
        {
        }
    }
}