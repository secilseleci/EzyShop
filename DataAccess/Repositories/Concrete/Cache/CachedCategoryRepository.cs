using Core.Utilities.Results;
using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache
{
    public class CachedCategoryRepository(
    CategoryRepository _decorated,
    IMemoryCache _cache) : ICategoryRepository
    {
        private static readonly List<string> CachedKeys = [];

        
        public async Task<int> CreateAsync(Category entity)
        {
            int result = await _decorated.CreateAsync(entity);
            RemoveAllCachedItems(result);
            return result;
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var result = await _decorated.DeleteAsync(id);
            RemoveAllCachedItems(result);
            return result;
        }
        public async Task<int> UpdateAsync(Category entity)
        {
            var result = await _decorated.UpdateAsync(entity);
            RemoveAllCachedItems(result);
            return result;
        }
       

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            string key = $"category-{id}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetByIdAsync(id);
            });
        }

        public async Task<IEnumerable<Category>?> GetAllAsync(Expression<Func<Category, bool>> predicate)
        {
            string key = $"all-categories-{predicate}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetAllAsync(predicate);
            });
        }
        public async Task<Category?> GetCategoryWithProductsAsync(Guid productId)
        {
            string key = $"with-product-{productId}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetCategoryWithProductsAsync(productId);
            });
        }

        public async Task<int> GetMaxDisplayOrderAsync()
        {
            var result = await _decorated.GetMaxDisplayOrderAsync();
            RemoveAllCachedItems(result);
            return result;
        }
        public async Task<int> AddRangeAsync(IEnumerable<Category> entities)
        {
            var result = await _decorated.AddRangeAsync(entities);
            RemoveAllCachedItems(result);
            return result;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Category> entities)
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

            CachedKeys.Clear();
        }

      


        #endregion
    }
}
