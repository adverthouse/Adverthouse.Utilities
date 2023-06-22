using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Test.WebUI.Models;

namespace Test.WebUI.Controllers
{
    public class CacheController : Controller
    {
        private readonly ICacheManager<MemoryCacheManager> _cacheManager;

        public CacheController(ICacheManager<MemoryCacheManager> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public IActionResult Index()
        {

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RoleByIDCacheKey, 1);
            cacheKey.CacheTime = TimeSpan.FromHours(1);

            var cacheRefreshKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RefreshRoleByIDCacheKey, 1);
            cacheRefreshKey.CacheTime = TimeSpan.FromSeconds(10);

            NoSQLKey CategoriesByLangCacheKey = new NoSQLKey("Redis.CategoriesByLang-{0}");
            return View();
        }

        public IActionResult Index2()
        {

            return View();
        }

        public IActionResult UpdateLastUpdateDate()
        {
            // This is update lastUpdate date for trigger.
            CoreData.LastUpdateDate = DateTime.Now;
            return Content("");
        }
    }
}
