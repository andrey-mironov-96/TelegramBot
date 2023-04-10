using app.web.view.ClientBot;
using Telegram.Bot;

namespace app.web.view.Services
{
    public class ReceiverService : ReceiverServiceBase<UpdateHandler>
    {
        public ReceiverService(ITelegramBotClient botClient, UpdateHandler updateHandler, ILogger<ReceiverServiceBase<UpdateHandler>> logger) : base(botClient, updateHandler, logger)
        {
        }
    }
}