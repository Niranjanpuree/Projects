using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Models;
using Northwind.Web.Helpers;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Models.ViewModels.RevenueRecognition;
using Northwind.Core.Entities.ContractRefactor;
using EnumGlobal = Northwind.Web.Models.ViewModels.EnumGlobal;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Infrastructure.AuditLog;
using NLog;
using EnumGlobalCore = Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Services;
using Northwind.Web.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;
using Northwind.Core.Interfaces.DocumentMgmt;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.CostPoint.Interfaces;
using static Northwind.Core.Entities.GenericNotification;
using System.Text;

namespace Northwind.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IFileService _fileService;
        //private readonly IContractService _contractService;
        private readonly IContractsService _contractsService;
        private readonly ICommonService _commonService;
        private readonly ICountryService _countryService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IRevenueRecognitionService _revenueRecognitionService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IStateService _stateService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IUrlHelper _urlHelper;
        private readonly IMapper _mapper;
        private readonly IFarContractService _farContractService;
        private readonly Logger _logger;
        private readonly Logger _eventLogger;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IFolderStructureMasterService _folderStructureMasterService;
        private readonly IFolderStructureFolderService _folderStructureFolderService;
        private readonly IFolderService _folderService;
        private readonly IContractResourceFileService _contractResourceFileService;
        private readonly IQuestionaireUserAnswerService _questionaireUserAnswerService;
        private readonly IGroupPermissionService _groupPermission;
        private readonly IGenericNotificationService _genericNotificationService;
        private readonly IProjectServiceCP _projectServiceCP;
        private readonly ICustomerContactService _customerContactService;
        private readonly ICustomerService _customerService;
        private readonly INaicsService _naicsService;
        private readonly IPscService _pscService;

        public ProjectController(
                                 IContractsService contractsService,
                                 ICommonService commonService,
                                 IFarContractTypeService farContractTypeService,
                                 IConfiguration configuration,
                                 IResourceAttributeValueService resourceAttributeValueService,
                                 IRevenueRecognitionService revenueRecognitionService,
                                 INotificationTemplatesService notificationTemplatesService,
                                 INotificationBatchService notificationBatchService,
                                 INotificationMessageService notificationMessageService,
                                 IFileService fileService,
                                 ICountryService countryService,
                                 IStateService stateService,
                                 IContractModificationService contractModificationService,
                                 IUserService userService,
                                 IFarContractService farContractService,
                                 IQuestionaireUserAnswerService questionaireUserAnswerService,
                                 IUrlHelper urlHelper,
                                 IDocumentManagementService documentManagementService,
                                 IFolderStructureMasterService folderStructureMasterService,
                                 IFolderStructureFolderService folderStructureFolderService,
                                 IFolderService folderService,
                                 IContractResourceFileService contractResourceFileService,
                                 IGroupPermissionService groupPermission,
                                 IGenericNotificationService genericNotificationService,
                                 IProjectServiceCP projectServiceCP,
                                 ICustomerContactService customerContactService,
                                 ICustomerService customerService,
                                 INaicsService naicsService,
                                 IPscService pscService,
                                 IMapper mapper)
        {
            _fileService = fileService;
            _contractsService = contractsService;
            _farContractTypeService = farContractTypeService;
            _revenueRecognitionService = revenueRecognitionService;
            _commonService = commonService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _configuration = configuration;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationBatchService = notificationBatchService;
            _notificationMessageService = notificationMessageService;
            _countryService = countryService;
            _stateService = stateService;
            _userService = userService;
            _urlHelper = urlHelper;
            _documentManagementService = documentManagementService;
            _folderStructureMasterService = folderStructureMasterService;
            _folderStructureFolderService = folderStructureFolderService;
            _contractModificationService = contractModificationService;
            _mapper = mapper;
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _farContractService = farContractService;
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
            _folderService = folderService;
            _genericNotificationService = genericNotificationService;
            _contractResourceFileService = contractResourceFileService;
            _questionaireUserAnswerService = questionaireUserAnswerService;
            _groupPermission = groupPermission;
            _projectServiceCP = projectServiceCP;
            _customerContactService = customerContactService;
            _customerService = customerService;
            _naicsService = naicsService;
            _pscService = pscService;
        }

        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.List)]
        public ActionResult Index(Guid contractGuid, string searchValue)
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.ContractGuid = contractGuid;
            contractViewModel.SearchValue = searchValue;
            return View(contractViewModel);
        }

        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.List)]
        public ActionResult GetProjectWithModReportFields(Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return PartialView("ReportFields", new List<GridviewField>());
            var contract = _contractsService.GetDetailById(contractGuid);
            try
            {
                var fields = new List<GridviewField>();
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Contract Number", FieldName = "contractNumber", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 1 });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Project Number", FieldName = "projectNumber", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 2 });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Title", FieldName = "title", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 4 });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Mod", FieldName = "mod", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 3 });
                //fields.Add(new GridviewField { Clickable = false, FieldLabel = "Effective Date", FieldName = "effectiveDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 5, Format = "{0:d}", Type = "date" });
                //fields.Add(new GridviewField { Clickable = false, FieldLabel = "Date Entered", FieldName = "dateEntered", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 6, Format = "{0:d}", Type = "date" });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Pop Start", FieldName = "startDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 7, Format = "{0:d}", Type = "date" });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Pop End", FieldName = "endDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 8, Format = "{0:d}", Type = "date" });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Award Amount", FieldName = "amount", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 9, Format = "{0:" + contract.Currency + " ###,###.00}", Type = "numeric" });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Funding Amount", FieldName = "fundingAmount", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 10, Format = "{0:" + contract.Currency + " ###,###.00}", Type = "numeric" });
                return PartialView("ReportFields", fields);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return PartialView("ReportFields");
            }

        }

        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.List)]
        public IActionResult GetProjectWithModFields(Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return PartialView("ReportFields", new List<GridviewField>());
            var contract = _contractsService.GetDetailById(contractGuid);
            try
            {
                var fields = new List<GridviewField>();
                //fields.Add(new GridviewField { Clickable = false, FieldLabel = "Project Number", FieldName = "projectNumber", IsDefaultSortField = true, IsFilterable = false, IsSortable = true, OrderIndex = 1, visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Title", FieldName = "title", IsDefaultSortField = false, IsFilterable = false, IsSortable = true, OrderIndex = 3, visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Modification Type", FieldName = "modificationType", IsDefaultSortField = false, IsFilterable = false, IsSortable = true, OrderIndex = 2, visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Mod", FieldName = "mod", IsDefaultSortField = false, IsFilterable = false, IsSortable = true, OrderIndex = 1, visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Pop Start", FieldName = "startDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 3, Format = "{0:d}", Type = "date", GridColumnCss = "kendo-grid-text-center", visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Pop End", FieldName = "endDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 4, Format = "{0:d}", Type = "date", GridColumnCss = "kendo-grid-text-center", visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Award Amount", FieldName = "amount", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 5, Format = "{0:c}", GridColumnCss = "kendo-grid-text-right", visibleToGrid = true });
                fields.Add(new GridviewField { Clickable = false, FieldLabel = "Funded Amount", FieldName = "fundingAmount", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 6, Format = "{0:c}", GridColumnCss = "kendo-grid-text-right", visibleToGrid = true });
                //fields.Add(new GridviewField { Clickable = false, FieldLabel = "Effective Date", FieldName = "effectiveDate", IsDefaultSortField = false, IsFilterable = false, IsSortable = true, OrderIndex = 4, Format = "{0:d}", Type = "date", visibleToGrid = true });
                //fields.Add(new GridviewField { Clickable = false, FieldLabel = "Date Entered", FieldName = "dateEntered", IsDefaultSortField = false, IsFilterable = false, IsSortable = false, OrderIndex = 5, Format = "{0:d}", Type = "date" });


                return Ok(fields);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }

        }

        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.List)]
        public IActionResult GetProjectWithMods(Guid projectGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                sortField = string.IsNullOrEmpty(sortField) ? "ProjectNumber" : sortField;
                var list = new List<ModListModel>();
                var projects = _contractsService.GetDetailById(projectGuid);
                if (projects != null)
                {
                    var project = projects;
                    var proj = new ModListModel
                    {

                        Amount = project.AwardAmount ?? 0,
                        FundingAmount = project.FundingAmount ?? 0,
                        ContractNumber = project.ContractNumber,
                        ProjectNumber = project.ProjectNumber,
                        IsMod = false,
                        Id = project.ContractGuid,
                        Mod = "Base",
                        Title = project.ContractTitle,
                        StartDate = project.POPStart.HasValue && project.POPStart.Value.Year > 1900 ? project.POPStart.Value.ToString("MM/dd/yyyy") : "",
                        EndDate = project.POPEnd.HasValue && project.POPEnd.Value.Year > 1900 ? project.POPEnd.Value.ToString("MM/dd/yyyy") : "",
                        EffectiveDate = "",
                        DateEntered = "",
                        currency = projects.Currency,
                        Status = project.IsActive
                    };

                    list.Add(proj);

                    //var modList = _projectModificationService.GetAll(project.ContractGuid, "", 100000, 0, "ModificationNumber", "desc");

                    var modList = _contractModificationService.GetAll(project.ContractGuid, true, "", 100000, 0, "ModificationNumber", "desc");

                    modList = modList.OrderByDescending(c => c.ModificationNumber);
                    foreach (var mod in modList)
                    {
                        var item = new ModListModel
                        {
                            Amount = mod.AwardAmount.HasValue ? mod.AwardAmount.Value : 0,
                            FundingAmount = mod.FundingAmount.HasValue ? mod.FundingAmount.Value : 0,
                            ContractNumber = project.ContractNumber,
                            ProjectNumber = mod.ProjectNumber,
                            IsMod = true,
                            Id = mod.ContractModificationGuid,
                            Mod = mod.ModificationNumber,
                            Title = mod.ModificationTitle,
                            ModificationType = mod.ModificationType,
                            StartDate = mod.POPStart.HasValue && mod.POPStart.Value.Year > 1900 ? mod.POPStart.Value.ToString("MM/dd/yyyy") : "No Change",
                            EndDate = mod.POPEnd.HasValue && mod.POPEnd.Value.Year > 1900 ? mod.POPEnd.Value.ToString("MM/dd/yyyy") : "No Change",
                            EffectiveDate = mod.EffectiveDate.HasValue && mod.EffectiveDate.Value.Year > 1900 ? mod.EffectiveDate.Value.ToString("MM/dd/yyyy") : "",
                            DateEntered = mod.EnteredDate.HasValue && mod.EnteredDate.Value.Year > 1900 ? mod.EnteredDate.Value.ToString("MM/dd/yyyy") : "",
                            Status = mod.IsActive,
                            currency = projects.Currency
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
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.List)]
        public IActionResult GetProjectsWithMods(Guid contractGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            try
            {
                sortField = string.IsNullOrEmpty(sortField) ? "ProjectNumber" : sortField;
                var list = new List<ModListModel>();
                var projects = _contractsService.GetAllProject(contractGuid, searchValue, take, skip, sortField, dir);
                foreach (var project in projects)
                {
                    var proj = new ModListModel
                    {

                        Amount = project.AwardAmount ?? 0,
                        ContractNumber = project.ContractNumber,
                        ProjectNumber = $"{project.ProjectNumber} {project.ContractTitle}",
                        //ProjectNumber = $"{project.ProjectNumber} {project.ContractTitle}",
                        IsMod = false,
                        Id = project.ContractGuid,
                        Mod = "Base",
                        Title = project.ContractTitle,
                        StartDate = project.POPStart.HasValue ? project.POPStart.Value.ToString("MM/dd/yyyy") : "",
                        EndDate = project.POPEnd.HasValue ? project.POPEnd.Value.ToString("MM/dd/yyyy") : "",
                        EffectiveDate = "",
                        DateEntered = "",
                        currency = project.Currency,
                        Status = project.IsActive,
                        FundingAmount = project.FundingAmount.HasValue ? project.FundingAmount.Value : 0
                    };

                    list.Add(proj);

                    var modList = _contractModificationService.GetAll(project.ContractGuid, true, "", 100000, 0, "ModificationNumber", "desc");
                    modList = modList.OrderByDescending(c => c.ModificationNumber);
                    foreach (var mod in modList)
                    {
                        var item = new ModListModel
                        {
                            Amount = mod.AwardAmount.HasValue ? mod.AwardAmount.Value : 0,
                            ContractNumber = project.ContractNumber,
                            ProjectNumber = $"{project.ProjectNumber} {project.ContractTitle}",
                            //ProjectNumber = $"{project.ContractNumber}::{project.ContractTitle}({project.ProjectNumber})",
                            IsMod = true,
                            Id = mod.ContractModificationGuid,
                            Mod = mod.ModificationNumber,
                            ModificationType = mod.ModificationType,
                            Title = mod.ModificationTitle,
                            StartDate = mod.POPStart.HasValue && mod.POPStart.Value.Year > 1900 ? mod.POPStart.Value.ToString("MM/dd/yyyy") : "No Change",
                            EndDate = mod.POPEnd.HasValue && mod.POPEnd.Value.Year > 1900 ? mod.POPEnd.Value.ToString("MM/dd/yyyy") : "No Change",
                            EffectiveDate = mod.EffectiveDate.HasValue && mod.EffectiveDate.Value.Year > 1900 ? mod.EffectiveDate.Value.ToString("MM/dd/yyyy") : "",
                            DateEntered = mod.EnteredDate.HasValue && mod.EnteredDate.Value.Year > 1900 ? mod.EnteredDate.Value.ToString("MM/dd/yyyy") : "",
                            Status = mod.IsActive,
                            currency = project.Currency,
                            FundingAmount = mod.FundingAmount.HasValue ? mod.FundingAmount.Value : 0
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
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        #region  Crud
        [HttpGet]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Add)]
        public ActionResult Add(Guid contractGuid)
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            int projectCounter = 0;
            if (contractGuid != Guid.Empty)
            {
                //info of contract are almost same for its projects so all contract info maps into the newly added project..
                //var contractEntity = _contractService.GetDetailsForProjectByContractId(contractGuid);

                var contractEntity = _contractsService.GetDetailsForProjectByContractId(contractGuid);
                //contractViewModel = _mapper.Map<ContractViewModel>(contractEntity);

                contractViewModel = ContractsMapper.MapEntityToModel(contractEntity, false);
                contractViewModel.BasicContractInfo.ParentContractNumber = contractEntity.ContractNumber;
                contractViewModel.BasicContractInfo.ParentProjectNumber = contractEntity.ProjectNumber;

                projectCounter = _contractsService.GetTotalCountProjectByContractId(contractGuid);
                contractViewModel.BasicContractInfo.ProjectNumber = contractViewModel.BasicContractInfo.ProjectNumber + "." + (projectCounter + 1).ToString().PadLeft(3, '0');
                contractViewModel.BasicContractInfo.ContractTitle = string.Empty;
                contractViewModel.BasicContractInfo.ContractNumber = string.Empty;
                contractViewModel.BasicContractInfo.ProjectNumber = string.Empty;
                contractViewModel.BasicContractInfo.POPStart = null;
                contractViewModel.BasicContractInfo.POPEnd = null;
                contractViewModel.BasicContractInfo.FarContractTypeGuid = Guid.Empty;
                contractViewModel.BasicContractInfo.QualityLevelRequirements = false;
                contractViewModel.BasicContractInfo.Description = string.Empty;
                contractViewModel.FinancialInformation = new FinancialInformationViewModel() { Currency = "USD" };
                contractViewModel.CustomerInformation = new CustomerInformationViewModel();

                contractViewModel.KeyPersonnel.ContractRepresentativeName = string.Empty;
                contractViewModel.KeyPersonnel.ProjectManagerName = string.Empty;
                contractViewModel.KeyPersonnel.ProjectControlName = string.Empty;
                contractViewModel.KeyPersonnel.AccountingRepresentativeName = string.Empty;
                ViewBag.Resourcekey = Core.Entities.EnumGlobal.ResourceType.TaskOrder.ToString();

                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.DisplayName;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
            }

            var customerAttributeValuesViewModel = InitialLoad();

            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            contractViewModel.ProjectCounter = projectCounter + 1;
            contractViewModel.ParentContractGuid = contractGuid;

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
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Details)]
        public ActionResult Details(Guid id)
        {
            var contractEntity = _contractsService.GetDetailById(id);

            //create folder template ..for seeded taskorder..
            var userGuid = UserHelper.CurrentUserGuid(HttpContext);
            _folderService.CreateFolderTemplate(contractEntity.ContractGuid.ToString(), contractEntity.BasicContractInfo.ContractNumber, EnumGlobalCore.ResourceType.Contract.ToString(), contractEntity.ContractGuid, userGuid);

            var contractViewModel = ContractsMapper.MapEntityToModel(contractEntity, true);
            contractViewModel = CheckAuthorizationByRole(contractViewModel);
            if (contractViewModel.BasicContractInfo.FarContractTypeGuid != null ||
            contractViewModel.BasicContractInfo.FarContractTypeGuid != Guid.Empty)
            {
                var farContractType = _farContractTypeService.GetById(contractViewModel.BasicContractInfo.FarContractTypeGuid);
                contractViewModel.BasicContractInfo.FarContractType = farContractType != null ? farContractType.Title : "Not Entered";
            }

            //get project guid by the contract and  from  its current counter..

            var getPreviousProjectGuid = _contractsService.GetPreviousTaskOfContractByCounter(contractViewModel.ParentContractGuid ?? contractViewModel.ContractGuid, contractViewModel.ProjectCounter ?? 0);

            var getNextProjectGuid = _contractsService.GetNextTaskOfContractByCounter(contractViewModel.ParentContractGuid ?? contractViewModel.ContractGuid, contractViewModel.ProjectCounter ?? 0);

            Guid loginUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            contractViewModel.PreviousProjectGuid = getPreviousProjectGuid;
            contractViewModel.NextProjectGuid = getNextProjectGuid;
            contractViewModel.ModuleType = Models.ViewModels.EnumGlobal.ModuleType.Project;

            contractViewModel.JobRequest = _mapper.Map<JobRequestViewModel>(contractEntity.JobRequest);
            if (contractViewModel.JobRequest != null)
            {
                var jobRequestDisplayName = _userService.GetUserByUserGuid(contractViewModel.JobRequest.UpdatedBy);
                contractViewModel.JobRequest.Displayname = jobRequestDisplayName.DisplayName;
            }

            contractViewModel.ContractWBS = _mapper.Map<ContractWBSViewModel>(contractEntity.ContractWBS);
            if (contractViewModel.ContractWBS != null)
            {
                var contractWBSDisplayName = _userService.GetUserByUserGuid(contractViewModel.ContractWBS.UpdatedBy);
                contractViewModel.ContractWBS.Displayname = contractWBSDisplayName.DisplayName;
            }

            contractViewModel.EmployeeBillingRatesViewModel = _mapper.Map<EmployeeBillingRatesViewModel>(contractEntity.EmployeeBillingRates);
            if (contractViewModel.EmployeeBillingRatesViewModel != null)
            {
                var employeeBillingRatesDisplayName = _userService.GetUserByUserGuid(contractViewModel.EmployeeBillingRatesViewModel.UpdatedBy);
                contractViewModel.EmployeeBillingRatesViewModel.Displayname = employeeBillingRatesDisplayName.DisplayName;
            }

            contractViewModel.LaborCategoryRates = _mapper.Map<LaborCategoryRatesViewModel>(contractEntity.LaborCategoryRates);

            if (contractViewModel.LaborCategoryRates != null)
            {
                var laborCategoryRatesDisplayName = _userService.GetUserByUserGuid(contractViewModel.LaborCategoryRates.UpdatedBy);
                contractViewModel.LaborCategoryRates.Displayname = laborCategoryRatesDisplayName.DisplayName;
            }


            contractViewModel.RevenueRecognitionModel = _mapper.Map<RevenueRecognitionViewModel>(contractEntity.RevenueRecognization);
            if (contractViewModel.RevenueRecognitionModel != null)
            {
                var totalRevenueByContract = _revenueRecognitionService.CountRevenueByContractGuid(id);
                if (totalRevenueByContract > 0)
                    contractViewModel.RevenueRecognitionModel.isViewHistory = true;

                if (contractViewModel.RevenueRecognitionModel.IsNotify)
                {
                    if (contractViewModel.IsAccountingRepresentative)
                    {
                        contractViewModel.IsAuthorizedForRevenue = true;
                        if (contractViewModel.IsAccountingRepresentative)
                            contractViewModel.RevenueRecognitionModel.IsAccountRepresentive = true;
                    }
                    else if (contractViewModel.IsContractRepresentative)
                        contractViewModel.IsAuthorizedForRevenue = true;
                }
                else if (contractViewModel.IsContractRepresentative)
                {
                    contractViewModel.IsAuthorizedForRevenue = true;
                    contractViewModel.RevenueRecognitionModel.IsAccountRepresentive = false;
                }
                var revenueRecognitionDisplayName = _userService.GetUserByUserGuid(contractViewModel.RevenueRecognitionModel.UpdatedBy);
                if (revenueRecognitionDisplayName != null)
                {
                    contractViewModel.RevenueRecognitionModel.Displayname = revenueRecognitionDisplayName.DisplayName;
                }
            }

            var questionniareData = _contractsService.GetAnswer(contractViewModel.ContractGuid);
            QuestionaireViewModel questionaireViewModel = new QuestionaireViewModel();
            if (questionniareData != null)
            {
                questionaireViewModel.UpdatedOn = questionniareData.UpdatedOn;
                questionaireViewModel.DisplayName = _userService.GetUserByUserGuid(questionniareData.UpdatedBy)?.DisplayName;
                contractViewModel.Questionaire = questionaireViewModel;
            }

            if (contractViewModel.JobRequest != null)
            {
                switch (contractViewModel.JobRequest.Status)
                {
                    case 2:
                        if (contractViewModel.IsProjectControls)
                            contractViewModel.IsJobEditable = true;
                        break;
                    case 3:
                        if (contractViewModel.IsProjectManager)
                            contractViewModel.IsJobEditable = true;
                        break;
                    case 4:
                        if (contractViewModel.IsAccountingRepresentative)
                            contractViewModel.IsJobEditable = true;
                        break;
                    default:
                        contractViewModel.IsJobEditable = false;
                        break;
                }
            }

            var farContract = _farContractService.GetAvailableFarContractByContractGuid(contractViewModel.ContractGuid);
            if (farContract != null)
            {
                contractViewModel.IsFarContractAvailable = true;
                contractViewModel.Questionaire = new QuestionaireViewModel();
                contractViewModel.Questionaire.DisplayName = farContract.DisplayName;
                contractViewModel.Questionaire.UpdatedOn = farContract.UpdatedOn;
            }

            var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
            var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
            ViewBag.UpdatedBy = users.DisplayName;
            ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
            ViewBag.ResourceId = contractViewModel.ContractGuid;
            ViewBag.Resourcekey = Core.Entities.EnumGlobal.ResourceType.TaskOrder.ToString();

            var parentContractDetails = _contractsService.GetBasicContractById(contractViewModel.ParentContractGuid ?? Guid.Empty);

            var parentContractNumber = parentContractDetails == null ? "Not Entered" : parentContractDetails.ContractNumber;
            contractViewModel.BasicContractInfo.ParentProjectNumber = parentContractDetails == null ? "Not Entered" : parentContractDetails.ProjectNumber;

            ViewBag.FilePath = string.Format(
             $@"{parentContractNumber}\TaskOrder\{contractViewModel.BasicContractInfo.ContractNumber}");

            ViewBag.ParentContractNumber = parentContractNumber;

            if (contractViewModel.IsContractRepresentative || contractViewModel.IsProjectManager || contractViewModel.IsAccountingRepresentative)
            {
                contractViewModel.IsAuthorizedForContractClose = true;
            }
            if (contractViewModel.BasicContractInfo.IsIDIQContract)
            {
                contractViewModel.IsAuthorizedForContractClose = false;
            }

            var closeOutDetails = _questionaireUserAnswerService.GetDetailById(contractViewModel.ContractGuid, ResourceType.ContractCloseOut.ToString());

            if (closeOutDetails != null)
            {
                var closeOut = new ContractCloseOutDetail();
                var user = _userService.GetUserByUserGuid(closeOutDetails.UpdatedBy);
                closeOut.UpdatedBy = user.DisplayName;
                closeOut.UpdatedOn = closeOutDetails.UpdatedOn.ToString("MM/dd/yyyy");
                contractViewModel.ContractCloseOutDetail = closeOut;
            }

            ///Get group permissions in viewbags..
            GetGroupPermissions();

            try
            {
                //Check if this taks order exist in cost point..
                var projectCP = _projectServiceCP.GetCostPointProjectByProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber);
                ViewBag.IsExistInCostPoint = projectCP == null ? false : true;

                //get user by id 
                var user = _userService.GetUserByUserGuid(contractViewModel.CreatedBy);
                if (user != null)
                    contractViewModel.CreatedByName = user.DisplayName;

                return View("Details", contractViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractViewModel);
            }
        }

        [HttpPost]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Add)]
        public IActionResult Add(ContractViewModel contractViewModel)
        {
            try
            {
                if (contractViewModel.ParentContractGuid != Guid.Empty)
                {
                    ModelState.Remove("BasicContractInfo.ContractNumber");
                }

                //check validations..

                var IsExistContractNumber =
                    _contractsService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractNumber))
                {
                    ModelState.AddModelError("", "Task Order Number not found!!");
                }
                if (IsExistContractNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Task Order Number !!");
                }

                var isExistProjectNumber =
                        _contractsService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber, contractViewModel.ContractGuid);

                if (isExistProjectNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Project Number !!");
                }

                if (contractViewModel.BasicContractInfo.ORGID == Guid.Empty)
                {
                    ModelState.AddModelError("", "Please select organization Id.");
                }

                var isExistProjectTitle =
                    _contractsService.IsExistContractTitle(contractViewModel.BasicContractInfo.ContractTitle, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractTitle))
                {
                    ModelState.AddModelError("", "Task Order Title not found!!");
                }
                if (isExistProjectTitle)
                {
                    ModelState.AddModelError("", " Found Duplicate Task Order Title !!");
                }

                if (ModelState.IsValid)
                {
                    //var projectModel = MapContractModel.Map(contractViewModel);
                    contractViewModel.Status = ContractStatus.Active.ToString();
                    var contractEntity = ContractsMapper.MapModelToEntity(contractViewModel);

                    contractEntity.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractEntity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractEntity.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractEntity.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();

                    contractEntity.IsActive = true;
                    contractEntity.IsDeleted = false;

                    var result = _contractsService.Save(contractEntity);

                    if (result.StatusName.ToString().ToLower() == "fail")
                    {
                        foreach (KeyValuePair<string, object> msg in result.Message)
                        {
                            var ob = (List<string>)msg.Value;
                            foreach (var msg1 in ob)
                            {
                                ModelState.AddModelError("", msg1.ToString());
                            }
                        }
                        throw new ArgumentException();
                    }

                    var contractGuid = result.Message["contractGuid"];


                    //create folder template ..
                    //CreateFolderTemplate(contractModel.BasicContractInfo.ContractNumber, "Contract", new Guid(contractGuid.ToString()));
                    var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                    _folderService.CreateFolderTemplate(contractEntity.ContractGuid.ToString(), contractViewModel.BasicContractInfo.ProjectNumber, EnumGlobalCore.ResourceType.Contract.ToString(), new Guid(contractGuid.ToString()), userGuid);


                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, EnumGlobalCore.CrudTypeForAdditionalLogMessage.Added.ToString(), EnumGlobalCore.ResourceType.TaskOrder.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractGuid);
                    // var resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.TaskOrder.ToString(), contractEntity.ContractGuid, contractEntity.ContractNumber, contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    var resource = string.Format("{0} </br> Project No:{1} Title:{2}", "Task Order", contractEntity.ProjectNumber, contractEntity.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractEntity.BasicContractInfo, resource, (Guid)contractGuid, UserHelper.GetHostedIp(HttpContext), "Task Order Added", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                    //end of log..

                    if (result.StatusName.ToString().ToLower() == "fail")
                    {
                        foreach (KeyValuePair<string, object> msg in result.Message)
                        {
                            var ob = (List<string>)msg.Value;
                            foreach (var msg1 in ob)
                            {
                                ModelState.AddModelError("", msg1.ToString());
                            }
                        }
                        ViewBag.Resourcekey = Core.Entities.EnumGlobal.ResourceType.TaskOrder.ToString();
                        var taskorderViewModel = CatchExceptionAndReturnView(contractViewModel);
                        return View(taskorderViewModel);
                    }

                    if (contractEntity.FarContractTypeGuid != null && contractEntity.FarContractTypeGuid != Guid.Empty)
                    {
                        SaveFarContract((Guid)contractGuid, contractEntity.FarContractTypeGuid);
                    }

                    SaveAndNotifyRevenueRepresentative(contractEntity, (Guid)contractGuid);
                    //after adding new  task order send notification..
                    var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(Core.Entities.EnumGlobal.ResourceType.Project.ToString(),
                        Core.Entities.EnumGlobal.CrudType.Create.ToString());

                    var redirectUrl = string.Format($@"/Project/Details/{contractGuid}");

                    //parent redirection in bread crumb in project case supplied in notification page..
                    var parentRedirectUrl = string.Format($@"/Contract/Details/{contractEntity.ParentContractGuid}");
                    var parentContractNumber = _contractsService.GetContractNumberById(contractViewModel.ParentContractGuid ?? Guid.Empty);
                    var parentProjectNumber = _contractsService.GetProjectNumberById(contractViewModel.ParentContractGuid ?? Guid.Empty);

                    var parameter = new
                    {
                        redirectUrl = redirectUrl,
                        key = key,
                        cameFrom = "Contract Management",
                        resourceName = contractEntity.ProjectNumber,
                        resourceDisplayName = "Task Order",
                        resourceId = contractGuid,
                        parentContractNumber = parentProjectNumber,
                        parentRedirection = parentRedirectUrl
                    };
                    //return RedirectToAction("Index", "Notification", parameter);

                    //generate link for notification...
                    var notificationLink = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "Notification", "Index", parameter);


                    //get file info..
                    var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys("Base", (Guid)contractGuid);


                    //return RedirectToAction("Index", "Notification", parameter);
                    var uploadPath = string.Format(
                     $@"{parentContractNumber}\TaskOrder\{contractViewModel.BasicContractInfo.ContractNumber}");

                    var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                    var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);

                    var jsonObject = new
                    {
                        status = true,
                        notificationLink = notificationLink,
                        resourceId = contractGuid,
                        //uploadPath = uploadPath,
                        uploadPath = contractResourceFile.FilePath,
                        updatedBy = users.DisplayName,
                        parentId = contractResourceFile.ContractResourceFileGuid,
                        updatedOn = CurrentDateTimeHelper.GetCurrentDateTime()
                    };
                    return Json(jsonObject);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToAction("Add", contractViewModel);
            }

            return new JsonResult(new
            {
                status = false,
                Errors = ModelState
            });
        }

        [HttpGet]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            ContractViewModel ContractViewModel = new ContractViewModel();
            if (id != Guid.Empty)
            {
                var contractEntity = _contractsService.GetDetailById(id);
                //ContractViewModel = _mapper.Map<ContractViewModel>(contractEntity);
                ContractViewModel = ContractsMapper.MapEntityToModel(contractEntity, false);
            }
            var customerAttributeValuesViewModel = InitialLoad();

            ContractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName); ;
            ContractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            var parentContractDetails = _contractsService.GetBasicContractById(ContractViewModel.ParentContractGuid ?? Guid.Empty);

            var parentContractNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ContractNumber;
            ContractViewModel.BasicContractInfo.ParentProjectNumber = parentContractDetails == null ? "N/A" : parentContractDetails.ProjectNumber;

            FileAttachmentParameter(ContractViewModel);

            try
            {
                return View("Edit", ContractViewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(ContractViewModel);
            }
        }

        private void FileAttachmentParameter(ContractViewModel ContractViewModel)
        {
            var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
            var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
            ViewBag.UpdatedBy = users.DisplayName;
            ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
            ViewBag.ResourceId = ContractViewModel.ContractGuid;
            ViewBag.Resourcekey = Core.Entities.EnumGlobal.ResourceType.TaskOrder.ToString();
        }

        [HttpPost]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Edit)]
        public IActionResult Edit(ContractViewModel contractViewModel)
        {
            try
            {
                var isProjectNumberChanged = false;
                if (contractViewModel.ParentContractGuid != Guid.Empty)
                {
                    ModelState.Remove("BasicContractInfo.ContractNumber");
                }

                //check validations..

                var IsExistContractNumber =
                    _contractsService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractNumber))
                {
                    ModelState.AddModelError("", "Task Order Number not found!!");
                }
                if (IsExistContractNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Task Order Number !!");
                }


                var isExistProjectNumber =
                        _contractsService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber, contractViewModel.ContractGuid);

                if (contractViewModel.BasicContractInfo.ORGID == Guid.Empty)
                {
                    ModelState.AddModelError("", "Please select organization Id.");
                }

                if (isExistProjectNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Project Number !!");
                }

                var isExistProjectTitle =
                    _contractsService.IsExistContractTitle(contractViewModel.BasicContractInfo.ContractTitle, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractTitle))
                {
                    ModelState.AddModelError("", "Task Order Title not found!!");
                }
                if (isExistProjectTitle)
                {
                    ModelState.AddModelError("", " Found Duplicate Task Order Title !!");
                }

                if (ModelState.IsValid)
                {
                    var projectModel = MapContractModel.Map(contractViewModel);
                    var projectEntity = ContractsMapper.MapModelToEntity(contractViewModel);

                    projectEntity = ValidateModel(projectEntity);

                    var previousProjectdetails = _contractsService.GetAmountById(contractViewModel.ContractGuid);
                    decimal? previousProjectAwardAmount = 0.00M;
                    if (previousProjectdetails.RevenueRecognitionGuid != Guid.Empty && previousProjectdetails.RevenueRecognitionGuid != null)
                    {
                        previousProjectAwardAmount = previousProjectdetails.AwardAmount;
                    }

                    projectEntity.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    projectEntity.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                    var existingContract = _contractsService.GetBasicContractById(projectEntity.ContractGuid);

                    if (existingContract != null)
                    {
                        if (existingContract.ProjectNumber != projectEntity.ProjectNumber)
                        {
                            isProjectNumberChanged = true;
                        }
                    }

                    var result = _contractsService.Save(projectEntity);

                    //Rename Document root foldername
                    if (isProjectNumberChanged)
                    {
                        _documentManagementService.RenameRootFolder("Contract", projectEntity.ProjectNumber, projectEntity.ContractGuid, UserHelper.CurrentUserGuid(HttpContext));
                    }

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, EnumGlobalCore.CrudTypeForAdditionalLogMessage.Edited.ToString(), EnumGlobalCore.ResourceType.TaskOrder.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + projectEntity.ContractGuid);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.TaskOrder.ToString(), projectEntity.ContractGuid, projectEntity.ContractNumber, projectEntity.ProjectNumber, projectEntity.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Task Order", projectEntity.ProjectNumber, projectEntity.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractViewModel.BasicContractInfo, resource, projectEntity.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Task Order Edited", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                    //end of log..

                    if (result.StatusName.ToString().ToLower() == "fail")
                    {
                        foreach (KeyValuePair<string, object> msg in result.Message)
                        {
                            var ob = (List<string>)msg.Value;
                            foreach (var msg1 in ob)
                            {
                                ModelState.AddModelError("", msg1.ToString());
                            }
                        }
                        ViewBag.Resourcekey = EnumGlobalCore.ResourceType.TaskOrder.ToString();
                        var taskorderViewModel = CatchExceptionAndReturnView(contractViewModel);
                        return View(taskorderViewModel);
                    }

                    if (projectEntity.FarContractTypeGuid != null && projectEntity.FarContractTypeGuid != Guid.Empty)
                    {
                        SaveFarContract(projectEntity.ContractGuid, projectEntity.FarContractTypeGuid);
                    }


                    // For Revenue
                    decimal? totalModAwardAmountSum = 0.00M;
                    //sum of mod award amount where the mod amount was not calculated previously.
                    var sumOfModAwardAmount = _contractModificationService.GetTotalAwardAmount(projectEntity.ContractGuid);
                    if (sumOfModAwardAmount != null)
                    {
                        totalModAwardAmountSum = (sumOfModAwardAmount.AwardAmount == null ? 0.00M : sumOfModAwardAmount.AwardAmount);
                    }
                    //gets the actual amount or the differenced amount
                    decimal? actualAmount = projectEntity.AwardAmount - previousProjectAwardAmount;
                    if (actualAmount > 0)
                    {
                        decimal? currentAmount = totalModAwardAmountSum + actualAmount;
                        if (RevenueRecognitionHelper.IsValidForRevenueRecognitionRequest(_configuration, projectEntity.ContractType, currentAmount, 0.00M))
                        {
                            projectEntity.IsUpdated = true;
                            AddNewRevenueAndUpdateContractRevenueGuid(projectEntity, projectEntity.ContractGuid);
                        }
                    }


                    //after updating  task order send notification..
                    var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(Core.Entities.EnumGlobal.ResourceType.Project.ToString(),
                        Core.Entities.EnumGlobal.CrudType.Edit.ToString());

                    var redirectUrl = string.Format($@"/Project/Details/{projectEntity.ContractGuid}");

                    //parent redirection in bread crumb in project case supplied in notification page..
                    var parentRedirectUrl = string.Format($@"/Contract/Details/{projectEntity.ParentContractGuid}");
                    var parentContractNumber = _contractsService.GetContractNumberById(contractViewModel.ParentContractGuid ?? Guid.Empty);
                    var parentProjectNumber = _contractsService.GetProjectNumberById(contractViewModel.ParentContractGuid ?? Guid.Empty);

                    var parameter = new
                    {
                        redirectUrl = redirectUrl,
                        key = key,
                        cameFrom = "Contract Management",
                        resourceName = projectEntity.ProjectNumber,
                        resourceDisplayName = "Task Order",
                        AdditionalMessage = "",
                        resourceId = projectEntity.ContractGuid,
                        parentContractNumber = parentProjectNumber,
                        parentRedirection = parentRedirectUrl
                    };

                    return RedirectToAction("Index", "Notification", parameter);
                }
                else
                {

                    var customerAttributeValuesViewModel = InitialLoad();

                    contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName); ;

                    contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

                    FileAttachmentParameter(contractViewModel);
                    return View(contractViewModel);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return RedirectToAction("Edit", contractViewModel);
            }
        }

        private CustomerAttributeValuesViewModel InitialLoad()
        {
            CustomerAttributeValuesViewModel customerAttributeValuesViewModel = new CustomerAttributeValuesViewModel();
            customerAttributeValuesViewModel.ddlProjectType =
                _resourceAttributeValueService.GetDropDownByResourceType("Contract", "ContractType");
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
            customerAttributeValuesViewModel.radioAppWageDetermine_ServiceProjectAct =
                _resourceAttributeValueService.GetDropDownByValue("ServiceContractAct");

            customerAttributeValuesViewModel.radioCPAREligible = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioIsPrimeProject = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioQualityLevelRequirements = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioSBA = KeyValueHelper.GetYesNo();

            customerAttributeValuesViewModel.ddlBillingFormula = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "BillingFormula");
            customerAttributeValuesViewModel.ddlRevenueFormula = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "RevenueFormula");
            customerAttributeValuesViewModel.ddlStatus = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "Status");
            customerAttributeValuesViewModel.ddlFarContractType = _farContractTypeService.GetAllList().ToDictionary(x => x.FarContractTypeGuid, x => x.Title);
            return customerAttributeValuesViewModel;
        }

        [HttpPost]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Delete)]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _contractsService.Delete(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractsService.GetContractEntityByContractId(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, EnumGlobalCore.CrudTypeForAdditionalLogMessage.Deleted.ToString(), EnumGlobalCore.ResourceType.TaskOrder.ToString().ToLower());
                    //var resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.TaskOrder.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Task Order", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Task Order  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Edit)]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _contractsService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractsService.GetContractEntityByContractId(id);

                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, EnumGlobalCore.CrudTypeForAdditionalLogMessage.Disabled.ToString(), EnumGlobalCore.ResourceType.TaskOrder.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractModel.ContractGuid);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.TaskOrder.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Task Order", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Task Order Disabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Disabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [Secure(EnumGlobalCore.ResourceType.Contract, EnumGlobalCore.ResourceActionPermission.Edit)]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _contractsService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractsService.GetContractEntityByContractId(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, EnumGlobalCore.CrudTypeForAdditionalLogMessage.Enabled.ToString(), EnumGlobalCore.ResourceType.TaskOrder.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractModel.ContractGuid);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> TaskOrder No:{2} </br> Project No:{3} </br> TaskOrder Title:{4}", ResourceType.TaskOrder.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Task Order", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Task Order Enabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                }
                //end of log..

                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Enabled !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }
        #endregion

        [HttpGet]
        [Authorize]
        public IActionResult GetStatesByCountryId(Guid countryId)
        {
            try
            {
                var states = _stateService.GetStateByCountryGuid(countryId);
                var stateList = states.Select(x => new
                { Keys = x.StateId, Values = x.GRT == true ? x.StateName + " (GRT)" : x.StateName });
                List<SelectListItem> lst = new List<SelectListItem>();
                foreach (var s in stateList)
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
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetSelectedStatesByStateIds(string statesIds)
        {
            try
            {
                if (string.IsNullOrEmpty(statesIds))
                {
                    return null;
                }
                var statesIdStringArray = statesIds.Split(',');
                var states = _stateService.GetStatesByStateIds(statesIdStringArray);
                var stateWithGrtOrNot = states.Select(x => new
                {
                    Keys = x.StateId,
                    Values = x.GRT == true ? x.StateName + " (GRT)" : x.StateName
                });
                List<SelectListItem> lst = new List<SelectListItem>();
                foreach (var s in stateWithGrtOrNot)
                {
                    SelectListItem selectList = new SelectListItem();
                    selectList.Text = s.Values;
                    selectList.Value = s.Keys.ToString();
                    lst.Add(selectList);
                }
                return Json(lst);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region AutoPopulate Texboxes..
        [HttpPost]
        [Authorize]
        public IActionResult GetOrganizationData([FromBody] string searchText)
        {
            try
            {
                var listData = _contractsService.GetOrganizationData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Infrastructure.Helpers.FormatHelper.FormatAutoCompleteData(ob.Name, ob.Title);
                    model.label = result;
                    model.value = ob.OrgIDGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetNaicsCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _naicsService.GetNaicsList(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Infrastructure.Helpers.FormatHelper.FormatAutoCompleteData(ob.Code, ob.Title);
                    model.label = result;
                    model.value = ob.NAICSGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetPscCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _pscService.GetPscList(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = ob.CodeDescription;
                    model.label = result;
                    model.value = ob.PSCGuid.ToString();
                    multiSelectReturnModels.Add(model);
                }
                return Ok(new { data = multiSelectReturnModels });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllContactByCustomer(Guid customerId)
        {
            try
            {
                var ProjectRepresentative = _customerContactService.GetAllContactByCustomer(customerId, ContactType.ContractRepresentative.ToString());
                var technicalProjectRepresentative = _customerContactService.GetAllContactByCustomer(customerId, ContactType.TechnicalContractRepresentative.ToString());

                var joinContactInfo = ProjectRepresentative.Union(technicalProjectRepresentative).ToList();
                return Json(new { projectRepresentative = joinContactInfo, technicalProjectRepresentative = joinContactInfo });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult GetCompanyRegionOfficeNameByCode([FromBody] EntityCode entityCode)
        {
            try
            {
                var result = _contractsService.GetCompanyRegionAndOfficeNameByCode(entityCode);
                var json = new
                {
                    CompanyPresident = result.CompanyPresident,
                    RegionManager = result.RegionManager,
                    CompanyName = result.CompanyName,
                    RegionName = result.RegionName,
                    OfficeName = result.OfficeName,
                    DeputyRegionalManager = result.DeputyRegionManager,
                    HSRegionalManager = result.HealthAndSafetyRegionManager,
                    BDRegionalManager = result.BusinessDevelopmentRegionManager,
                    OperationManager = result.OperationManager
                };
                return Ok(new { data = json });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        public IActionResult HasChild(Guid projectGuid)
        {
            try
            {
                var result = _contractsService.HasChild(projectGuid);
                var hasChild = result > 0 ? true : false;
                return Ok(new { data = hasChild });
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }
        #endregion

        [HttpGet]
        [Authorize]
        public ActionResult DetailsOfUser(Guid id)
        {
            try
            {
                var user = _userService.GetUserByUserGuid(id);
                return Json(user);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new User());
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DetailsOfContact(Guid id)
        {
            try
            {
                var contact = _customerContactService.GetDetailsById(id);
                return Json(contact);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new CustomerContact());
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult DetailsOfAgency(Guid id)
        {
            try
            {
                var agency = _customerService.GetCustomerById(id);
                return Json(agency);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new Customer());
            }
        }

        #region private

        private ContractViewModel CheckAuthorizationByRole(ContractViewModel model)
        {
            var accountdetail = _contractsService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                                .FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative);
            if (accountdetail != null)
            {
                Guid accountingRepresentative = accountdetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, accountingRepresentative))
                {
                    model.IsAccountingRepresentative = true;
                }
            }
            var contractRepresentativeDetail = _contractsService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                               .FirstOrDefault(x => x.UserRole == ContractUserRole._contractRepresentative);
            if (contractRepresentativeDetail != null)
            {
                Guid contractRepresentative = contractRepresentativeDetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, contractRepresentative))
                {
                    model.IsContractRepresentative = true;
                }
            }

            var projectManagerDetail = _contractsService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                               .FirstOrDefault(x => x.UserRole == ContractUserRole._projectManager);
            if (projectManagerDetail != null)
            {
                Guid projectManagerGuid = projectManagerDetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, projectManagerGuid))
                {
                    model.IsProjectManager = true;
                }
            }

            var projectControlsDetail = _contractsService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                               .FirstOrDefault(x => x.UserRole == ContractUserRole._projectControls);
            if (projectControlsDetail != null)
            {
                Guid projectControlsGuid = projectControlsDetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, projectControlsGuid))
                {
                    model.IsProjectControls = true;
                }
            }
            return model;
        }

        private Contracts ValidateModel(Contracts contracts)
        {
            string contractType = _configuration.GetSection("ContractType").GetValue<string>("ContractTypeWithOptions");
            if (contracts.SetAside != contractType)
            {
                contracts.SBA = false;
                contracts.SelfPerformancePercent = 0.00M;
            }
            return contracts;
        }

        private bool SaveAndNotifyRevenueRepresentative(Contracts contractModel, Guid contractGuid)
        {
            try
            {
                if (contractModel != null)
                {
                    bool isRevenueTriggered = RevenueRecognitionHelper.IsValidForRevenueRecognitionRequest(_configuration, contractModel.ContractType, contractModel.AwardAmount, contractModel.FundingAmount);
                    if (isRevenueTriggered)
                    {
                        AddNewRevenueAndUpdateContractRevenueGuid(contractModel, contractGuid);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                EventLogHelper.Error(_eventLogger, new EventLog
                {
                    EventGuid = Guid.NewGuid(),
                    Action = "",
                    Application = "ESS",
                    EventDate = DateTime.UtcNow,
                    Message = ex.Message,
                    Resource = ResourceType.Notification.ToString(),
                    StackTrace = ex.StackTrace,
                    UserGuid = userGuid
                });
                return false;
            }
        }

        private bool AddNewRevenueAndUpdateContractRevenueGuid(Contracts contractModel, Guid contractGuid)
        {
            Guid revenueRecognitionGuid = Guid.NewGuid();
            AddNotificationMessage(contractModel, contractGuid);
            bool isSaved = _revenueRecognitionService.AddRevenueWithResourceGuid(
               new RevenueRecognition
               {
                   RevenueRecognizationGuid = revenueRecognitionGuid,
                   ResourceGuid = contractGuid,
                   ContractGuid = contractGuid,
                   UpdatedBy = contractModel.UpdatedBy,
                   UpdatedOn = contractModel.UpdatedOn,
                   CreatedBy = contractModel.UpdatedBy,
                   CreatedOn = contractModel.UpdatedOn
               });
            if (isSaved)
            {
                _contractsService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, contractGuid);
                _contractModificationService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, contractGuid);
            }
            return isSaved;
        }

        private bool AddNotificationMessage(Contracts contractModel, Guid contatGuid)
        {
            try
            {
                var notificationModel = new GenericNotificationViewModel();
                var notificationTemplatesDetails = new NotificationTemplatesDetail();
                var userList = new List<User>();
                var receiverInfo = new User();
                Guid? receiverGuid = Guid.Empty;
                decimal thresholdAmount = 0.00M;
                string key = string.Empty;
                if (contractModel.IsUpdated)
                {
                    key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                            EnumGlobal.ResourceAction.TaskOrderUpdate.ToString());
                }
                else
                {
                    key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                               EnumGlobal.ResourceAction.TaskOrderCreate.ToString());
                }

                notificationModel.ResourceId = contractModel.ContractGuid;
                notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/project/Details/" + contractModel.ContractGuid);
                notificationModel.NotificationTemplateKey = key;
                notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                notificationModel.SendEmail = true;


                var keyPersonnels = _contractsService.GetKeyPersonnelByContractGuid(contractModel.ContractGuid);

                if (keyPersonnels?.Any() == true)
                {
                    receiverGuid = keyPersonnels.FirstOrDefault(x => x.UserRole == ContractUserRole._contractRepresentative)?.UserGuid;
                    if (receiverGuid != Guid.Empty)
                    {
                        thresholdAmount = RevenueRecognitionHelper.GetAmountByContractType(_configuration, contractModel.ContractType);

                        receiverInfo = _userService.GetUserByUserGuid(receiverGuid ?? Guid.Empty);

                        var resourcevalue = _resourceAttributeValueService.GetResourceAttributeValueByValue(contractModel.ContractType);
                        string contracttype = string.Empty;
                        if (resourcevalue != null)
                        {
                            contracttype = resourcevalue.Name;
                        }

                        if (receiverInfo != null)
                        {
                            userList.Add(receiverInfo);
                            notificationModel.IndividualRecipients = userList;
                        }

                        var keyList = "<ul>";
                        keyList += "<li>" + receiverInfo.DisplayName + " (" + receiverInfo.JobTitle + ")" + "</li>";
                        StringBuilder additionalUser = new StringBuilder(keyList);

                        notificationTemplatesDetails.ContractNumber = contractModel.ContractNumber;
                        notificationTemplatesDetails.Title = contractModel.ContractTitle;
                        notificationTemplatesDetails.ContractType = contracttype;
                        notificationTemplatesDetails.ContractTitle = contractModel.ContractTitle;
                        notificationTemplatesDetails.ProjectNumber = contractModel.ProjectNumber;
                        notificationTemplatesDetails.AdditionalUser = additionalUser.ToString();
                        notificationTemplatesDetails.ThresholdAmount = thresholdAmount;
                        notificationTemplatesDetails.Status = "";
                        notificationModel.NotificationTemplatesDetail = notificationTemplatesDetails;
                        _genericNotificationService.AddNotificationMessage(notificationModel);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                EventLogHelper.Error(_eventLogger, new EventLog
                {
                    EventGuid = Guid.NewGuid(),
                    Action = "Index",
                    Application = "ESS",
                    EventDate = DateTime.UtcNow,
                    Message = ex.Message,
                    Resource = ResourceType.Contract.ToString(),
                    StackTrace = ex.StackTrace,
                    UserGuid = userGuid
                });
                return false;
            }
        }

        private ContractViewModel CatchExceptionAndReturnView(ContractViewModel contractViewModel)
        {
            var customerAttributeValuesViewModel = InitialLoad();
            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName); ;
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            return contractViewModel;
        }

        private bool SaveFarContract(Guid contractGuid, Guid farContractTypeClauseGuid)
        {
            var model = new FarContract();
            model.ContractGuid = contractGuid;
            model.FarContractTypeGuid = farContractTypeClauseGuid;
            model.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
            model.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
            model.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            model.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            model.IsDeleted = false;
            _farContractService.AddRequiredData(model);
            return true;
        }


        private bool IsAuthorizedForResource(EnumGlobalCore.ResourceType resourceType, EnumGlobalCore.ResourceActionPermission action)
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var result = _groupPermission.IsUserPermitted(userGuid, resourceType, action);
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void GetGroupPermissions()
        {
            ///group permission 
            ///Revenue recognition
            ViewBag.RevenueRecognitionAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.RevenueRecognition, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.RevenueRecognitionEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.RevenueRecognition, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.RevenueRecognitionList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.RevenueRecognition, EnumGlobalCore.ResourceActionPermission.List);

            ///Job Request
            ViewBag.JobRequestAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.JobRequest, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.JobRequestEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.JobRequest, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.JobRequestList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.JobRequest, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.JobRequestDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.JobRequest, EnumGlobalCore.ResourceActionPermission.Details);

            ///Wbs
            ViewBag.WbsAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.WorkBreakDownStructure, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.WbsEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.WorkBreakDownStructure, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.WbsList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.WorkBreakDownStructure, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.WbsDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.WorkBreakDownStructure, EnumGlobalCore.ResourceActionPermission.Details);

            ///Employee Billing Rates
            ViewBag.EmployeeBillingRatesAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.EmployeeBillingRates, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.EmployeeBillingRatesEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.EmployeeBillingRates, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.EmployeeBillingRatesList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.EmployeeBillingRates, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.EmployeeBillingRatesDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.EmployeeBillingRates, EnumGlobalCore.ResourceActionPermission.Details);

            ///Labor Category Rates
            ViewBag.LaborCategoryRatesAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.LaborCategoryRates, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.LaborCategoryRatesEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.LaborCategoryRates, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.LaborCategoryRatesList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.LaborCategoryRates, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.LaborCategoryRatesDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.LaborCategoryRates, EnumGlobalCore.ResourceActionPermission.Details);

            ///Contract Close
            ViewBag.ContractCloseAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractCloseOut, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.ContractCloseEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractCloseOut, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.ContractCloseList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractCloseOut, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.ContractCloseDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractCloseOut, EnumGlobalCore.ResourceActionPermission.Details);

            ///Contract Notice
            ViewBag.ContractNoticeAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractNotice, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.ContractNoticeEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractNotice, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.ContractNoticeList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractNotice, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.ContractNoticeDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractNotice, EnumGlobalCore.ResourceActionPermission.Details);

            ///Contract Clause
            ViewBag.ContractClauseAdd = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractClauses, EnumGlobalCore.ResourceActionPermission.Add);
            ViewBag.ContractClauseEdit = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractClauses, EnumGlobalCore.ResourceActionPermission.Edit);
            ViewBag.ContractClauseList = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractClauses, EnumGlobalCore.ResourceActionPermission.List);
            ViewBag.ContractClauseDetail = IsAuthorizedForResource(EnumGlobalCore.ResourceType.ContractClauses, EnumGlobalCore.ResourceActionPermission.Details);

            ///Document Manager
            ViewBag.DocumentManagerManage = IsAuthorizedForResource(EnumGlobalCore.ResourceType.DocumentManager, EnumGlobalCore.ResourceActionPermission.Manage);
            ViewBag.ViewDocument = IsAuthorizedForResource(EnumGlobalCore.ResourceType.DocumentManager, EnumGlobalCore.ResourceActionPermission.View);
        }

        #endregion
    }
}