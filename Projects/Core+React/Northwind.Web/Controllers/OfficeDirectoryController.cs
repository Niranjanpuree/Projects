using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Interfaces;
using Northwind.Web.Models.ViewModels;
using Northwind.Core.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class OfficeDirectoryController : Controller
    {
        private readonly IOfficeService _officeService;
        private readonly ICommonService _commonService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IConfiguration _configuration;
        public OfficeDirectoryController(IOfficeService officeService,
                                ICommonService commonService,
                                IStateService stateService,
                                ICountryService countryService,
                                IConfiguration configuration)
        {
            _officeService = officeService;
            _commonService = commonService;
            _countryService = countryService;
            _stateService = stateService;
            _configuration = configuration;
        }

        // GET: Office
        public ActionResult Index(string searchValue)
        {
            OfficeViewModel officeViewModel = new OfficeViewModel();
            officeViewModel.SearchValue = searchValue;
            return View(officeViewModel);
        }

        private dynamic GetExportableData(List<OfficeViewModel> officeList)
        {
            var data = officeList.Select(x => new
            {
                OfficeCode = x.OfficeCode,
                OfficeName = x.OfficeName,
                PhysicalAddress = x.PhysicalAddress,
                MailingAddress = x.MailingAddress,
                Phone = x.Phone,
                Fax = x.Fax,
                UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy"),
                IsActiveStatus = x.IsActiveStatus,
            }).ToList();

            return data;
        }

        public IActionResult Get(string searchValue,string filterBy, int pageSize, int skip, string sortField, string sortDirection,bool isExport)
        {
            try
            {
                var total = 0;
                var offices = _officeService.GetOfficeListForUser(searchValue,filterBy, pageSize, skip, sortField, sortDirection);
                List<OfficeViewModel> officeViewModels = new List<OfficeViewModel>();
                foreach (var ob in offices)
                {
                    var mapVal = Models.ObjectMapper<Office, OfficeViewModel>.Map(ob);
                    officeViewModels.Add(mapVal);
                }

                if (pageSize == 100)
                    total = offices.Count();
                else
                    total = _officeService.GetOfficeTotalCountForUser(searchValue,filterBy);

                if (isExport) {
                    return Json(new { total = total, data = officeViewModels });
                }
                    
                return Json(new { total = total, data = officeViewModels });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }
        [HttpGet]
        public IActionResult GetStatesByCountryId(Guid countryId)
        {
            try
            {
                var states = _stateService.GetStateByCountryGuid(countryId);
                return Json(new { data = states });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }
    }
}