using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.FarClause;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FarClauseController : Controller
    {
        private readonly IFarClauseService _farClauseService;
        private readonly IFarContractTypeClauseService _farContractTypeClauseService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly ICommonService _commonService;
        private readonly IStateService _stateService;
        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;
        private readonly IMapper _mapper;


        public FarClauseController(IFarClauseService farClauseService,
                                IFarContractTypeClauseService farContractTypeClauseService,
                                IFarContractTypeService farContractTypeService,
                                ICommonService commonService,
                                IStateService stateService,
                                ICountryService countryService,
                                IUserService userService,
                                IMapper mapper,
                                IConfiguration configuration)
        {
            _farClauseService = farClauseService;
            _mapper = mapper;
            _farContractTypeClauseService = farContractTypeClauseService;
            _farContractTypeService = farContractTypeService;
            _commonService = commonService;
            _countryService = countryService;
            _stateService = stateService;
            _userService = userService;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }
        // GET: far Clause
        [Secure(ResourceType.Admin, ResourceActionPermission.ManageFar)]
        public ActionResult Index(string searchValue)
        {
            return View();
        }

        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);

                List<FarClauseViewModel> FarClauseViewModel = new List<FarClauseViewModel>();
                var farClauses = _farClauseService.GetAll(searchValue, pageSize, skip, take, sortField, dir);

                var result = farClauses.Select(x => new
                {
                    FarClauseGuid = x.FarClauseGuid,
                    Number = x.Number,
                    Title = x.Title,
                    Paragraph = x.Paragraph,
                    IsDeleted = x.IsDeleted,
                    UpdatedBy = x.UpdatedBy == Guid.Empty ? string.Empty : _userService.GetUserByUserGuid(x.UpdatedBy) == null ? "" : _userService.GetUserByUserGuid(x.UpdatedBy).DisplayName
                }).ToList();

                return Ok(new { result, count = _farClauseService.TotalRecord(searchValue) });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this,e);
            }
        }

        public IActionResult GetAll()
        {
            try
            {
                var result = _farClauseService.GetAll();
                return Ok(new { result });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region  Crud

        [HttpGet]
        public ActionResult Add()
        {
            try
            {
                return PartialView(new FarClauseViewModel());
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody]FarClauseViewModel farClauseViewModel)
        {
            try
            {
                var farClauseEntity = Models.ObjectMapper<FarClauseViewModel, Core.Entities.FarClause>.Map(farClauseViewModel);

                ///Checking duplicate far clause number is not applicable because there might have multiple same number with alternative titles..
                //if (_farClauseService.CheckDuplicateFarClauseNumber(farClauseEntity) > 0)
                //{
                //    var errorMessage = "Duplicate value entered for far clause number !!";
                //    ModelState.AddModelError("", errorMessage);
                //    return BadRequest(ModelState);
                //}

                if (ModelState.IsValid)
                {
                    farClauseEntity.FarClauseGuid = UserHelper.GetNewGuid();
                    farClauseEntity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    farClauseEntity.IsDeleted = false;
                    _farClauseService.Add(farClauseEntity);

                    //now get body stream.. to save in FarContractTypeCaluse table..
                    Stream req = Request.Body;
                    req.Seek(0, SeekOrigin.Begin);
                    string json = new StreamReader(req).ReadToEnd();

                    var dictValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    foreach (KeyValuePair<string, object> entry in dictValues)
                    {
                        var farContractType = _farContractTypeService.GetByCode(entry.Key);

                        if (farContractType != null)
                        {
                            FarContractTypeClause farContractTypeCaluseEntity = new FarContractTypeClause();

                            farContractTypeCaluseEntity.FarClauseGuid = farClauseEntity.FarClauseGuid;
                            farContractTypeCaluseEntity.FarContractTypeGuid = farContractType.FarContractTypeGuid;

                            farContractTypeCaluseEntity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                            farContractTypeCaluseEntity.IsDeleted = false;

                            farContractTypeCaluseEntity.IsRequired = false;
                            farContractTypeCaluseEntity.IsApplicable = false;
                            farContractTypeCaluseEntity.IsOptional = false;

                            if (entry.Value.Equals("Required"))
                            {
                                farContractTypeCaluseEntity.IsRequired = true;
                            }
                            else if (entry.Value.Equals("Applicable"))
                            {
                                farContractTypeCaluseEntity.IsApplicable = true;
                            }
                            else if (entry.Value.Equals("Optional"))
                            {
                                farContractTypeCaluseEntity.IsOptional = true;
                            }
                            _farContractTypeClauseService.Add(farContractTypeCaluseEntity);
                        }
                        //edit here
                    }

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), "Far Clause");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Admin/FarClause");

                    string[] resourceTitles = { "ResourceTitle", "GUID", "Number", "Title" };
                    string[] resourceValues = { "Far Clause", farClauseEntity.FarClauseGuid.ToString(), farClauseEntity.Number, farClauseEntity.Title };
                    var resource = Infrastructure.Helpers.FormatHelper.AuditLogResourceFormat(resourceTitles, resourceValues);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), farClauseEntity, resource, farClauseEntity.FarClauseGuid, UserHelper.GetHostedIp(HttpContext), "Far Clause Added", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                    //end of log..

                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            FarClauseViewModel farClauseViewModel = new FarClauseViewModel();

            if (id != Guid.Empty)
            {
                var farClauseEntity = _farClauseService.GetById(id);
                farClauseViewModel = Models.ObjectMapper<Core.Entities.FarClause, FarClauseViewModel>.Map(farClauseEntity);
            }
            try
            {
                return PartialView(farClauseViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(farClauseViewModel);
            }
        }


        [HttpPost]
        public IActionResult Edit([FromBody]FarClauseViewModel farClauseViewModel)
        {
            try
            {
                var farClauseEntity = Models.ObjectMapper<FarClauseViewModel, Core.Entities.FarClause>.Map(farClauseViewModel);

                if (ModelState.IsValid)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    farClauseEntity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                    //update the farclause..
                    _farClauseService.Edit(farClauseEntity);

                    //now get body stream.. to edit FarContractTypeCaluse table....
                    Stream req = Request.Body;
                    req.Seek(0, SeekOrigin.Begin);
                    string json = new StreamReader(req).ReadToEnd();

                    var dictValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    foreach (KeyValuePair<string, object> entry in dictValues)
                    {
                        var farContractType = _farContractTypeService.GetByCode(entry.Key);
                        if (farContractType != null)
                        {
                            var farContractTypeCaluse = _farContractTypeClauseService.GetByFarClauseFarContractTypeGuid(farClauseViewModel.FarClauseGuid, farContractType.FarContractTypeGuid);

                            if (farContractTypeCaluse == null)
                            {
                                //add if there is no farcontracttypeclause record..
                                FarContractTypeClause farContractTypeCaluseEntity = new FarContractTypeClause
                                {
                                    FarClauseGuid = farClauseEntity.FarClauseGuid,
                                    FarContractTypeGuid = farContractType.FarContractTypeGuid,
                                    UpdatedBy = UserHelper.CurrentUserGuid(HttpContext),
                                    IsDeleted = false,
                                    IsRequired = false,
                                    IsApplicable = false,
                                    IsOptional = false
                                };

                                if (entry.Value.Equals("Required"))
                                {
                                    farContractTypeCaluseEntity.IsRequired = true;
                                }
                                else if (entry.Value.Equals("Applicable"))
                                {
                                    farContractTypeCaluseEntity.IsApplicable = true;
                                }
                                else if (entry.Value.Equals("Optional"))
                                {
                                    farContractTypeCaluseEntity.IsOptional = true;
                                }
                                _farContractTypeClauseService.Add(farContractTypeCaluseEntity);
                                continue;
                            }
                            farContractTypeCaluse.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                            farContractTypeCaluse.IsDeleted = false;

                            farContractTypeCaluse.IsRequired = false;
                            farContractTypeCaluse.IsApplicable = false;
                            farContractTypeCaluse.IsOptional = false;

                            if (entry.Value.Equals("Required"))
                            {
                                farContractTypeCaluse.IsRequired = true;
                            }
                            else if (entry.Value.Equals("Applicable"))
                            {
                                farContractTypeCaluse.IsApplicable = true;
                            }
                            else if (entry.Value.Equals("Optional"))
                            {
                                farContractTypeCaluse.IsOptional = true;
                            }
                            _farContractTypeClauseService.Edit(farContractTypeCaluse);
                        }
                        //edit here
                    }

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), "Far Clause");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Admin/FarClause");

                    string[] resourceTitles = { "ResourceTitle", "GUID", "Number", "Title" };
                    string[] resourceValues = { "Far Clause", farClauseEntity.FarClauseGuid.ToString(), farClauseEntity.Number, farClauseEntity.Title };
                    var resource = FormatHelper.AuditLogResourceFormat(resourceTitles, resourceValues);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), farClauseEntity, resource, farClauseEntity.FarClauseGuid, UserHelper.GetHostedIp(HttpContext), "Far Clause Edited", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                    //end of log..

                    return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
                }
                else
                {
                    return View(farClauseEntity);
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex); ;
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            FarClauseViewModel farClauseViewModel = new FarClauseViewModel();
            List<FarContractTypeClauseViewModel> lstFarContractTypeClauseViewModel = new List<FarContractTypeClauseViewModel>();
            try
            {
                if (id != Guid.Empty)
                {
                    var farClauseEntity = _farClauseService.GetById(id);
                    farClauseViewModel = Models.ObjectMapper<Core.Entities.FarClause, FarClauseViewModel>.Map(farClauseEntity);
                }
                try
                {
                    return PartialView(farClauseViewModel);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    return View(farClauseViewModel);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(farClauseViewModel);
            }
        }

        [HttpPost]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                var updatedBy = UserHelper.CurrentUserGuid(HttpContext);
                foreach (var id in ids)
                {
                    _farClauseService.Delete(id, updatedBy);

                    //audit log..
                    var farClauseEntity = _farClauseService.GetById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), "Far Clause");

                    string[] resourceTitles = { "ResourceTitle", "GUID", "Number", "Title" };
                    string[] resourceValues = { "Far Clause", farClauseEntity.FarClauseGuid.ToString(), farClauseEntity.Number, farClauseEntity.Title };
                    var resource = FormatHelper.AuditLogResourceFormat(resourceTitles, resourceValues);

                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), farClauseEntity, resource, farClauseEntity.FarClauseGuid, UserHelper.GetHostedIp(HttpContext), "Far Clause Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                    //end of log..
                }

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
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
