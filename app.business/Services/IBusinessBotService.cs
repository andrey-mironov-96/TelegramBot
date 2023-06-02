

using app.common.DTO;

namespace app.business.Services
{
    public interface IBusinessBotService
    {
        public Task<MessageDTO> PreviusStep(string message, long userId);

        public Task<MessageDTO> UserChoose(string message, long userId);
    }
}