using WebParse.Utils.Enums;

namespace WebParse.Models
{
    public class AdmissionPlan
    {
        public string SpecialtyName { get; set; }
        public int GeneralCompetition { get; set; }
        public int QuotaLOP { get; set; }
        public int TargetAdmissionQuota { get; set; }
        public int SpecialQuota { get; set; }
        public int ExtrabudgetaryPlaces { get; set; }
        public TypeEducation TypeEducation { get; set; }

        public string FacultetName {get;set;}

        public override string ToString()
        {
            return 
            $"FacultetName: {FacultetName}," +
            $"SpecialtyName: {SpecialtyName}, " +
            $"GeneralCompetition: {GeneralCompetition}, " +
            $"QuotaLOP: {QuotaLOP}, " +
            $"TargetAdmissionQuota: {TargetAdmissionQuota}, " +
            $"SpecialQuota: {SpecialQuota}, " +
            $"ExtrabudgetaryPlaces: {ExtrabudgetaryPlaces}, " +
            $"TypeEducation: {TypeEducation.ToString()} ";
        }
    }
}





 





