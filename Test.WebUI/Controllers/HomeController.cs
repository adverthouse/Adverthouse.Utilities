using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Test.WebUI.Models;
using Test.WebUI.PSFs;
using Test.WebUI.Validators;

namespace Test.WebUI.Controllers
{
    public static class AdminDefaults
    {
        public static NoSQLKey RoleByIDCacheKey => new NoSQLKey("Mem.RolesByID-{0}");

    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberValidator _memberValidator;
        private readonly ICacheManager<MemoryCacheManager> _cacheManager;

        public HomeController(ILogger<HomeController> logger,
            ICacheManager<MemoryCacheManager> cacheManager)
        {
            _logger = logger;
            _memberValidator = new MemberValidator("#EditForm");
            _cacheManager = cacheManager;
        }

        public IActionResult Index()
        {
            DateTime saat() {
                return DateTime.Now;
            }

            var cacheKey = _cacheManager.PrepareKeyForDefaultCache(AdminDefaults.RoleByIDCacheKey, 1);

            DateTime dt = _cacheManager.Get(cacheKey, saat);


            ViewBag.dt = dt;

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
