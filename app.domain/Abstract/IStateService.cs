using app.common.Utils.Enums;

namespace app.domain.Abstract
{
    public interface IStateService
    {
        public Task ChangeStateAsync(StateType type, string id);
        public Task<StateType> GetStateAsync(string id);
    }
}