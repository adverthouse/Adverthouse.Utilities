using Adverthouse.Common.NoSQL;
using System;

namespace Adverthouse.Common.Data.Redis
{
    public interface IRedisCacheManager
    {
        T GetList<T>(RedisKey key);
        T GetValue<T>(RedisKey key);
        bool IsKeyExist(RedisKey key);
        bool NearToExpire(RedisKey key, int minute = 5);
        void SetValue<T>(RedisKey key, T value);
        void SetValue<T>(RedisKey key, T value, TimeSpan timeout);
        bool StoreList<T>(RedisKey key, T value, TimeSpan timeout);
    }
}