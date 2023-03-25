using app.common.Utils;

namespace app.common.DTO
{
    public class QuestionDTO : ABaseDTOEntity
    {
        public string Title { get; set; }

        public long TestId { get; set; }
        public short Position { get; set; }
        public List<AnswerDTO> Answers { get; set; }

    }
}
