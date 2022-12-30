using app.common.DTO;
using app.common.Utils.Enums;
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
        public StateService(ILogger<StateService> logger, IConnectionMultiplexer conMultiplexer)
        {
            _logger = logger;
            _cache = conMultiplexer.GetDatabase();
        }
        public async Task ChangeStateAsync(StateValue cacheValue, string id)
        {
             string value = JsonConvert.SerializeObject(cacheValue);
             await _cache.StringSetAsync(id.ToString(), value, TimeSpan.FromMinutes(expiry));
        }

        public async Task<StateValue> GetStateAsync(string id)
        {
            var valueFromCache = await _cache.StringGetAsync(id.ToString(), CommandFlags.None);
            if (!valueFromCache.HasValue)
            {
                StateValue defaultCacheValue = new StateValue(){State = StateType.None, Message = string.Empty};
                await this.ChangeStateAsync(defaultCacheValue, id);
                return defaultCacheValue;
            }
            StateValue cacheValue = JsonConvert.DeserializeObject<StateValue>(valueFromCache);
            return cacheValue;
        }

    }
}