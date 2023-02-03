using app.common.DTO;
using app.common.Utils;
using app.domain.Data.Models;
namespace app.domain.Abstract
{
    public interface IFacultyRepository : IRepository<Faculty, FacultyDTO>
    {
        Task<PageableData<FacultyDTO>> GetPage(PageableData<FacultyDTO> data);
    }
}