using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.WebUI.Controllers
{
    public class RedisController : Controller
    {
        private readonly ICacheManager<MemoryCacheManager> _cacheManager;

        public RedisController(ICacheManager<MemoryCacheManager> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public IActionResult Index()
        {
            TTLExtendableCacheObject<DateTime> saat()
            {
                return new TTLExtendableCacheObject<DateTime>(DateTime.Now, DateTime.Now);
            }

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RoleByIDCacheKey, 1);
            cacheKey.CacheTime = TimeSpan.FromHours(1);

            var cacheRefreshKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RefreshRoleByIDCacheKey, 1);
            cacheRefreshKey.CacheTime = TimeSpan.FromSeconds(10);

            DateTime LastUpdateDate() =>
                DateTime.Now;

            TTLExtendableCacheObject<DateTime> dt = _cacheManager.GetOrCreate(cacheKey,
                saat, cacheRefreshKey, LastUpdateDate);

            ViewBag.dt = dt.CacheObject; 
        

            NoSQLKey CategoriesByLangCacheKey = new NoSQLKey("Redis.CategoriesByLang-{0}");
            return View();
        }
    }
}
