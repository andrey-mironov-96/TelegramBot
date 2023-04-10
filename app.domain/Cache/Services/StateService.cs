using app.domain.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace app.domain.Cache.Services
{
    public class StateService : IStateService
    {
        private readonly ILogger<StateService> _logger;
        private readonly IDatabase _cache;
        private const int expiry = 120;

        public string facultyKey => "facultyies";

        public StateService(ILogger<StateService> logger, IConnectionMultiplexer conMultiplexer)
        {
            _logger = logger;
            _cache = conMultiplexer.GetDatabase();
        }
       
        public async Task<bool> SetKeyAsync<T>(T value, string key)
        {
            string strData = JsonConvert.SerializeObject(value, new JsonSerializerSettings(){
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _cache.StringSetAsync(key, strData);
            await _cache.KeyExpireAsync(key, TimeSpan.FromHours(expiry));
            return true;
        }

        public async Task<T> GetKeyAsync<T>(string key)
        {
            RedisValue redisValue = await _cache.StringGetAsync(key);
            if (redisValue.HasValue)
            {   string strData = redisValue.ToString();
                T data = JsonConvert.DeserializeObject<T>(strData);
                await _cache.KeyExpireAsync(key, TimeSpan.FromHours(expiry));
                return data;
            }
            return default;
        }

        public async Task<bool> RemoveKey(string key)
        {
            await _cache.KeyDeleteAsync(key);
            return true;
        }
    }
}