using app.common.DTO;
using app.domain.Data.Models;
namespace app.domain.Abstract
{
    public interface IFacultyRepository : IRepository<Faculty, FacultyDTO>
    {
        
    }
}