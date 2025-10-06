using System;
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

        private Func<T> _acquire;
        private Func<DateTime> _lastModifiedDateOfData;

        private int LockCount = 0;
        private readonly object _assignmentLock = new();

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

        private void SeedData()
        {
            var lad = _lastModifiedDateOfData();
            if (lad != LastModifiedDateOfData)
            {
                lock (_assignmentLock)
                {
                    Data = _acquire();
                    LastModifiedDateOfData = lad;
                }
            }

            lock (_assignmentLock)
            {
                LastDateOfRefreshControl = DateTime.Now;
            }
        }

        public T GetFreshData(bool enforce = false)
        {
            if (!enforce)
                if (NextDateOfRefreshControl > DateTime.Now) return Data;

            if (Interlocked.CompareExchange(ref LockCount, 1, 0) == 0)
            {
                try
                {
                    SeedData();
                }
                finally
                {
                    Interlocked.Decrement(ref LockCount);
                }
            }
            
            return Data;
        }

        public RefreshableStaticData(TimeSpan refreshInterval, Func<T> acquire, Func<DateTime> lastModifiedDateOfData, bool willSeedData = true, bool seedDataAsync = false)
        {
            _refreshInterval = refreshInterval;
            _acquire = acquire;
            _lastModifiedDateOfData = lastModifiedDateOfData;
            if (willSeedData)
            {
                if (seedDataAsync)
                    Task.Run(() => SeedData());
                else
                    SeedData();
            }
        }
        public RefreshableStaticData(TimeSpan refreshInterval, Func<T> acquire, Func<DateTime> lastModifiedDateOfData, T data)
        {
            _refreshInterval = refreshInterval;
            _acquire = acquire;
            _lastModifiedDateOfData = lastModifiedDateOfData;

            LastModifiedDateOfData = DateTime.Now;
            LastDateOfRefreshControl = LastModifiedDateOfData;

            Data = data;
        }
    }
}