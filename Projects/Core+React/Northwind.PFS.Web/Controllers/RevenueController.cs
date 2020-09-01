using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class RevenueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(Guid projectId)
        {
            var lst = new List<dynamic>();
            lst.Add(new { Name = "01", Amount = 4000 });
            lst.Add(new { Name = "02", Amount = 7000 });
            lst.Add(new { Name = "03", Amount = 6600 });
            lst.Add(new { Name = "04", Amount = 8000 });
            lst.Add(new { Name = "05", Amount = 9000 });
            return Json(lst);
        }
    }
}