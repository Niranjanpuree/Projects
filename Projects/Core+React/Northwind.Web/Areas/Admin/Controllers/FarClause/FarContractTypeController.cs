using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Services;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Models.ViewModels.FarClause;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Models;

namespace Northwind.Web.Areas.Admin.Controllers.FarClause
{
    [Area("Admin")]
    [Authorize]
    public class FarContractTypeController : Controller
    {
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public FarContractTypeController(
            IFarContractTypeService farContractTypeService,
            IMapper mapper,
            IConfiguration configuration,
            IUrlHelper urlHelper)
        {
            _farContractTypeService = farContractTypeService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [Secure(ResourceType.Admin, ResourceActionPermission.ManageFar)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            try
            {
                string trimmedSearchText = string.IsNullOrEmpty(searchValue) ? "" : searchValue.Trim();
                var model = _farContractTypeService.GetAll(trimmedSearchText, pageSize, skip, take, sortField, dir);
                var count = _farContractTypeService.TotalRecord(trimmedSearchText);

                List<FarContractTypeViewModel> result = new List<FarContractTypeViewModel>();
                foreach (var item in model)
                {
                    var mapping = _mapper.Map<FarContractTypeViewModel>(item);
                    result.Add(mapping);
                }
                return Ok(new { result, count = count });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        public IActionResult GetAll()
        {
            try
            {
                var result = _farContractTypeService.GetAll();
                //var mapping = _mapper.Map<FarContractTypeViewModel>(result);

                List<FarContractTypeViewModel> lsttFarContractTypeViewModel = new List<FarContractTypeViewModel>();
                foreach (var item in result)
                {
                    var mapping = _mapper.Map<FarContractTypeViewModel>(item);
                    lsttFarContractTypeViewModel.Add(mapping);
                }
                return Ok(new { result = lsttFarContractTypeViewModel });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region Crud
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            var farContractTypeModel = _farContractTypeService.GetById(id);
            FarContractTypeViewModel farContractTypeViewModel = new FarContractTypeViewModel();
            farContractTypeViewModel = Models.ObjectMapper<FarContractType, FarContractTypeViewModel>.Map(farContractTypeModel);
            return PartialView(farContractTypeViewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            FarContractTypeViewModel FarContractTypeViewModel = new FarContractTypeViewModel();
            return PartialView(FarContractTypeViewModel);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var FarContractTypeModel = _farContractTypeService.GetById(id);
            FarContractTypeViewModel FarContractTypeViewModel = new FarContractTypeViewModel();
            FarContractTypeViewModel = Models.ObjectMapper<FarContractType, FarContractTypeViewModel>.Map(FarContractTypeModel);
            return PartialView(FarContractTypeViewModel);
        }

        [HttpPost]
        public IActionResult Add([FromBody]FarContractTypeViewModel farContractTypeViewModel)
        {
            try
            {
                var farContractTypeModel = Models.ObjectMapper<FarContractTypeViewModel, FarContractType>.Map(farContractTypeViewModel);
                farContractTypeModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                farContractTypeModel.IsDeleted = false;
                farContractTypeModel.FarContractTypeGuid = Guid.NewGuid();
                if (_farContractTypeService.CheckDuplicate(farContractTypeModel) > 0)
                {
                    throw new ArgumentException("Duplicate value entered for code !!");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _farContractTypeService.Add(farContractTypeModel);
                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.FarContractType.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/FarContractType";
                string[] resourceTitles = { "Code", "Title" };
                string[] resourceValues = { farContractTypeModel.Code, farContractTypeModel.Title };
                var resource = FormatHelper.AuditLogResourceFormat(resourceTitles, resourceValues);
                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), farContractTypeModel, resource, Guid.Empty, UserHelper.GetHostedIp(HttpContext), "FarContractType Added", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);

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
        public IActionResult Edit([FromBody]FarContractTypeViewModel farContractTypeViewModel)
        {
            try
            {
                var farContractTypeModel = Models.ObjectMapper<FarContractTypeViewModel, FarContractType>.Map(farContractTypeViewModel);
                if (_farContractTypeService.CheckDuplicate(farContractTypeModel) > 0)
                {
                    throw new ArgumentException("Duplicate value entered for code !!");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                farContractTypeModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                _farContractTypeService.Update(farContractTypeModel);

                //audit log..
                var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.FarContractType.ToString());
                var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + "/admin/FarContractType";
                string[] resourceTitles = { "Code", "Title" };
                string[] resourceValues = { farContractTypeModel.Code, farContractTypeModel.Title };
                var resource = FormatHelper.AuditLogResourceFormat(resourceTitles, resourceValues);
                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), farContractTypeModel, resource, Guid.Empty, UserHelper.GetHostedIp(HttpContext), "FarContractType Edited", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);

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
        public ActionResult Delete([FromBody]Guid[] idArr)
        {
            try
            {
                foreach (var id in idArr)
                {
                    _farContractTypeService.Delete(id);
                }
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        #endregion
    }
}