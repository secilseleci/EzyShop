using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

public class CachedProductRepository : IProductRepository
{
    private readonly ProductRepository _decorated;
    private readonly IMemoryCache _cache;
    private readonly List<string> CachedKeys = new();


    public CachedProductRepository(
         ProductRepository decorated,
         IMemoryCache cache         )
    {
        _decorated = decorated;
        _cache = cache;
      
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        string key = $"product-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetByIdAsync(id);
        });
    }
    public async Task<IEnumerable<Product>?> GetFilteredProductsAsync(string? name, string? category, string? color, decimal? minPrice, decimal? maxPrice)
    {
        string key = $"ProductFilterCacheKey-{name}-{category}-{color}-{minPrice}-{maxPrice}";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetFilteredProductsAsync(name, category, color, minPrice, maxPrice);
        });
    }
    public async Task<Product?> GetProductWithCategoryAsync(Guid id)
    {
        string key = $"product-with-category-{id}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetProductWithCategoryAsync(id);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllAsync(predicate);
        });
    }

    public async Task<IEnumerable<Product>?> GetAllProductsWithCategoryAsync(Expression<Func<Product, bool>> predicate)
    {
        string key = $"all-products-with-category-{predicate}";
        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            CachedKeys.Add(key);
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
            return await _decorated.GetAllProductsWithCategoryAsync(predicate);
        });
    }

    public async Task<int> CreateAsync(Product entity)
    {
        int result = await _decorated.CreateAsync(entity);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> AddRangeAsync(IEnumerable<Product> entities)
    {
        var result = await _decorated.AddRangeAsync(entities);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> UpdateAsync(Product entity)
    {
        var result = await _decorated.UpdateAsync(entity);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var result = await _decorated.DeleteAsync(id);
        RemoveAllCachedItems(result);
        return result;
    }

    public async Task<int> DeleteRangeAsync(IEnumerable<Product> entities)
    {
        var result = await _decorated.DeleteRangeAsync(entities);
        RemoveAllCachedItems(result);
        return result;
    }

   

    #region Helper Methods
    private void RemoveAllCachedItems(int result)
    {
        if (result > 0)
        {
            foreach (var key in CachedKeys)
            {
                _cache.Remove(key);
            }
        }
        _cache.Remove("all-products");
        _cache.Remove("all-products-with-category");
        _cache.Remove("ProductFilterCacheKey");
        CachedKeys.Clear();
    }
   
    #endregion
}
