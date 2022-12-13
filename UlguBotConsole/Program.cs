using BusinesDAL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UlguBotConsole.Services;

public class Program
{
   
    private static async Task Main(string[] args)
    {
        await  CreateHostBuilder().RunConsoleAsync();    
    }
    static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureLogging(_ => _.AddConsole())
            .ConfigureServices(service => {
                WebParse.Configure.WebParseConfigure.Build(service);
                service.AddScoped<ICommandService, CommandService>();
                service.AddScoped<IAdmissionPlanService, AdmissionPlanService>();
                service.AddHostedService<BotHostedService>();
            });
    }
        

}