using app.common.DTO;
using app.common.Utils.Abstract;

namespace app.business.Abstract
{
    public interface IQuestionBusinessService : IBaseBusinessService<QuestionDTO>
    {
        public int GetNextQuestionPosition(long testId);
    }
}