using app.common.DTO;
using app.common.Utils.Abstract;

namespace app.business.Abstract
{
    public interface IFacultyBusinessService : IBaseBusinessService<FacultyDTO>
    {
        Task<IEnumerable<FacultyDTO>> GetFacultiesAsync();

        Task<FacultyDTO> GetFacultyAsync(long id);

        // Task<FacultyDTO> SaveAsync(FacultyDTO faculty);
    }
}