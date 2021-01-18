using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks; 

namespace Adverthouse.Common.Data.Caching
{
    public class RedisCacheManager : CacheServiceBase, ICacheManager<RedisCacheManager>
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private int _currentDatabaseID = 0;
        private Core.Configuration.RedisConfig _redisConfig;
        public RedisCacheManager(AppSettings appSettings):base(appSettings)
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
    
        public void SetValue<T>(NoSQLKey key, T value)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            _database.StringSet(key.Key, jsonData, key.CacheTime); 
        }

        public T GetOrCreate<T>(NoSQLKey key,Func<T> acquire)
        {
            if (IsKeyExist(key))
            {
                var resultExist = _database.StringGet(key.Key);                

                return JsonConvert.DeserializeObject<T>(resultExist);
            }

            var result = acquire();

            if (key.CacheTime.TotalMinutes > 0)
                SetValue(key, result);

            return result;
        }

        public TTLExtendableCacheObject<T> GetOrCreate<T>(NoSQLKey key, Func<TTLExtendableCacheObject<T>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire)
        {
            var result = GetOrCreate(key, acquire);

            if (!IsKeyExist(refreshKey))
            {
                var lad = ladAcquire();

                if (key.CacheTime.TotalMinutes > 0)
                    SetValue(refreshKey, lad);

                if (lad == result.LastUpdateDate) _database.KeyExpire(key.Key, key.CacheTime);
            }

            return result;
        }

        /// <summary>
        ///  if lastupdatedate didn't change, this method extends targetKey ttl.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="refreshKey"></param>
        /// <param name="ladAcquire"></param>
        /// <param name="targetKey"></param>
        /// <param name="eco"></param>
        public void ExtendTTL<T>(NoSQLKey refreshKey, Func<DateTime> ladAcquire, NoSQLKey targetKey, TTLExtendableCacheObject<T> eco)
        {
            if (!IsKeyExist(refreshKey))
            {
                var lad = ladAcquire();
                SetValue(refreshKey, lad);

                if (lad == eco.LastUpdateDate) _database.KeyExpire(targetKey.Key, targetKey.CacheTime);  
            }
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
