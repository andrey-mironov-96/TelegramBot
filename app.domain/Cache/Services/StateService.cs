using app.common.Utils.Enums;
using app.domain.Abstract;
using Microsoft.Extensions.Logging;
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
        public async Task ChangeStateAsync(StateType type, string id)
        {
            await _cache.StringSetAsync(id.ToString(), type.ToString(), TimeSpan.FromMinutes(expiry));
        }

        public async Task<StateType> GetStateAsync(string id)
        {
            var valueFromCache = await _cache.StringGetAsync(id.ToString(), CommandFlags.None);
            if (!valueFromCache.HasValue)
            {
                await this.ChangeStateAsync(StateType.None, id);
                return StateType.None;
            }
            StateType type = (StateType)Enum.Parse(typeof(StateType), valueFromCache.ToString());
            return type;
        }
    }
}