using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class ElasticSearchConfig: IConfig
    {
        public bool Enabled { get; set; } = false;

        public string HostAddresses { get; set; } = "127.0.0.1";
        public bool EnableDebugMode { get; set; } = false;
        public bool EnableAuthentication { get; set; } = false;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
