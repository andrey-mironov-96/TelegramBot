using app.common.DTO;
using app.common.Utils.Enums;
using app.domain.Data.Configuration;
using app.domain.Data.Models;
using app.test.core.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WebParse.Business;

namespace app.domain.test;

public class WebParseServiceTest : ABaseTest<IWebParseService>
{

    public override IWebParseService GetCurrentService()
    {
        return this.GetWebParseService();
    }
    [Fact]
    public void GetFacultiesAndSpecialities()
    {
        IWebParseService webParse = this.GetCurrentService();
        const string linkForParse = "https://abiturient.ulsu.ru/tiles/documents/86";
        Dictionary<string, List<AdmissionPlan>> admissionPlans = webParse.GetFacultiesAndSpecialitiesAsync().Result;
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
            foreach (var s in item.Value)
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
                    EducationType = (EducationType)Enum.Parse(typeof(EducationType), s.TypeEducation.ToString())
                };
                specialties.Add(specialty);
            }
            faculty.Specialities = specialties;
            faculties.Add(faculty);
        }
        dbContext.Faculties.AddRange(faculties);
        dbContext.SaveChanges();
    }


    [Fact]
    public async Task GetDataFromULGUSiteTest()
    {
        IWebParseService webParseService = GetCurrentService();
        try
        {
           ParsingResult<Dictionary<string, List<AdmissionPlan>>> result = await webParseService.GetDataFromULGUSite();
        }
        catch (Exception e)
        {
            
        }





    }


}