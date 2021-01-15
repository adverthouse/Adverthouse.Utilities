namespace Adverthouse.Common.NoSQL
{
    public interface INoSQLKeyService
    {
        NoSQLKey PrepareKey(NoSQLKey noSqlKey, params object[] cacheKeyParameters);
        NoSQLKey PrepareKeyForDefaultCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters);
        NoSQLKey PrepareKeyForShortTermCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters);
        string PrepareKeyPrefix(string prefix, params object[] prefixParameters);
    }
}