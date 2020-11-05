using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Adverthouse.Common.Data.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly RedisEndpoint _redisEndpoint;
        private readonly IRedisClient _rc;
        private Core.Configuration.RedisConfig _redisConfig;
        public RedisCacheManager(AppSettings appSettings)
        {
            _redisConfig = appSettings.RedisConfig;
          
            var host = _redisConfig.RedisHost;
            var port = Convert.ToInt32(_redisConfig.RedisPort);
            _redisEndpoint = new RedisEndpoint()
            {
                Host = host,
                Port = port,
                ConnectTimeout = 0
            };
            _rc = new RedisClient(_redisEndpoint);
        }
        public bool IsKeyExist(RedisKey key)
        {
            return _rc.ContainsKey(key.Key) ? true : false;
        }

        public bool NearToExpire(RedisKey key, int minute = 5)
        {
            return _rc.GetTimeToLive(key.Key) < TimeSpan.FromMinutes(5) ? true : false;
        }

        public void SetValue<T>(RedisKey key, T value)
        {
            _rc.Set(key.Key, value);
        }
        public void SetValue<T>(RedisKey key, T value, TimeSpan timeout)
        {
            _rc.Set(key.Key, value, timeout);
        }

        public T Get<T>(RedisKey key,Func<T> acquire)
        {
            if (IsKeyExist(key))
            {
                var resultExist = _rc.Get<T>(key.Key);

                if (resultExist != null && !resultExist.Equals(default(T)))
                    return resultExist;
            }

            var result = acquire();

            if (key.CacheTime > 0)
                SetValue(key, result);

            return result;
        }
        public bool StoreList<T>(RedisKey key, T value, TimeSpan timeout)
        {
            try
            {
                _rc.Set<T>(key.Key, value, timeout);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public T GetList<T>(RedisKey key)
        {
            T result;
            var wrapper = _rc.As<T>();
            result = wrapper.GetValue(key.Key);
            return result;
        }

        protected virtual object CreateCacheKeyParameters(object parameter)
        {
            return parameter switch
            {
                null => "null",
                //IEnumerable<int> ids => CreateIdsHash(ids),
                //IEnumerable<IEntity> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
                //BaseEntity entity => entity.Id,
                decimal param => param.ToString(CultureInfo.InvariantCulture),
                _ => parameter
            };
        }
        public RedisKey PrepareKeyForDefaultCache(RedisKey cacheKey, params object[] cacheKeyParameters)
        {
            var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

            key.CacheTime = _redisConfig.DefaultCacheTime;

            return key;
        }
    }
}
