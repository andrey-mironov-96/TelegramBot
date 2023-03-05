using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.common.Utils.Abstract
{
    public interface IBaseBusinessService<T>
        where T:ABaseDTOEntity
    {
        Task<T> SaveAsync(T item);
        Task<T> GetByIdAsync(long id);
        Task<bool> DeleteAsync(long id);
        Task<PageableData<T>> GetPage(PageableData<T> data);
    }
}