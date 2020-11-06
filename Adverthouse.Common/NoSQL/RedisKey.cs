using Adverthouse.Core.Configuration;
using Adverthouse.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adverthouse.Common.NoSQL
{
    public class RedisKey
    {        
        public string Key { get; protected set; }
        public List<string> Prefixes { get; protected set; } = new List<string>();
        public bool ReCacheNearToExpires { get; protected set; } = Singleton<AppSettings>.Instance.RedisConfig.EnableReCache;
        public TimeSpan ReCacheTime { get; protected set; } = TimeSpan.FromMinutes(Singleton<AppSettings>.Instance.RedisConfig.DefaultReCacheTime);
        public TimeSpan CacheTime { get; set; } = TimeSpan.FromMinutes(Singleton<AppSettings>.Instance.RedisConfig.DefaultCacheTime);

        public RedisKey(string key, params string[] prefixes)
        {
            Key = key;
            Prefixes.AddRange(prefixes.Where(prefix => !string.IsNullOrEmpty(prefix)));
        }

        public virtual RedisKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
        {
            var cacheKey = new RedisKey(Key, Prefixes.ToArray());

            if (!keyObjects.Any())
                return cacheKey;

            cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

            for (var i = 0; i < cacheKey.Prefixes.Count; i++)
                cacheKey.Prefixes[i] = string.Format(cacheKey.Prefixes[i], keyObjects.Select(createCacheKeyParameters).ToArray());

            return cacheKey;
        }
    }
}
