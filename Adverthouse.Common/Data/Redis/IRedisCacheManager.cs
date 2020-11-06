using Adverthouse.Common.NoSQL;
using System;

namespace Adverthouse.Common.Data.Redis
{
    public interface IRedisCacheManager : IDisposable
    {
        bool IsKeyExist(RedisKey key);
        T Get<T>(RedisKey key, Func<T> acquire);
        void SetValue<T>(RedisKey key, T value, TimeSpan timeout);
        bool NearToExpire(RedisKey key, TimeSpan time);
        RedisKey PrepareKeyForDefaultCache(RedisKey cacheKey, params object[] cacheKeyParameters);
    }
}