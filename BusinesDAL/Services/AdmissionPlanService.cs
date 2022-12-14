using app.common.DTO;
using app.common.Utils.Enums;
using app.domain.Abstract;
using app.domain.Services;
using BusinesDAL.Models;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public class AdmissionPlanService : IAdmissionPlanService
    {
        private readonly ILogger<AdmissionPlanService> _logger;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IStateService _stateService;

        public AdmissionPlanService(ILogger<AdmissionPlanService> logger, IFacultyRepository facultyRepository, IStateService stateService)
        {
            _logger = logger;
            _facultyRepository = facultyRepository;
            _stateService = stateService;

        }

        public async Task<MessageDTO> AdmissionHandlerAsync(string message, long userId)
        {
            try
            {
                StateType stateType = await _stateService.GetStateAsync(userId.ToString());
                Task<MessageDTO> action = stateType switch
                {
                    StateType.None or
                    StateType.Faculty => GetFacultiesAsync(userId),
                    StateType.Speciality => GetSpecialtiesAsync(message, userId),
                    StateType.Information => GetInfoAboutSpecialtyAsync(message, userId),
                    _ => GetFacultiesAsync(userId)
                };
                return await action;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        private async Task<MessageDTO> GetInfoAboutSpecialtyAsync(string message, long userId)
        {
            IEnumerable<FacultyDTO> faculties = await _facultyRepository.GetAsync();
            IEnumerable<SpecialtyDTO> specialities = faculties.SelectMany(fac => fac.Specialities);
            SpecialtyDTO? speciality = specialities.SingleOrDefault(sp => sp.Name.ToLower() == message.ToLower());
            if (speciality == null)
            {
                return new MessageDTO
                {
                    Message = "Я не знаю такую специальность. Попробуйте снова",
                    KeyboardMarkup = await GetKeyboardFacultiesAsync()
                };
            }
            return new MessageDTO
            {
                Message = speciality.ToString()
            };

        }

        private async Task<MessageDTO> GetSpecialtiesAsync(string message, long userId)
        {
            try
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = await GetKeyboardSpecialtiesAsync(message);
                await _stateService.ChangeStateAsync(StateType.Information, userId.ToString());
                return new MessageDTO
                {
                    Message = "Специальности (направления) по выбранному факультету:",
                    KeyboardMarkup = replyKeyboardMarkup
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new MessageDTO
                {
                    Message = "Не могу найти специальности по выбранному факультету.\n Вот какие факультеты я знаю",
                    KeyboardMarkup = await GetKeyboardFacultiesAsync()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }


        }

        private async Task<MessageDTO> GetFacultiesAsync(long userId)
        {
            try
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = await GetKeyboardFacultiesAsync();
                await _stateService.ChangeStateAsync(StateType.Speciality, userId.ToString());
                return new MessageDTO
                {
                    Message = "Факультеты вуза:",
                    KeyboardMarkup = replyKeyboardMarkup
                };
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return new MessageDTO
                {
                    Message = "Не могу найти выбранный факультет.\n Вот какие факультеты я знаю",
                    KeyboardMarkup = await GetKeyboardFacultiesAsync()
                };
            }
        }

        private async Task<ReplyKeyboardMarkup> GetKeyboardFacultiesAsync()
        {
            List<KeyboardButton[]> facultetButtons = new List<KeyboardButton[]>();
            foreach (var facultet in await _facultyRepository.GetAsync())
            {
                facultetButtons.Add(new KeyboardButton[] { facultet.Name });
            }
            ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(facultetButtons)
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

        private async Task<ReplyKeyboardMarkup> GetKeyboardSpecialtiesAsync(string message)
        {
            List<KeyboardButton[]> specialitiesButtons = new List<KeyboardButton[]>();
            ReplyKeyboardMarkup replyKeyboardMarkup = null;
            var faculties = await _facultyRepository.GetAsync();
            var faculty = faculties.SingleOrDefault(fac => fac.Name.ToLower() == message.ToLower());
            if (faculty == null)
            {
                throw new ArgumentException($"Not found faculty with name: {message}");
            }
            foreach (var speciality in faculty.Specialities)
            {
                specialitiesButtons.Add(new KeyboardButton[] { speciality.Name });
            }
            replyKeyboardMarkup = new ReplyKeyboardMarkup(specialitiesButtons)
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }
    }
}