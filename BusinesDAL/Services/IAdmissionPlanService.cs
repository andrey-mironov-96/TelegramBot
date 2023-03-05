
using app.common.DTO;

namespace BusinesDAL.Services
{
    public interface IAdmissionPlanService
    {
        Task<MessageDTO> AdmissionHandlerAsync(string message, long userId);
        Task<MessageDTO> GetFacultiesAsync(long userId, string additionalMessage = "");

        Task<MessageDTO> GoBackAsync(long userId);

    }
}