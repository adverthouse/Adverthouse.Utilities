using Adverthouse.Common.NoSQL;
using System;

namespace Adverthouse.Common.Data.Caching
{
    public interface ICacheManager<T> where T :class, ICacheManager<T>
    {
        string PrepareKeyPrefix(string prefix, params object[] prefixParameters);
        NoSQLKey PrepareKeyForDefaultCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters);

        bool RemoveKey(NoSQLKey key);
        bool IsKeyExist(NoSQLKey key);
        T Get<T>(NoSQLKey key, Func<T> acquire);
        void SetValue<T>(NoSQLKey key, T value, TimeSpan timeout);
        bool NearToExpire(NoSQLKey key, TimeSpan time);    }
}