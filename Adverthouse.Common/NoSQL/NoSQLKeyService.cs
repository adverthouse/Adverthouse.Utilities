using Adverthouse.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Common.NoSQL
{
    public class NoSQLKeyService : INoSQLKeyService
    {
        private readonly AppSettings _appSettings;
        public NoSQLKeyService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        private object CreateCacheKeyParameters(object parameter)
        {
            return parameter switch
            {
                null => "null",
                decimal param => param.ToString(CultureInfo.InvariantCulture),
                _ => parameter
            };
        }

        public string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
        {
            return prefixParameters?.Any() ?? false
                ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
                : prefix;
        }
        public NoSQLKey PrepareKey(NoSQLKey noSqlKey, params object[] cacheKeyParameters)
        {
            return noSqlKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
        }

        public NoSQLKey PrepareKeyForDefaultCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters)
        {
            var key = noSqlKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

            key.CacheTime = TimeSpan.FromMinutes(_appSettings.RedisConfig.DefaultCacheTime);

            return key;
        }

        public NoSQLKey PrepareKeyForShortTermCache(NoSQLKey noSqlKey, params object[] cacheKeyParameters)
        {
            var key = noSqlKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

            key.CacheTime = TimeSpan.FromMinutes(_appSettings.RedisConfig.DefaultCacheTime);

            return key;
        }
    }
}
