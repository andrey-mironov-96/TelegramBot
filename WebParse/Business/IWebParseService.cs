using WebParse.Models;
using WebParse.Utils.Enums;

namespace WebParse.Business
{
    public interface IWebParseService
    {
        public Task<Dictionary<string, List<AdmissionPlan>>> GetAsync(string http, HttpType httpType);
    }
}