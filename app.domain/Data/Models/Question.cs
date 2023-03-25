using app.domain.Data.Utils;

namespace app.domain.Data.Models
{
    public class Question : ABaseModel
    {
        public long TestId { get; set; }
        public short Position { get; set; }

        public string Title { get; set; }

        public virtual Test Test { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
