using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class AppSettings
    {
        public MongoDBConfig MongoConfig { get; set; } = new MongoDBConfig();
        public SMTPConfig SMTPConfig { get; set; } = new SMTPConfig();
        public reCAPTCHAConfig reCAPTCHAConfig { get; set; } = new reCAPTCHAConfig();
        public RedisConfig RedisConfig { get; set; } = new RedisConfig();
        public string BestHDWMembers { get; set; }
        public string BlogPhotos { get; set; }
        public string Categories { get; set; }
        public string WebsiteUrl { get; set; }
        public string SubmitWebsiteUrl { get; set; }
        public string RawImages { get; set; }
        public string Images { get; set; }
        public string BestHDWImages { get; set; }
        public string EmailTemplates { get; set; }
        public string SitemapRootFolder { get; set; }
 
        public string elastic_server { get; set; }
    
        public string TestServerMode { get; set; }

 
        public AppSettings()
        {

        }
    }
}
