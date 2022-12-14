using BusinesDAL.Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public interface IAdmissionPlanService
    {
        Task<MessageDTO> AdmissionHandlerAsync(string message, long userId);

    }
}