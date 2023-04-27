using app.common.DTO;
using app.common.Utils.Abstract;

namespace BusinesDAL.Abstract
{
    public interface IQuestionBusinessService : IBaseBusinessService<QuestionDTO>
    {
        public int GetNextQuestionPosition(long testId);
    }
}