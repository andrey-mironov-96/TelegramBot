using app.common.DTO;
namespace app.business.Services
{
    public interface IProfTestService
    {
        public Task<MessageDTO> Answer(string message, long userId);
    }
}