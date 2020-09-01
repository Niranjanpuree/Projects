using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Core.Utilities;
using Northwind.Core.ViewModels;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Web.Helpers;
using Northwind.Web.Models;
using static Northwind.Core.Entities.EnumGlobal;
using System.IO;
//using Northwind.Web.Models.ViewModels.Contract;

namespace Northwind.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IContractService _contractService;
        private readonly ICommonService _commonService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IConfiguration _configuration;
        private readonly IJobRequestService _jobRequestService;
        private readonly IMapper _mapper;
        public ContractController(IContractService contractService,
            ICommonService commonService,
            IContractModificationService contractModificationService,
            IConfiguration configuration,
            IJobRequestService jobRequestService,
            IResourceAttributeValueService resourceAttributeValueService,
            IFileService fileService,
            IMapper mapper,
            IHostingEnvironment env)
        {
            _fileService = fileService;
            _contractService = contractService;
            _jobRequestService = jobRequestService;
            _commonService = commonService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _contractModificationService = contractModificationService;
            _configuration = configuration;
            _mapper = mapper;
        }
        // GET: Contract
        public ActionResult Index(string SearchValue, int FilterList, bool ShowHideTaskOrder)
        {
            var refineSerchVal = string.Empty;
            if (!string.IsNullOrEmpty(SearchValue))
            {
                var splittedVal = SearchValue.Split(' ');
                refineSerchVal = string.Join(" ", splittedVal.Where(x => !string.IsNullOrEmpty(x)));
            }
            ContractViewModel ContractViewModel = new ContractViewModel();
            ContractViewModel.SearchValue = refineSerchVal;
            ContractViewModel.idFilterList = FilterList;
            ContractViewModel.ShowHideTaskOrder = ShowHideTaskOrder;
            return View(ContractViewModel);
        }
        public IActionResult Get(string SearchValue, int FilterList, bool ShowHideTaskOrder, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                List<Northwind.Web.Models.ViewModels.Contract.ContractViewModel> contractViewModel = new List<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>();
                int totalRecordCount = 0;
                var IsIDIQContract = true;

                //if (ShowHideTaskOrder)
                //{
                //    contractViewModel = _contractService.GetAllWithTaskOrder(SearchValue, IsIDIQContract, FilterList, pageSize, skip, sortField, sortDirection).ToList();
                //    contractViewModel = contractViewModel.Skip(skip).Take(pageSize).ToList();
                //    totalRecordCount = contractViewModel.Count;
                //}
                //else
                //{
                //    IsIDIQContract = false;
                //    contractViewModel = _contractService.GetAll(SearchValue, IsIDIQContract, FilterList, pageSize, skip, sortField, sortDirection).ToList();
                //    totalRecordCount = _contractService.TotalRecord();
                //}

                //added for viewmodel
                //edited for mapping view model with entity model
                var model = new List<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>();
                //IList<Northwind.Web.Models.ViewModels.Contract.ContractViewModel> model;
                if (ShowHideTaskOrder)
                {
                    var contractList = _contractService.GetAllWithTaskOrder(SearchValue, IsIDIQContract, FilterList, pageSize, skip, sortField, sortDirection).ToList();
                    contractViewModel = _mapper.Map<List<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>>(contractList);
                    model = model.Skip(skip).Take(pageSize).ToList();
                    totalRecordCount = contractViewModel.Count;
                }
                else
                {
                    IsIDIQContract = false;
                    var contractList = _contractService.GetAll(SearchValue, IsIDIQContract, FilterList, pageSize, skip, sortField, sortDirection).ToList();
                    contractViewModel = _mapper.Map<List<Northwind.Web.Models.ViewModels.Contract.ContractViewModel>>(contractList);
                    totalRecordCount = contractViewModel.Count;
                }
                //end of edited

                //for other filters..
                //                switch (FilterList)
                //                {
                //                    case 1:
                //                        IsIDIQContract = false;
                //                        contractViewModel = _contractService.GetAll(SearchValue, IsIDIQContract, FilterList, pageSize, skip, sortField, sortDirection).ToList();
                //                        totalRecordCount = _contractService.TotalRecord();
                //                        break;
                //                }

                var data = contractViewModel.Select(x => new
                {
                    //basic info..
                    ContractGuid = x.ContractGuid,
                    ContractNumber = x.ContractNumber,
                    ContractTitle = x.ContractTitle,
                    ProjectNumber = x.ProjectNumber,
                    AwardAmount = x.AwardAmount.ToString("C", new CultureInfo("en-US")),
                    POPStart = x.Periodofperformancestart.ToString("MM/dd/yyyy"),
                    POPEnd = x.Periodofperformanceend.ToString("MM/dd/yyyy"),
                    IDIQContract = x.IDIQContract,
                    IsActiveStatus = x.IsActiveStatus,
                    PaymentTerms = x.PaymentTerms,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy"),
                    IsContract = x.IsContract,

                    Description = x.Description,
                    SubContractNumber = x.SubContractNumber,
                    CountryOfPerformance = x.CountryOfPerformance,
                    PlaceOfPerformance = x.PlaceOfPerformance,
                    NaicsCodeName = x.NAICSCodeName,
                    PscCodeName = x.PSCCodeName,
                    OrganizationName = x.OrganizationName,

                    //key personal info..
                    CompanyPresidentName = x.CompanyPresidentName,
                    RegionalManagerName = x.RegionalManagerName,
                    ProjectManagerName = x.ProjectManagerName,
                    ProjectControlName = x.ProjectControlName,
                    AccountingRepresentativeName = x.AccountingRepresentativeName,
                    ContractRepresentativeName = x.ContractRepresentativeName,

                    //customer info..
                    AwardingAgencyOfficeName = x.AwardingAgencyOfficeName,
                    FundingAgencyOfficeName = x.FundingAgencyOfficeName,

                    //financial info..
                    ContractTypeName = x.ContractTypeName,
                    OverHead = x.OverHead,
                    G_A_Percent = x.G_A_Percent,
                    Fee_Percent = x.Fee_Percent,
                    CurrencyName = x.CurrencyName,
                    BlueSkyAward_Amount = x.BlueSkyAward_Amount.ToString("C", new CultureInfo("en-US")),
                    InvoiceSubmissionMethod = x.InvoiceSubmissionMethod,

                }).ToList();
                return Json(new { total = totalRecordCount, data = data });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        public IActionResult GetContracts(string SearchValue, int pageSize, int skip, int take, string sortField, string dir, int switchOn = 0)
        {
            try
            {
                
                if (switchOn == 0)
                {
                    var contracts = _contractService.GetContract(SearchValue, pageSize, skip, take, sortField, dir);
                    List<Models.ViewModels.Contract.ContractAndProjectView> result = new List<Models.ViewModels.Contract.ContractAndProjectView>();
                    foreach (var contract in contracts)
                    {
                        var mapVal = Mapper<ContractForList, Models.ViewModels.Contract.ContractAndProjectView>.Map(contract);
                        result.Add(mapVal);
                    }
                    return Ok(new { result, count = _contractService.GetContractCount(SearchValue) });
                }
                else
                {
                    var result = _contractService.GetContract(SearchValue, pageSize, skip, take, sortField, dir);
                    var list = new List<Models.ViewModels.Contract.ContractAndProjectView>();
                    foreach (var contract in result)
                    {
                        var cont = Mapper<ContractForList, Models.ViewModels.Contract.ContractAndProjectView>.Map(contract);
                        cont.IsContract = true;
                        list.Add(cont);
                        var projects = _contractService.GetProjectByContractGuid(cont.ContractGuid);
                        projects = projects.OrderByDescending(c => c.ProjectNumber).ToList();
                        foreach (var prj in projects)
                        {
                            var prjView = Mapper<ProjectForList, Models.ViewModels.Contract.ContractAndProjectView>.Map(prj);
                            prjView.ContractTitle = prj.ContratTitle;
                            prjView.ContractNumber = cont.ContractNumber;
                            prjView.IsActiveStatus = prjView.IsActive ? ActiveStatus.Active.ToString() : ActiveStatus.Inactive.ToString();
                            prjView.IsContract = false;
                            list.Add(prjView);
                        }
                    }
                    return Ok(new { result = list, count = _contractService.GetContractCount(SearchValue) });
                }

            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }

        public ActionResult GetContractWithMod(Guid contractGuid)
        {
            var list = new List<ModListModel>();
            try
            {
                var contracts = _contractService.GetDetailsForProjectByContractId(contractGuid);
                if (contracts != null)
                {
                    var contract = contracts;
                    var proj = new ModListModel
                    {

                        Amount = contract.FinancialInformation.AwardAmount.Value,
                        ContractNumber = contract.ContractNumber,
                        ProjectNumber = contract.BasicContractInfo.ProjectNumber,
                        IsMod = false,
                        Id = contract.ContractGuid,
                        Mod = "Base",
                        DateEntered = "",
                        EffectiveDate = "",
                        Title = contract.BasicContractInfo.ContractTitle, // project title..
                        StartDate = contract.BasicContractInfo.POPStart.HasValue && contract.BasicContractInfo.POPStart.Value.Year > 1900 ? contract.BasicContractInfo.POPStart.Value.ToString("MM/dd/yyyy") : "",
                        EndDate = contract.BasicContractInfo.POPEnd.HasValue && contract.BasicContractInfo.POPEnd.Value.Year > 1900 ? contract.BasicContractInfo.POPEnd.Value.ToString("MM/dd/yyyy") : "",
                        Status = contract.IsActive
                    };

                    list.Add(proj);

                    var modList = _contractModificationService.GetAll(contract.ContractGuid, "", 100000, 0, "ModificationNumber", "desc");
                    modList = modList.OrderByDescending(c => c.ModificationNumber);
                    foreach (var mod in modList)
                    {
                        var item = new ModListModel
                        {
                            Amount = mod.AwardAmount.HasValue ? mod.AwardAmount.Value : 0,
                            ContractNumber = contract.ContractNumber,
                            ProjectNumber = mod.ProjectNumber,
                            IsMod = true,
                            Id = mod.ContractModificationGuid,
                            Mod = mod.ModificationNumber,
                            Title = mod.ModificationTitle,
                            StartDate = mod.POPStart.HasValue && mod.POPStart.Value.Year > 1900 ? mod.POPStart.Value.ToString("MM/dd/yyyy") : "",
                            EndDate = mod.POPEnd.HasValue && mod.POPEnd.Value.Year > 1900 ? mod.POPEnd.Value.ToString("MM/dd/yyyy") : "",
                            DateEntered = mod.EnteredDate.HasValue && mod.EnteredDate.Value.Year > 1900 ? mod.EnteredDate.Value.ToString("MM/dd/yyyy") : "",
                            EffectiveDate = mod.EffectiveDate.HasValue && mod.EffectiveDate.Value.Year > 1900 ? mod.EffectiveDate.Value.ToString("MM/dd/yyyy") : "",
                            Status = mod.IsActive
                        };
                        list.Add(item);

                    }

                }
                return Ok(new { result = list, count = 1 });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ModelState);
            }
        }

        #region  Crud

        [HttpGet]
        public ActionResult Add()
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            BasicContractInfoViewModel basicContractInfoViewModel = new BasicContractInfoViewModel();
            FinancialInformationViewModel FinancialInformationViewModel = new FinancialInformationViewModel();
            KeyPersonnelViewModel keyPersonnelViewModel = new KeyPersonnelViewModel(); //just initialize ..

            var customerAttributeValuesViewModel = InitialLoad();

            basicContractInfoViewModel.IsIDIQContract = true;
            basicContractInfoViewModel.IsPrimeContract = true;
            basicContractInfoViewModel.CPAREligible = true;
            basicContractInfoViewModel.QualityLevelRequirements = true;

            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            contractViewModel.BasicContractInfo = basicContractInfoViewModel;
            contractViewModel.FinancialInformation = FinancialInformationViewModel;
            contractViewModel.KeyPersonnel = keyPersonnelViewModel;

            try
            {
                return View("Add", contractViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractViewModel);
            }
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {

            //            Guid LoginUserGuid = new Guid("5c982b44-e168-4a56-84ed-75ec2d4a6fa9"); //contract representative 
            //                                                                                   Guid LoginUserGuid = new Guid("fb6abca6-04f5-4e5f-94da-5418ae6881cd"); //project control
            Guid LoginUserGuid = new Guid("d059dd0c-6e29-4198-8314-33ae04b3dcd2"); //project manager
                                                                                   //            Guid LoginUserGuid = new Guid("f00946ff-32cc-4d3a-ac33-892b0ec548f8"); //account review

            var contractModel = _contractService.GetInfoById(id);
            //            var jobRequestStatus = _jobRequestService.GetJobRequestStatusByUserId(LoginUserGuid);

            if (contractModel.KeyPersonnel.ContractRepresentative == LoginUserGuid)
            {
                contractModel.isPermissionJobRequest = true;
            }
            else if (contractModel.JobRequest != null)
            {
                contractModel.isPermissionJobRequest = _commonService.CheckReviewStage(contractModel.JobRequest.JobRequestGuid, LoginUserGuid);
            }

            // Untill the Login Session is Made the permission is set to true
            contractModel.isPermissionJobRequest = true;

            contractModel.ModuleType = EnumGlobal.ModuleType.Contract;
            try
            {
                return View("Details", contractModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractModel);
            }
        }
        [HttpPost]
        public IActionResult Add(ContractViewModel contractViewModel)
        {
            try
            {

                var isExistContractNumber =
                    _contractService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber);

                if (isExistContractNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Contract Number !!");
                }
                var isExistProjectNumber =
                           _contractService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber);

                if (isExistProjectNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Project Number !!");
                }
                if (ModelState.IsValid)
                {
                    var contractModel = MapContractModel.Map(contractViewModel);

                    Guid id = Guid.NewGuid();
                    contractModel.ContractGuid = id;
                    contractModel.CreatedOn = DateTime.Now;
                    contractModel.CreatedBy = id;
                    contractModel.UpdatedOn = DateTime.Now;
                    contractModel.UpdatedBy = id;
                    contractModel.IsActive = true;
                    contractModel.IsDeleted = false;
                    _contractService.AddContract(contractModel);
                    return RedirectToAction("Details", new { id = contractModel.ContractGuid });
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                ModelState.AddModelError("", " Some error occurred !!");
            }
            // in any case return this if not success..
            var customerAttributeValuesViewModel = InitialLoad();

            contractViewModel.BasicContractInfo.IsIDIQContract = true;
            contractViewModel.BasicContractInfo.IsPrimeContract = true;
            contractViewModel.BasicContractInfo.CPAREligible = true;
            contractViewModel.BasicContractInfo.QualityLevelRequirements = true;

            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();

            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            return View(contractViewModel);
        }



        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            //ContractViewModel contractViewModel = new ContractViewModel();
            //if (id != Guid.Empty)
            //{
            //    contractViewModel = _contractService.GetDetailsById(id);
            //}

            //later added for view model
            ContractViewModel contractViewModel = new ContractViewModel();
            if (id != Guid.Empty)
            {
                var contract = _contractService.GetDetailsById(id);
                contractViewModel = _mapper.Map<ContractViewModel>(contract);
            }
            //end of modification for model

            var customerAttributeValuesViewModel = InitialLoad();
            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            try
            {
                return View("Edit", contractViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractViewModel);
            }
        }
        [HttpPost]
        public IActionResult Edit(ContractViewModel contractViewModel)
        {
            try
            {
                //check for duplicate modification number..
                var contractDetail = _contractService.GetDetailsById(contractViewModel.ContractGuid);

                if (contractDetail != null)
                {
                    if (!contractDetail.BasicContractInfo.ContractNumber.Equals(contractViewModel.BasicContractInfo.ContractNumber))
                    {
                        if (_contractService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber))
                        {
                            ModelState.AddModelError("", " Found Duplicate Contract Number !!");
                        }
                    }
                    if (!contractDetail.BasicContractInfo.ProjectNumber.Equals(contractViewModel.BasicContractInfo.ProjectNumber))
                    {
                        if (_contractService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber))
                        {
                            ModelState.AddModelError("", " Found Duplicate Project Number !!");
                        }
                    }
                }

                if (ModelState.IsValid)
                {

                    var contractModel = MapContractModel.Map(contractViewModel);

                    Guid id = Guid.NewGuid();
                    contractModel.UpdatedOn = DateTime.Now;
                    contractModel.UpdatedBy = id;
                    _contractService.UpdateContract(contractModel);
                    return RedirectToAction("Details", new { id = contractModel.ContractGuid });
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                ModelState.AddModelError("", " Some error occurred !!");
            }
            // in any case return this if not success..
            var customerAttributeValuesViewModel = InitialLoad();

            contractViewModel.BasicContractInfo.IsIDIQContract = true;
            contractViewModel.BasicContractInfo.IsPrimeContract = true;
            contractViewModel.BasicContractInfo.CPAREligible = true;
            contractViewModel.BasicContractInfo.QualityLevelRequirements = true;

            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            contractViewModel.CountrySelectListItems = _commonService.GetCountryDropDown();

            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            return View(contractViewModel);
        }

        private CustomerAttributeValuesViewModel InitialLoad()
        {
            CustomerAttributeValuesViewModel customerAttributeValuesViewModel = new CustomerAttributeValuesViewModel();

            customerAttributeValuesViewModel.ddlBillingFrequency =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "BillingFrequency");
            customerAttributeValuesViewModel.ddlContractType =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "ContractType");
            customerAttributeValuesViewModel.ddlQualityLevel =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "QualityLevel");
            customerAttributeValuesViewModel.ddlCurrency =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "Currency");
            customerAttributeValuesViewModel.ddlInvoiceSubmissionMethod =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "InvoiceSubmissionMethod");
            customerAttributeValuesViewModel.ddlPaymentTerms =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "PaymentTerms");
            customerAttributeValuesViewModel.ddlSetAside =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "SetAside");
            customerAttributeValuesViewModel.radioCompetition =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "Competition");
            customerAttributeValuesViewModel.radioAppWageDetermine_DavisBaconAct =
                _resourceAttributeValueService.GetDropDownByValue("DavisBaconAct");
            customerAttributeValuesViewModel.radioAppWageDetermine_ServiceContractAct =
                _resourceAttributeValueService.GetDropDownByValue("ServiceContractAct");
            customerAttributeValuesViewModel.radioCPAREligible = _commonService.GetYesNo();
            customerAttributeValuesViewModel.radioIsIDIQContract = _commonService.GetYesNo();
            customerAttributeValuesViewModel.radioIsPrimeContract = _commonService.GetYesNo();
            customerAttributeValuesViewModel.radioQualityLevelRequirements = _commonService.GetYesNo();
            customerAttributeValuesViewModel.radioSBA = _commonService.GetYesNo();
            return customerAttributeValuesViewModel;
        }

        [HttpPost]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _contractService.DeleteContract(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpPost]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _contractService.DisableContract(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpPost]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _contractService.EnableContract(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #endregion

        [HttpGet]
        public IActionResult GetStatesByCountryId(Guid countryId)
        {
            try
            {
                var states = _commonService.GetStateByCountryDropDown(countryId);

                var stateWithGrtOrNot = states.Select(x => new
                { Keys = x.Keys, Values = x.Descriptions == true ? x.Values + " (GRT)" : x.Values });
                List<SelectListItem> lst = new List<SelectListItem>();
                foreach (var s in stateWithGrtOrNot)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Text = s.Values;
                    selectList.Value = s.Keys.ToString();
                    lst.Add(selectList);
                }
                //                return Json(new { states });
                return Json(lst);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpGet]
        public IActionResult GetSelectedStatesByStateIds(string statesIds)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                if (!string.IsNullOrEmpty(statesIds))
                {
                    var statesIdStringArray = statesIds.Split(',');
                    var states = _commonService.GetSelectedStatesByStateIds(statesIdStringArray);
                    var stateWithGrtOrNot = states.Select(x => new
                    { Keys = x.Keys, Values = x.Descriptions == true ? x.Values + " (GRT)" : x.Values });

                    foreach (var s in stateWithGrtOrNot)
                    {
                        SelectListItem selectList = new SelectListItem();
                        selectList.Text = s.Values;
                        selectList.Value = s.Keys.ToString();
                        lst.Add(selectList);
                    }
                }
                return Json(lst);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        #region FormsInDetails
        #region FarClause
        [HttpGet]
        public ActionResult _AddCustomerQuestionaire(Guid id)
        {
            try
            {
                ContractQuestionaire contractQuestionaire = new ContractQuestionaire();
                contractQuestionaire.ContractGuid = id;
                contractQuestionaire.radioCPARS = _commonService.GetYesNo();
                contractQuestionaire.radioFARClause = _commonService.GetYesNo();
                contractQuestionaire.radioGovFurnishedProperty = _commonService.GetYesNo();
                contractQuestionaire.radioGQAC = _commonService.GetYesNo();
                contractQuestionaire.radioGSAschedule = _commonService.GetYesNo();
                contractQuestionaire.radioReportingExecCompensation = _commonService.GetYesNo();
                contractQuestionaire.radioSBsubcontracting = _commonService.GetYesNo();
                contractQuestionaire.radioServiceContractReport = _commonService.GetYesNo();
                contractQuestionaire.radioWarranties = _commonService.GetYesNo();
                contractQuestionaire.radioWarrantyProvision = _commonService.GetYesNo();
                return PartialView("FormsInDetail/_FormContractQuestionaire", contractQuestionaire);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _EditCustomerQuestionaire(Guid id)
        {
            var ContractModel = _contractService.GetContractQuestionariesById(id);
            ContractModel.radioWarrantyProvision = _commonService.GetYesNo();
            ContractModel.radioCPARS = _commonService.GetYesNo();
            ContractModel.radioFARClause = _commonService.GetYesNo();
            ContractModel.radioGovFurnishedProperty = _commonService.GetYesNo();
            ContractModel.radioGQAC = _commonService.GetYesNo();
            ContractModel.radioGSAschedule = _commonService.GetYesNo();
            ContractModel.radioReportingExecCompensation = _commonService.GetYesNo();
            ContractModel.radioSBsubcontracting = _commonService.GetYesNo();
            ContractModel.radioServiceContractReport = _commonService.GetYesNo();
            ContractModel.radioWarranties = _commonService.GetYesNo();
            try
            {
                return PartialView("FormsInDetail/_FormContractQuestionaire", ContractModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _ViewCustomerQuestionaire(Guid id)
        {
            var ContractModel = _contractService.GetContractQuestionariesById(id);
            ContractModel.radioWarrantyProvision = _commonService.GetYesNo();
            ContractModel.radioCPARS = _commonService.GetYesNo();
            ContractModel.radioFARClause = _commonService.GetYesNo();
            ContractModel.radioGovFurnishedProperty = _commonService.GetYesNo();
            ContractModel.radioGQAC = _commonService.GetYesNo();
            ContractModel.radioGSAschedule = _commonService.GetYesNo();
            ContractModel.radioReportingExecCompensation = _commonService.GetYesNo();
            ContractModel.radioSBsubcontracting = _commonService.GetYesNo();
            ContractModel.radioServiceContractReport = _commonService.GetYesNo();
            ContractModel.radioWarranties = _commonService.GetYesNo();
            try
            {
                return PartialView("FormsInDetail/_ViewCustomerQuestionaire", ContractModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _AddCustomerQuestionaire([FromBody]ContractQuestionaire contractQuestionaire)
        {
            try
            {
                Guid id = Guid.NewGuid();
                contractQuestionaire.ContractQuestionaireGuid = id;
                contractQuestionaire.CreatedOn = DateTime.Now;
                contractQuestionaire.UpdatedOn = DateTime.Now;
                contractQuestionaire.CreatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                contractQuestionaire.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                contractQuestionaire.IsActive = true;
                contractQuestionaire.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.AddContractQuestionaires(contractQuestionaire);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _EditCustomerQuestionaire([FromBody]ContractQuestionaire contractQuestionaire)
        {
            try
            {
                contractQuestionaire.UpdatedOn = DateTime.Now;
                contractQuestionaire.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.UpdateContractQuestionairesById(contractQuestionaire);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Updated !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        #endregion

        #region WBS
        [HttpGet]
        public ActionResult _AddContractWBS(Guid id)
        {
            try
            {
                ContractWBS contractWBS = new ContractWBS();
                Guid ContractWBSGuid = Guid.NewGuid();
                contractWBS.ContractWBSGuid = ContractWBSGuid;
                contractWBS.ContractGuid = id;

                return PartialView("FormsInDetail/_WorkBreakdownStructureAdd", contractWBS);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _AddContractWBS([FromForm]ContractWBS contractWBS)
        {
            try
            {
                var isfileValid = _fileService.UploadFileTypeCheck(contractWBS.FileToUpload);
                contractWBS.CreatedOn = DateTime.Now;
                contractWBS.UpdatedOn = DateTime.Now;
                contractWBS.CreatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                contractWBS.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                contractWBS.IsActive = true;
                contractWBS.IsDeleted = false;
                var ContractNumber = _contractService.GetContractNumberById(contractWBS.ContractGuid);
                if (contractWBS.FileToUpload != null || contractWBS.FileToUpload.Length != 0)
                {
                    var filename = _fileService.FilePost($@"DocumentRoot\{ContractNumber}/WorkBreakdownStructure/", contractWBS.FileToUpload);
                    if (isfileValid != "success")
                    {
                        contractWBS.IsCsv = false;
                    }
                    else
                    {
                        contractWBS.IsCsv = true;
                        var datacheck = _fileService.UploadFileDataCheck(filename, UploadMethodName.WorkBreakDownStructure);
                        if (datacheck != "success")
                        {
                            throw new ArgumentException(datacheck);
                        }
                    }
                    contractWBS.UploadFileName = filename;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.DeleteContractWBS(contractWBS.ContractGuid);
                _contractService.AddContractWBS(contractWBS);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _ViewContractWBS(Guid id)
        {
            var model = _contractService.GetContractWBSById(id);
            try
            {
                return PartialView("FormsInDetail/_WorkBreakdownStructureView", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        public IActionResult GetContractWBS(Guid id)
        {
            try
            {
                var model = _contractService.GetContractWBSById(id);
                var contractViewModels = _contractService.GetWBSList(model.UploadFileName);
                var data = contractViewModels.Select(x => new
                {
                    ContractGuid = id,
                    WBSCode = x.WBSCode,
                    Description = x.Description,
                    Value = x.Value,
                    ContractType = x.ContractType,
                    InvoiceAtThisLevel = x.InvoiceAtThisLevel
                }).ToList();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult GetContractWBS([FromBody] string searchText)
        {
            try
            {
                List<StringBuilder> listsb = new List<StringBuilder>();
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                var listdata = list.Select(x => new
                {
                    WBSCode = x.WBSCode,
                    Description = x.Description,
                    Value = x.Value,
                    ContractType = x.ContractType,
                    InvoiceAtThisLevel = x.InvoiceAtThisLevel
                }).ToList();
                var datacheck = _fileService.UploadFileEditDataCheck(UploadMethodName.WorkBreakDownStructure, list);
                if (datacheck != "success")
                {
                    throw new ArgumentException(datacheck);
                }
                var ContractGuid = list[0].ContractGuid;
                var model = _contractService.GetContractWBSById(ContractGuid);
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                CsvWriter csvWriter = new CsvWriter();
                csvWriter.Write(listdata, fileName, true);
                return Ok(new { status = ResponseStatus.success.ToString(), path = fileName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult _EditContractWBS(Guid id)
        {
            var model = _contractService.GetContractWBSById(id);
            try
            {
                return PartialView("FormsInDetail/_WorkBreakdownStructureEdit", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _EditContractWBS([FromForm]ContractWBS contractWBS)
        {
            try
            {
                contractWBS.UpdatedOn = DateTime.Now;
                contractWBS.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                contractWBS.IsActive = true;
                contractWBS.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.UpdateContractWBS(contractWBS);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }

        #endregion



        [HttpGet]
        public ActionResult _AddEmployeeBillingRates(Guid id)
        {
            try
            {
                EmployeeBillingRates employeeBillingRates = new EmployeeBillingRates();
                Guid BillingRateGuid = Guid.NewGuid();
                employeeBillingRates.BillingRateGuid = BillingRateGuid;
                employeeBillingRates.ContractGuid = id;
                return PartialView("FormsInDetail/_EmployeeBillingRatesAdd", employeeBillingRates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _AddEmployeeBillingRates([FromForm]EmployeeBillingRates employeeBillingRates)
        {
            try
            {
                var isfileValid = _fileService.UploadFileTypeCheck(employeeBillingRates.FileToUpload);
                employeeBillingRates.CreatedOn = DateTime.Now;
                employeeBillingRates.UpdatedOn = DateTime.Now;
                employeeBillingRates.CreatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                employeeBillingRates.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                employeeBillingRates.IsActive = true;
                employeeBillingRates.IsDeleted = false;
                var ContractNumber = _contractService.GetContractNumberById(employeeBillingRates.ContractGuid);
                if (employeeBillingRates.FileToUpload != null || employeeBillingRates.FileToUpload.Length != 0)
                {
                    var filename = _fileService.FilePost($@"DocumentRoot\{ContractNumber}/EmployeeBillingRates/", employeeBillingRates.FileToUpload);
                    if (isfileValid != "success")
                    {
                        employeeBillingRates.IsCsv = false;
                    }
                    else
                    {
                        employeeBillingRates.IsCsv = true;
                        var datacheck = _fileService.UploadFileDataCheck(filename, UploadMethodName.EmployeeBillingRate);
                        if (datacheck != "success")
                        {
                            throw new ArgumentException(datacheck);
                        }
                    }
                    employeeBillingRates.UploadFileName = filename;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.DeleteEmployeeBillingRates(employeeBillingRates.ContractGuid);
                _contractService.AddEmployeeBillingRates(employeeBillingRates);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _ViewEmployeeBillingRates(Guid id)
        {
            var model = _contractService.GetEmployeeBillingRatesById(id);
            try
            {
                return PartialView("FormsInDetail/_EmployeeBillingRatesView", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        public IActionResult GetEmployeeBillingRates(Guid id)
        {
            try
            {
                var model = _contractService.GetEmployeeBillingRatesById(id);
                var contractViewModels = _contractService.GetBillingRatesList(model.UploadFileName);
                var data = contractViewModels.Select(x => new
                {
                    ContractGuid = id,
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult GetEmployeeBillingRates([FromBody] string searchText)
        {
            try
            {
                List<StringBuilder> listsb = new List<StringBuilder>();
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                var ContractGuid = list[0].ContractGuid;
                var model = _contractService.GetEmployeeBillingRatesById(ContractGuid);
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                var listdata = list.Select(x => new
                {
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
                var datacheck = _fileService.UploadFileEditDataCheck(UploadMethodName.EmployeeBillingRate, list);
                if (datacheck != "success")
                {
                    throw new ArgumentException(datacheck);
                }
                CsvWriter csvWriter = new CsvWriter();
                csvWriter.Write(listdata, fileName, true);
                return Ok(new { status = ResponseStatus.success.ToString(), path = fileName });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), ex.Message);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _EditEmployeeBillingRates(Guid id)
        {
            var model = _contractService.GetEmployeeBillingRatesById(id);
            try
            {
                return PartialView("FormsInDetail/_EmployeeBillingRatesEdit", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _EditEmployeeBillingRates([FromForm]EmployeeBillingRates employeeBillingRates)
        {
            try
            {
                employeeBillingRates.UpdatedOn = DateTime.Now;
                employeeBillingRates.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                employeeBillingRates.IsActive = true;
                employeeBillingRates.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.UpdateEmployeeBillingRates(employeeBillingRates);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }





        [HttpGet]
        public ActionResult _AddLaborCategoryRates(Guid id)
        {
            try
            {
                LaborCategoryRates laborCategoryRates = new LaborCategoryRates();
                Guid CategoryRateGuid = Guid.NewGuid();
                laborCategoryRates.CategoryRateGuid = CategoryRateGuid;
                laborCategoryRates.ContractGuid = id;
                return PartialView("FormsInDetail/_LaborCategoryRatesAdd", laborCategoryRates);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _AddLaborCategoryRates([FromForm]LaborCategoryRates laborCategoryRates)
        {
            try
            {
                var isfileValid = _fileService.UploadFileTypeCheck(laborCategoryRates.FileToUpload);
                laborCategoryRates.CreatedOn = DateTime.Now;
                laborCategoryRates.UpdatedOn = DateTime.Now;
                laborCategoryRates.CreatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                laborCategoryRates.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                laborCategoryRates.IsActive = true;
                laborCategoryRates.IsDeleted = false;
                var ContractNumber = _contractService.GetContractNumberById(laborCategoryRates.ContractGuid);
                if (laborCategoryRates.FileToUpload != null || laborCategoryRates.FileToUpload.Length != 0)
                {
                    var filename = _fileService.FilePost($@"DocumentRoot\{ContractNumber}/SubcontractorBillingRates/", laborCategoryRates.FileToUpload);
                    if (isfileValid != "success")
                    {
                        laborCategoryRates.IsCsv = false;
                    }
                    else
                    {
                        laborCategoryRates.IsCsv = true;
                        var datacheck = _fileService.UploadFileDataCheck(filename, UploadMethodName.SubcontractorLaborBillingRates);
                        if (datacheck != "success")
                        {
                            throw new ArgumentException(datacheck);
                        }
                    }
                    laborCategoryRates.UploadFileName = filename;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.DeleteLaborCategoryRates(laborCategoryRates.ContractGuid);
                _contractService.AddLaborCategoryRates(laborCategoryRates);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpGet]
        public ActionResult _ViewLaborCategoryRates(Guid id)
        {
            var model = _contractService.GetLaborCategoryRatesById(id);
            try
            {
                return PartialView("FormsInDetail/_LaborCategoryRatesView", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        public IActionResult GetLaborCategoryRates(Guid id)
        {
            try
            {
                var model = _contractService.GetLaborCategoryRatesById(id);
                var contractViewModels = _contractService.GetCategoryRatesList(model.UploadFileName);
                var data = contractViewModels.Select(x => new
                {
                    ContractGuid = id,
                    SubContractor = x.SubContractor,
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult GetLaborCategoryRates([FromBody] string searchText)
        {
            try
            {
                List<StringBuilder> listsb = new List<StringBuilder>();
                var list = JsonConvert.DeserializeObject<List<GridHeaderModel>>(searchText);
                var listdata = list.Select(x => new
                {
                    SubContractor = x.SubContractor,
                    LaborCode = x.LaborCode,
                    EmployeeName = x.EmployeeName,
                    Rate = x.Rate,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                }).ToList();
                var datacheck = _fileService.UploadFileEditDataCheck(UploadMethodName.SubcontractorLaborBillingRates, list);
                if (datacheck != "success")
                {
                    throw new ArgumentException(datacheck);
                }
                var ContractGuid = list[0].ContractGuid;
                var model = _contractService.GetLaborCategoryRatesById(ContractGuid);
                string fileName = string.Format(model.UploadFileName, System.AppContext.BaseDirectory);
                CsvWriter csvWriter = new CsvWriter();
                csvWriter.Write(listdata, fileName, true);
                return Ok(new { status = ResponseStatus.success.ToString(), path = fileName });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public ActionResult _EditLaborCategoryRates(Guid id)
        {
            var model = _contractService.GetLaborCategoryRatesById(id);
            try
            {
                return PartialView("FormsInDetail/_LaborCategoryRatesEdit", model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IActionResult _EditLaborCategoryRates([FromForm]LaborCategoryRates laborCategoryRates)
        {
            try
            {
                laborCategoryRates.UpdatedOn = DateTime.Now;
                laborCategoryRates.UpdatedBy = new Guid("50FAB585-C32A-4D89-B642-012E34B96D2D");
                laborCategoryRates.IsActive = true;
                laborCategoryRates.IsDeleted = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _contractService.UpdateLaborCategoryRates(laborCategoryRates);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Edited !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.ToString(), _configuration.GetSection("ExceptionErrorMessage").Value);
                return BadRequest(ModelState);
            }
        }







        public IActionResult DownloadDocument(string filePath, string fileName)
        {
            try
            {
                return _fileService.GetFile(fileName, filePath);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult _DetailsOfUser(Guid id)
        {
            try
            {
                var user = _contractService.GetUsersDataByUserId(id);
                return Json(user);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new User());
            }
        }
        [HttpGet]
        public ActionResult _DetailsOfContact(Guid id)
        {
            try
            {
                var contact = _contractService.GetContactsDataByContactId(id);
                return Json(contact);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new CustomerContact());
            }
        }
        [HttpGet]
        public ActionResult _DetailsOfAgency(Guid id)
        {
            try
            {
                var agency = _contractService.GetCustomersDataByCustomerId(id);
                return Json(agency);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new Customer());
            }
        }
        #endregion

        #region AutoPopulate Texboxes..
        [HttpPost]
        public IActionResult GetOrganizationData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetOrganizationData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatAutoCompleteData(ob.Name, ob.Description);
                    model.label = result.Trim();
                    model.value = ob.OrgIDGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetProjectManagerData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetUsersData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatFullNamewithDesignation(ob.Firstname, String.Empty, ob.Lastname, ob.JobTitle);
                    model.label = result.Trim();
                    model.value = ob.UserGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetProjectControlData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetUsersData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatFullNamewithDesignation(ob.Firstname, String.Empty, ob.Lastname, ob.JobTitle);
                    model.label = result.Trim();
                    model.value = ob.UserGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetContractRepresentativeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetUsersData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatFullNamewithDesignation(ob.Firstname, String.Empty, ob.Lastname, ob.JobTitle);
                    model.label = result.Trim();
                    model.value = ob.UserGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetAccountingRepresentativeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetUsersData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatFullNamewithDesignation(ob.Firstname, String.Empty, ob.Lastname, ob.JobTitle);
                    model.label = result.Trim();
                    model.value = ob.UserGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetNAICSCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetNAICSCodeData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatAutoCompleteData(ob.Code, ob.Title);
                    model.label = result.Trim();
                    model.value = ob.NAICSGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetPSCCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetPSCCodeData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = ob.CodeDescription;
                    model.label = result.Trim();
                    model.value = ob.PSCGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult GetAwardingAgencyOfficeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetAwardingAgencyOfficeData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatCustomerNameAndAddress(ob.CustomerName, ob.Department, ob.Address, ob.City, ob.StatesName,
                        ob.ZipCode, ob.CountryName);
                    model.label = result.Trim();
                    model.value = ob.CustomerGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult GetFundingAgencyOfficeData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractService.GetFundingAgencyOfficeData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = FormatHelper.FormatCustomerNameAndAddress(ob.CustomerName, ob.Department, ob.Address, ob.City, ob.StatesName,
                        ob.ZipCode, ob.CountryName);
                    model.label = result.Trim();
                    model.value = ob.CustomerGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult GetAllContactByCustomer(Guid customerId)
        {
            try
            {
                var contractRepresentative = _contractService.GetAllContactByCustomer(customerId, ContactType.ContractRepresentative.ToString());
                var technicalContractRepresentative = _contractService.GetAllContactByCustomer(customerId, ContactType.TechnicalContractRepresentative.ToString());
                var joinContactResult = contractRepresentative.Union(technicalContractRepresentative).ToList();
                return Json(new { contractRepresentative = joinContactResult, technicalContractRepresentative = joinContactResult });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }
        [HttpPost]
        public IActionResult GetCompanyRegionOfficeNameByCode([FromBody] EntityCode entityCode)
        {
            try
            {
                var result = _contractService.GetCompanyRegionAndOfficeNameByCode(entityCode);
                var json = new
                {
                    CompanyPresident = result.CompanyPresident,
                    RegionManager = result.RegionManager,
                    CompanyName = result.CompanyName,
                    RegionName = result.RegionName,
                    OfficeName = result.OfficeName
                };
                return Ok(new { data = json });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
