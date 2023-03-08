using app.common.DTO;
using app.common.Utils.CustomException;
using app.common.Utils.Enums;
using app.domain.Abstract;
using Telegram.Bot.Types.ReplyMarkups;

namespace app.domain.Services
{
    public class BotService : IBotService
    {
        private const string CHOOSE_SPECIALITY = "Выберите интересующую специальнось";
        private const string CHOOSE_FACULTY = "Выберите интересующий факультет";
        private readonly IStateService stateService;
        private readonly IFacultyRepository fRepository;
        public BotService(IStateService stateService, IFacultyRepository fRepository)
        {
            this.stateService = stateService;
            this.fRepository = fRepository;
        }

        public async Task<MessageDTO> DoWork(long userId, string message)
        {
            StateValue stateValue = await GetUserState(userId);
            if (stateValue == null)
            {
                throw new EmptyStateException();
            }
            Task<MessageDTO> action = stateValue.activeBlockType switch
            {
                ActiveBlockType.Faculty => this.FacultyJob(stateValue, userId, message),
                ActiveBlockType.ProfTest => this.ProfTestJob(stateValue, userId, message)
            };
            return await action;
        }

        public async Task<MessageDTO> GetPrevius(long userId)
        {
            StateValue stateValue = await GetUserState(userId);
            if (stateValue is null)
            {
                throw new EmptyStateException();
            }
            if (stateValue.activeBlockType == ActiveBlockType.ProfTest)
            {
                stateValue = InitUserState();
                await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
            }
            else if (stateValue.activeBlockType == ActiveBlockType.Faculty)
            {
                IEnumerable<FacultyDTO> faculties = await stateService.GetKeyAsync<IEnumerable<FacultyDTO>>(stateService.facultyKey);
                if (faculties is null || faculties.Count() == 0)
                {
                    faculties = await fRepository.GetAsync();
                }
                MessageDTO resultMessage = null;
                switch (stateValue.stateFaculty.currectStep)
                {
                    case StateType.None:
                    case StateType.Faculty:
                        stateValue.stateFaculty.ActiveFaculty = null;
                        stateValue.stateFaculty.currectStep = StateType.None;
                        stateValue.stateFaculty.ActiveSpeciality = null;
                        resultMessage = this.GetBaseMessage();
                        break;
                    case StateType.Speciality:
                    case StateType.Information:
                        stateValue.stateFaculty.ActiveFaculty = null;
                        stateValue.stateFaculty.currectStep = StateType.Faculty;
                        stateValue.stateFaculty.ActiveSpeciality = null;
                        resultMessage = new MessageDTO()
                        {
                            Message = CHOOSE_FACULTY,
                            KeyboardMarkup = GenerateKeyboardMarkup(faculties.Select(s => s.Name).ToArray())
                        };
                        break;
                }
                await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                return resultMessage;
            }
            return this.GetBaseMessage();
        }

        public async Task InitFacultyAsync(long userId)
        {
            StateValue stateValue = await GetUserState(userId);
            if (stateValue is null)
            {
                stateValue = InitUserState();
            }
            stateValue.activeBlockType = ActiveBlockType.Faculty;
            IEnumerable<FacultyDTO> faculties = await stateService.GetKeyAsync<IEnumerable<FacultyDTO>>(stateService.facultyKey);
            if (faculties is null || faculties.Count() == 0)
            {
                faculties = await fRepository.GetAsync();
                await stateService.SetKeyAsync<IEnumerable<FacultyDTO>>(faculties, stateService.facultyKey);

            }
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
        }

        public async Task InitProfAsync(long userId)
        {
            StateValue stateValue = await GetUserState(userId);
            if (stateValue is null)
            {
                stateValue = InitUserState();
            }
            stateValue.activeBlockType = ActiveBlockType.ProfTest;
            List<QuestionProf> questions = InitQuestionsProf();
            QuestionProf question = questions.First();
            stateValue.activeBlockType = ActiveBlockType.ProfTest;
            stateValue.stateQuestion.Questions = questions;
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
        }

        private async Task<MessageDTO> FacultyJob(StateValue stateValue, long userId, string message)
        {
            IEnumerable<FacultyDTO> faculties = await this.stateService.GetKeyAsync<IEnumerable<FacultyDTO>>(stateService.facultyKey);
            long currentFacultyId = 0;
            FacultyDTO currentFaculty = null;
            List<SpecialtyDTO> specialties = null;
            MessageDTO resultMessage = null;
            if (faculties is null || faculties.Count() == 0)
            {
                faculties = await fRepository.GetAsync();
            }
            switch (stateValue.stateFaculty.currectStep)
            {
                case StateType.None:
                    stateValue.stateFaculty.currectStep = StateType.Faculty;
                    resultMessage = new MessageDTO()
                    {
                        Message = CHOOSE_FACULTY,
                        KeyboardMarkup = GenerateKeyboardMarkup(faculties.Select(s => s.Name).ToArray())
                    };
                    break;
                case StateType.Faculty:
                    currentFaculty = faculties.SingleOrDefault(w => w.Name.ToLower() == message.ToLower());
                    if (currentFaculty is null)
                    {
                        throw new NotFoundExeption($"Not found faculty with name {message}");
                    }
                    specialties = faculties.Single(w => w.Id == currentFaculty.Id).Specialities;
                    if (specialties is null || specialties.Count == 0)
                    {
                        throw new ArgumentException($"Imposible getting specialities becouse faculty with id '{currentFacultyId}' does't contains specialities");
                    }

                    resultMessage = new MessageDTO()
                    {
                        Message = CHOOSE_SPECIALITY,
                        KeyboardMarkup = GenerateKeyboardMarkup(specialties.Select(s => s.Name).ToArray())
                    };
                    stateValue.stateFaculty.currectStep = StateType.Speciality;
                    stateValue.stateFaculty.ActiveFaculty = currentFaculty;
                    break;
                case StateType.Speciality:
                    currentFaculty = stateValue.stateFaculty.ActiveFaculty;
                    if (currentFaculty is null || currentFaculty.Id == 0)
                    {
                        throw new ArgumentException("Imposible getting specialities becouse current faculty equal 0 or null");
                    }
                    SpecialtyDTO chooseSpeciality = faculties.Single(w => w.Id == currentFaculty.Id).Specialities.SingleOrDefault(spec => spec.Name.ToLower() == message.ToLower());
                    if (chooseSpeciality is null)
                    {
                        throw new ArgumentException($"Imposible getting speciality becouse faculty with id '{currentFacultyId}' does't contains specialities or speciality with name {message} not found");
                    }
                    stateValue.stateFaculty.ActiveSpeciality = chooseSpeciality;
                    resultMessage = new MessageDTO()
                    {
                        Message = chooseSpeciality.ToString()
                    };
                    stateValue.stateFaculty.currectStep = StateType.Information;
                    break;
                case StateType.Information:
                    SpecialtyDTO specialty = stateValue.stateFaculty.ActiveSpeciality;
                    return new MessageDTO()
                    {
                        Message = specialty.ToString()
                    };

            }
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
            return resultMessage;
        }

        private async Task<MessageDTO> ProfTestJob(StateValue stateValue, long userId, string message)
        {
            stateValue.activeBlockType = ActiveBlockType.ProfTest;
            MessageDTO resultMessage = null;
            if (stateValue.stateQuestion.CurrentQuestion == null)
            {
                if (stateValue.stateQuestion.Questions == null || stateValue.stateQuestion.Questions.Count == 0)
                {
                    stateValue.stateQuestion.Questions = InitQuestionsProf();
                }
                stateValue.stateQuestion.CurrentQuestion = stateValue.stateQuestion.Questions.OrderBy(p => p.Position).First();
                stateValue.stateQuestion.CurrentPosition = stateValue.stateQuestion.CurrentQuestion.Position;
                resultMessage = new MessageDTO
                {
                    Message = $"Вопрос {stateValue.stateQuestion.CurrentPosition} из {stateValue.stateQuestion.Questions.Count}\n{stateValue.stateQuestion.CurrentQuestion.Question}",
                    KeyboardMarkup = GenerateKeyboardMarkup(stateValue.stateQuestion.CurrentQuestion.Answers)
                };
            }
            else
            {
                QuestionProf currentQuestion = stateValue.stateQuestion.CurrentQuestion;
                string userAnswer = currentQuestion.Answers.SingleOrDefault(a => a.ToLower() == message.ToLower());
                if (string.IsNullOrWhiteSpace(userAnswer) || string.IsNullOrEmpty(userAnswer))
                {
                    resultMessage = this.NotUndestandMessageProfTest(stateValue);
                }
                else
                {
                    QuestionProf question = stateValue.stateQuestion.Questions.Single(q => q.Position == currentQuestion.Position);
                    question.UserChoose = userAnswer;
                    if (question.Position >= stateValue.stateQuestion.Questions.Count)
                    {
                        int[] answerYes = new int[] { 2, 5, 6, 8, 10, 11, 13, 14, 15, 16, 17, 18, 19, 20, 21, 24, 25, 26, 27 };
                        int userPoints = 0;
                        foreach (QuestionProf itm in stateValue.stateQuestion.Questions)
                        {
                            if (answerYes.Contains(itm.Position) && itm.UserChoose == itm.Answers[0])
                            {
                                userPoints = userPoints + 2;
                            }
                            else if (itm.UserChoose == itm.Answers[1])
                            {
                                userPoints++;
                            }
                            else if (!answerYes.Contains(itm.Position) && itm.UserChoose == itm.Answers[2])
                            {
                                userPoints = userPoints + 2;
                            }
                        }
                        if (userPoints >= 49) //&& userPoints <= 56
                        {
                            resultMessage = new MessageDTO
                            {
                                Message = $"Вы набарили {userPoints}.\n" +
                                "Высокий интерес к знаковым системам.\n" +
                                "Идеальные профессии – корректор, секретарь, экономист, чертежник, картограф",
                                KeyboardMarkup = GenerateKeyboardMarkup(new string[0])
                            };
                        }
                        else if (userPoints >= 37 && userPoints <= 48)
                        {
                            resultMessage = new MessageDTO
                            {
                                Message = $"Вы набарили {userPoints}.\n" +
                                "Повышенный интерес к знаковым системам.\n" +
                                "Лучше всего отдать предпочтение профессиям менеджера, юриста, финансиста, журналиста",
                                KeyboardMarkup = GenerateKeyboardMarkup(new string[0])
                            };
                        }
                        else if (userPoints >= 25 && userPoints <= 36)
                        {
                            resultMessage = new MessageDTO
                            {
                                Message = $"Вы набарили {userPoints}.\n" +
                                "Определенные интересы к точным наукам.\n" +
                                "Лучше всего отдать предпочтение профессиям связанные с точными науками: инженер, конструктор, программист, физик, математик",
                                KeyboardMarkup = GenerateKeyboardMarkup(new string[0])
                            };
                        }
                        else if (userPoints >= 13 && userPoints <= 24)
                        {
                            resultMessage = new MessageDTO
                            {
                                Message = $"Вы набарили {userPoints}.\n" +
                                "Выраженный интерес к творчеству.\n" +
                                "Лучшие сферы деятельности - продюсирование, реклама, дизайн, психология, журналистика и т.д.",
                                KeyboardMarkup = GenerateKeyboardMarkup(new string[0])
                            };
                        }
                        else if (userPoints >= 0 && userPoints <= 12)
                        {
                            resultMessage = new MessageDTO
                            {
                                Message = $"Вы набарили {userPoints}.\n" +
                                "«Свободный художник».\n" +
                                "В этом случае лучше всего работать индивидуальным предпринимателем или фрилансером",
                                KeyboardMarkup = GenerateKeyboardMarkup(new string[0])
                            };
                        }
                    }
                    else
                    {
                        QuestionProf nextQuestion = stateValue.stateQuestion.Questions.Single(q => q.Position == currentQuestion.Position + 1);
                        stateValue.stateQuestion.CurrentPosition = nextQuestion.Position;
                        stateValue.stateQuestion.CurrentQuestion = nextQuestion;
                        resultMessage = new MessageDTO
                        {
                            Message = $"Вопрос {stateValue.stateQuestion.CurrentPosition} из {stateValue.stateQuestion.Questions.Count}\n{stateValue.stateQuestion.CurrentQuestion.Question}",
                            KeyboardMarkup = GenerateKeyboardMarkup(stateValue.stateQuestion.CurrentQuestion.Answers)
                        };
                    }
                }
            }
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
            return resultMessage;
        }

        private MessageDTO NotUndestandMessageProfTest(StateValue userSate)
        {
            return new MessageDTO()
            {
                Message = "Я не понял ваш ответ, пожалуйста дайте ответ снова",
                KeyboardMarkup = GenerateKeyboardMarkup(userSate.stateQuestion.CurrentQuestion.Answers)
            };
        }

        private async Task<StateValue> GetUserState(long userId)
        {
            StateValue stateValue = await stateService.GetKeyAsync<StateValue>(userId.ToString());
            return stateValue;
        }

        private StateValue InitUserState()
        {
            StateValue stateValue = new StateValue()
            {
                activeBlockType = ActiveBlockType.None,
                stateFaculty = new StateFaculty()
                {
                    currectStep = 0,
                    ActiveFaculty = null,
                    ActiveSpeciality = null
                },
                stateQuestion = new StateQuestionProf()
                {
                    CurrentPosition = 0,
                    CurrentQuestion = null,
                    Questions = null
                }
            };
            return stateValue;
        }

        public ReplyKeyboardMarkup GenerateKeyboardMarkup(params string[] titles)
        {
            List<KeyboardButton[]> btns = new List<KeyboardButton[]>();
            foreach (string title in titles)
            {
                btns.Add(new KeyboardButton[] { title });
            }
            btns.Add(new KeyboardButton[] { "Назад" });
            return new ReplyKeyboardMarkup(btns) { ResizeKeyboard = true };
        }

        private MessageDTO GetBaseMessage()
        {
            return new MessageDTO()
            {
                Message = "Чем тебе могу помочь?",
                KeyboardMarkup = GenerateKeyboardMarkup("Факультеты", "Тест на проф. ориентацию")
            };
        }

        public List<QuestionProf> InitQuestionsProf()
        {
            return new List<QuestionProf>(){
                new QuestionProf() {Position = 1, Question = "Я предпочту заниматься финансовыми операциями, а не, например, музыкой.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 2, Question = "Работа, связанная с учетом и контролем, – это довольно скучно", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 3, Question = "Невозможно точно рассчитать, сколько времени уйдет на дорогу до работы, по крайней мере, мне.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 4, Question = "Я часто рискую.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 5, Question = "Меня раздражает беспорядок.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 6, Question = "Я охотно почитал(а) бы на досуге о последних достижениях в различных областях науки.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 7, Question = "Записи, которые я делаю, не очень хорошо структурированы и организованы.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 8, Question = "Я предпочитаю разумно распределять деньги, а не тратить все сразу.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 9, Question = "У меня наблюдается, скорее, рабочий беспорядок на столе, чем расположение вещей по аккуратным «стопочкам».", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 10, Question = "Меня привлекает работа, где необходимо действовать согласно инструкции или четко заданному алгоритму.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 11, Question = "Если бы я что-то собирал(а), я бы постарался(ась) привести в порядок коллекцию, все разложить по папочкам и полочкам.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 12, Question = "Терпеть не могу наводить порядок и систематизировать что бы то ни было.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 13, Question = "Мне нравится работать на компьютере – оформлять или просто набирать тексты, производить расчеты.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 14, Question = "Прежде чем действовать, надо продумать все детали.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 15, Question = "На мой взгляд, графики и таблицы – очень удобный и информативный способ предоставления информации.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 16, Question = "Мне нравятся игры, в которых я могу точно рассчитать шансы на успех и сделать осторожный, но точный ход.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 17, Question = "При изучении иностранного языка я предпочитаю начинать с грамматики, а не получать разговорный опыт без знания грамматических основ.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 18, Question = "Сталкиваясь с какой-либо проблемой, я пытаюсь всесторонне ее изучить (ознакомиться с соответствующей литературой, поискать нужную информацию в интернете, поговорить со специалистами).", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 19, Question = "Если я выражаю свои мысли на бумаге, мне важнее...", Answers = new string[] {"Логичность текста","Затрудняюсь ответить","Образность изложения"}},
                new QuestionProf() {Position = 20, Question = "У меня есть ежедневник, в который я записываю важную информацию на несколько дней вперед.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 21, Question = "Я с удовольствием смотрю новости политики и экономики.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 22, Question = "Я бы хотел(а), чтобы моя будущая профессия.", Answers = new string[] {"Обеспечивала меня нужной порцией адреналина","Затрудняюсь ответить","Давала бы мне ощущение спокойствия и надежности"}},
                new QuestionProf() {Position = 23, Question = "Я доделываю работу в последний момент.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 24, Question = "Взяв книгу, я всегда ставлю ее на место.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 25, Question = "Когда я ложусь спать, то уже наверняка знаю, что буду делать завтра.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 26, Question = "В своих словах и поступках я следую пословице «Семь раз отмерь, один – отрежь».", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 27, Question = "Перед ответственными делами я всегда составляю план их выполнения.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
                new QuestionProf() {Position = 28, Question = "После вечеринки мытье посуды я откладываю до утра.", Answers = new string[] {"Да", "Затрудняюсь ответить", "Нет"}},
            };
        }
    }
}