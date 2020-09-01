using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Interfaces;
using Northwind.Web.Models.ViewModels;
using System;
using System.Linq;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class UsCustomerOfficeListController : Controller
    {
        private readonly IUsCustomerOfficeListService _usCustomerOfficeListService;
        private readonly IConfiguration _configuration;

        public UsCustomerOfficeListController(
                                  IUsCustomerOfficeListService usCustomerOfficeListService,
                                  IConfiguration configuration)
        {
            _configuration = configuration;
            _usCustomerOfficeListService = usCustomerOfficeListService;
        }
        #region Json
        public JsonResult GetCustomerDepartment()
        {
            try
            {
                var departmentList = _usCustomerOfficeListService.GetUsCustomerOfficeDepartmentList();
                var model = (from c in departmentList select new KeyValuePairModel<string, string> { Keys = c.DepartmentName, Values = c.DepartmentName }).ToList();
                model.Insert(0, new KeyValuePairModel<string, string> { Keys = null, Values = "--Select--" });
                return Json(model);
            }
            catch
            {
                return Json("");
            }
        }
        public JsonResult GetCustomerByDepartment(string departmentValue)
        {
            try
            {
                var departmentList = _usCustomerOfficeListService.GetDistinctCustomerNameByDepartment(departmentValue);
                var model = (from c in departmentList select new KeyValuePairModel<string, string> { Keys = c.CustomerName, Values = c.CustomerName });
                return Json(model);
            }
            catch (Exception)
            {
                return Json("");
            }
        }

        public JsonResult GetCustomerDetailBySixDigitCode(string sixDigitCode)
        {
            try
            {
                var model = _usCustomerOfficeListService.GetUsCustomerOfficeDepartmentListBySixDigitCode(sixDigitCode);
                return Json(model);
            }
            catch (Exception)
            {
                return Json("");
            }
        }
        #endregion
    }
}