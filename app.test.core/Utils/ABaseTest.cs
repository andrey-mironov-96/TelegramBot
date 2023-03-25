using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Abstract;
using app.domain.Cache.Services;
using app.domain.Data.Configuration;
using app.domain.Services;
using BusinesDAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StackExchange.Redis;
using WebParse.Business;
using WebParse.Services;

namespace app.test.core.Utils
{
    public abstract class ABaseTest<TService>
    {
        public abstract TService GetCurrentService();

        public AppDbContext GetAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Server = localhost; User Id = bot; Password = bot; Port = 5432; Database = bot")
                .LogTo(Console.WriteLine, LogLevel.Information);
            return new AppDbContext(options.Options);
        }

        public IFacultyRepository GetFacultyRepository()
        {
            return new FacultyRepository(
               GetLogger<FacultyRepository>(),
               GetAppDbContext()
               );
        }

        public ILogger<TLoggerService> GetLogger<TLoggerService>()
        {
            return new NullLogger<TLoggerService>();
        }

        protected IConnectionMultiplexer GetRedis()
        {
            return ConnectionMultiplexer.Connect("localhost:6379");
        }

        protected IStackTraceService GetStackTraceService()
        {
            return new StackTraceService(
                new NullLogger<StackTraceService>(),
                GetRedis()
            );
        }

        protected IWebParseService GetWebParseService()
        {
            return new WebParseService(
                new NullLogger<WebParseService>(),
                GetStackTraceService()
            );
        }

        protected IStateService GetStateService()
        {
            return new StateService(
                new NullLogger<StateService>(),
                GetRedis()
            );
        }

        protected IBotService GetBotService()
        {
            return new BotService(
                GetStateService(),
                GetFacultyRepository()
            );
        }

        protected IBusinessBotService GetBusinessBotService()
        {
            return new BusinessBotService(
                GetBotService()
            );
        }

        protected ITestRepository GetTestRepository()
        {
            return new TestRepository(this.GetAppDbContext());
        }
    }
}