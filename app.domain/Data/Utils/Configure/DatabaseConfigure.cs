using app.common.Configure;
using app.domain.Data.Configuration;
using app.domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace app.domain.Data.Utils.Configure
{
    public static class DatabaseConfigure
    {
        public static void Build(IServiceCollection services)
        {
            //TODO:remove test connection string
            Environment.SetEnvironmentVariable("Database",$"Server = localhost; User Id = bot; Password = bot; Port = 5432; Database = telegram_bot");
            string connectionString = EnvironmentConfigure.GetEnvironment(ConnectionStringEnum.Database);
            if (String.IsNullOrEmpty(connectionString) || String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("Connection line to database is empty");
            }
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString, db =>
                {
                    db.MigrationsAssembly("app.view");
                    db.EnableRetryOnFailure();
                }).LogTo(Console.WriteLine, LogLevel.Information);
                options.EnableSensitiveDataLogging();
            });
        }
    }
}