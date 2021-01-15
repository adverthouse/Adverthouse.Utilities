using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks; 
using NoSQLKey = Adverthouse.Common.NoSQL.NoSQLKey;

namespace Adverthouse.Common.Data.Caching
{
    public class RedisCacheManager : ICacheManager
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

        public bool RemoveKey(NoSQLKey key) {
            return _database.KeyDelete(key.Key);
        }
        public bool IsKeyExist(NoSQLKey key)
        {
            return _database.KeyExists(key.Key);
        }

        public bool NearToExpire(NoSQLKey key, TimeSpan time)
        {
            TimeSpan? ttl = _database.KeyTimeToLive(key.Key);
            return ttl.HasValue ? (ttl.Value.Seconds < time.Seconds ? true : false) : false;
        }
    
        public void SetValue<T>(NoSQLKey key, T value, TimeSpan timeout)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            _database.StringSet(key.Key, jsonData, timeout); 
        }

        public T Get<T>(NoSQLKey key,Func<T> acquire)
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
    }
}
