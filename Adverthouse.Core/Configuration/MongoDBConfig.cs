using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class MongoDBConfig : IConfig
    {
        public string DBName { get; set; } = "";
        public bool HasCredential { get; set; } = false;
        public string Host { get; set; } = "127.0.0.1"; 
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
