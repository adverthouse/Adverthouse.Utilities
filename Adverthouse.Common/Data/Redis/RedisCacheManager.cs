using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using ServiceStack.Redis;
using System;

namespace Adverthouse.Common.Data.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private readonly RedisEndpoint _redisEndpoint;
        private readonly IRedisClient _rc;

        public RedisCacheManager(AppSettings appSettings)
        {
            var host = appSettings.RedisConfig.RedisHost;
            var port = Convert.ToInt32(appSettings.RedisConfig.RedisPort);
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
        public T GetValue<T>(RedisKey key)
        {
            return _rc.Get<T>(key.Key);
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
    }
}
