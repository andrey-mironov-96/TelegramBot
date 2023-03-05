using app.common.DTO;
using Telegram.Bot.Types.ReplyMarkups;

namespace app.domain.Abstract
{
    public interface IBotService
    {
        public Task<MessageDTO> GetPrevius(long userId);

        public Task<MessageDTO> DoWork(long userId, string message);

        public Task InitFacultyAsync(long userId);

        public Task InitProfAsync(long userId);
        public ReplyKeyboardMarkup GenerateKeyboardMarkup(params string[] titles);
    }
}