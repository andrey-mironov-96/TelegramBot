using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using app.domain.Utils.Extensions;
using app.domain.Data.Utils.Configure;

public class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
           .ConfigureLogging(_ => _.AddConsole())
           .ConfigureServices(services => {
                DatabaseConfigure.Build(services);
                WebHostExtensions.MigrateDatabase(services);
           });


}