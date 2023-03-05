using Telegram.Bot.Types.ReplyMarkups;

namespace app.common.DTO
{
    public class MessageDTO
    {
        #pragma warning disable CS8618
        public string Message { get; set; }

        public IReplyMarkup KeyboardMarkup { get; set; }
    }
}