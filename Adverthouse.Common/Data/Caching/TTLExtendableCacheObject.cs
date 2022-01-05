using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.Caching
{
    /// <summary>
    /// TTL value extendable cache object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TTLExtendableCacheObject<T>
    {
        /// <summary>
        /// Max. Last update date of CacheObject
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// Cache object
        /// </summary>
        public T CacheObject { get; set; } 

        public TTLExtendableCacheObject(DateTime lastUpdateDate,T cacheObject)
        {
            LastUpdateDate = lastUpdateDate;
            CacheObject = cacheObject;
        }
    }
}
