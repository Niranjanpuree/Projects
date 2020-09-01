using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.PFS.Web.Models;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;
using Northwind.Core.Models;
using Northwind.Core.Interfaces;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class WbsController : Controller
    {
        IWbsServiceCP _wbsService;
        IWbsDictionaryService _wbsDictionaryService;
        public WbsController(IWbsServiceCP wbsService, IWbsDictionaryService wbsDictionaryService)
        {
            _wbsService = wbsService;
            _wbsDictionaryService = wbsDictionaryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "wbs";
                }
                var result = _wbsService.GetWbs(projectNumber, searchValue, skip, take, orderBy, dir, new List<AdvancedSearchRequest>());
                var count = _wbsService.GetCount(projectNumber, searchValue, new List<AdvancedSearchRequest>());
                var list = new List<WbsViewModel>();
                foreach(var wbs in result)
                {
                    var dictionaries = _wbsDictionaryService.GetWbsDictionary(wbs.ProjectNumber, wbs.Wbs, "", 0, 4, "CreatedOn", "ASC");
                    var item = new WbsViewModel {
                        WbsGuid = wbs.WbsGuid,
                        Description= wbs.Description,
                        Wbs=wbs.Wbs,
                        AllowCharging= wbs.AllowCharging,
                        ProjectNumber=wbs.ProjectNumber,
                        WbsDictionaryTitle = new List<string>()
                    };
                    foreach(var d in dictionaries)
                    {
                        item.WbsDictionaryTitle.Add(d.WbsDictionaryTitle);
                    }
                    list.Add(item);
                }
                return Json(new { result = list, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue)
        {
            try
            {
                var result = _wbsService.GetWbs(projectNumber, searchValue, skip, take, orderBy, dir, postValue);
                var count = _wbsService.GetCount(projectNumber, searchValue, postValue);
                var list = new List<WbsViewModel>();
                foreach (var wbs in result)
                {
                    var dictionaries = _wbsDictionaryService.GetWbsDictionary(wbs.ProjectNumber, wbs.Wbs, "", 0, 4, "CreatedOn", "ASC");
                    var item = new WbsViewModel
                    {
                        WbsGuid = wbs.WbsGuid,
                        ProjectNumber = wbs.ProjectNumber,
                        AllowCharging = wbs.AllowCharging,
                        Wbs = wbs.Wbs,
                        Description = wbs.Description,
                        WbsDictionaryTitle = new List<string>()
                    };
                    foreach (var d in dictionaries)
                    {
                        item.WbsDictionaryTitle.Add(d.WbsDictionaryTitle);
                    }
                    list.Add(item);
                }
                return Json(new { result = list, count });
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