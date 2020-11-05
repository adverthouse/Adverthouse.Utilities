using Adverthouse.Common.NoSQL;
using System;

namespace Adverthouse.Common.Data.Redis
{
    public interface IRedisCacheManager
    {
        bool IsKeyExist(RedisKey key);
        T Get<T>(RedisKey key, Func<T> acquire);



        T GetList<T>(RedisKey key);     
   
        bool NearToExpire(RedisKey key, int minute = 5);
        RedisKey PrepareKeyForDefaultCache(RedisKey cacheKey, params object[] cacheKeyParameters);
        void SetValue<T>(RedisKey key, T value);
        void SetValue<T>(RedisKey key, T value, TimeSpan timeout);
        bool StoreList<T>(RedisKey key, T value, TimeSpan timeout);
    }
}