using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Infrastructure
{
    public class BaseSingleton
    {
        public static IDictionary<Type, object> AllSingletons { get; }
        static BaseSingleton()
        {
            AllSingletons = new Dictionary<Type, object>();
        }
    }
}
