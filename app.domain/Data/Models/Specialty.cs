using app.domain.Data.Utils;
using app.domain.Data.Utils.Enums;

namespace app.domain.Data.Models
{
    public class Specialty : ABaseModel
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
        public virtual Faculty Faculty { get; set; }

    }
}