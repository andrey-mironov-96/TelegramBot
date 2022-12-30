using app.common.DTO;
using app.common.Utils.Enums;

namespace app.domain.Abstract
{
    public interface IStateService
    {
        public Task ChangeStateAsync(StateValue value, string id);
        public Task<StateValue> GetStateAsync(string id);
    }
}