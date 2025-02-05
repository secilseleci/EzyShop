using DataAccess.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Models.Entities.Concrete;
using System.Linq.Expressions;

namespace DataAccess.Repositories.Concrete.Cache
{
    internal class CachedSellerApplicationRepositoy(
    SellerApplicationRepository _decorated,
    IMemoryCache _cache) : ISellerApplicationRepository
    {
        private static readonly List<string> CachedKeys = [];

        public async Task<int> CreateAsync(SellerApplication entity)
        {
            int result = await _decorated.CreateAsync(entity);
            RemoveAllCachedItems(result);
            return result;
        }
        public async Task<int> UpdateAsync(SellerApplication entity)
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

        public async Task<IEnumerable<SellerApplication>?> GetAllAsync(Expression<Func<SellerApplication, bool>> predicate)
        {
            string key = $"all-applications-{predicate}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetAllAsync(predicate);
            });
        }
        public async Task<SellerApplication?> GetByIdAsync(Guid id)
        {
            string key = $"application-{id}";
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                CachedKeys.Add(key);
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
                return await _decorated.GetByIdAsync(id);
            });
        }

        public async Task<int> AddRangeAsync(IEnumerable<SellerApplication> entities)
        {
            var result = await _decorated.AddRangeAsync(entities);
            RemoveAllCachedItems(result);
            return result;
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<SellerApplication> entities)
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
