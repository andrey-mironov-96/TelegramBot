namespace app.common.DTO
{
    public class StateQuestionProf
    {
        public QuestionProf CurrentQuestion { get; set; }
        public short CurrentPosition { get; set; }
        public List<QuestionProf> Questions { get; set; }
    }
}