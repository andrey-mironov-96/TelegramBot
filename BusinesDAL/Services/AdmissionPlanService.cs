using app.common.DTO;
using app.common.Utils.Enums;
using app.domain.Abstract;
using app.domain.Services;
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

        public async Task<ReplyKeyboardMarkup> AdmissionHandlerAsync(string message, long userId)
        {
            try
            {
                StateType stateType = await _stateService.GetStateAsync(userId.ToString());
                Task<ReplyKeyboardMarkup> action = stateType switch
                {
                    StateType.None or
                    StateType.Faculty       => GetFacultiesAsync(userId),
                    StateType.Speciality    => GetSpecialtiesAsync(message, userId),
                    StateType.Information   => GetInfoAboutSpecialtyAsync(message, userId),
                    _                       => GetFacultiesAsync(userId)
                };
                return await action;
            }
            catch(ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }


        }

        private Task<ReplyKeyboardMarkup> GetInfoAboutSpecialtyAsync(string message, long userId)
        {
            throw new NotImplementedException();
        }

        private async Task<ReplyKeyboardMarkup> GetSpecialtiesAsync(string message, long userId)
        {
            List<KeyboardButton[]> specialitiesButtons = new List<KeyboardButton[]>();
            ReplyKeyboardMarkup replyKeyboardMarkup = null;
            var faculties = await _facultyRepository.GetAsync();
            var faculty = faculties.SingleOrDefault(fac => fac.Name.ToLower() == message.ToLower());
            if (faculty == null)
            {
                throw new ArgumentException($"Not found faculty with name {message}");
            }
            foreach (var speciality in faculty.Specialities)
            {
                specialitiesButtons.Add(new KeyboardButton[] { speciality.Name });
            }
            replyKeyboardMarkup = new ReplyKeyboardMarkup(
               specialitiesButtons
            )
            {
                ResizeKeyboard = true
            };
            return replyKeyboardMarkup;
        }

        private async Task<ReplyKeyboardMarkup> GetFacultiesAsync(long userId)
        {
            List<KeyboardButton[]> facultetButtons = new List<KeyboardButton[]>();
            foreach (var facultet in await _facultyRepository.GetAsync())
            {
                facultetButtons.Add(new KeyboardButton[] { facultet.Name });
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