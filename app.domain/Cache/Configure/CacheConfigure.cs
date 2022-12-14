using app.common.Configure;
using app.domain.Abstract;
using app.domain.Cache.Services;
using app.domain.Utils;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace app.domain.Cache.Configuration
{
    public class CacheConfigure
    {
        public static void Build(IServiceCollection services)
        {
            //TODO:remove test connection string
            Environment.SetEnvironmentVariable("Cache",$"localhost:6379");
            string connectionString = EnvironmentConfigure.GetEnvironment(ConnectionStringEnum.Cache);
            if (String.IsNullOrEmpty(connectionString) || String.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("Connection line to database is empty");
            }
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
            services.AddScoped<IStateService, StateService>();
        }
    }

    
}