using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache
{
    public class CachedProductRepository(
    ProductRepository _decorated,
    IMemoryCache _cache) : IProductRepository
    {
        private static readonly List<string> CachedKeys = [];

        public async Task<int> CreateAsync(Product entity)
        {
            int result = await _decorated.CreateAsync(entity);
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


        public async Task<IEnumerable<Product>?> GetAllAsync(Expression<Func<Product, bool>> predicate)
        {
            string key = $"all-products-{predicate}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetAllAsync(predicate);
            });
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
