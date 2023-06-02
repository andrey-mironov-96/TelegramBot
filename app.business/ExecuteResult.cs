using Telegram.Bot.Types.ReplyMarkups;

namespace app.business
{
    public class ExecuteResult
    {
        public string Message { get; set; }

        public IReplyMarkup replyMarkup { get; set; }
    }
}