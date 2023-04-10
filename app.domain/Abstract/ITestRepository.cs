using app.common.DTO;
using app.common.Utils;
using app.domain.Data.Models;

namespace app.domain.Abstract
{
    public interface ITestRepository : IRepository<Test, TestDTO>
    {
        public Task<PageableData<TestDTO>> GetPageableDataAsync(PageableData<TestDTO> data);

        public Task<List<TestDTO>> Get();
    }
}
