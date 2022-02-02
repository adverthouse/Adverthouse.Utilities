using Adverthouse.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;
using Test.WebUI.Models;

namespace Test.WebUI.Controllers
{
    public class UtilityController : Controller
    {
        public IActionResult Index()
        {
            List<Member> members = new List<Member>();
            members.Add(new Member() { FirstName = "Yunus" });
            members.Add(new Member() { FirstName = "Vehbi" });

            DataTable dt = members.ToDataTable();

            return View();
        }
    }
}
