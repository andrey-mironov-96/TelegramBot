// using app.common.DTO;
// using app.common.Utils.Enums;
// using app.domain.Abstract;
// using app.business.Abstract;
// using app.business.Models;
// using Microsoft.Extensions.Logging;
// using Telegram.Bot.Types.ReplyMarkups;

// namespace app.business.Services
// {
//     public class AdmissionPlanService //: IAdmissionPlanService
//     {
//         private readonly ILogger<AdmissionPlanService> _logger;
//         private readonly IFacultyRepository _facultyRepository;
//         private readonly IStateService _stateService;
//         private readonly IFuzzyService _fuzzyService;

//         public AdmissionPlanService(
//             ILogger<AdmissionPlanService> logger,
//             IFacultyRepository facultyRepository,
//             IStateService stateService,
//             IFuzzyService fuzzyService)
//         {
//             _logger = logger;
//             _facultyRepository = facultyRepository;
//             _stateService = stateService;
//             _fuzzyService = fuzzyService;
//         }

//         public async Task<MessageDTO> AdmissionHandlerAsync(string message, long userId)
//         {
//             // try
//             // {
//             //     StateValue statevalue = await _stateService.GetStateAsync(userId.ToString());
//             //     Task<MessageDTO> action = statevalue.State switch
//             //     {
//             //         StateType.None    => GetFacultiesAsync(userId),
//             //         StateType.Faculty => GetSpecialtiesAsync(message, userId),
//             //         StateType.Speciality => GetInfoAboutSpecialtyAsync(message, userId),
//             //         StateType.Information => GetInfoAboutSpecialtyAsync(message, userId),
//             //         _ => GetFacultiesAsync(userId)
//             //     };
//             //     return await action;
//             // }
//             // catch (ArgumentException ex)
//             // {
//             //     _logger.LogError(ex, ex.Message);
//             //     throw ex;
//             // }
//             throw new NotImplementedException();
//         }

//         public async Task<MessageDTO> GetFacultiesAsync(long userId, string additionalMessage = "")
//         {
//             try
//             {
//                 ReplyKeyboardMarkup replyKeyboardMarkup = await GetKeyboardFacultiesAsync();
//                 await _stateService.ChangeStateAsync(new StateValue() { State = StateType.Faculty, Message = String.Empty }, userId.ToString());
//                 return new MessageDTO
//                 {
//                     Message = string.IsNullOrEmpty(additionalMessage) || string.IsNullOrWhiteSpace(additionalMessage) ? "Факультеты вуза:" : additionalMessage,
//                     KeyboardMarkup = replyKeyboardMarkup
//                 };
//             }
//             catch (ArgumentException ex)
//             {
//                 _logger.LogError(ex, ex.Message);
//                 return new MessageDTO
//                 {
//                     Message = "Не могу найти выбранный факультет.\n Вот какие факультеты я знаю",
//                     KeyboardMarkup = await GetKeyboardFacultiesAsync()
//                 };
//             }
//         }

//         public async Task<MessageDTO> GoBackAsync(long userId)
//         {
//             StateValue stateValue = await _stateService.GetStateAsync(userId.ToString());
//             if (stateValue == null)
//             {
//                 return await GetFacultiesAsync(userId);
//             }
//             Task<MessageDTO> message = stateValue.State switch
//             {
//                 StateType.None or
//                 StateType.Faculty => this.GetFacultiesAsync(userId),
//                 StateType.Speciality => this.GetFacultiesAsync(userId),
//                 StateType.Information => this.GetRelatedSpecialtiesOfFaculty(stateValue.Message, userId),
//                 _ => this.GetFacultiesAsync(userId)
//             };
//             return await GetFacultiesAsync(userId);
//         }


//         private async Task<MessageDTO> GetInfoAboutSpecialtyAsync(string message, long userId)
//         {
//             IEnumerable<FacultyDTO> faculties = await _facultyRepository.GetAsync();
//             IEnumerable<SpecialtyDTO> specialities = faculties.SelectMany(fac => fac.Specialities);
//             SpecialtyDTO? speciality = specialities.FirstOrDefault(sp => sp.Name.ToLower() == message.ToLower());
//             if (speciality == null)
//             {
//                 return await GetFacultiesAsync(userId, "Я не знаю такую специальность. Давайте попробуем снова");
//             }
//             await _stateService.ChangeStateAsync(new StateValue() { Message = message, State = StateType.Information }, userId.ToString());
//             return new MessageDTO
//             {
//                 Message = speciality.ToString(),
//                 KeyboardMarkup = new ReplyKeyboardMarkup(new List<KeyboardButton[]>{new KeyboardButton[] { "Назад" }})
//             };
//         }

//         private async Task<MessageDTO> GetSpecialtiesAsync(string message, long userId)
//         {
//             try
//             {
//                 ReplyKeyboardMarkup replyKeyboardMarkup = await GetKeyboardSpecialtiesAsync(message);
//                 await _stateService.ChangeStateAsync(new StateValue() { State = StateType.Speciality, Message = message }, userId.ToString());
//                 return new MessageDTO
//                 {
//                     Message = "Специальности (направления) по выбранному факультету:",
//                     KeyboardMarkup = replyKeyboardMarkup
//                 };
//             }
//             catch (ArgumentException ex)
//             {
//                 _logger.LogError(ex, ex.Message);
//                 return new MessageDTO
//                 {
//                     Message = "Не могу найти специальности по выбранному факультету.\n Вот какие факультеты я знаю",
//                     KeyboardMarkup = await GetKeyboardFacultiesAsync()
//                 };
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, ex.Message);
//                 throw ex;
//             }
//         }

//         private async Task<ReplyKeyboardMarkup> GetKeyboardFacultiesAsync()
//         {
//             List<KeyboardButton[]> facultetButtons = new List<KeyboardButton[]>();
//             foreach (var facultet in await _facultyRepository.GetAsync())
//             {
//                 facultetButtons.Add(new KeyboardButton[] { facultet.Name });
//             }
//             ReplyKeyboardMarkup replyKeyboardMarkup = new ReplyKeyboardMarkup(facultetButtons)
//             {
//                 ResizeKeyboard = true
//             };
//             return replyKeyboardMarkup;
//         }

//         private async Task<ReplyKeyboardMarkup> GetKeyboardSpecialtiesAsync(string message)
//         {
//             List<KeyboardButton[]> specialitiesButtons = new List<KeyboardButton[]>();
//             ReplyKeyboardMarkup replyKeyboardMarkup = null;
//             var faculties = await _facultyRepository.GetAsync();
//             string facultyName = _fuzzyService.Run(message, faculties.Select(s => s.Name), true);
//             var faculty = faculties.SingleOrDefault(faculty => faculty.Name.ToLower() == facultyName.ToLower());
//             if (faculty == null)
//             {
//                 throw new ArgumentException($"Not found faculty with name: {message}");
//             }
//             specialitiesButtons.Add(new KeyboardButton[] { "Назад" });
//             foreach (var speciality in faculty.Specialities)
//             {
//                 specialitiesButtons.Add(new KeyboardButton[] { speciality.Name });
//             }
//             replyKeyboardMarkup = new ReplyKeyboardMarkup(specialitiesButtons)
//             {
//                 ResizeKeyboard = true
//             };
//             return replyKeyboardMarkup;
//         }

//         private async Task<MessageDTO> GetSpecilitiesByFacultyName(long userId, string facultyName)
//         {
//             try
//             {
//                 ReplyKeyboardMarkup replyKeyboardMarkup = await GetKeyboardSpecialtiesAsync(facultyName);
//                 await _stateService.ChangeStateAsync(new StateValue() { State = StateType.Information, Message = facultyName }, userId.ToString());
//                 return new MessageDTO
//                 {
//                     Message = "Специальности (направления) по факультету " + facultyName,
//                     KeyboardMarkup = replyKeyboardMarkup
//                 };
//             }
//             catch (ArgumentException ex)
//             {
//                 _logger.LogError(ex, ex.Message);
//                 return new MessageDTO
//                 {
//                     Message = "Не могу найти специальности по выбранному факультету.\n Вот какие факультеты я знаю",
//                     KeyboardMarkup = await GetKeyboardFacultiesAsync()
//                 };
//             }
//             catch (Exception ex)
//             {
//                 _logger.LogError(ex, ex.Message);
//                 throw ex;
//             }
//         }

//         private async Task<MessageDTO> GetRelatedSpecialtiesOfFaculty(string specialityName, long userId)
//         {
//             IEnumerable<FacultyDTO> faculties = await _facultyRepository.GetAsync();
//             long facultyId = faculties
//                                     .SelectMany(faculty => faculty.Specialities)
//                                     .Single(speciality => speciality.Name == specialityName).FacultyId;
//             string facultyName = faculties.Single(faculty => faculty.Id == facultyId).Name;
//             MessageDTO message = new MessageDTO()
//             {
//                 KeyboardMarkup = await GetKeyboardSpecialtiesAsync(facultyName),
//                 Message = $"Специальности факультета: {facultyName}"
//             };
//             await _stateService.ChangeStateAsync(new StateValue{Message = facultyName, State = StateType.Faculty}, userId.ToString());
//             return message;
//         }
//     }
// }