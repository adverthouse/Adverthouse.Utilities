namespace Adverthouse.Core.Configuration
{
    public class RedisConfig : IConfig
    {
        public bool Enabled { get; set; } = false;
        public string RedisHost { get; set; } = "127.0.0.1";
        public int RedisPort { get; set; } = 6379;
        public int DefaultCacheTime { get; set; } = 15;
        public bool AbortOnConnectFail { get; set; } = false;
        public bool Ssl { get; set; } = false;
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
