using app.common.DTO;
using app.common.Utils;
using app.domain.Data.Models;

namespace app.domain.Abstract
{
    public interface ITestScoreRepository : IRepository<TestScore, TestScoreDTO>
    {
        Task<PageableData<TestScoreDTO>> GetPage(PageableData<TestScoreDTO> data);

       public IEnumerable<TestScoreDTO> GetByTestId(long testId);
    }
}