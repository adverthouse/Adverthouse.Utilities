using Microsoft.AspNetCore.Mvc;

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
