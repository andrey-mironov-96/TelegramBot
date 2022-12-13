using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;
using WebParse.Models;

namespace BusinesDAL.Services
{
    public class AdmissionPlanService : IAdmissionPlanService
    {
        private readonly ILogger<AdmissionPlanService> _logger;
        private readonly Dictionary<string, List<AdmissionPlan>> _admissionPlans;

        public AdmissionPlanService(ILogger<AdmissionPlanService> logger, Dictionary<string, List<AdmissionPlan>> admissionPlans)
        {
            _logger = logger;
            _admissionPlans = admissionPlans;
        }

        public ReplyKeyboardMarkup DoWork(string userText)
        {
            List<KeyboardButton[]> facultetButtons = new List<KeyboardButton[]>();
            foreach (var facultet in _admissionPlans)
            {
                facultetButtons.Add(new KeyboardButton[] { facultet.Key });
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(
               facultetButtons
            )
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

    }
}