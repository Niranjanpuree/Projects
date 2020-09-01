using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;
        public CompanyController(ICompanyService companyService, IUserService userService, IConfiguration configuration)
        {
            _companyService = companyService;
            _userService = userService;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Company

        [Secure(ResourceType.Admin, ResourceActionPermission.ManageCompany)]
        public ActionResult Index(string searchValue)
        {
            CompanyViewModel companyViewModel = new CompanyViewModel() { SearchValue = searchValue };
            return View(companyViewModel);
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                List<CompanyViewModel> companyViewModel = new List<CompanyViewModel>();
                var company = _companyService.GetAll(searchValue, pageSize, skip, sortField, sortDirection);
                foreach (var ob in company)
                {
                    var mapVal = Models.ObjectMapper<Company, CompanyViewModel>.Map(ob);
                    companyViewModel.Add(mapVal);
                }
                var total = _companyService.TotalRecord(searchValue);
                var data = companyViewModel.Select(x => new
                {
                    CompanyGuid = x.CompanyGuid,
                    CompanyCode = x.CompanyCode,
                    CompanyName = x.CompanyName,
                    Abbreviation = x.Abbreviation,
                    PresidentName = x.PresidentName,
                    President = x.President,
                    IsActive = x.IsActive,
                    IsActiveStatus = x.IsActiveStatus,
                    SearchValue = x.SearchValue,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                return Json(new { total = total, data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        #region Crud
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            var companyModel = _companyService.GetDetailsById(id);
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel = Models.ObjectMapper<Company, CompanyViewModel>.Map(companyModel);
            companyViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(companyViewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(companyViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var companyModel = _companyService.GetDetailsById(id);
            CompanyViewModel companyViewModel = new CompanyViewModel();
            companyViewModel = Models.ObjectMapper<Company, CompanyViewModel>.Map(companyModel);
            companyViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(companyViewModel);
        }

        [HttpPost]
        public IActionResult Add([FromBody]CompanyViewModel companyViewModel)
        {
            try
            {
                var companyModel = Models.ObjectMapper<CompanyViewModel, Company>.Map(companyViewModel);
                if (_companyService.CheckDuplicates(companyModel) > 0)
                {
                    throw new ArgumentException("Duplicate value entered for either code or name !!");
                }
                Guid id = Guid.NewGuid();
                companyModel.CompanyGuid = id;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                companyModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                companyModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                companyModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                companyModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                companyModel.IsActive = true;
                companyModel.IsDeleted = false;
                var Company = _companyService.Add(companyModel);

                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.Company.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Company";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Company Name: {2} </br> Company Code: {3}", ResourceType.Company.ToString(), companyModel.CompanyGuid, companyModel.CompanyName, companyModel.CompanyCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), companyModel, resource, companyModel.CompanyGuid, UserHelper.GetHostedIp(HttpContext), "Company Added", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);


                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody]CompanyViewModel companyViewModel)
        {
            try
            {
                var companyModel = Models.ObjectMapper<CompanyViewModel, Company>.Map(companyViewModel);
                if (_companyService.CheckDuplicates(companyModel) > 0)
                {
                    throw new ArgumentException("Duplicate value entered for either code or name !!");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                companyModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                companyModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                _companyService.Edit(companyModel);

                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.Company.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Company";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Company Name: {2} </br> Company Code: {3}", ResourceType.Company.ToString(), companyModel.CompanyGuid, companyModel.CompanyName, companyModel.CompanyCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), companyModel, resource, companyModel.CompanyGuid, UserHelper.GetHostedIp(HttpContext), "Company Edited", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public ActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {

                foreach (var id in ids)
                {
                    //audit log..
                    var companyModel = _companyService.GetDetailsById(id);
                    if (companyModel != null)
                    {
                        _companyService.DeleteById(id);

                        var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.Company.ToString().ToLower());

                        var resource = string.Format("{0} </br> GUID: {1} </br> Company Name: {2} </br> Company Code: {3}", ResourceType.Company.ToString(), companyModel.CompanyGuid, companyModel.CompanyName, companyModel.CompanyCode);

                        AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), companyModel, resource, companyModel.CompanyGuid, UserHelper.GetHostedIp(HttpContext), "Company  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                    }
                }


                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex); ;
            }
        }

        [HttpPost]
        public ActionResult Disable([FromBody]Guid[] ids)
        {
            try
            {
                _companyService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var companyModel = _companyService.GetDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.Company.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Company";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Company Name: {2} </br> Company Code: {3}", ResourceType.Company.ToString(), companyModel.CompanyGuid, companyModel.CompanyName, companyModel.CompanyCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, companyModel.CompanyGuid, UserHelper.GetHostedIp(HttpContext), "Company Disabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex); ;
            }
        }

        [HttpPost]
        public ActionResult Enable([FromBody]Guid[] ids)
        {
            try
            {
                _companyService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var companyModel = _companyService.GetDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.Company.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Company";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Company Name: {2} </br> Company Code: {3}", ResourceType.Company.ToString(), companyModel.CompanyGuid, companyModel.CompanyName, companyModel.CompanyCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, companyModel.CompanyGuid, UserHelper.GetHostedIp(HttpContext), "Company Enabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex); 
            }
        }
        #endregion
    }
}