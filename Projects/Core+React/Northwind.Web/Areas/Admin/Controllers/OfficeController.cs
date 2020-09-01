using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OfficeController : Controller
    {
        private readonly IOfficeService _officeService;
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IConfiguration _configuration;
        private readonly IContractsService _contractsService;
        private readonly Logger _logger;

        public OfficeController(IOfficeService officeService,
                                IUserService userService,
                                ICommonService commonService,
                                IStateService stateService,
                                ICountryService countryService,
                                IContractsService contractsService,
                                IConfiguration configuration)
        {
            _officeService = officeService;
            _commonService = commonService;
            _countryService = countryService;
            _userService = userService;
            _stateService = stateService;
            _contractsService = contractsService;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Office
        [Secure(ResourceType.Admin, ResourceActionPermission.ManageOffice)]
        public ActionResult Index(string searchValue)
        {
            OfficeViewModel officeViewModel = new OfficeViewModel();
            officeViewModel.SearchValue = searchValue;
            return View(officeViewModel);
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                var offices = _officeService.GetAll(searchValue, pageSize, skip, sortField, sortDirection);
                List<OfficeViewModel> officeViewModels = new List<OfficeViewModel>();
                foreach (var ob in offices)
                {
                    var mapVal = Models.ObjectMapper<Office, OfficeViewModel>.Map(ob);
                    officeViewModels.Add(mapVal);
                }
                var total = _officeService.TotalRecord(searchValue);
                var data = officeViewModels.Select(x => new
                {
                    Fax = x.Fax,
                    MailingAddress = x.MailingAddressDisplay,
                    MailingCity = x.MailingCity,
                    MailingCountry = x.MailingCountryName,
                    MailingState = x.MailingStateName,
                    MailingZipCode = x.MailingZipCode,
                    OfficeCode = x.OfficeCode,
                    OfficeGuid = x.OfficeGuid,
                    OfficeName = x.OfficeName,
                    OperationManagerName = x.OperationManagerName,
                    Phone = x.Phone,
                    PhysicalAddress = x.PhysicalAddressDisplay,
                    PhysicalCity = x.PhysicalCity,
                    PhysicalCountry = x.PhysicalCountryName,
                    PhysicalState = x.PhysicalStateName,
                    PhysicalZipCode = x.PhysicalZipCode,
                    IsActive = x.IsActive,
                    IsActiveStatus = x.IsActiveStatus,
                    SearchValue = x.SearchValue,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                return Json(new { total = total, data = data });
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
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #region Crud Get
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            OfficeViewModel officeViewModel = new OfficeViewModel();
            try
            {
                var officeModel = _officeService.GetDetailById(id);
                officeViewModel = Models.ObjectMapper<Office, OfficeViewModel>.Map(officeModel);

                return PartialView(officeViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(officeViewModel);
            }
        }

        [HttpGet]
        public ActionResult Add()
        {
            OfficeViewModel officeViewModel = new OfficeViewModel();
            try
            {
                officeViewModel.PhysicalCountryId = _countryService.GetCountryGuidBy3DigitCode("USA");
                officeViewModel.MailingCountryId = _countryService.GetCountryGuidBy3DigitCode("USA");
                officeViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
                officeViewModel.StatePrimarySelectListItems = _stateService.GetStateByCountryGuid(officeViewModel.PhysicalCountryId).ToDictionary(x => x.StateId, x => x.StateName);
                officeViewModel.StateMailingSelectListItems = _stateService.GetStateByCountryGuid(officeViewModel.MailingCountryId ?? Guid.Empty).ToDictionary(x => x.StateId, x => x.StateName);
                officeViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
                return PartialView(officeViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(officeViewModel);
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            OfficeViewModel officeViewModel = new OfficeViewModel();
            try
            {
                var officeModel = _officeService.GetById(id);
                officeViewModel = Models.ObjectMapper<Office, OfficeViewModel>.Map(officeModel);

                officeViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x=>x.CountryId,x=>x.CountryName);
                officeViewModel.StatePrimarySelectListItems = _stateService.GetStateByCountryGuid(officeViewModel.PhysicalCountryId).ToDictionary(x => x.StateId, x => x.StateName);
                officeViewModel.StateMailingSelectListItems = _stateService.GetStateByCountryGuid(officeViewModel.MailingCountryId ?? Guid.Empty).ToDictionary(x => x.StateId, x => x.StateName);
                officeViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
                return PartialView(officeViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(officeViewModel);
            }
        }
        #endregion

        #region Crud Json Post
        [HttpPost]
        public IActionResult Add([FromBody] OfficeViewModel officeViewModel)
        {
            try
            {
                var officeModel = Models.ObjectMapper<OfficeViewModel, Office>.Map(officeViewModel);

                if (_officeService.CheckDuplicate(officeModel) > 0)
                {
                    var errorMessage = "Duplicate value entered for either code or name !!";
                    ModelState.AddModelError("", errorMessage);
                    return BadRequest(ModelState);
                }
                if (ModelState.IsValid)
                {
                    Guid id = Guid.NewGuid();
                    officeModel.OfficeGuid = id;
                    officeModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    officeModel.CreatedBy = id;
                    officeModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    officeModel.UpdatedBy = id;
                    officeModel.IsActive = true;
                    officeModel.IsDeleted = false;
                    _officeService.Add(officeModel);


                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.Office.ToString());
                    //var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Office/Details/" + officeModel.OfficeGuid);
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/office";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID:{1} </br> Office Name:{2} </br> Office Code:{3}", ResourceType.Office.ToString(), officeModel.OfficeGuid, officeModel.OfficeName, officeModel.OfficeCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), officeModel, resource, officeModel.OfficeGuid, UserHelper.GetHostedIp(HttpContext), "Office Added", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);


                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", office = new { officeGuid = id, officeName = officeModel.OfficeName } });
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody] OfficeViewModel officeViewModel)
        {
            try
            {
                var officeModel = Models.ObjectMapper<OfficeViewModel, Office>.Map(officeViewModel);

                if (_officeService.CheckDuplicate(officeModel) > 0)
                {
                    var errorMessage = "Duplicate value entered for either code or name !!";
                    ModelState.AddModelError("", errorMessage);
                    return BadRequest(ModelState);
                }
                if (ModelState.IsValid)
                {
                    //                    var officeModelFromRepo = _officeService.GetOfficeDetailsById(OfficeModel.OfficeGuid);
                    officeModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    officeModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    officeModel.UpdatedBy = Guid.NewGuid();  // later comes through session..
                    var isOfficeUpated = _officeService.Edit(officeModel);
                    if(isOfficeUpated >= 1)
                        _contractsService.UpdateAllUserByRole(officeModel.OperationManagerGuid,ContractUserRole._operationManager);
                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.Office.ToString());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/office";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID:{1} </br> Office Name:{2} </br> Office Code:{3}", ResourceType.Office.ToString(), officeModel.OfficeGuid, officeModel.OfficeName, officeModel.OfficeCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), officeModel, resource, officeModel.OfficeGuid, UserHelper.GetHostedIp(HttpContext), "Office Edited", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    //audit log..
                    var officeModel = _officeService.GetDetailById(id);
                    if(officeModel != null)
                    {
                        var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.Office.ToString().ToLower());

                        var resource = string.Format("{0} </br> GUID:{1} </br> Office Name:{2} </br> Office Code:{3}", ResourceType.Office.ToString(), officeModel.OfficeGuid, officeModel.OfficeName, officeModel.OfficeCode);

                        AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), officeModel, resource, officeModel.OfficeGuid, UserHelper.GetHostedIp(HttpContext), "Office  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");

                        _officeService.DeleteById(id);
                    }
                }
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _officeService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var officeModel = _officeService.GetDetailById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.Office.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/office";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID:{1} </br> Office Name:{2} </br> Office Code:{3}", ResourceType.Office.ToString(), officeModel.OfficeGuid, officeModel.OfficeName, officeModel.OfficeCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, officeModel.OfficeGuid, UserHelper.GetHostedIp(HttpContext), "Office Disabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _officeService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var officeModel = _officeService.GetDetailById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.Office.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/office";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID:{1} </br> Office Name:{2} </br> Office Code:{3}", ResourceType.Office.ToString(), officeModel.OfficeGuid, officeModel.OfficeName, officeModel.OfficeCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, officeModel.OfficeGuid, UserHelper.GetHostedIp(HttpContext), "Office Enabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }
        #endregion
    }
}
