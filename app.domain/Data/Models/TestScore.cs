using app.common.Utils.Enums;
using app.domain.Data.Utils;

namespace app.domain.Data.Models
{
    public class TestScore : ABaseModel
    {
        public long TestId { get; set; }

        public virtual Test Test { get; set; }

        public short From { get; set; }

        public short To { get; set; }

        public long? TargetId { get; set; }

        public TargetType TargetType{ get; set; }

        public string Text { get; set; }
    }
}
