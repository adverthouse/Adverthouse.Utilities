namespace Adverthouse.Core.Configuration
{
    public class PayPalConfig : IConfig
    {
        public string ClientId { get; set; } = "";        
        public string ClientScreet { get; set; } = ""; 
        public string Mode { get; set; } = "sandbox"; 
    }
}
