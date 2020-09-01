using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class POController : Controller
    {
        IPOServiceCP _poService;
        public POController(IPOServiceCP poService)
        {
            _poService = poService;
        }

        [HttpGet]
        public IActionResult Get(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if(string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "PoIssueDate";
                }
                var result = _poService.GetPO(projectNumber, searchValue, skip, take, orderBy, dir, new List<Core.Models.AdvancedSearchRequest>(), startDate, endDate);
                var count = _poService.GetCount(projectNumber, searchValue, new List<Core.Models.AdvancedSearchRequest>(), startDate, endDate);
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
        public IActionResult Get(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<Core.Models.AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "PoIssueDate";
                }
                var result = _poService.GetPO(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
                var count = _poService.GetCount(projectNumber, searchValue, postValue, startDate, endDate);
                return Json(new { result, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
    }
}