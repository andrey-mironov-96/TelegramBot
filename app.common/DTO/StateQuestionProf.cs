namespace app.common.DTO
{
    public class StateQuestionProf
    {
        public QuestionDTO CurrentQuestion { get; set; }
        public short CurrentPosition { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}