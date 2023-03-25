using app.common.Utils;

namespace app.common.DTO
{
    public class AnswerDTO : ABaseDTOEntity
    {
        public long QuestionId { get; set; }
        public string Text { get; set; }

        public short Point { get; set; }
    }
}
