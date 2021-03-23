using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Test.WebUI.Models;
using Test.WebUI.Models.Services;
using Test.WebUI.PSFs;
using Test.WebUI.Validators;

namespace Test.WebUI.Controllers
{
    public static class AdminDefaults
    {
        public static NoSQLKey RoleByIDCacheKey => new NoSQLKey("Mem.RolesByID-{0}");
        public static NoSQLKey RefreshRoleByIDCacheKey => new NoSQLKey("Mem.Refresh.RolesByID-{0}");
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberValidator _memberValidator;
        private readonly ICacheManager<MemoryCacheManager> _cacheManager;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger,
            ICacheManager<MemoryCacheManager> cacheManager, ICategoryService categoryService)
        {
            _logger = logger;
            _memberValidator = new MemberValidator("#EditForm");
            _cacheManager = cacheManager;
            _categoryService = categoryService;
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
            });
            string temp = "Data set";
            _categoryService.AllZero();
            temp = "All zero";
            */
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


            return Ok("");
        }
        public IActionResult Index()
        { 
            var lad = DateTime.Now;
            TTLExtendableCacheObject<DateTime> saat() {
                return new TTLExtendableCacheObject<DateTime>(lad, lad);    
            }
            List<String> tem = new List<string>();
            tem.Add("deneme");
            tem.Add("deneme 2");

      

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RoleByIDCacheKey, 1);
            cacheKey.CacheTime = TimeSpan.FromHours(1);

            var cacheRefreshKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RefreshRoleByIDCacheKey, 1);
            cacheRefreshKey.CacheTime = TimeSpan.FromSeconds(60);

            DateTime LastUpdateDate() =>
                lad; 

            TTLExtendableCacheObject<DateTime> dt = _cacheManager.GetOrCreate(cacheKey,
                saat,cacheRefreshKey,LastUpdateDate); 

            ViewBag.dt = dt.CacheObject;

            PSFMember pSFMember = new PSFMember();
            pSFMember.FFirstName = "Yunus";

            string temp = pSFMember.Filter;
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
