using app.common.DTO;
using app.common.Utils.Enums;

namespace app.domain.Abstract
{
    public interface IStateService
    {
        Task<bool> SetKeyAsync<T>(T value, string key);
        Task<T> GetKeyAsync<T>(string key);
        public Task<bool> RemoveKey(string key);

        public string facultyKey {get;}
    }
}