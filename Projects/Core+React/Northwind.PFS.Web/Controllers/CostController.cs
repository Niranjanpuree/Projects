using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class CostController : Controller
    {
        ICostServiceCP _costService;
        public CostController(ICostServiceCP costService)
        {
            _costService = costService;
        }

        [HttpGet]
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "fiscalyear, period";
                }
                var result = _costService.GetCosts(projectNumber, searchValue, skip, take, orderBy, dir, new List<AdvancedSearchRequest>(), startDate, endDate);
                var count = _costService.GetCount(projectNumber, searchValue, new List<AdvancedSearchRequest>(), startDate, endDate);
                return Json(new { result, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "fiscalyear, period";
                }
                var result = _costService.GetCosts(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
                var count = _costService.GetCount(projectNumber, searchValue, postValue, startDate, endDate);
                return Json(new { result, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult GetPieChart(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            var result = _costService.GetCostForPieChart(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
            var lst = new List<dynamic>();
            foreach (var r in result)
            {
                lst.Add(new { r.Name, Value = r.Amount });
            }
            return Json(lst);
        }

        [HttpPost]
        public IActionResult GetBarChart(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            var result = _costService.GetCostForBarChart(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
            var lst = new List<dynamic>();
            foreach (var r in result)
            {
                lst.Add(new { r.Name, Value = r.Amount });
            }
            return Json(lst);
        }
    }
}