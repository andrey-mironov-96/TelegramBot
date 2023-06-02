using app.common.DTO;
using app.common.Utils;
using app.common.Utils.Abstract;

namespace app.business.Abstract
{
    public interface ISpecialityBusinessService : IBaseBusinessService<SpecialtyDTO>
    {
        Task<PageableData<SpecialtyDTO>> GetPageSpecialityOfFacultyAsync(PageableData<SpecialtyDTO> data, long facultyId);

        Task<IEnumerable<SpecialtyDTO>> GetSpecialtiesAsync();
    }
}