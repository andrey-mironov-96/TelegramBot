using Telegram.Bot.Types.ReplyMarkups;
using WebParse.Models;
using Microsoft.Extensions.Logging;
using BusinesDAL.Models;
using BusinesDAL.Models.Utils;

namespace BusinesDAL.Services
{
    public class CommandService : ICommandService
    {
        private readonly ILogger<CommandService> _logger;
        private readonly Dictionary<string, List<AdmissionPlan>> _admissionPlans;
        public CommandService(ILogger<CommandService> logger, Dictionary<string, List<AdmissionPlan>> admissionPlans)
        {
            _logger = logger;
            _admissionPlans = admissionPlans;
        }
        public Task<ExecuteResult> ExecuteCommandAsync(string command)
        {
            ExecuteResult executeResult = new ExecuteResult();
            Dictionary<int, Fuzzy> words = this.InitDictionary();
            foreach (KeyValuePair<int, Fuzzy> word in words)
            {
                words[word.Key].Weight = FuzzySearch.LevenshteinDistance(command, word.Value.Label, true);
            }
            KeyValuePair<int, Fuzzy> userChoice = words.Aggregate((l, r) => l.Value.Weight < r.Value.Weight ? l : r);

            // if (userChoice.Key >= 0 || userChoice.Key <= 3)
            // {
            //     //HigherEduc
            //     executeResult.Message = "Выберите интересующий вас факультет:";
            //     List<KeyboardButton[]> KeyboardButtons = new List<KeyboardButton[]>();
            //     foreach (var item in _admissionPlans)
            //     {
            //         KeyboardButtons.Add(new KeyboardButton[] { item.Key });
            //     }
            //     executeResult.replyMarkup = new ReplyKeyboardMarkup(KeyboardButtons) { ResizeKeyboard = false, OneTimeKeyboard= true };
            // }
            // else if (userChoice.Key >= 4 || userChoice.Key <= 9)
            // {
            //     //middle education
            // }
            // else if (userChoice.Key >= 10 || userChoice.Key <= 14)
            // {
                executeResult.Message = "Выберите тип образования";
                executeResult.replyMarkup = new ReplyKeyboardMarkup(new[]
                {
                    
                            new KeyboardButton[]{"1.1", "11"},
                         new KeyboardButton[]{"1.2", "12"},
                    
                });
            // }
            // else
            // {

            // }
            return Task.FromResult(executeResult);
        }

        private Dictionary<int, Fuzzy> InitDictionary()
        {
            Dictionary<int, Fuzzy> dictionary = new Dictionary<int, Fuzzy>()
            {
                //0-3 во, 4-9 спо
                {0, new Fuzzy(){ Label = "высшее образование",  Type = FuzzyType.Button}},
                {1, new Fuzzy(){ Label = "вышка",  Type = FuzzyType.Button}},
                {2, new Fuzzy(){ Label = "высшее",  Type = FuzzyType.Button}},
                {3, new Fuzzy(){ Label = "во",  Type = FuzzyType.Button}},
                {4, new Fuzzy(){ Label = "спо",  Type = FuzzyType.Button}},
                {5, new Fuzzy(){ Label = "средне-профессиональное образование",  Type = FuzzyType.Button}},
                {6, new Fuzzy(){ Label = "средне образование",  Type = FuzzyType.Button}},
                {7, new Fuzzy(){ Label = "средне-специальное образование",  Type = FuzzyType.Button}},
                {8, new Fuzzy(){ Label = "специальное образование",  Type = FuzzyType.Button}},
                {9, new Fuzzy(){ Label = "среднее",  Type = FuzzyType.Button}},
                {10, new Fuzzy(){ Label = "рестарт",  Type = FuzzyType.Button}},
                {11, new Fuzzy(){ Label = "выход",  Type = FuzzyType.Button}},
                {12, new Fuzzy(){ Label = "сброс",  Type = FuzzyType.Button}},
                {13, new Fuzzy(){ Label = "restart",  Type = FuzzyType.Button}},
                {14, new Fuzzy(){ Label = "clear",  Type = FuzzyType.Button}}
            };
            int index = 10;
            foreach (KeyValuePair<string, List<AdmissionPlan>> facultet in _admissionPlans)
            {
                //dictionary.Add(index, new Fuzzy() { Label = facultet.Key, Type = FuzzyType.Button }); //adding facultet name
                index++;
                foreach(AdmissionPlan admissionPlan in _admissionPlans[facultet.Key])
                {
                    
                }
            }
            return dictionary;
        }
    }
}