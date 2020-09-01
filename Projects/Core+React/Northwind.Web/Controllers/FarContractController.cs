using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Web.Models.ViewModels.FarClause;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    public class FarContractController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IFarContractService _farContractService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IContractQuestionariesService _contractQuestionariesService;
        private readonly IContractsService _contractService;

        public FarContractController(IFarContractService farContractService,
                                    IFarContractTypeService farContractTypeService,
                                    IContractQuestionariesService contractQuestionariesService,
                                    IConfiguration configuration,
                                    IUserService userService,
                                    IContractsService contractService,
                                    IMapper mapper)
        {
            _contractQuestionariesService = contractQuestionariesService;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
            _farContractTypeService = farContractTypeService;
            _contractService = contractService;
            _farContractService = farContractService;
        }

        [Secure(ResourceType.ContractClauses, ResourceActionPermission.List)]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Secure(ResourceType.ContractClauses, ResourceActionPermission.Add)]
        public ActionResult Add(Guid id)
        {
            try
            {
                var viewModel = GetByContractGuid(id, "Add");
                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return PartialView();
            }
        }

        [HttpGet]
        [Secure(ResourceType.ContractClauses, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            try
            {
                var viewModel = GetByContractGuid(id, "Edit");
                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return PartialView();
            }
        }

        [HttpGet]
        [Secure(ResourceType.ContractClauses, ResourceActionPermission.Details)]
        public ActionResult Detail(Guid id)
        {
            try
            {
                var viewModel = new FarContractViewModel();
                var requiredList = new List<FarContractDetail>();
                var applicableList = new List<FarContractDetail>();
                var contractQuestionary = new ContractQuestionaireViewModel();


                Guid farContractType = _contractService.GetFarContractTypeGuidById(id);
                var farContractModel = _farContractTypeService.GetById(farContractType);
                var requiredListData = _farContractService.GetRequiredData(id, farContractType);
                var applicableListData = _farContractService.GetSelectedData(id, farContractType);
                requiredList = requiredListData;
                applicableList = applicableListData;


                var contractQuestionaireEntity = _contractQuestionariesService.GetContractQuestionariesById(id);
                var contractQuestionaireModel = _mapper.Map<ContractQuestionaireViewModel>(contractQuestionaireEntity);

                viewModel.ContractGuid = id;
                viewModel.ContractQuestionaires = contractQuestionary;
                viewModel.RequiredFarClauses = requiredList;
                viewModel.ApplicableFarClauses = applicableList;
                viewModel.FarContractTypeCode = farContractModel.Code;
                viewModel.FarContractTypeName = farContractModel.Title;

                if (contractQuestionaireModel != null)
                {
                    contractQuestionary = contractQuestionaireModel;
                    var users = _userService.GetUserByUserGuid(contractQuestionaireModel.UpdatedBy);
                    viewModel.UpdatedBy = users != null ? users.DisplayName : "";
                    viewModel.UpdatedOn = contractQuestionaireModel.UpdatedOn;
                    viewModel.ContractQuestionaires = contractQuestionary;

                }
                viewModel.Questionniare = _contractQuestionariesService.GetContractQuestionaries(ResourceType.FarContract.ToString(), "Edit", id).ToList();
                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return PartialView();
            }
        }

        [HttpPost]
        [Secure(ResourceType.ContractClauses, ResourceActionPermission.Add)]
        public ActionResult Add(FarContractViewModel farContractViewModel)
        {
            try
            {
                Save(farContractViewModel);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(e.Message, _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequestFormatter.BadRequest(this,e);
            }
        }

        [HttpPost]
        [Secure(ResourceType.ContractClauses, ResourceActionPermission.Edit)]
        public ActionResult Edit(FarContractViewModel farContractViewModel)
        {
            try
            {
                Save(farContractViewModel);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(e.Message, _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }

        [Authorize]
        public JsonResult GetFarContract(Guid id, int pageSize, int skip, string text)
        {
            Guid farContractType = _contractService.GetFarContractTypeGuidById(id);
            var list = _farContractService.GetAvailableAndOptional(id, farContractType);
            var result = string.Empty;
            if (list?.Any() == true)
            {
                var validData = list.Where(x => !string.IsNullOrEmpty(x.FarClauseNumber));
                var jsonList = validData.Select(x => new
                {
                    Title = x.FarClauseNumber + " (" + x.FarClauseParagraph + ") - " + x.FarClauseTitle,
                    Id = x.FarContractTypeClauseGuid
                });
                return Json(jsonList);
            }

            return Json(result);
        }

        [Authorize]
        public JsonResult GetSelectedFarContract(Guid id)
        {
            Guid farContractType = _contractService.GetFarContractTypeGuidById(id);
            var list = _farContractService.GetSelectedData(id, farContractType);
            var jsonList = list.Select(x => new
            {
                Id = x.FarContractTypeClauseGuid
            });
            return Json(jsonList);
        }

        private ActionResult Save(FarContractViewModel farContractViewModel)
        {
            DateTime CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            Guid CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
            DateTime UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            Guid UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            bool IsDeleted = false;

            if (farContractViewModel.Questionniare != null)
            {
                _contractQuestionariesService.AddQuestionaires(farContractViewModel.Questionniare, farContractViewModel.ContractGuid, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy);
            }

            //// Soft Deletes the previous FAR CONTRACT and then adds new.
            _farContractService.SoftDelete(farContractViewModel.ContractGuid);


            if (farContractViewModel.SelectedValuesList != null)
            {
                foreach (var item in farContractViewModel.SelectedValuesList)
                {
                    var farContract = new FarContract();
                    farContract.ContractGuid = farContractViewModel.ContractGuid;
                    farContract.FarContractTypeClauseGuid = new Guid(item);
                    farContract.IsDeleted = IsDeleted;
                    farContract.CreatedBy = CreatedBy;
                    farContract.CreatedOn = CreatedOn;
                    farContract.UpdatedBy = UpdatedBy;
                    farContract.UpdatedOn = UpdatedOn;
                    _farContractService.Add(farContract);
                }
            }
            //// Hard deletes the soft deleted data..
            _farContractService.Delete(farContractViewModel.ContractGuid);
            return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
        }

        private FarContractViewModel GetByContractGuid(Guid id, string Action)
        {

            var viewModel = new FarContractViewModel();
            var contractQuestionary = new ContractQuestionaireViewModel();

            Guid farContractType = _contractService.GetFarContractTypeGuidById(id);
            var farContractModel = _farContractTypeService.GetById(farContractType);
            var list = _farContractService.GetRequiredData(id, farContractType);
            viewModel.FarContractTypeCode = farContractModel.Code;
            viewModel.FarContractTypeName = farContractModel.Title;
            viewModel.ContractGuid = id;
            contractQuestionary.DictionaryBoolString = KeyValueHelper.GetYesNo();
            viewModel.ContractQuestionaires = contractQuestionary;
            viewModel.RequiredFarClauses = list;
            viewModel.Questionniare = _contractQuestionariesService.GetContractQuestionaries(ResourceType.FarContract.ToString(), Action, id).ToList();

            return viewModel;
        }
    }
}
