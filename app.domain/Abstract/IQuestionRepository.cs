using app.common.DTO;
using app.common.Utils;
using app.domain.Data.Models;

namespace app.domain.Abstract
{
    public interface IQuestionRepository : IRepository<Question, QuestionDTO>
    {
        Task<PageableData<QuestionDTO>> GetPage(PageableData<QuestionDTO> data);

        public short GetNextQuestionPosition();
    }
}