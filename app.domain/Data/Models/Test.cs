using app.domain.Data.Utils;

namespace app.domain.Data.Models
{
    public class Test : ABaseModel
    {
        public string Title { get; set; }

        public virtual List<Question> Questions{ get; set; }

        public virtual List<TestScore> TestScores { get; set; }
    }
}
