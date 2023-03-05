using app.common.DTO;

namespace BusinesDAL.Abstract
{
    public interface IFacultyBusinessService
    {
        Task<IEnumerable<FacultyDTO>> GetFacultiesAsync();

        Task<FacultyDTO> GetFacultyAsync(long id);

        Task<FacultyDTO> SaveAsync(FacultyDTO faculty);
    }
}