using app.common.Utils.Enums;

namespace app.common.DTO
{
    public class StateFaculty
    {
        public FacultyDTO ActiveFaculty { get; set; }
        public SpecialtyDTO ActiveSpeciality { get; set; }
        public StateType currectStep {get;set;}
    }
}