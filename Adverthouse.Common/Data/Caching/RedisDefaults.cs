using Adverthouse.Common.Interfaces;
using Adverthouse.Common.NoSQL;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data.Caching
{
    public static class RedisDefaults<TEntity> where TEntity : IEntity
    {
        public static string TypeName => typeof(TEntity).Name.ToLowerInvariant();
        public static NoSQLKey ByIDCacheKey => new NoSQLKey($"Redis.{TypeName}.byid.{{0}}", ByIdPrefix, Prefix);
        public static string Prefix => $"Redis.{TypeName}.";
        public static string ByIdPrefix => $"Redis.{TypeName}.byid.";
        public static string ByIdsPrefix => $"Redis.{TypeName}.byids.";
        public static string AllPrefix => $"Redis.{TypeName}.all.";
    }
}
