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
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private Core.Configuration.RedisConfig _redisConfig;
        private static readonly ConcurrentDictionary<string, CancellationTokenSource> _prefixes = new ConcurrentDictionary<string, CancellationTokenSource>();
        private static CancellationTokenSource _clearToken = new CancellationTokenSource();

        public MemoryCacheManager(AppSettings appSettings, IMemoryCache memoryCache)
        {
            _redisConfig = appSettings.RedisConfig;
            _memoryCache = memoryCache;
        }
        private MemoryCacheEntryOptions PrepareEntryOptions(NoSQLKey key)
        {
            //set expiration time for the passed cache key
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(150), // key.CacheTime
                SlidingExpiration = TimeSpan.FromSeconds(5)
            }.RegisterPostEvictionCallback(callback: EvictionCallback, state: this);

            //add tokens to clear cache entries
            options.AddExpirationToken(new CancellationChangeToken(_clearToken.Token));
            foreach (var keyPrefix in key.Prefixes.ToList())
            {
                var tokenSource = _prefixes.GetOrAdd(keyPrefix, new CancellationTokenSource());
                options.AddExpirationToken(new CancellationChangeToken(tokenSource.Token));
            }

            return options;
        }

        private void EvictionCallback(object key, object value,  
            EvictionReason reason, object state)
        {
            _memoryCache.Set(key, value); //if near to expire recache            
        }
        public T Get<T>(NoSQLKey key, Func<T> acquire)
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

        public bool IsKeyExist(NoSQLKey key)
        {
            return _memoryCache.TryGetValue(key.Key, out _);
        }

        public bool NearToExpire(NoSQLKey key, TimeSpan time)
        {
            return false; // not have ttl
        }

        public bool RemoveKey(NoSQLKey key)
        {      
            _memoryCache.Remove(key.Key);
            return true;
        }

        public void SetValue<T>(NoSQLKey key, T value, TimeSpan timeout)
        {
            if ((key?.CacheTime.TotalSeconds ?? 0) <= 0 || value == null)
                return;

            _memoryCache.Set(key.Key, value, PrepareEntryOptions(key));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
