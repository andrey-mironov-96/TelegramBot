using app.common.DTO;
using WebParse.Models;

namespace WebParse.Business
{
    public interface IWebParseService
    {
        public Task<Dictionary<string, List<AdmissionPlan>>> GetFacultiesAndSpecialitiesAsync();
        public Task<ParsingResult<Dictionary<string, List<AdmissionPlan>>>> GetPriceForSpecialities(Dictionary<string, List<AdmissionPlan>> faculties);

        public Task<ParsingResult<Dictionary<string, List<AdmissionPlan>>>> GetDataFromULGUSite();
    }
}