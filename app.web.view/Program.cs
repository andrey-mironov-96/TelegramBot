using app.domain.Abstract;
using app.domain.Cache.Configuration;
using app.domain.Data.Utils.Configure;
using app.domain.Services;
using app.web.view.Services;
using app.business.Abstract;
using app.business.Services;
using Telegram.Bot;
using static app.web.view.Configure.TelegramBotConfigure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.
        builder.Services.AddControllers();

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

        CacheConfigure.Build(builder.Services);
        DatabaseConfigure.Build(builder.Services);

        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app.web.view v1"));
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
        app.Run();
    }
}