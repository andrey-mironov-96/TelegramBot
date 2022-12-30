using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Models
{
    public class MessageDTO
    {
        #pragma warning disable CS8618
        public string Message { get; set; }

        public IReplyMarkup? KeyboardMarkup { get; set; }
    }
}