using app.common.Utils;

namespace app.common.DTO
{
    public class FacultyDTO : ABaseDTOEntity
    {
        #pragma warning disable CS8618
        public string Name { get; set; }

        public List<SpecialtyDTO> Specialities { get; set; }
    }
}