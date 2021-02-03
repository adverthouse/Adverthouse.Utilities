using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.Caching
{
    public class MemoryCacheManager : CacheServiceBase, ICacheManager<MemoryCacheManager>
    {
        private readonly IMemoryCache _memoryCache;
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static CancellationTokenSource _clearToken = new CancellationTokenSource();

        public MemoryCacheManager(AppSettings appSettings, IMemoryCache memoryCache):base(appSettings)
        {
            _memoryCache = memoryCache;
        }
        private MemoryCacheEntryOptions PrepareEntryOptions(NoSQLKey key) 
        {
            //set expiration time for the passed cache key
            var options = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.NeverRemove,
                AbsoluteExpirationRelativeToNow = key.CacheTime
            };

            //add tokens to clear cache entries
            options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
            foreach (var keyPrefix in key.Prefixes.ToList())
            {
                var tokenSource = _prefixes.GetOrAdd(keyPrefix, new CancellationTokenSource());
                options.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            return options;
        }

        public bool IsKeyExist(NoSQLKey key) => _memoryCache.TryGetValue(key.Key, out _);
        public Task<bool> IsKeyExistAsync(NoSQLKey key) =>
            Task.Run(() => _memoryCache.TryGetValue(key.Key, out _));

        public bool RemoveKey(NoSQLKey key)
        {
            _memoryCache.Remove(key.Key);
            return true;
        }
        public Task<bool> RemoveKeyAsync(NoSQLKey key)
        {
            return Task.Run(() =>
            {
                _memoryCache.Remove(key.Key);
                return true;
            });
        }

        public void SetValue<T2>(NoSQLKey key, T2 value)
        {
            if ((key?.CacheTime.TotalSeconds ?? 0) <= 0 || value == null)
                return;

            _memoryCache.Set(key.Key, value, PrepareEntryOptions(key));
        }

        public Task SetValueAsync<T2>(NoSQLKey key, T2 value)
        {
            return Task.Run(() =>
            {
                if ((key?.CacheTime.TotalSeconds ?? 0) <= 0 || value == null)
                    return;

                _memoryCache.Set(key.Key, value, PrepareEntryOptions(key));
            });           
        }

        public T2 GetOrCreate<T2>(NoSQLKey key, Func<T2> acquire)
        {
            if ((key?.CacheTime.TotalSeconds ?? 0) <= 0)
                return acquire();

            var result = _memoryCache.GetOrCreate(key.Key, entry => {
                entry.SetOptions(PrepareEntryOptions(key)); 
       
                return acquire();
            });

            if (result == null)
                RemoveKey(key);

            return result;
        }
        public async Task<T2> GetOrCreateAsync<T2>(NoSQLKey key, Func<Task<T2>> acquire)
        {
            if ((key?.CacheTime.TotalSeconds ?? 0) <= 0)
                return await acquire();

            var result = await _memoryCache.GetOrCreateAsync(key.Key, async entry => {
                entry.SetOptions(PrepareEntryOptions(key));

                return await acquire();
            });

            if (result == null)
                RemoveKey(key);

            return result;
        }

        public TTLExtendableCacheObject<T2> GetOrCreate<T2>(NoSQLKey key, Func<TTLExtendableCacheObject<T2>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire)
        {
            var result = GetOrCreate(key, acquire);

            Task.Run(() =>
            {
                var ladResult = GetOrCreate(refreshKey, ladAcquire);

                if (ladResult != result.LastUpdateDate)
                {
                    result = acquire();
                    SetValue(key, result);
                }
            });

            return result;
        }
    }
}
