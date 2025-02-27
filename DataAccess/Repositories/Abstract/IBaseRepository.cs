using Microsoft.EntityFrameworkCore.Storage;
using Models.Entities.Abstract;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Abstract
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        Task<int> CreateAsync(T entity);
        Task<int> DeleteAsync(Guid Id);
        Task<int> UpdateAsync(T entity);

        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate);

        Task<int> AddRangeAsync(IEnumerable<T> entities);
        Task<int> DeleteRangeAsync(IEnumerable<T> entities);
        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
