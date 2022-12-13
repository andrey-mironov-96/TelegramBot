using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL
{
    public class ExecuteResult
    {
        public string Message { get; set; }

        public IReplyMarkup? replyMarkup { get; set; }
    }
}