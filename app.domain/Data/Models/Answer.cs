using app.domain.Data.Utils;

namespace app.domain.Data.Models
{
    public class Answer : ABaseModel
    {
        public long QuestionId { get; set; }

        public string Text { get; set; }

        public short Point { get; set; }

        public virtual Question Question { get; set; }
    }
}
