using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels.FarClause;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Areas.Admin.Controllers.FarClause
{
    [Area("Admin")]
    [Authorize]
    public class FarContractTypeClauseController : Controller
    {
        private readonly IFarContractTypeClauseService _farContractTypeClauseService;
        private readonly IFarClauseService _farClauseService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;

        public FarContractTypeClauseController(
            IFarContractTypeClauseService farContractTypeClauseService,
            IFarClauseService farClauseService,
            IFarContractTypeService farContractTypeService,
            IMapper mapper,
            IConfiguration configuration,
            IUrlHelper urlHelper)
        {
            _farContractTypeClauseService = farContractTypeClauseService;
            _farClauseService = farClauseService;
            _farContractTypeService = farContractTypeService;
            _mapper = mapper;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Get(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            try
            {
                var loggedUser = UserHelper.CurrentUserGuid(HttpContext);

                List<FarClauseViewModel> farClauseViewModel = new List<FarClauseViewModel>();
                var farClauses = _farClauseService.GetByAlphabetFilter(searchValue, take, skip, sortField, dir, filterBy);

                foreach (var ob in farClauses)
                {
                    List<FarContractTypeClauseViewModel> lstFarContractTypeClauseViewModel = new List<FarContractTypeClauseViewModel>();
                    FarContractTypeClauseViewModel farContractTypeClauseViewModel = new FarContractTypeClauseViewModel();

                    var farClauseMapped = Models.ObjectMapper<Core.Entities.FarClause, FarClauseViewModel>.Map(ob);
                    var farContractTypeClauseEntity = _farContractTypeClauseService.GetFarContractTypeByFarClauseId(ob.FarClauseGuid);
                    lstFarContractTypeClauseViewModel = _mapper.Map<List<FarContractTypeClauseViewModel>>(farContractTypeClauseEntity);
                    farClauseMapped.FarContractTypeClauseViewModels = lstFarContractTypeClauseViewModel;
                    farClauseViewModel.Add(farClauseMapped);
                }

                return Ok(new { result = farClauseViewModel, count = _farClauseService.TotalRecord(searchValue) });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        public IActionResult GetFarContractTypeByFarClauseId(Guid farClauseId)
        {
            try
            {
                var datas = _farContractTypeClauseService.GetFarContractTypeByFarClauseId(farClauseId);
                var list = new List<FarContractTypeClauseViewModel>();

                foreach (var data in datas)
                {
                    FarContractTypeClauseViewModel viewModel = Mapper<FarContractTypeClause, FarContractTypeClauseViewModel>.Map(data);
                    var farContractType = _farContractTypeService.GetById(viewModel.FarContractTypeGuid);

                    viewModel.Code = farContractType.Code;
                    viewModel.Title = farContractType.Title;
                    viewModel.IsApplicable = data.IsApplicable;
                    viewModel.IsRequired = data.IsRequired;
                    viewModel.IsOptional = data.IsOptional;
                    list.Add(viewModel);
                }

                return Ok(new
                {
                    result = list,
                });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody]FarContractTypeClauseViewModel farContractTypeClauseViewModel)
        {
            try
            {
                var farContractTypeClauseModel = Models.ObjectMapper<FarContractTypeClauseViewModel, FarContractTypeClause>.Map(farContractTypeClauseViewModel);

                ///Checking duplicate far clause number is not applicable because there might have multiple same number with alternative titles..
                if (_farContractTypeClauseService.CheckDuplicateByFarClauseAndFarContractTypeComposition(farContractTypeClauseModel) > 0)
                {
                    var errorMessage = "Duplicate value entered for far clause number and far contract Type code !!";
                    ModelState.AddModelError("", errorMessage);
                    return BadRequestFormatter.BadRequest(this, errorMessage);
                }

                farContractTypeClauseModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                farContractTypeClauseModel.IsDeleted = false;

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _farContractTypeClauseService.Add(farContractTypeClauseModel);

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        public IActionResult Edit([FromBody] FarContractTypeClause data)
        {
            try
            {
                var entity = _farContractTypeClauseService.GetById(data.FarContractTypeClauseGuid);
                entity.IsApplicable = data.IsApplicable;
                entity.IsRequired = data.IsRequired;
                entity.IsOptional = data.IsOptional;
                entity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                _farContractTypeClauseService.Edit(entity);

                return Json(new { status = true });
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