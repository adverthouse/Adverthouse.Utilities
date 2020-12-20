using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks; 
using RedisKey = Adverthouse.Common.NoSQL.RedisKey;

namespace Adverthouse.Common.Data.Redis
{
    public class RedisCacheManager : IRedisCacheManager
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private int _currentDatabaseID = 0;
        private Core.Configuration.RedisConfig _redisConfig;
        public RedisCacheManager(AppSettings appSettings)
        {
            _redisConfig = appSettings.RedisConfig; 
            _connectionMultiplexer = ConnectionMultiplexer.Connect($"{ _redisConfig.RedisHost}:{ _redisConfig.RedisPort}");
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseID);
        }

        public bool RemoveKey(RedisKey key) {
            return _database.KeyDelete(key.Key);
        }
        public bool IsKeyExist(RedisKey key)
        {
            return _database.KeyExists(key.Key);
        }

        public bool NearToExpire(RedisKey key, TimeSpan time)
        {
            TimeSpan? ttl = _database.KeyTimeToLive(key.Key);
            return ttl.HasValue ? (ttl.Value.Seconds < time.Seconds ? true : false) : false;
        }
    
        public void SetValue<T>(RedisKey key, T value, TimeSpan timeout)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            _database.StringSet(key.Key, jsonData, timeout); 
        }

        public T Get<T>(RedisKey key,Func<T> acquire)
        {
            if (IsKeyExist(key))
            {
                var resultExist = _database.StringGet(key.Key);

                if (key.ReCacheNearToExpires)
                {
                    if (NearToExpire(key,key.ReCacheTime))
                    {
                        Task.Run(() => SetValue(key, acquire(), key.CacheTime));
                    }
                }

                return JsonConvert.DeserializeObject<T>(resultExist);
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
