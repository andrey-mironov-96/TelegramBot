using app.common.DTO;
using app.common.Utils;
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
        private readonly ITestRepository testRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly ITestScoreRepository testScoreRepository;
        public BotService(
            IStateService stateService,
            IFacultyRepository fRepository,
            ITestRepository testRepository,
            IQuestionRepository questionRepository,
            ITestScoreRepository testScoreRepository)
        {
            this.stateService = stateService;
            this.fRepository = fRepository;
            this.testRepository = testRepository;
            this.questionRepository = questionRepository;
            this.testScoreRepository = testScoreRepository;
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
                ActiveBlockType.SelectedProfTest or
                ActiveBlockType.ChooseProfTest or
                ActiveBlockType.RunProfTest  => this.ProfTestJob(stateValue, userId, message),
                _ => Task.FromResult(this.GetBaseMessage())
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
            switch (stateValue.activeBlockType)
            {
                case ActiveBlockType.ChooseProfTest:
                case ActiveBlockType.SelectedProfTest:
                case ActiveBlockType.RunProfTest:
                    stateValue = this.InitUserState();
                    await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                    return GetBaseMessage();
                case ActiveBlockType.Faculty:
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
            stateValue.activeBlockType = ActiveBlockType.ChooseProfTest;
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

        private Task<MessageDTO> ProfTestJob(StateValue stateValue, long userId, string message)
        {
            Task<MessageDTO> messageWithProfTest = stateValue.activeBlockType switch
            {
                ActiveBlockType.ChooseProfTest => this.ChoiseProfTestHandlerAsync(stateValue, userId, message),
                ActiveBlockType.SelectedProfTest => this.SelectedProfTestHandlerAsync(stateValue, userId, message),
                ActiveBlockType.RunProfTest => this.RunProfTestHandlerAsync(stateValue, userId, message),
                _ => Task.FromResult(GetBaseMessage())
            };
            return messageWithProfTest;
        }

        private async Task<MessageDTO> ChoiseProfTestHandlerAsync(StateValue stateValue, long userId, string message)
        {
            List<TestDTO> listTests = await testRepository.Get();
            stateValue.activeBlockType = ActiveBlockType.SelectedProfTest;
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
            return new MessageDTO()
            {
                Message = $"Для успешного прохождения профифильного тестирования небходимо ответить на все вопросы." +
    "Не переживайте, прохождение выбранного теста занимает не более 10 минут. \nДавайте выберем тест...",
                KeyboardMarkup = this.GenerateKeyboardMarkup(listTests.Select(test => test.Title).ToArray())
            };
        }

        private async Task<MessageDTO> SelectedProfTestHandlerAsync(StateValue stateValue, long userId, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                IEnumerable<TestDTO> tests = await this.testRepository.GetAsync();
                TestDTO test = tests.FirstOrDefault(test => test.Title.Trim().ToUpper() == message.Trim().ToUpper());
                if (test != null)
                {
                    stateValue.TestId = test.Id;
                    await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                    PageableData<QuestionDTO> data = await this.questionRepository.GetPage(new PageableData<QuestionDTO>(1, 1, 10)
                    {
                        Filters = new Filter[1] { new Filter() { Field = "testId", Value = test.Id.ToString() } }
                    });
                    stateValue.activeBlockType = ActiveBlockType.RunProfTest;
                    await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                    return new MessageDTO()
                    {
                        Message = $"Прохождение данного теста займет около {(data.Total * 20) / 60} минут. \n" +
                        $"В тесте всего {data.Total} вопросов, ну что начнем?",
                        KeyboardMarkup = this.GenerateKeyboardMarkup("Начать")
                    };
                }
            }
            stateValue.activeBlockType = ActiveBlockType.ChooseProfTest;
            await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
            return await ProfTestJob(stateValue, userId, "Не могу найти тест, попробуй снова");
        }

        private async Task<MessageDTO> RunProfTestHandlerAsync(StateValue stateValue, long userId, string message)
        {
            List<QuestionDTO> questions = this.questionRepository.GetQuestionsOfTest(stateValue.TestId).OrderBy(q => q.Position).ToList();
            if (message.Split(" ")[0].Trim().ToUpper().StartsWith("НАЧАТЬ"))
            {
                stateValue.stateQuestion = new StateQuestionProf();
                stateValue.stateQuestion.Questions = questions;
                stateValue.stateQuestion.CurrentQuestion = questions[0];
                stateValue.stateQuestion.CurrentPosition = questions[0].Position;
                await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                return new MessageDTO()
                {
                    Message = questions[0].Title,
                    KeyboardMarkup = this.GenerateKeyboardMarkup(questions[0].Answers.Select(a => a.Text).ToArray())
                };
            }

            QuestionDTO currentQuestion = stateValue.stateQuestion.CurrentQuestion;
            AnswerDTO chooseAnswer = currentQuestion.Answers.FirstOrDefault(a => a.Text.Trim().ToUpper() == message.Trim().ToUpper());
            if (chooseAnswer == null)
            {
                return new MessageDTO()
                {
                    Message = "Я вас не понимаю, попробуйте снова",
                    KeyboardMarkup = GenerateKeyboardMarkup("Факультеты", "Тесты на проф. ориентацию")
                };
            }
            stateValue.stateQuestion.Questions[currentQuestion.Position].UserChoose = chooseAnswer;
            if (currentQuestion.Position + 1 < stateValue.stateQuestion.Questions.Count())
            {
                QuestionDTO nextQuestion = stateValue.stateQuestion.Questions.ElementAt(currentQuestion.Position + 1);
                stateValue.stateQuestion.CurrentPosition = nextQuestion.Position;
                stateValue.stateQuestion.CurrentQuestion = nextQuestion;
                await stateService.SetKeyAsync<StateValue>(stateValue, userId.ToString());
                return new MessageDTO()
                {
                    Message = nextQuestion.Title,
                    KeyboardMarkup = this.GenerateKeyboardMarkup(nextQuestion.Answers.Select(a => a.Text).ToArray())
                };
            }
            else
            {
                List<TestScoreDTO> testScores = this.testScoreRepository.GetByTestId(stateValue.TestId).ToList();
                List<AnswerDTO> userAnswers = stateValue.stateQuestion.Questions.Select(s => s.UserChoose).ToList();
                short userPoint = 0;
                userAnswers.ForEach(answer => userPoint += answer.Point);
                TestScoreDTO lookForTestScore = null;
                testScores.ForEach(score =>
                {
                    if (score.From <= userPoint && score.To >= userPoint)
                    {
                        lookForTestScore = score;
                    }
                });
                if (lookForTestScore == null)
                {
                    return new MessageDTO()
                    {
                        Message = "Ой, ошибка в расчетах, уже устраняем проблему. Не переживайте, ваши результаты сохранены",
                    };
                }
                else
                {
                    return new MessageDTO()
                    {
                        Message = $"Вы набрали {userPoint} балов. \nРезультат тестирования:\n{lookForTestScore.Text}",
                        KeyboardMarkup = GenerateKeyboardMarkup()
                    };
                }
            }

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
                KeyboardMarkup = GenerateKeyboardMarkup("Факультеты", "Тесты на проф. ориентацию")
            };
        }

    }
}