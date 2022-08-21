using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Library.Common.Interfaces
{
    public interface IRepo<T>
    {
        Task<int> AddAsync(T entity);

        Task<int> AddRangeAsync(IEnumerable<T> entities);

        Task<T> GetOneAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<int> DeleteAsync(Guid id);

        Task<int> DeleteRangeAsync(IEnumerable<Guid> ids);

        Task<int> SaveAsync(T entity);
    }
}
