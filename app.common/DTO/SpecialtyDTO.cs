using app.common.Utils;
using app.common.Utils.Enums;

namespace app.common.DTO
{
    public class SpecialtyDTO : ABaseDTOEntity
    {
        #pragma warning disable CS8618
        public string Name { get; set; }
        public int GeneralCompetition { get; set; }
        public int QuotaLOP { get; set; }
        public int TargetAdmissionQuota { get; set; }
        public int SpecialQuota { get; set; }
        public int ExtrabudgetaryPlaces { get; set; }
        public EducationType EducationType {get;set;}
        public long FacultyId { get; set; }
        public  FacultyDTO Faculty { get; set; }
    }
}