using app.domain.Abstract;
using app.domain.Cache.Configuration;
using app.domain.Data.Utils.Configure;
using app.domain.Services;
using AppBot.Services;
using BusinesDAL.Abstract;
using BusinesDAL.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;


public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<BotConfiguration>(
                   context.Configuration.GetSection(BotConfiguration.Configuration));
                services.AddHttpClient("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        
                        TelegramBotClientOptions options = new TelegramBotClientOptions("5970266602:AAE8KptXWJwi_pUzCfSNSKxbpGWLoi_Z5lk");
                        return new TelegramBotClient(options, httpClient);
                    });
                    services.AddScoped<UpdateHandler>();
                    services.AddScoped<ReceiverService>();
                    services.AddHostedService<PollingService>();
                   services.AddScoped<IBusinessBotService, BusinessBotService>();
                   services.AddScoped<IBotService, BotService>();
                    services.AddScoped<IProfTestService, ProfTestService>();
                    services.AddScoped<IFuzzyService, FuzzyService>();
                    services.AddScoped<IFacultyRepository, FacultyRepository>();
                    WebParse.Configure.WebParseConfigure.Build(services);
                    DatabaseConfigure.Build(services);
                    CacheConfigure.Build(services);
            }).Build();

        await host.RunAsync();
    }
}

public class BotConfiguration
{
    public static readonly string Configuration = "BotConfiguration";
    public string BotToken { get; set; } = "";
}

