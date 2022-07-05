using Microsoft.AspNetCore.Mvc;
using Test.WebUI.Models.Services;

namespace Test.WebUI.Controllers
{
    public class ShutterController : Controller
    {
        public IActionResult Index(string q)
        { 
            ShutterClient _sc = new ShutterClient();
            var suggestion = _sc.GetSuggestions("Sunlight descending on the earth among black clouds and a hay bale in the middle of a dried plant field ");
            ImageLst lst = _sc.GetSponsoredImages(1, 20, "Purple Unicorn");
            return View(lst);
        }
    }
}
