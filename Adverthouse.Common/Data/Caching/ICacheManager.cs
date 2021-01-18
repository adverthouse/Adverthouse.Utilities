using Adverthouse.Common.NoSQL;
using System;

namespace Adverthouse.Common.Data.Caching
{
    public interface ICacheManager<T> where T : class, ICacheManager<T>
    {
        string PrepareKeyPrefix(string prefix, params object[] prefixParameters);
        NoSQLKey PrepareKeyForDefaultCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters);

        bool RemoveKey(NoSQLKey key);
        bool IsKeyExist(NoSQLKey key);
        T GetOrCreate<T>(NoSQLKey key, Func<T> acquire);
        void SetValue<T>(NoSQLKey key, T value);                
        TTLExtendableCacheObject<T> GetOrCreate<T>(NoSQLKey key, Func<TTLExtendableCacheObject<T>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire);
    }
}