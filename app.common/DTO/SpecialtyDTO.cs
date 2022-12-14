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

        public override string ToString()
        {
            return $"Направление (специальность): {Name}\n" +
                   $"Форма обучения: {GetEducationType()}\n" +
                   $"Общий конкурс: {GeneralCompetition}\n" +
                   $"Квота ЛОП: {QuotaLOP}\n" +
                   $"Квота целевого приёма: {TargetAdmissionQuota}\n" +
                   $"Специальная квота: {SpecialQuota}\n" +
                   $"Внебюджетных мест: {ExtrabudgetaryPlaces}";
        }

        private string GetEducationType()
        {
            return EducationType switch
            {
                EducationType.Distance => "Заочная",
                EducationType.PartTime => "Очно-заочная",
                EducationType.FullTime => "Очная",
                _                      => "Очная"
            };
                
        }
    }
}