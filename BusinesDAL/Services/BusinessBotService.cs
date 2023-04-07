using app.common.DTO;
using app.common.Utils.CustomException;
using app.domain.Abstract;
using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public class BusinessBotService : IBusinessBotService
    {
        private readonly IBotService botService;

        public BusinessBotService(IBotService botService)
        {
            this.botService = botService;
        }
        public Task<MessageDTO> PreviusStep(string message, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDTO> UserChoose(string message, long userId)
        {
            //смотрим в стейт для получения текущего шага
            // если стейт пуст, то начинаем с начала
            //если стейт не пуст, то продолжаем с места
            return MappingDefaultPhrase(message, userId);
        }

        private Task<MessageDTO> MappingDefaultPhrase(string message, long userId)
        {
            Task<MessageDTO> action = message.ToLower() switch
            {
                "начать" or
                "/start" => ActionWelcome(),
                "факультеты" => ActionFacultiesAndSpecialities(userId, message),
                "тест на проф. ориентацию" or
                "тест" => ActionAboutTest(userId, message),
                "начать тест на проф. ориентацию" or
                "начать тест" => ActionStartTest(userId, message),
                "/clear" or
                "сброс" => ActionClear(userId),
                "назад" => ActionPreviusStep(userId, message),
                _ => Action(userId, message)
            };
            return action;
        }

        private Task<MessageDTO> ActionWelcome()
        {
            return Task.FromResult(new MessageDTO()
            {
                Message = "Привет! Я чат-бот УлГу.\nЧем тебе могу помочь?",
                KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "Тест на проф. ориентацию")
            });
        }

        private async Task<MessageDTO> ActionFacultiesAndSpecialities(long userId, string message)
        {
            await botService.InitFacultyAsync(userId);
            return await botService.DoWork(userId, message);
        }


        private Task<MessageDTO> ActionAboutTest(long userId, string message)
        {
            return Task.FromResult(new MessageDTO()
            {
                Message = $"Для успешного прохождения профифильного тестирования небходимо ответить на все вопросы." +
                "Не переживайте, прохождение теста займет не более 5 минут. Начнём?",
                KeyboardMarkup = botService.GenerateKeyboardMarkup("Начать тест")
            });
        }
        private async Task<MessageDTO> ActionStartTest(long userId, string message)
        {
            await botService.InitProfAsync(userId);
            var result = await botService.DoWork(userId, message);
            return result;
        }

        private Task<MessageDTO> ActionClear(long userId)
        {
            return Task.FromResult(new MessageDTO()
            {
                Message = "Привет! Я чат-бот УлГу.\nЧем тебе могу помочь?",
                KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "Тест на проф. ориентацию")
            });
        }

        private async Task<MessageDTO> ActionPreviusStep(long userId, string message)
        {
            return await this.botService.GetPrevius(userId);
        }

        private async Task<MessageDTO> Action(long userId, string message)
        {
            try
            {
                MessageDTO workResult = await botService.DoWork(userId, message);
                return workResult;
            }
            catch (EmptyStateException)
            {
                return new MessageDTO()
                {
                    Message = "Я Вас не понял, прошу воспользоваться командами:\n" +
                          "факультеты - поиск информации о специальности факультета\n" +
                          "тест - прохождение теста на проф. ориентацию",

                    KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "Тест на проф. ориентацию")
                };
            }
        }
    }
}