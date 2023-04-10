using app.common.DTO;
using app.common.Utils.CustomException;
using app.domain.Abstract;

namespace BusinesDAL.Services
{
    public class BusinessBotService : IBusinessBotService
    {
        private readonly IBotService botService;

        public BusinessBotService(
            IBotService botService
            )
        {
            this.botService = botService;


        }
        public Task<MessageDTO> PreviusStep(string message, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDTO> UserChoose(string message, long userId)
        {
            return MappingDefaultPhrase(message, userId);
        }

        private Task<MessageDTO> MappingDefaultPhrase(string message, long userId)
        {
            Task<MessageDTO> action = message.ToLower() switch
            {
                "/start" => ActionWelcome(),
                "факультеты" => ActionFacultiesAndSpecialities(userId, message),
                "тесты на проф. ориентацию" or
                "тесты" => ActionAboutTest(userId, message),
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
                KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "тесты на проф. ориентацию")
            });
        }

        private async Task<MessageDTO> ActionFacultiesAndSpecialities(long userId, string message)
        {
            await botService.InitFacultyAsync(userId);
            return await botService.DoWork(userId, message);
        }


        private async Task<MessageDTO> ActionAboutTest(long userId, string message)
        {
            await botService.InitProfAsync(userId);
            var result = await botService.DoWork(userId, message);
            return result;
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
                KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "Тесты на проф. ориентацию")
            });
        }

        private async Task<MessageDTO> ActionPreviusStep(long userId, string message)
        {
            try
            {
                 return await this.botService.GetPrevius(userId);
            }
            catch(EmptyStateException)
            {
                return NotUndestand();
            }
           
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
                  return NotUndestand();
            }
        }

        private MessageDTO NotUndestand()
        {
            return new MessageDTO()
                {
                    Message = "Я Вас не понял, прошу воспользоваться командами:\n" +
                          "факультеты - поиск информации о специальности факультета\n" +
                          "тест - прохождение теста на проф. ориентацию",

                    KeyboardMarkup = botService.GenerateKeyboardMarkup("Факультеты", "Тесты на проф. ориентацию")
                };
        }
    }
}