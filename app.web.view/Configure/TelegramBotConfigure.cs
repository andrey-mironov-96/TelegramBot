using app.domain.Abstract;
using app.domain.Services;
using app.web.view.Services;
using app.business.Abstract;
using app.business.Services;
using Telegram.Bot;

namespace app.web.view.Configure
{
    public class TelegramBotConfigure
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            builder.Services.Configure<BotConfiguration>(
                   builder.Configuration.GetSection(BotConfiguration.Configuration));

            builder.Services.AddHttpClient("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        TelegramBotClientOptions options = new TelegramBotClientOptions("5970266602:AAE8KptXWJwi_pUzCfSNSKxbpGWLoi_Z5lk");
                        return new TelegramBotClient(options, httpClient);
                    });

            builder.Services.AddScoped<UpdateHandler>();
            builder.Services.AddScoped<ReceiverService>();
            builder.Services.AddHostedService<PollingService>();
            builder.Services.AddScoped<IBusinessBotService, BusinessBotService>();
            builder.Services.AddScoped<IBotService, BotService>();

            builder.Services.AddScoped<IFacultyBusinessService, FacultyBusinessService>();
            builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();

            builder.Services.AddScoped<ISpecialityBusinessService, SpecialityBusinessService>();
            builder.Services.AddScoped<ISpecialityRepository, SpecialityRepository>();
            // builder.Services.AddScoped<IWebParseService, WebParseService>();
            builder.Services.AddScoped<ITestBusinessService, TestBusinessService>();
            builder.Services.AddScoped<ITestRepository, TestRepository>();
            builder.Services.AddScoped<ITestScoreBusinessService, TestScoreBusinessService>();
            builder.Services.AddScoped<ITestScoreRepository, TestScoreRepository>();
            builder.Services.AddScoped<IQuestionBusinessService, QuestionBusinessService>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
        }

        public class BotConfiguration
        {
            public static readonly string Configuration = "BotConfiguration";
            public string BotToken { get; set; } = "";
        }
    }
}