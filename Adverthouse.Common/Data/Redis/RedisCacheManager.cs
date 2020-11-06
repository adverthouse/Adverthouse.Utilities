using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

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

        public bool NearToExpire(RedisKey key, TimeSpan time)
        {
            return _rc.GetTimeToLive(key.Key) < time ? true : false;
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

                if (key.ReCacheNearToExpires)
                {
                    if (NearToExpire(key,key.ReCacheTime))
                    {
                        Task.Run(() => SetValue(key, acquire(), key.CacheTime));
                    }
                }

                if (resultExist != null && !resultExist.Equals(default(T)))
                    return resultExist;
            }

            var result = acquire();

            if (key.CacheTime.TotalMinutes > 0)
                SetValue(key, result, key.CacheTime);

            return result;
        } 

        protected virtual object CreateCacheKeyParameters(object parameter)
        {
            return parameter switch
            {
                null => "null",
                decimal param => param.ToString(CultureInfo.InvariantCulture),
                _ => parameter
            };
        }
        public RedisKey PrepareKeyForDefaultCache(RedisKey cacheKey, params object[] cacheKeyParameters)
        {
            var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

            key.CacheTime = TimeSpan.FromMinutes(_redisConfig.DefaultCacheTime);

            return key;
        }
        public void Dispose()
        {
           // Dispose(true);
            GC.SuppressFinalize(this);
        } 
    }
}
