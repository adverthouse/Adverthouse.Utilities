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

        public bool RemoveKey(NoSQLKey key) => _database.KeyDelete(key.Key);
        public Task<bool> RemoveKeyAsync(NoSQLKey key) => _database.KeyDeleteAsync(key.Key);

        public bool IsKeyExist(NoSQLKey key) => _database.KeyExists(key.Key);
        public Task<bool> IsKeyExistAsync(NoSQLKey key) => _database.KeyExistsAsync(key.Key);

        public void SetValue<T2>(NoSQLKey key, T2 value)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            _database.StringSet(key.Key, jsonData, key.CacheTime); 
        }
        public async Task SetValueAsync<T2>(NoSQLKey key, T2 value)
        {
            string jsonData = JsonConvert.SerializeObject(value);
            await _database.StringSetAsync(key.Key, jsonData, key.CacheTime);
        }

        public T2 GetOrCreate<T2>(NoSQLKey key,Func<T2> acquire)
        {
            RedisValue resultExist = _database.StringGet(key.Key);

            if (resultExist.HasValue)
                return JsonConvert.DeserializeObject<T2>(resultExist);
 
            var result = acquire();

            if (key.CacheTime.TotalMinutes > 0)
                SetValue(key, result);

            return result;
        }

        public async Task<T2> GetOrCreateAsync<T2>(NoSQLKey key, Func<Task<T2>> acquire)
        {
            RedisValue resultExist = await _database.StringGetAsync(key.Key);

            if (resultExist.HasValue)
                return JsonConvert.DeserializeObject<T2>(resultExist);

            var result = await acquire();

            if (key.CacheTime.TotalMinutes > 0)
                await SetValueAsync(key, result);

            return result;
        }

        public TTLExtendableCacheObject<T2> GetOrCreate<T2>(NoSQLKey key, Func<TTLExtendableCacheObject<T2>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire)
        {
            var result = GetOrCreate(key, acquire);

            Task.Run(() =>
            {
                if (!IsKeyExist(refreshKey))
                {
                    var lad = ladAcquire();

                    if (refreshKey.CacheTime.TotalMinutes > 0)
                        SetValue(refreshKey, lad);

                    if (lad == result.LastUpdateDate)
                        _database.KeyExpire(key.Key, key.CacheTime);
                }
            });

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
