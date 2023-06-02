using app.business.Services;
using app.domain.Abstract;
using app.domain.Cache.Services;
using app.domain.Data.Configuration;
using app.domain.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StackExchange.Redis;

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
                GetFacultyRepository(),
                GetTestRepository(),
                GetQuestionRepository(),
                GetTestScoreRepository()
            );
        }
        public IQuestionRepository  GetQuestionRepository()
        {
            return new QuestionRepository(
                new NullLogger<QuestionRepository>(),
                GetAppDbContext()
            );
        }
        public ITestScoreRepository  GetTestScoreRepository()
        {
            return new TestScoreRepository(
                new NullLogger<TestScoreRepository>(),
                GetAppDbContext()
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