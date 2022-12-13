using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public interface IAdmissionPlanService
    {
        Task<ReplyKeyboardMarkup> AdmissionHandlerAsync(string message, long userId);

    }
}