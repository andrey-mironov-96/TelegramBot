using app.common.DTO;
using app.common.Utils.Abstract;

namespace BusinesDAL.Abstract
{
    public interface IFacultyBusinessService : IBaseBusinessService<FacultyDTO>
    {
        Task<IEnumerable<FacultyDTO>> GetFacultiesAsync();
    }
}