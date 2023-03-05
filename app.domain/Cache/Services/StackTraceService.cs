using app.common.DTO;
using app.domain.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace app.domain.Cache.Services
{
    public class StackTraceService : IStackTraceService
    {
        private const string STACK_TRACE_KEY = "stack-trace";
        private readonly ILogger<StackTraceService> _logger;
        private readonly IDatabase _cache;
        private const int expiry = 120;
        public StackTraceService(ILogger<StackTraceService> logger, IConnectionMultiplexer conMultiplexer)
        {
            _logger = logger;
            _cache = conMultiplexer.GetDatabase();
        }
        public async Task AddStackTrace(StackTraceDTO stackTrace)
        {
           await _cache.HashSetAsync(STACK_TRACE_KEY,stackTrace.Identity.ToString(), JsonConvert.SerializeObject(stackTrace));
           await _cache.KeyExpireAsync(STACK_TRACE_KEY, TimeSpan.FromHours(expiry));
        }

        public List<StackTraceDTO> GetStackTraces()
        {
            HashEntry[] all = _cache.HashGetAll(STACK_TRACE_KEY);
            List<StackTraceDTO> stackTraces = null;
            if (all != null && all.Length > 0)
            {
               stackTraces = all.Select(s => JsonConvert.DeserializeObject<StackTraceDTO>(s.Value)).ToList();
               _cache.KeyExpire(STACK_TRACE_KEY, TimeSpan.FromHours(expiry));
               return stackTraces;
            }
            return null;

        }
    }
}