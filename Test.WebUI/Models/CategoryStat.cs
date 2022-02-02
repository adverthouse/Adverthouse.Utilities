using Adverthouse.Common.Data.MongoDB;
using Adverthouse.Common.Interfaces;
using Nest;

namespace Test.WebUI.Models
{
    [ElasticsearchType(RelationName = "categories", IdProperty = "CategoryID")]
    public class CategoryStat : IViewModel, IDocument
    {
        public int CategoryID { get; set; }
        public double TotalDownloadCount { get; set; }
        public double TotalViewCount { get; set; }

        public CategoryStat()
        {

        }
    }
}
