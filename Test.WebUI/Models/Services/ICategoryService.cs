using System.Collections.Generic;

namespace Test.WebUI.Models.Services
{
    public interface ICategoryService
    {
        void AllZero();
        void Create(Category category);
        void CreateElastic(List<Category> categories);
        void UpdateAllElastic(List<CategoryStat> stats);
    }
}