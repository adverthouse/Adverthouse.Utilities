using Microsoft.AspNetCore.Mvc;
using Test.WebUI.Models;
using Test.WebUI.Validators;

namespace Test.WebUI.Controllers
{
    public class FormValidationController : Controller
    {
        private MemberValidator _memberValidator;


        public FormValidationController()
        {
            
        }

        public IActionResult Index()
        {
            //_memberValidator.AdditionalMethods("");
  //          ViewBag.ValidationScript = _memberValidator.GetValidationScript();
            return View();
        }

        [HttpPost]
        public IActionResult Index(VMMember member)
        {
            _memberValidator = new MemberValidator("#EditForm",member.Password); 
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
