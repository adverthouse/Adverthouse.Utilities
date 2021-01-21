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
        private bool _disposed;

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
            

        public T GetOrCreate<T>(NoSQLKey key, Func<T> acquire)
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
        public TTLExtendableCacheObject<T> GetOrCreate<T>(NoSQLKey key, Func<TTLExtendableCacheObject<T>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire)
        {
            var result = GetOrCreate(key, acquire);

            Task.Run(() =>
            {
                _memoryCache.GetOrCreate(refreshKey.Key, entry =>
                {
                    entry.SetOptions(PrepareEntryOptions(refreshKey));

                    var lad = ladAcquire();
                    if (lad == result.LastUpdateDate) SetValue(key, result);

                    return lad;
                });
            });

            return result;
        }


         
        public bool IsKeyExist(NoSQLKey key)
        {
            return _memoryCache.TryGetValue(key.Key, out _);
        }
 
        public bool RemoveKey(NoSQLKey key)
        {      
            _memoryCache.Remove(key.Key);
            return true;
        }

        public void SetValue<T>(NoSQLKey key, T value)
        {
            if ((key?.CacheTime.TotalSeconds ?? 0) <= 0 || value == null)
                return;

            _memoryCache.Set(key.Key, value, PrepareEntryOptions(key));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

         protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _memoryCache.Dispose();
            }

            _disposed = true;
        }

    }
}
