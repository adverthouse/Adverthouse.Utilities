namespace Adverthouse.Core.Configuration
{
    public class reCAPTCHAConfig : IConfig
    {
        public string SecretKey { get; set; } = "";
        public string SiteKey { get; set; } = "";
    }
}
