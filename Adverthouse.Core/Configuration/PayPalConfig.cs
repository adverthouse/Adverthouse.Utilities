namespace Adverthouse.Core.Configuration
{
    public class PayPalConfig : IConfig
    {
        public string ClientId { get; set; } = "";        
        public string ClientSecret { get; set; } = ""; 
        public string Mode { get; set; } = "sandbox"; 
    }
}
