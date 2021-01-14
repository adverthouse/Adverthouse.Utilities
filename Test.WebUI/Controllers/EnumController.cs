using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.WebUI.Controllers
{
    public class EnumController : Controller
    {
       
        public EnumController()
        {
          
        }

        public IActionResult Index()
        {
            
            return View();
        }
    }
}
