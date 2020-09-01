using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class BillingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(Guid projectId)
        {
            var lst = new List<dynamic>();
            lst.Add(new { Name = "01", Amount = 3000 });
            lst.Add(new { Name = "02", Amount = 3500 });
            lst.Add(new { Name = "03", Amount = 3300 });
            lst.Add(new { Name = "04", Amount = 4000 });
            lst.Add(new { Name = "05", Amount = 4500 });
            return Json(lst);
        }
    }
}