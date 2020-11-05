using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.NoSQL
{
    public class RedisKey : BaseKey
    {
        public RedisKey(string key, params string[] prefixes) : base(key, prefixes)
        {
        }
    }
}
