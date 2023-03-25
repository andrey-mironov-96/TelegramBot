using app.common.DTO;
using app.domain.Data.Models;

namespace app.domain.Abstract
{
    public interface ITestRepositoty : IRepository<Test, TestDTO>
    {
    }
}
