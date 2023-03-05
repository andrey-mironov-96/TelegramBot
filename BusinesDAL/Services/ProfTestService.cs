using app.common.DTO;
using app.domain.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace BusinesDAL.Services
{
    public class ProfTestService : IProfTestService
    {
        private readonly ILogger<ProfTestService> _logger;
        private readonly IStateService _stateService;

        public ProfTestService(
            ILogger<ProfTestService> logger,
            IStateService stateService
        )
        {
            _logger = logger;
            _stateService = stateService;

        }
        public async Task<MessageDTO> Answer(string message, long userId)
        {
            List<QuestionProf> profTest = await _stateService.GetKeyAsync<List<QuestionProf>>(userId.ToString());
            QuestionProf currentItem = null;
            ReplyKeyboardMarkup keyboardMarkup = null;
            if (profTest == null || profTest.Count == 0)
            {
                profTest = Init();
            }
            profTest = profTest.OrderBy(o => o.Position).ToList();
            currentItem = profTest.First(itm => string.IsNullOrEmpty(itm.UserChoose));
            keyboardMarkup = GenerateBtns(currentItem.Answers);
            return new MessageDTO()
            {
                Message = currentItem.Question,
                KeyboardMarkup = keyboardMarkup
            };

        }

        private List<QuestionProf> Init()
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

        private ReplyKeyboardMarkup GenerateBtns(string[] values)
        {
            List<KeyboardButton[]> facultetButtons = new List<KeyboardButton[]>();
            foreach (string value in values)
            {
                facultetButtons.Add(new KeyboardButton[] { value });
            }
            return new ReplyKeyboardMarkup(facultetButtons)
            {
                ResizeKeyboard = true
            };
        }
    }
}