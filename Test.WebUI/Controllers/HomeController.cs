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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberValidator _memberValidator;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _memberValidator = new MemberValidator("#EditForm");
        }

        public IActionResult Index()
        {
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
