using AppBot.Services;
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
                    services.AddScoped<IAdmissionPlanService>();
                    WebParse.Configure.WebParseConfigure.Build(services);
            }).Build();

        await host.RunAsync();
    }
}

public class BotConfiguration
{
    public static readonly string Configuration = "BotConfiguration";
    public string BotToken { get; set; } = "";
}

