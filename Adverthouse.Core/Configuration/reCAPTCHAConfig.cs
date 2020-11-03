using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class reCAPTCHAConfig : IConfig
    {
        public string SecretKey { get; set; } = "";
        public string SiteKey { get; set; } = "";
    }
}
