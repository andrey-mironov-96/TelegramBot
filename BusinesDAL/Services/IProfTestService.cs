using app.common.DTO;
namespace BusinesDAL.Services
{
    public interface IProfTestService
    {
        public Task<MessageDTO> Answer(string message, long userId);
    }
}