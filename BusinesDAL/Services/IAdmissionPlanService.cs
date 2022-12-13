using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public interface IAdmissionPlanService
    {
        ReplyKeyboardMarkup DoWork(string userText);
    }
}