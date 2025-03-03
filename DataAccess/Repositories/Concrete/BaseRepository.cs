using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models.Entities.Abstract;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete;

public class BaseRepository<T> : IBaseRepository<T> where T : class,IBaseEntity, new()
{
    protected readonly ApplicationDbContext _dataContext;
    private readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _dataContext = context;
        _dbSet = _dataContext.Set<T>();
    }
    public async Task<int> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        var result = await _dataContext.SaveChangesAsync();

        return result;
    }
    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dataContext.Remove(entity);
            return await _dataContext.SaveChangesAsync();
        }
        return -1;
    }
    public async Task<int> UpdateAsync(T entity)
    {
        _dataContext.Update(entity);
        return await _dataContext.SaveChangesAsync();
    }

    public async Task<int> AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return await _dataContext.SaveChangesAsync();
    }
    public async Task<int> DeleteRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return await _dataContext.SaveChangesAsync();
    }
    public async Task<int> UpdateRangeAsync(IEnumerable<T> entities)
    {
        _dbSet.UpdateRange(entities);
        return await _dataContext.SaveChangesAsync();
    }
   
    public async Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate)
     => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
   => await _dbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);

   
    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _dataContext.Database.BeginTransactionAsync();
    }

}