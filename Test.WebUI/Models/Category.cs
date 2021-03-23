using Adverthouse.Common.Data.MongoDB;
using Adverthouse.Common.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.WebUI.Models
{
    [BsonCollection("categories")]
    [ElasticsearchType(RelationName = "categories", IdProperty = "CategoryID")]
    public class Category : IViewModel, IDocument
    {
        [BsonId]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public double TotalDownloadCount { get; set; }
        public double TotalViewCount { get; set; }

        public Category()
        {

        }
    }
}
