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
        private object _assignmentLock = new object();

        public T Data { get; private set; }

        public DateTime LastModifiedDateOfData { get; private set; } = DateTime.MinValue;

        public DateTime LastDateOfRefreshControl { get; private set; } = DateTime.MinValue;

        public DateTime NextDateOfRefreshControl
        {
            get
            {
                return this.LastDateOfRefreshControl.Add(_refreshInterval);
            }
        }

        public T GetFreshData(Func<T> acquire, Func<DateTime> lastModifiedDateOfData)
        {
            if (NextDateOfRefreshControl > DateTime.Now) return Data;

            if (Interlocked.CompareExchange(ref LockCount, 1, 0) == 0)
            {
                try
                {
                    var lad = lastModifiedDateOfData();
                    if (lad != LastModifiedDateOfData)
                    {
                        var result = acquire();

                        lock (_assignmentLock)
                        {
                            Data = result;
                            LastModifiedDateOfData = lad;
                        }
                    }

                    lock (_assignmentLock)
                    {
                        LastDateOfRefreshControl = DateTime.Now;
                    }

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