//using DataAccess.Repositories.Abstract;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.Extensions.Caching.Memory;
//using Models.Entities.Concrete;
//using System.Linq.Expressions;

//namespace DataAccess.Repositories.Concrete.Cache
//{

//    public class CachedShopRepository(
//       ShopRepository _decorated,
//       IMemoryCache _cache) : IShopRepository
//    {
//        private static readonly List<string> CachedKeys = [];
//        public async Task<IDbContextTransaction> BeginTransactionAsync()
//        {
//            return await _decorated.BeginTransactionAsync();
//        }
//        public async Task<int> CreateAsync(Shop entity)
//        {
//            int result = await _decorated.CreateAsync(entity);
//            RemoveAllCachedItems(result);
//            return result;
//        }
//        public async Task<int> UpdateRangeAsync(IEnumerable<Shop> entities)
//        {
//            var result = await _decorated.UpdateRangeAsync(entities);
//            RemoveAllCachedItems(result);
//            return result;
//        }
//        public async Task<int> UpdateAsync(Shop entity)
//        {
//            var result = await _decorated.UpdateAsync(entity);
//            RemoveAllCachedItems(result);
//            return result;

//        }

//        public async Task<int> DeleteAsync(Guid id)
//        {
//            var result = await _decorated.DeleteAsync(id);
//            RemoveAllCachedItems(result);
//            return result;
//        }


//        public async Task<Shop?> GetBySellerIdAsync(Guid sellerId)
//        {
//            string key = $"shop-{sellerId}";
//            return await _cache.GetOrCreateAsync(key, async entry =>
//            {
//                CachedKeys.Add(key);
//                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
//                return await _decorated.GetBySellerIdAsync(sellerId);
//            });
//        }
//        public async Task<Shop?> GetByIdAsync(Guid id)
//        {
//            string key = $"shop-{id}";
//            return await _cache.GetOrCreateAsync(key, async entry =>
//            {
//                CachedKeys.Add(key);
//                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
//                return await _decorated.GetByIdAsync(id);
//            });
//        }


//        public async Task<IEnumerable<Shop>?> GetAllAsync(Expression<Func<Shop, bool>> predicate)
//        {
//            string key = $"all-shops-{predicate}";
//            return await _cache.GetOrCreateAsync(key, async entry =>
//            {
//                CachedKeys.Add(key);
//                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(3));
//                return await _decorated.GetAllAsync(predicate);
//            });
//        }
//        #region Helper Methods
//        private void RemoveAllCachedItems(int result)
//        {
//            if (result > 0)
//            {
//                foreach (var key in CachedKeys)
//                {
//                    _cache.Remove(key);
//                }
//            }

//            CachedKeys.Clear();
//        }

//        public async Task<int> AddRangeAsync(IEnumerable<Shop> entities)
//        {
//            var result = await _decorated.AddRangeAsync(entities);
//            RemoveAllCachedItems(result);
//            return result;
//        }

//        public async Task<int> DeleteRangeAsync(IEnumerable<Shop> entities)
//        {
//            var result = await _decorated.DeleteRangeAsync(entities);
//            RemoveAllCachedItems(result);
//            return result;
//        }


//        #endregion
//    }

//}
