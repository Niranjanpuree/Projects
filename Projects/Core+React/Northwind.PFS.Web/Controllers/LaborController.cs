using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.CostPoint.Interfaces;
using Northwind.Web.Infrastructure.Models;
using Northwind.Core.Models;

namespace Northwind.PFS.Web.Areas.PFS.Controllers
{
    public class LaborController : Controller
    {
        private readonly ILaborServiceCP _laborPayrollService;
        private readonly IMapper _mapper;
        public LaborController(ILaborServiceCP laborPayrollService, IMapper mapper)
        {
            _laborPayrollService = laborPayrollService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "TransactionDate";
                }
                var result = _laborPayrollService.GetLabor(projectNumber, searchValue, skip, take, orderBy, dir, new List<AdvancedSearchRequest>(), startDate, endDate);
                var count = _laborPayrollService.GetCount(projectNumber, searchValue, new List<AdvancedSearchRequest>(), startDate, endDate);
                var list = new List<LaborPayrollViewModel>();
                foreach(var item in result)
                {
                    var listViewModel = _mapper.Map<LaborPayrollViewModel>(item);
                    listViewModel.Currency = "USD";
                    list.Add(listViewModel);
                };
                
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
        public IActionResult Get(string projectNumber, bool excludeIWOs, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (string.IsNullOrEmpty(orderBy))
                {
                    orderBy = "TransactionDate";
                }
                var result = _laborPayrollService.GetLabor(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
                var count = _laborPayrollService.GetCount(projectNumber, searchValue, postValue, startDate, endDate);
                var list = new List<LaborPayrollViewModel>();
                foreach (var item in result)
                {
                    var listViewModel = _mapper.Map<LaborPayrollViewModel>(item);
                    listViewModel.Currency = "USD";
                    list.Add(listViewModel);
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

        public IActionResult GetFiscalYearAndPeriod()
        {
            var data = new List<dynamic>();
            for(int yr=2019; yr <= 2020; yr++)
            {
                var Periods = new List<dynamic>();
                for (int i = 1; i <= 12; i++)
                {
                    Periods.Add(new { Name = i.ToString().PadLeft(2, '0'), Value = i.ToString().PadLeft(2, '0') });
                }
                var py = new { Name = yr.ToString(), Value = yr.ToString(), Options = Periods };
                data.Add(py);
            }
            return Json(data);
        }

        [HttpPost]
        public IActionResult GetPieChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            var result = _laborPayrollService.GetLaborForPieChart(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
            var lst = new List<dynamic>();
            foreach(var r in result)
            {
                lst.Add(new { r.Name, Value = r.Amount });
            }
            return Json(lst);
        }

        [HttpPost]
        public IActionResult GetBarChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, [FromBody] List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            var result = _laborPayrollService.GetLaborForBarChart(projectNumber, searchValue, skip, take, orderBy, dir, postValue, startDate, endDate);
            var lst = new List<dynamic>();
            foreach (var r in result)
            {
                lst.Add(new { r.Name, Value = r.Amount });
            }
            return Json(lst);
        }
    }
}