using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.domain.Utils.Extensions
{
    public static class WebHostExtensions
    {//CreateHostBuilder(args).Build().MigrateDatabase<AppDbContext>().Run();
        public static IHost MigrateDatabase<T>(this IHost webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return webHost;
        }
    }
}