using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.Caching
{
    /// <summary>
    /// This class creates object in resfesable manner and update it from original source by refresh interval and lastmodified date
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RefreshableStaticData<T>
    {
        private TimeSpan _refreshInterval;

        private int LockCount = 0;

        public T Data { private set; get; }

        public DateTime LastModifiedDateOfData { get; private set; } = DateTime.MinValue;

        public DateTime LastDateOfRefresh { get; private set; } = DateTime.MinValue;

        public DateTime NextDateOfRefresh
        {
            get
            {
                return this.LastDateOfRefresh.Add(_refreshInterval);
            }
        }

        private void SetData(Func<T> acquire, Func<DateTime> lastModifiedDateOfData)
        {
            var lad = lastModifiedDateOfData();
            if (lad != LastModifiedDateOfData)
            {
                Data = acquire();
                LastModifiedDateOfData = lad;
            }
            LastDateOfRefresh = DateTime.Now;
        }

        public T GetFreshData(Func<T> acquire, Func<DateTime> lastModifiedDateOfData)
        {
            if (NextDateOfRefresh > DateTime.Now) return Data;

            if (Interlocked.CompareExchange(ref LockCount, 1, 0) == 0)
            {
                try
                {
                    SetData(acquire, lastModifiedDateOfData);
                    return Data;
                }
                finally
                {
                    Interlocked.Decrement(ref LockCount);
                }
            }
            else return Data;
        }

        public RefreshableStaticData(TimeSpan refreshInterval)
        {
            _refreshInterval = refreshInterval;
        }
    }
}