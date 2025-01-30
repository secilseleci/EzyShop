using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
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

    public async Task<IEnumerable<T>?> GetAllAsync(Expression<Func<T, bool>> predicate)
     => await _dbSet.AsNoTracking().Where(predicate).ToListAsync();

    public async Task<T?> GetByIdAsync(Guid id)
   => await _dbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id);

}