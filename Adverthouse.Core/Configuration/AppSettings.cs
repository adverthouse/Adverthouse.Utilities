using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Adverthouse.Core.Configuration
{
    /// <summary>
    /// Represets the app settings
    /// </summary>
    public class AppSettings
    {
        public MongoDBConfig MongoDBConfig { get; set; } = new MongoDBConfig();
        public SMTPConfig SMTPConfig { get; set; } = new SMTPConfig();
        public reCAPTCHAConfig reCAPTCHAConfig { get; set; } = new reCAPTCHAConfig();
        public RedisConfig RedisConfig { get; set; } = new RedisConfig();
        public ElasticSearchConfig ElasticSearchConfig { get; set; } = new ElasticSearchConfig();
        
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalSettings { get; set; }
    }
}
