using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebParse.Business;
using WebParse.Models;
using WebParse.Services;

namespace WebParse.Configure
{
    public  class WebParseConfigure
    {
        public static void Build(IServiceCollection services)
        {
            ILogger<WebParseConfigure> logger = LoggerFactory.Create(_ => _.AddConsole()).CreateLogger<WebParseConfigure>();
            IWebParseService webParse = new  WebParseService();
            // const string linkForParse = "https://abiturient.ulsu.ru/tiles/documents/86";
            // logger.LogInformation($"Starting parse data from {linkForParse}");
            // Dictionary<string, List<AdmissionPlan>> admissionPlans = webParse.GetAsync(linkForParse, WebParse.Utils.Enums.HttpType.Get).Result;
            // logger.LogInformation($"Finished parse data");
            // services.AddSingleton<Dictionary<string, List<AdmissionPlan>>>(admissionPlans);
        }
    }
}