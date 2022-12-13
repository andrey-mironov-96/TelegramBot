using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.test.core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace app.test.core.Services
{
    public class LoggerTest<TService> : ITestService<ILogger<TService>>
    {
        public ILogger<TService> GetService()
        {
           return new NullLogger<TService>();
        }
    }
}