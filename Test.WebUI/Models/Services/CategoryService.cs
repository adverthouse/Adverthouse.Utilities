using Adverthouse.Common.Data.ElasticSearch;
using Adverthouse.Common.Data.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.WebUI.Models.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMongoRepository<Category> _mongoCategoryRepository;
        private readonly IElasticRepository<Category> _elasticCategoryRepository;
        private readonly IElasticRepository<CategoryStat> _elasticCategoryStatRepository;

        public CategoryService(IMongoRepository<Category> mongoCategoryRepository,
                               IElasticRepository<Category> elasticCategoryRepository, 
                               IElasticRepository<CategoryStat> elasticCategoryStatRepository)
        {
            _mongoCategoryRepository = mongoCategoryRepository;
            _elasticCategoryRepository = elasticCategoryRepository;
            _elasticCategoryStatRepository = elasticCategoryStatRepository;
        }

        public void Create(Category category)
        {
            _mongoCategoryRepository.ReplaceOne(category.CategoryID, category);          
        }


        public void AllZero() {
            _mongoCategoryRepository.UpdateMany(
                Builders<Category>.Filter.Where(x=>x.CategoryID == 1),
                Builders<Category>.Update.Set(p => p.TotalDownloadCount, 30)
                                         .Set(p => p.TotalViewCount,40)
           );
        }

        public void CreateElastic(List<Category> categories) {
            _elasticCategoryRepository.UpsertDocument("category", categories);
        }

        public void UpdateAllElastic(List<CategoryStat> stats) {
            _elasticCategoryStatRepository.UpsertDocument("category", stats);
        }
    }
}
