using Adverthouse.Core.Configuration;
using Adverthouse.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adverthouse.Common.NoSQL
{
    public class NoSQLKey
    {        
        public string Key { get; protected set; }
        public List<string> Prefixes { get; protected set; } = new List<string>();       
        
        public TimeSpan CacheTime { get; set; } = TimeSpan.FromMinutes(Singleton<AppSettings>.Instance.RedisConfig.DefaultCacheTime);

        public NoSQLKey(string key, params string[] prefixes)
        {
            Key = key;
            Prefixes.AddRange(prefixes.Where(prefix => !string.IsNullOrEmpty(prefix)));
        }

        public virtual NoSQLKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
        {
            var cacheKey = new NoSQLKey(Key, Prefixes.ToArray());

            if (!keyObjects.Any())
                return cacheKey;

            cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

            for (var i = 0; i < cacheKey.Prefixes.Count; i++)
                cacheKey.Prefixes[i] = string.Format(cacheKey.Prefixes[i], keyObjects.Select(createCacheKeyParameters).ToArray());

            return cacheKey;
        }
    }
}
