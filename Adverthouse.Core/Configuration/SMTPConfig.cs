using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class SMTPConfig : IConfig
    {
        public bool IsAuthenticationEnabled { get; set; } = false;
        public SmtpDeliveryMethod Deliverymethod { get; set; } = SmtpDeliveryMethod.Network;
        public bool EnableSSL { get; set; } = false;
        public string Encoding { get; set; } = "utf-8";        
        public int SMTPPort { get; set; } = 587;
        public string SMTPServer { get; set; } = "";
        public string User { get; set; } = "";
        public string Password { get; set; } = ""; 
        public string FromAddress { get; set; } = "";
        public string FromName { get; set; } = "";
        public string TestReceiver { get; set; } = "";
        public bool IsBCCEnabled { get; set; } = false;
        public string BCCAddresses { get; set; } = "";  
    }
}
