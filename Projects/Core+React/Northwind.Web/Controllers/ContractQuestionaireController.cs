using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Models.ViewModels.Contract;

namespace Northwind.Web.Controllers
{
    public class ContractQuestionaireController : Controller
    {
        private readonly IContractQuestionariesService _contractQuestionariesService;
        private readonly IMapper _mapper;
        public ContractQuestionaireController(
            IContractQuestionariesService contractQuestionariesService,
            IMapper mapper
            )
        {
            _contractQuestionariesService = contractQuestionariesService;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult Add(Guid id)
        {
            try
            {
                var contractQuestionaireModel = new ContractQuestionaireViewModel();
                contractQuestionaireModel.ContractGuid = id;
                contractQuestionaireModel.radioCPARS = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioFARClause = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioGovFurnishedProperty = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioGQAC = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioGSAschedule = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioReportingExecCompensation = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioSBsubcontracting = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioServiceContractReport = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioWarranties = KeyValueHelper.GetYesNo();
                contractQuestionaireModel.radioWarrantyProvision = KeyValueHelper.GetYesNo();
                return PartialView(contractQuestionaireModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var contractQuestionaireEntity = _contractQuestionariesService.GetContractQuestionariesById(id);
            var contractQuestionaireModel = _mapper.Map<ContractQuestionaireViewModel>(contractQuestionaireEntity);
            contractQuestionaireModel.radioWarrantyProvision = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioCPARS = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioFARClause = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGovFurnishedProperty = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGQAC = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGSAschedule = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioReportingExecCompensation = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioSBsubcontracting = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioServiceContractReport = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioWarranties = KeyValueHelper.GetYesNo();
            try
            {
                return PartialView(contractQuestionaireModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult Detail(Guid id)
        {
            var contractQuestionaireEntity = _contractQuestionariesService.GetContractQuestionariesById(id);
            var contractQuestionaireModel = _mapper.Map<ContractQuestionaireViewModel>(contractQuestionaireEntity);
            contractQuestionaireModel.radioWarrantyProvision = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioCPARS = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioFARClause = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGovFurnishedProperty = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGQAC = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioGSAschedule = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioReportingExecCompensation = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioSBsubcontracting = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioServiceContractReport = KeyValueHelper.GetYesNo();
            contractQuestionaireModel.radioWarranties = KeyValueHelper.GetYesNo();
            try
            {
                return PartialView(contractQuestionaireModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult Add([FromBody]ContractQuestionaireViewModel contractQuestionaire)
        {
            try
            {
                Guid id = Guid.NewGuid();
                contractQuestionaire.ContractQuestionaireGuid = id;
                contractQuestionaire.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractQuestionaire.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractQuestionaire.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                contractQuestionaire.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                contractQuestionaire.IsActive = true;
                contractQuestionaire.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var questionaireEntity = _mapper.Map<ContractQuestionaire>(contractQuestionaire);
                _contractQuestionariesService.AddContractQuestionaires(questionaireEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult Edit([FromBody]ContractQuestionaireViewModel contractQuestionaire)
        {
            try
            {
                contractQuestionaire.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                contractQuestionaire.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var questionaireEntity = _mapper.Map<ContractQuestionaire>(contractQuestionaire);
                _contractQuestionariesService.UpdateContractQuestionairesById(questionaireEntity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}