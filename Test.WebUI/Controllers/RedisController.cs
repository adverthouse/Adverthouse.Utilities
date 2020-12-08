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
        public IActionResult Index()
        {
            RedisKey CategoriesByLangCacheKey = new RedisKey("Redis.CategoriesByLang-{0}");
            return View();
        }
    }
}
