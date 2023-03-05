using app.common.DTO;
using app.common.Utils;
using app.domain.Data.Models;

namespace app.domain.Abstract
{
    public interface ISpecialityRepository : IRepository<Specialty, SpecialtyDTO>
    {
         Task<PageableData<SpecialtyDTO>> GetPage(PageableData<SpecialtyDTO> data);

         Task<PageableData<SpecialtyDTO>> GetPageSpecialityOfFacultyAsync(PageableData<SpecialtyDTO> data, long facultyId);
    }
}