using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.domain.Abstract;
using app.domain.Data.Configuration;
using app.domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace app.test.core.Utils
{
    public abstract class ABaseTest<TService>
    {
        public abstract TService GetCurrentService();

        public AppDbContext GetAppDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql("Server = localhost; User Id = bot; Password = bot; Port = 5432; Database = telegram_bot")
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
    }
}