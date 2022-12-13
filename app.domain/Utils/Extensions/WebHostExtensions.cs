using app.domain.Data.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace app.domain.Utils.Extensions
{
    public static class WebHostExtensions
    {
        public static void MigrateDatabase(IServiceCollection services)
        {
            ServiceProvider sp = services.BuildServiceProvider();
            var db = sp.GetRequiredService<AppDbContext>();
            db.Database.Migrate();




            // webHost.ConfigureServices
            // using (var scope = webHost.Services.CreateScope())
            // {
            //     var services = scope.ServiceProvider;
            //     try
            //     {
            //         var db = services.GetRequiredService<T>();
            //         db.Database.Migrate();
            //     }
            //     catch (Exception ex)
            //     {
            //         var logger = services.GetRequiredService<ILogger<AppDbContext>>();
            //         logger.LogError(ex, "An error occurred while migrating the database.");
            //     }
            // }

        }
    }
}