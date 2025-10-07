using Adverthouse.Common.NoSQL;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.Caching
{
    public interface ICacheManager<T> where T : class, ICacheManager<T>
    {
        string PrepareKeyPrefix(string prefix, params object[] prefixParameters);
        NoSQLKey PrepareKeyForDefaultCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters);
        bool RemoveKey(NoSQLKey key);
        Task<bool> RemoveKeyAsync(NoSQLKey key);
        bool IsKeyExist(NoSQLKey key);
        Task<bool> IsKeyExistAsync(NoSQLKey key);
        T2 Get<T2>(NoSQLKey key);
        T2 GetOrCreate<T2>(NoSQLKey key, Func<T2> acquire);
        Task<T2> GetOrCreateAsync<T2>(NoSQLKey key, Func<Task<T2>> acquire);
        void SetValue<T2>(NoSQLKey key, T2 value);
        Task SetValueAsync<T2>(NoSQLKey key, T2 value);
        TTLExtendableCacheObject<T2> GetOrCreate<T2>(NoSQLKey key,  Func<TTLExtendableCacheObject<T2>> acquire, NoSQLKey refreshKey, Func<DateTime> ladAcquire);        
    }
}