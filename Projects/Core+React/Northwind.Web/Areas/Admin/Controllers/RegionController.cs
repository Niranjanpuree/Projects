using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IContractsService _contractsService;
        private readonly Logger _logger;

        public RegionController(IRegionService regionService, IUserService userService, IContractsService contractsService, IConfiguration configuration)
        {
            _regionService = regionService;
            _userService = userService;
            _contractsService = contractsService;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // GET: Region

        [Secure(ResourceType.Admin, ResourceActionPermission.ManageRegion)]
        public ActionResult Index(string searchValue)
        {
            RegionViewModel regionViewModel = new RegionViewModel();
            regionViewModel.SearchValue = searchValue;
            return View(regionViewModel);
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string sortDirection)
        {
            try
            {
                List<RegionViewModel> regionViewModel = new List<RegionViewModel>();
                var region = _regionService.GetAll(searchValue, pageSize, skip,  take, sortField, sortDirection);
                foreach (var items in region)
                {
                    var mapValue = Models.ObjectMapper<Region, RegionViewModel>.Map(items);
                    regionViewModel.Add(mapValue);
                }
                var total = _regionService.GetCount(searchValue);
                var data = regionViewModel.Select(x => new
                {
                    ManagerName = x.ManagerName,
                    BDManagerName = x.BDManagerName,
                    DeputyManagerName = x.DeputyManagerName,
                    HSManagerName = x.HSManagerName,
                    RegionalManager = x.RegionalManager,
                    RegionCode = x.RegionCode,
                    RegionGuid = x.RegionGuid,
                    RegionName = x.RegionName,
                    IsActive = x.IsActive,
                    IsActiveStatus = x.IsActiveStatus,
                    SearchValue = x.SearchValue,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();
                // Return as JSON - the Kendo Grid will use the response
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
            var regionModel = _regionService.GetDetailsById(id);
            RegionViewModel regionViewModel = new RegionViewModel();
            regionViewModel = Models.ObjectMapper<Region, RegionViewModel>.Map(regionModel);
            regionViewModel.BDManagerName = _userService.GetUserByUserGuid(regionViewModel.BusinessDevelopmentRegionalManager)?.DisplayName;
            regionViewModel.ManagerName = _userService.GetUserByUserGuid(regionViewModel.RegionalManager)?.DisplayName;
            regionViewModel.HSManagerName = _userService.GetUserByUserGuid(regionViewModel.HSRegionalManager)?.DisplayName;
            regionViewModel.DeputyManagerName = _userService.GetUserByUserGuid(regionViewModel.DeputyRegionalManager)?.DisplayName;
            //regionViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(regionViewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            RegionViewModel regionViewModel = new RegionViewModel();
            regionViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(regionViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var regionModel = _regionService.GetDetailsById(id);
            RegionViewModel regionViewModel = new RegionViewModel();
            regionViewModel = Models.ObjectMapper<Region, RegionViewModel>.Map(regionModel);
            regionViewModel.UserSelectListItems = _userService.GetUsers().ToDictionary(x => x.UserGuid, x => x.DisplayName);
            return PartialView(regionViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Add([FromBody]RegionViewModel regionViewModel)
        {
            try
            {
                var regionModel = Models.ObjectMapper<RegionViewModel, Region>.Map(regionViewModel);
                if (_regionService.CheckDuplicates(regionModel) > 0)
                {
                    ModelState.AddModelError("", "Duplicate value entered for either code or name!!");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Guid id = Guid.NewGuid();
                regionModel.RegionGuid = id;
                regionModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                regionModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                regionModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                regionModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                regionModel.IsActive = true;
                regionModel.IsDeleted = false;
                _regionService.Add(regionModel);
                RegionUserRoleMapping regionUserRoleMapping = new RegionUserRoleMapping();
                regionUserRoleMapping.RegionGuid = id;
                regionUserRoleMapping.Keys = "Region";


                if (regionViewModel.BusinessDevelopmentRegionalManager != null)
                {
                    
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.RoleType = ContractUserRole._bdregionalManager;
                    regionUserRoleMapping.UserGuid = regionViewModel.BusinessDevelopmentRegionalManager;
                    _regionService.AddUpdateManager(regionUserRoleMapping);
                }

                if (regionViewModel.HSRegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.RoleType = ContractUserRole._hsregionalManager;
                    regionUserRoleMapping.UserGuid = regionViewModel.HSRegionalManager;
                    _regionService.AddUpdateManager(regionUserRoleMapping);

                }

                if (regionViewModel.DeputyRegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.RoleType = ContractUserRole._deputyregionalManager;
                    regionUserRoleMapping.UserGuid = regionViewModel.DeputyRegionalManager;
                    _regionService.AddUpdateManager(regionUserRoleMapping);

                }
                if (regionViewModel.RegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.RoleType = ContractUserRole._regionalManager;
                    regionUserRoleMapping.UserGuid = regionViewModel.RegionalManager;
                    _regionService.AddUpdateManager(regionUserRoleMapping);

                }


                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.Region.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Region";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Region Name: {2} </br> Region Code: {3}", ResourceType.Region.ToString(), regionModel.RegionGuid, regionModel.RegionName, regionModel.RegionCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), regionModel, resource, regionModel.RegionGuid, UserHelper.GetHostedIp(HttpContext), "Region Added", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromBody]RegionViewModel regionViewModel)
        {
            try
            {
                var regionModel = Models.ObjectMapper<RegionViewModel, Region>.Map(regionViewModel);
                if (_regionService.CheckDuplicates(regionModel) > 0)
                {
                    ModelState.AddModelError("", "Duplicate value entered for either code or name!!");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                regionModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                regionModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                var regiondetails = _regionService.GetById(regionModel.RegionGuid);
                regionModel.IsActive = regiondetails.IsActive;
                regionModel.IsDeleted = regiondetails.IsDeleted;
                regionModel.CreatedBy = regiondetails.CreatedBy;
                regionModel.CreatedOn = regiondetails.CreatedOn;
                _regionService.Edit(regionModel);
                RegionUserRoleMapping regionUserRoleMapping = new RegionUserRoleMapping();
                regionUserRoleMapping.RegionGuid = regionViewModel.RegionGuid;
                regionUserRoleMapping.Keys = "Region";


                if (regionViewModel.BusinessDevelopmentRegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.UserGuid = regionViewModel.BusinessDevelopmentRegionalManager;
                    regionUserRoleMapping.RoleType = ContractUserRole._bdregionalManager;
                    //_regionService.AddUpdateManager(regionUserRoleMapping);
                    _contractsService.UpdateAllUserByRole(regionViewModel.BusinessDevelopmentRegionalManager,ContractUserRole._bdregionalManager);
                }

                if (regionViewModel.HSRegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.UserGuid = regionViewModel.HSRegionalManager;
                    regionUserRoleMapping.RoleType = ContractUserRole._hsregionalManager;
                    //_regionService.AddUpdateManager(regionUserRoleMapping);
                    _contractsService.UpdateAllUserByRole(regionViewModel.HSRegionalManager, ContractUserRole._hsregionalManager);

                }

                if (regionViewModel.DeputyRegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.UserGuid = regionViewModel.DeputyRegionalManager;
                    regionUserRoleMapping.RoleType = ContractUserRole._deputyregionalManager;
                    //_regionService.AddUpdateManager(regionUserRoleMapping);
                    _contractsService.UpdateAllUserByRole(regionViewModel.DeputyRegionalManager, ContractUserRole._deputyregionalManager);

                }
                if (regionViewModel.RegionalManager != null)
                {
                    regionUserRoleMapping.RegionUserRoleMappingGuid = Guid.NewGuid();
                    regionUserRoleMapping.UserGuid = regionViewModel.RegionalManager;
                    regionUserRoleMapping.RoleType = ContractUserRole._regionalManager;
                    //_regionService.AddUpdateManager(regionUserRoleMapping);
                    _contractsService.UpdateAllUserByRole(regionViewModel.RegionalManager, ContractUserRole._regionalManager);
                }
                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.Region.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Region";

                var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                var resource = string.Format("{0} </br> GUID: {1} </br> Region Name: {2} </br> Region Code: {3}", ResourceType.Region.ToString(), regionModel.RegionGuid, regionModel.RegionName, regionModel.RegionCode);

                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), regionModel, resource, regionModel.RegionGuid, UserHelper.GetHostedIp(HttpContext), "Region Edited", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete([FromBody]Guid[] ids)
        {
            try
            {

                foreach (var id in ids)
                {
                    //audit log..
                    var regionModel = _regionService.GetDetailsById(id);
                    if (regionModel != null)
                    {
                        _regionService.DeleteById(id);

                        var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.Region.ToString().ToLower());
                        var resource = string.Format("{0} </br> GUID: {1} </br> Region Name: {2} </br> Region Code: {3}", ResourceType.Region.ToString(), regionModel.RegionGuid, regionModel.RegionName, regionModel.RegionCode);
                        AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), regionModel, resource, regionModel.RegionGuid, UserHelper.GetHostedIp(HttpContext), "Region  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                    }
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Disable([FromBody]Guid[] ids)
        {
            try
            {
                _regionService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var regionModel = _regionService.GetDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.Region.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Region";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Region Name: {2} </br> Region Code: {3}", ResourceType.Region.ToString(), regionModel.RegionGuid, regionModel.RegionName, regionModel.RegionCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, regionModel.RegionGuid, UserHelper.GetHostedIp(HttpContext), "Region Disabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Enable([FromBody]Guid[] ids)
        {
            try
            {
                _regionService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var regionModel = _regionService.GetDetailsById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.Region.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/Region";

                    var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);

                    var resource = string.Format("{0} </br> GUID: {1} </br> Region Name: {2} </br> Region Code: {3}", ResourceType.Region.ToString(), regionModel.RegionGuid, regionModel.RegionName, regionModel.RegionCode);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, regionModel.RegionGuid, UserHelper.GetHostedIp(HttpContext), "Region Disabled", Guid.Empty, "Successful", "", additionalInformationWithUri, additionalInformationURl);
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
