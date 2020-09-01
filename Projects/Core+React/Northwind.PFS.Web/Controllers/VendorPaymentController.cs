using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.CostPoint.Interfaces;

namespace Northwind.PFS.Web.Controllers
{
    public class VendorPaymentController : Controller
    {
        IVendorPaymentServiceCP _vendorPaymentService;
        public VendorPaymentController(IVendorPaymentServiceCP vendorPaymentService)
        {
            _vendorPaymentService = vendorPaymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(long projectId, string searchValue, int skip, int take, string orderBy, string dir)
        {
            var result = _vendorPaymentService.GetVendorPayments(projectId, searchValue, skip, take, orderBy, dir);
            var count = _vendorPaymentService.GetCount(projectId, searchValue);
            return Json(new { result, count });
        }
    }
}