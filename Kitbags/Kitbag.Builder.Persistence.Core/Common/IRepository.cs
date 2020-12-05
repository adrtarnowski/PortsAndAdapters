using System.Collections.Generic;
using System.Threading.Tasks;
using Kitbag.Builder.Core.Domain;

namespace Kitbag.Builder.Persistence.Core.Common
{
    public interface IRepository<T, TId> where T : class, IAggregateRoot<TId> where TId : Id
    {
        Task<bool> ExistsAsync(TId id);
        void Add(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        Task<T?> FindByIdAsync(TId id);
    }
}