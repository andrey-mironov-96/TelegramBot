using app.common.Utils;

namespace app.common.DTO
{
    public class TestDTO : ABaseDTOEntity
    {
        public string Title { get; set; }

        public List<QuestionDTO> Questions { get; set; }

        public List<TestScoreDTO> TestScores { get; set; }
    }
}
