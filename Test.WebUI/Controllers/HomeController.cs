using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using Test.WebUI.Models;
using Test.WebUI.Models.Services;
using Test.WebUI.Validators;

namespace Test.WebUI.Controllers
{
    public static class AdminDefaults
    {
        public static NoSQLKey RoleByIDCacheKey => new NoSQLKey("Mem.RolesByID-{0}");
        public static NoSQLKey RefreshRoleByIDCacheKey => new NoSQLKey("Mem.Refresh.RolesByID-{0}");

        public static NoSQLKey LastUpdateDateTIC => new NoSQLKey("Mem.TotalItemCount");
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberValidator _memberValidator;
        private readonly ICacheManager<MemoryCacheManager> _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger,
            ICacheManager<MemoryCacheManager> cacheManager, ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memberValidator = new MemberValidator("#EditForm");
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _memoryCache = memoryCache;
        }

        public IActionResult Fill() {
            /*
                 _categoryService.Create(new Category()
                 {
                     CategoryID = 1,
                     CategoryName = "Elektronik",
                     TotalDownloadCount = 10,
                     TotalViewCount = 20
                 });
                 _categoryService.Create(new Category()
                 {
                     CategoryID = 2,
                     CategoryName = "Ev & Yaşam",
                     TotalDownloadCount = 3,
                     TotalViewCount = 4
                 }); */
            string temp = "Data set";
            _categoryService.AllZero();
            temp = "All zero";
      
            /*
        var lst = new List<CategoryStat>();
        lst.Add(new CategoryStat()
        {
            CategoryID = 1, 
            TotalDownloadCount = 100,
            TotalViewCount = 200
        });
        lst.Add(new CategoryStat()
        {
            CategoryID = 2, 
            TotalDownloadCount = 300,
            TotalViewCount = 400
        });


        _categoryService.UpdateAllElastic(lst);
*/

            return Ok("");
        }
        private static long _lockFlag = DateTime.Now.Ticks; // 0 - free

        private static string abc = "deneme";

        [ResponseCache(CacheProfileName = "default")]
        public IActionResult Index()
        {            
            var lad = DateTime.Now;

            TTLExtendableCacheObject<DateTime> saat() {
                return new TTLExtendableCacheObject<DateTime>(lad, lad);    
            }      

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RoleByIDCacheKey, 1);
            cacheKey.CacheTime = TimeSpan.FromHours(1);

            var cacheRefreshKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RefreshRoleByIDCacheKey, 1);
            cacheRefreshKey.CacheTime = TimeSpan.FromSeconds(10);

            DateTime LastUpdateDate() =>
                lad; 

            TTLExtendableCacheObject<DateTime> dt = _cacheManager.GetOrCreate(cacheKey,
                saat,cacheRefreshKey,LastUpdateDate);

            ViewBag.dt = abc;         
         
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Create()
        {
            ViewBag.ValidationScript = _memberValidator.GetValidationScript();
            return View();
        }

        [HttpPost] 
        public IActionResult Create(Member member, string password2)
        {
            ViewBag.ValidationScript = _memberValidator.GetValidationScript();
            if (_memberValidator.IsValid(member))
            { 
                return RedirectToAction("Index");
            }
            ViewBag.ValidationScript = _memberValidator.GetValidationScript();
            ViewBag.ErrorLines = _memberValidator.GetValidationErrors();
            return View(member);
        }
    }
}
