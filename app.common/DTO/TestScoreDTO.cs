using app.common.Utils;
using app.common.Utils.Enums;

namespace app.common.DTO
{
    public class TestScoreDTO : ABaseDTOEntity
    {
        public long TestId { get; set; }

        public short From { get; set; }

        public short To { get; set; }

        public long? TargetId { get; set; }
        public TargetType TargetType { get; set; }

        public string Text { get; set; }
    }
}
