using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Core.Configuration
{
    public class AppSettings
    {
        public MongoDBConfig MongoConfig { get; set; } = new MongoDBConfig();

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
        public string redis_host { get; set; }
        public string redis_port { get; set; }
        public string elastic_server { get; set; }
        public string reCAPTCHA_Secret_key { get; set; }
        public string reCAPTCHA_Site_key { get; set; }
        public string TestServerMode { get; set; }

        public string smtp_authentication { get; set; }
        public string smtp_deliverymethod { get; set; }
        public string smtp_enable_ssl { get; set; }
        public string smtp_encoding { get; set; }
        public string smtp_password { get; set; }
        public string smtp_port { get; set; }
        public string smtp_server { get; set; }
        public string smtp_user { get; set; }
        public AppSettings()
        {

        }
    }
}
