using app.domain.Data.Configuration;
using app.domain.Data.Models;
using app.domain.Data.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace app.domain.test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        WebParseService webParse = new WebParseService();
        const string linkForParse = "https://abiturient.ulsu.ru/tiles/documents/86";
        Dictionary<string, List<AdmissionPlan>> admissionPlans = webParse.GetAsync(linkForParse, WebParse.Utils.Enums.HttpType.Get).Result;
        var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseNpgsql("Server = localhost; User Id = bot; Password = bot; Port = 5432; Database = telegram_bot")
         .LogTo(Console.WriteLine, LogLevel.Information);


        AppDbContext dbContext = new AppDbContext(options.Options);
        List<Faculty> faculties = new List<Faculty>();
        foreach (var item in admissionPlans)
        {
            Faculty faculty = new Faculty
            {
                CreateAt = DateTime.Now,
                ChangeAt = null,
                IsDeleted = false,
                Id = 0,
                Name = item.Key,
            };
            List<Specialty> specialties = new List<Specialty>();
            foreach(var s in item.Value)
            {
                Specialty specialty = new Specialty()
                {
                    ChangeAt = null,
                    CreateAt = DateTime.Now,
                    ExtrabudgetaryPlaces = s.ExtrabudgetaryPlaces,
                    FacultyId = 0,
                    Id = 0,
                    Name = s.SpecialtyName,
                    GeneralCompetition = s.GeneralCompetition,
                    IsDeleted = false,
                    QuotaLOP = s.QuotaLOP,
                    SpecialQuota = s.SpecialQuota,
                    TargetAdmissionQuota = s.TargetAdmissionQuota,
                    EducationType = (EducationType)Enum.Parse(typeof(EducationType),s.TypeEducation.ToString())
                };
                specialties.Add(specialty);
            }
            faculty.Specialities = specialties;
            faculties.Add(faculty);
        }
        dbContext.Faculties.AddRange(faculties);
        //dbContext.SaveChanges();
    }
}