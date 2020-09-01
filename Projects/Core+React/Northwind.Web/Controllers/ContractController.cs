using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Northwind.Web.Helpers;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels.Contract;
using Northwind.Web.Models.ViewModels;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Web.Models.ViewModels.RevenueRecognition;
using Microsoft.Extensions.Caching.Memory;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Models;
using EnumGlobal = Northwind.Core.Entities.EnumGlobal;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using NLog;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Core.Services;
using Northwind.Core.Interfaces.DocumentMgmt;
using Microsoft.AspNetCore.Authorization;
using Northwind.Web.Models.ViewModels.ContractBrief;
using Northwind.CostPoint.Interfaces;
using static Northwind.Core.Entities.GenericNotification;
using System.Text;

namespace Northwind.Web.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractModificationService _contractModificationService;
        //private readonly IContractService _contractService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IUserService _userService;
        private readonly IFileService _fileService;
        private readonly ICountryService _countryService;
        private readonly IFarContractTypeService _farContractTypeService;
        private readonly IRevenueRecognitionService _revenueRecognitionService;
        private readonly IStateService _stateService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IResourceAttributeService _resourceAttributeService;
        private readonly IConfiguration _configuration;
        private readonly IJobRequestService _jobRequestService;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IPscService _pscService;
        private readonly IContractsService _contractRefactorService;
        private readonly IUrlHelper _urlHelper;
        private readonly IRecentActivityService _recentActivityService;
        private readonly IFarContractService _farContractService;
        private readonly Logger _logger;
        private readonly Logger _eventLogger;
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IFolderStructureMasterService _folderStructureMasterService;
        private readonly IFolderStructureFolderService _folderStructureFolderService;
        private readonly IFolderService _folderService;
        private readonly IGroupPermissionService _groupPermission;
        private readonly IContractResourceFileService _contractResourceFileService;
        private readonly IQuestionaireUserAnswerService _questionaireUserAnswerService;
        private readonly IContractQuestionariesService _contractQuestionariesService;
        private readonly IProjectServiceCP _projectServiceCP;
        private readonly IProjectModServiceCP _projectModServiceCP;
        private readonly ICompanyRepository _companyRepository;
        private readonly IGenericNotificationService _genericNotificationService;
        private readonly ICustomerContactService _customerContactService;
        private readonly ICustomerService _customerService;
        private readonly INaicsService _naicsService;

        public ContractController(

            IMemoryCache cache,
            IPscService pscService,
            //IContractService contractService,
            IFileService fileService,
            IStateService stateService,
            ICountryService countryService,
            IFarContractTypeService farContractTypeService,
            IContractModificationService contractModificationService,
            IResourceAttributeService resourceAttributeService,
            IConfiguration configuration,
            IJobRequestService jobRequestService,
            INotificationBatchService notificationBatchService,
            INotificationMessageService notificationMessageService,
            IUserService userService,
            IResourceAttributeValueService resourceAttributeValueService,
            IMapper mapper,
            IContractsService contractRefactorService,
            INotificationTemplatesService notificationTemplatesService,
            IRevenueRecognitionService revenueRecognitionService,
            IUrlHelper urlHelper,
            IRecentActivityService recentActivityService,
            IQuestionaireUserAnswerService questionaireUserAnswerService,
            IFarContractService farContractService,
            IDocumentManagementService documentManagementService,
            IFolderStructureMasterService folderStructureMasterService,
            IFolderStructureFolderService folderStructureFolderService,
            IFolderService folderService,
            IGroupPermissionService groupPermission,
            IContractResourceFileService contractResourceFileService,
            IContractQuestionariesService contractQuestionariesService,
            IProjectServiceCP projectServiceCP,
            IProjectModServiceCP projectModServiceCP,
            ICompanyRepository companyRepository,
            IGenericNotificationService genericNotificationService,
            ICustomerContactService customerContactService,
            ICustomerService customerService,
            INaicsService naicsService,
            IHostingEnvironment env)
        {
            _cache = cache;
            _pscService = pscService;
            _fileService = fileService;
            //_contractService = contractService;
            _jobRequestService = jobRequestService;
            _farContractTypeService = farContractTypeService;
            _countryService = countryService;
            _stateService = stateService;
            _userService = userService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _resourceAttributeService = resourceAttributeService;
            _contractModificationService = contractModificationService;
            _revenueRecognitionService = revenueRecognitionService;
            _configuration = configuration;
            _urlHelper = urlHelper;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationBatchService = notificationBatchService;
            _notificationMessageService = notificationMessageService;
            _mapper = mapper;
            _recentActivityService = recentActivityService;
            _contractRefactorService = contractRefactorService;
            _configuration = configuration;
            _farContractService = farContractService;
            _documentManagementService = documentManagementService;
            _folderStructureMasterService = folderStructureMasterService;
            _folderStructureFolderService = folderStructureFolderService;
            _logger = LogManager.GetCurrentClassLogger();
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _folderService = folderService;
            _groupPermission = groupPermission;
            _contractResourceFileService = contractResourceFileService;
            _questionaireUserAnswerService = questionaireUserAnswerService;
            _contractQuestionariesService = contractQuestionariesService;
            _projectServiceCP = projectServiceCP;
            _projectModServiceCP = projectModServiceCP;
            _companyRepository = companyRepository;
            _genericNotificationService = genericNotificationService;
            _customerContactService = customerContactService;
            _customerService = customerService;
            _naicsService = naicsService;
        }

        private FilterByList GetFilterByList()
        {
            var filterList = new FilterByList();
            var filterBy = new Dictionary<string, string>();
            filterBy.Add("All", "All Contrat");
            filterBy.Add("Favourite", "My favourite");
            filterBy.Add("My", "My Contract");
            filterBy.Add("Recent", "Recently Viewed");
            filterList.FilterBy = filterBy;
            return filterList;
        }

        private string GetMessageForEmptyContractList(string filterType)
        {
            var message = string.Empty;
            if (string.IsNullOrWhiteSpace(filterType))
                return message;

            //for displaying message for empty list base on filter type
            if (filterType.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower())
            {
                message = @"No Contracts found created by you or assigned to you as a responsible party.  Please select an alternative filter criteria or view all available Contracts with the All Contracts filter.";
                return message;
            }
            else if (filterType.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower())
            {
                message = @"No Contracts have been added to Favorites. To add Contracts to Favorites, click on Contract List and select your Favorites from Contract row.";
                return message;
            }
            else if (filterType.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower())
            {
                message = @"No details for Contracts have been viewed previously. After Contract details are selected, these will display to show previous selections viewed.";
                return message;
            }
            else if (filterType.ToLower() == EnumGlobal.ActivityType.All.ToString().ToLower())
            {
                message = @"No Contracts found based upon the selected criteria.  Please select an alternative filter criteria or view all available Contracts with the All Contracts filter.";
                return message;
            }
            return message;
        }

        // GET: Contract
        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        public ActionResult Index(string searchValue, int filterList, bool showHideTaskOrder)
        {
            try
            {
                var refineSerchVal = string.Empty;
                if (!string.IsNullOrEmpty(searchValue))
                {
                    var splittedVal = searchValue.Split(' ');
                    refineSerchVal = string.Join(" ", splittedVal.Where(x => !string.IsNullOrEmpty(x)));
                }
                ContractViewModel contractViewModel = new ContractViewModel();
                contractViewModel.SearchValue = refineSerchVal;
                contractViewModel.idFilterList = filterList;
                contractViewModel.ShowHideTaskOrder = showHideTaskOrder;
                return View(contractViewModel);
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

                return BadRequest();
            }
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        [HttpGet]
        public IActionResult GetContracts(string searchValue, int pageSize, int skip, int take, string sortField, string dir, int switchOn = 0, string additionalFilter = "MyContract", string additionalFilterValue = "")
        {
            try
            {
                var postValue = new List<AdvancedSearchRequest>();
                var loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var contractList = new List<ContractAndProjectView>();
                int count = 0;
                var emptyListMessage = string.Empty;
                if (switchOn == 0)
                {
                    //later added for testing refactor code of contract
                    var cList = _contractRefactorService.GetContractList(searchValue, pageSize, skip, take, sortField, dir, postValue, loggedUserGuid, additionalFilter, false);
                    foreach (var contract in cList)
                    {
                        var view = ContractsMapper.MapEntityWithContractView(contract);
                        view.IsContract = true;
                        view.IsContractType = "Yes";
                        contractList.Add(view);
                    }
                    count = _contractRefactorService.GetAdvanceContractSearchCount(searchValue, postValue, loggedUserGuid, additionalFilter, false);
                }
                else
                {
                    var cList = _contractRefactorService.GetContractList(searchValue, pageSize, skip, take, sortField, dir, postValue, loggedUserGuid, additionalFilter, true);
                    foreach (var contract in cList)
                    {

                        var cont = ContractsMapper.MapEntityWithContractView(contract);
                        cont.IsContract = true;
                        cont.IsContractType = "Yes";
                        contractList.Add(cont);

                        if (contract.IsIDIQContract)
                        {
                            var projects = _contractRefactorService.GetTaskByContractGuid(cont.ContractGuid, loggedUserGuid);
                            projects = projects.OrderByDescending(c => c.ProjectNumber).ToList();
                            foreach (var prj in projects)
                            {
                                var prjView = ContractsMapper.MapEntityWithContractView(prj);
                                prjView.ContractTitle = prj.ContractTitle;
                                prjView.ContractNumber = cont.ContractNumber;
                                prjView.IsActiveStatus = prjView.IsActive ? ActiveStatus.Active.ToString() : ActiveStatus.Inactive.ToString();
                                prjView.IsContract = false;
                                prjView.IsContractType = "No";
                                prjView.ORGIDName = prj.Organisation.Name;
                                contractList.Add(prjView);
                            }
                        }
                    }
                    count = _contractRefactorService.GetAdvanceContractSearchCount(searchValue, postValue, loggedUserGuid, additionalFilter, true);
                }

                //for displaying message for empty list base on filter type
                if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.All.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                return Ok(new { result = contractList, count = count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        [HttpPost]
        public IActionResult GetContracts(string searchValue, int pageSize, int skip, int take, string sortField, string dir, int switchOn, [FromBody] List<AdvancedSearchRequest> postValue, string additionalFilter = "")
        {
            try
            {
                var loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var contractList = new List<ContractAndProjectView>();
                int count = 0;
                var emptyListMessage = string.Empty;
                if (string.IsNullOrWhiteSpace(additionalFilter))
                    additionalFilter = "All";
                if (switchOn == 0)
                {
                    //later added for testing refactor code of contract
                    var cList = _contractRefactorService.GetContractList(searchValue, pageSize, skip, take, sortField, dir, postValue, loggedUserGuid, additionalFilter, false);
                    foreach (var contract in cList)
                    {
                        var view = ContractsMapper.MapEntityWithContractView(contract);
                        view.IsContract = true;
                        contractList.Add(view);
                    }
                    count = _contractRefactorService.GetAdvanceContractSearchCount(searchValue, postValue, loggedUserGuid, additionalFilter, false);
                }
                else
                {
                    var cList = _contractRefactorService.GetContractList(searchValue, pageSize, skip, take, sortField, dir, postValue, loggedUserGuid, additionalFilter, true);
                    foreach (var contract in cList)
                    {
                        var cont = ContractsMapper.MapEntityWithContractView(contract);
                        cont.IsContract = true;
                        contractList.Add(cont);
                        if (contract.IsIDIQContract)
                        {
                            var projects = _contractRefactorService.GetTaskByContractGuid(cont.ContractGuid, loggedUserGuid);
                            projects = projects.OrderByDescending(c => c.ProjectNumber).ToList();
                            foreach (var prj in projects)
                            {
                                var prjView = ContractsMapper.MapEntityWithContractView(prj);
                                prjView.ContractTitle = prj.ContractTitle;
                                prjView.ContractNumber = cont.ContractNumber;
                                prjView.IsActiveStatus = prjView.IsActive ? ActiveStatus.Active.ToString() : ActiveStatus.Inactive.ToString();
                                prjView.IsContract = false;
                                contractList.Add(prjView);
                            }
                        }
                    }
                    count = _contractRefactorService.GetAdvanceContractSearchCount(searchValue, postValue, loggedUserGuid, additionalFilter, true);
                }

                //for displaying message for empty list base on filter type
                if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyContract.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.MyFavorite.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.RecentlyViewed.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                else if (additionalFilter.ToLower() == EnumGlobal.ActivityType.All.ToString().ToLower() && count == 0)
                {
                    emptyListMessage = GetMessageForEmptyContractList(additionalFilter);
                    return Ok(new { result = contractList, count = count, message = emptyListMessage });
                }
                return Ok(new { result = contractList, count });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        public ActionResult GetContractWithMod(Guid contractGuid)
        {
            var list = new List<ModListModel>();
            try
            {
                //later added service..
                var contracts = _contractRefactorService.GetDetailsForProjectByContractId(contractGuid);
                if (contracts != null)
                {
                    var contract = contracts;
                    var currency = string.Empty;
                    var currencyObject = _resourceAttributeService.GetByResource(contract.Currency).FirstOrDefault();
                    if (currencyObject != null)
                        currency = currencyObject.Title;
                    else
                        currency = contract.Currency;
                    var proj = new ModListModel
                    {
                        Amount = contract.FinancialInformation.AwardAmount ?? 0,
                        ContractNumber = contract.ContractNumber,
                        ProjectNumber = contract.BasicContractInfo.ProjectNumber,
                        IsMod = false,
                        Id = contract.ContractGuid,
                        Mod = "Base",
                        DateEntered = "",
                        EffectiveDate = "",
                        currency = currency,
                        Title = contract.BasicContractInfo.ContractTitle, // project title..
                        StartDate = contract.BasicContractInfo.POPStart.HasValue && contract.BasicContractInfo.POPStart.Value.Year > 1900 ? contract.BasicContractInfo.POPStart.Value.ToString("MM/dd/yyyy") : "",
                        EndDate = contract.BasicContractInfo.POPEnd.HasValue && contract.BasicContractInfo.POPEnd.Value.Year > 1900 ? contract.BasicContractInfo.POPEnd.Value.ToString("MM/dd/yyyy") : "",
                        Status = contract.IsActive,
                        FundingAmount = contract.FinancialInformation.FundingAmount
                    };
                    list.Add(proj);
                    var modList = _contractModificationService.GetAll(contract.ContractGuid, false, "", 100000, 0, "ModificationNumber", "desc");
                    modList = modList.OrderBy(c => c.ModificationNumber);
                    foreach (var mod in modList)
                    {
                        var item = new ModListModel
                        {
                            Amount = mod.AwardAmount.HasValue ? mod.AwardAmount.Value : 0,
                            ContractNumber = contract.ContractNumber,
                            ProjectNumber = mod.ProjectNumber,
                            IsMod = true,
                            currency = currency,
                            Id = mod.ContractModificationGuid,
                            Mod = mod.ModificationNumber,
                            ModificationType = mod.ModificationType,
                            Title = mod.ModificationTitle,
                            StartDate = mod.POPStart.HasValue && mod.POPStart.Value.Year > 1900 ? mod.POPStart.Value.ToString("MM/dd/yyyy") : "No change",
                            EndDate = mod.POPEnd.HasValue && mod.POPEnd.Value.Year > 1900 ? mod.POPEnd.Value.ToString("MM/dd/yyyy") : "No change",
                            DateEntered = mod.EnteredDate.HasValue && mod.EnteredDate.Value.Year > 1900 ? mod.EnteredDate.Value.ToString("MM/dd/yyyy") : "No change",
                            EffectiveDate = mod.EffectiveDate.HasValue && mod.EffectiveDate.Value.Year > 1900 ? mod.EffectiveDate.Value.ToString("MM/dd/yyyy") : "No change",
                            Status = mod.IsActive,
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

        [Secure(ResourceType.Contract, ResourceActionPermission.Add)]
        [HttpGet]
        public ActionResult Add()
        {
            ContractViewModel contractViewModel = new ContractViewModel();
            BasicContractInfoViewModel basicContractInfoViewModel = new BasicContractInfoViewModel();
            FinancialInformationViewModel FinancialInformationViewModel = new FinancialInformationViewModel() { Currency = "USD" };
            KeyPersonnelViewModel keyPersonnelViewModel = new KeyPersonnelViewModel(); //just initialize ..

            var customerAttributeValuesViewModel = InitialLoad();

            basicContractInfoViewModel.IsIDIQContract = true;
            basicContractInfoViewModel.IsPrimeContract = true;
            basicContractInfoViewModel.CPAREligible = true;
            basicContractInfoViewModel.QualityLevelRequirements = true;

            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;
            contractViewModel.BasicContractInfo = basicContractInfoViewModel;
            contractViewModel.FinancialInformation = FinancialInformationViewModel;
            contractViewModel.KeyPersonnel = keyPersonnelViewModel;
            contractViewModel.ContractFileViewModel = new ContractFileViewModel();


            ViewBag.Resourcekey = ResourceType.Contract.ToString();

            var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
            var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);

            ViewBag.UpdatedBy = users.DisplayName;
            ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");

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

        [Secure(ResourceType.Contract, ResourceActionPermission.Details)]
        [HttpGet]
        public ActionResult Details(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            var contractEntity = _contractRefactorService.GetDetailById(id);

            ContractViewModel contractModel = new ContractViewModel();
            contractModel = ContractsMapper.MapEntityToModel(contractEntity, true);
            contractModel = CheckAuthorizationByRole(contractModel);

            if (contractModel.BasicContractInfo.FarContractTypeGuid != null ||
               contractModel.BasicContractInfo.FarContractTypeGuid != Guid.Empty)
            {
                var farContractType = _farContractTypeService.GetById(contractModel.BasicContractInfo.FarContractTypeGuid);
                contractModel.BasicContractInfo.FarContractType = farContractType != null ? farContractType.Title : "Not Entered";
            }

            contractModel.JobRequest = _mapper.Map<JobRequestViewModel>(contractEntity.JobRequest);
            if (contractModel.JobRequest != null)
            {
                var jobRequestDisplayName = _userService.GetUserByUserGuid(contractModel.JobRequest.UpdatedBy);
                if (jobRequestDisplayName != null)
                {
                    contractModel.JobRequest.Displayname = jobRequestDisplayName.DisplayName;
                }
            }

            contractModel.ContractWBS = _mapper.Map<ContractWBSViewModel>(contractEntity.ContractWBS);
            if (contractModel.ContractWBS != null)
            {
                var contractWBSDisplayName = _userService.GetUserByUserGuid(contractModel.ContractWBS.UpdatedBy);
                if (contractWBSDisplayName != null)
                {
                    contractModel.ContractWBS.Displayname = contractWBSDisplayName.DisplayName;
                }
            }

            contractModel.EmployeeBillingRatesViewModel = _mapper.Map<EmployeeBillingRatesViewModel>(contractEntity.EmployeeBillingRates);
            if (contractModel.EmployeeBillingRatesViewModel != null)
            {
                var employeeBillingRatesDisplayName = _userService.GetUserByUserGuid(contractModel.EmployeeBillingRatesViewModel.UpdatedBy);
                if (employeeBillingRatesDisplayName != null)
                {
                    contractModel.EmployeeBillingRatesViewModel.Displayname = employeeBillingRatesDisplayName.DisplayName;
                }
            }

            contractModel.LaborCategoryRates = _mapper.Map<LaborCategoryRatesViewModel>(contractEntity.LaborCategoryRates);
            if (contractModel.LaborCategoryRates != null)
            {
                var laborCategoryRatesDisplayName = _userService.GetUserByUserGuid(contractModel.LaborCategoryRates.UpdatedBy);
                if (laborCategoryRatesDisplayName != null)
                {
                    contractModel.LaborCategoryRates.Displayname = laborCategoryRatesDisplayName.DisplayName;
                }
            }

            contractModel.RevenueRecognitionModel = _mapper.Map<RevenueRecognitionViewModel>(contractEntity.RevenueRecognization);
            if (contractModel.RevenueRecognitionModel != null)
            {
                var totalRevenueByContract = _revenueRecognitionService.CountRevenueByContractGuid(id);
                if (totalRevenueByContract > 0)
                    contractModel.RevenueRecognitionModel.isViewHistory = true;

                if (contractModel.RevenueRecognitionModel.IsNotify)
                {
                    if (contractModel.IsAccountingRepresentative)
                    {
                        contractModel.IsAuthorizedForRevenue = true;
                        if (contractModel.IsAccountingRepresentative)
                            contractModel.RevenueRecognitionModel.IsAccountRepresentive = true;
                    }
                    else if (contractModel.IsContractRepresentative)
                        contractModel.IsAuthorizedForRevenue = true;
                }
                else if (contractModel.IsContractRepresentative)
                {
                    contractModel.IsAuthorizedForRevenue = true;
                    contractModel.RevenueRecognitionModel.IsAccountRepresentive = false;
                }
                var revenueRecognitionDisplayName = _userService.GetUserByUserGuid(contractModel.RevenueRecognitionModel.UpdatedBy);
                if (revenueRecognitionDisplayName != null)
                {
                    contractModel.RevenueRecognitionModel.Displayname = revenueRecognitionDisplayName.DisplayName;
                }
            }

            if (contractModel.JobRequest != null)
            {
                switch (contractModel.JobRequest.Status)
                {
                    case 2:
                        if (contractModel.IsProjectControls)
                            contractModel.IsJobEditable = true;
                        break;
                    case 3:
                        if (contractModel.IsProjectManager)
                            contractModel.IsJobEditable = true;
                        break;
                    case 4:
                        if (contractModel.IsAccountingRepresentative)
                            contractModel.IsJobEditable = true;
                        break;
                    default:
                        contractModel.IsJobEditable = false;
                        break;
                }
            }

            var currentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var activity = new RecentActivity()
            {
                RecentActivityGuid = Guid.NewGuid(),
                CreatedBy = currentUserGuid,
                CreatedOn = DateTime.Now,
                Entity = "Contract",
                EntityGuid = id,
                UserAction = EnumGlobal.ActivityType.RecentlyViewed.ToString(),
                IsDeleted = false,
                UpdatedBy = currentUserGuid,
                UpdatedOn = DateTime.Now,
                UserGuid = currentUserGuid,
            };
            _recentActivityService.InsertRecentActivity(activity);

            var currentUser = _userService.GetUserByUserGuid(currentUserGuid);
            if (currentUser != null)
            {
                var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.DisplayName;
            }
            ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
            ViewBag.ResourceId = contractModel.ContractGuid;
            ViewBag.Resourcekey = ResourceType.Contract.ToString();
            ViewBag.FilePath = string.Format(
                    $@"{contractModel.BasicContractInfo.ContractNumber}");

            if (contractModel.IsContractRepresentative || contractModel.IsProjectManager || contractModel.IsAccountingRepresentative)
            {
                contractModel.IsAuthorizedForContractClose = true;
            }
            if (contractModel.BasicContractInfo.IsIDIQContract)
            {
                contractModel.IsAuthorizedForContractClose = false;
            }
            var closeOutDetails = _questionaireUserAnswerService.GetDetailById(contractModel.ContractGuid, ResourceType.ContractCloseOut.ToString());

            if (closeOutDetails != null)
            {
                var closeOut = new ContractCloseOutDetail();
                var users = _userService.GetUserByUserGuid(closeOutDetails.UpdatedBy);
                closeOut.UpdatedBy = users?.DisplayName;
                closeOut.UpdatedOn = closeOutDetails.UpdatedOn.ToString("MM/dd/yyyy");
                contractModel.ContractCloseOutDetail = closeOut;
            }
            var questionniareData = _contractRefactorService.GetAnswer(contractModel.ContractGuid);
            QuestionaireViewModel questionaireViewModel = new QuestionaireViewModel();
            var questionnniare = _questionaireUserAnswerService.GetDetailById(contractModel.ContractGuid, ResourceType.FarContract.ToString());

            if (questionnniare != null)
            {
                questionaireViewModel.UpdatedOn = questionnniare.UpdatedOn;
                questionaireViewModel.DisplayName = _userService.GetUserByUserGuid(questionnniare.UpdatedBy)?.DisplayName;
                contractModel.Questionaire = questionaireViewModel;
            }

            var farContract = _farContractService.GetAvailableFarContractByContractGuid(contractModel.ContractGuid);
            if (farContract != null)
            {
                contractModel.IsFarContractAvailable = true;
                contractModel.Questionaire = new QuestionaireViewModel();
                contractModel.Questionaire.DisplayName = farContract.DisplayName;
                contractModel.Questionaire.UpdatedOn = farContract.UpdatedOn;
            }

            ///Get group permissions in viewbags..
            GetGroupPermissions();

            try
            {
                //Check if this contract exist in cost point..
                var projectCP = _projectServiceCP.GetCostPointProjectByProjectNumber(contractModel.BasicContractInfo.ProjectNumber);
                ViewBag.IsExistInCostPoint = projectCP == null ? false : true;

                //get user by id 
                var user = _userService.GetUserByUserGuid(contractModel.CreatedBy);
                if (user != null)
                    contractModel.CreatedByName = user.DisplayName;

                return View("Details", contractModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractModel);
            }
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.Add)]
        [HttpPost]
        public IActionResult Add(ContractViewModel contractViewModel)
        {
            try
            {
                //check validations..
                var IsExistContractNumber =
                   _contractRefactorService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractNumber))
                {
                    ModelState.AddModelError("", "Contract Number not found!!");
                }
                if (IsExistContractNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Contract Number !!");
                }

                var isExistProjectNumber =
                        _contractRefactorService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber, contractViewModel.ContractGuid);


                if (isExistProjectNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Project Number !!");
                }

                if (contractViewModel.BasicContractInfo.ORGID == Guid.Empty)
                {
                    ModelState.AddModelError("", "Please select organization Id.");
                }

                var isExistContractTitle =
                    _contractRefactorService.IsExistContractTitle(contractViewModel.BasicContractInfo.ContractTitle, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractTitle))
                {
                    ModelState.AddModelError("", "Contract Title not found!!");
                }
                if (isExistContractTitle)
                {
                    ModelState.AddModelError("", " Found Duplicate Contract Title !!");
                }

                if (ModelState.IsValid)
                {
                    contractViewModel.Status = ContractStatus.Active.ToString();
                    var contractModel = ContractsMapper.MapModelToEntity(contractViewModel);
                    contractModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModel.IsActive = true;
                    contractModel.IsDeleted = false;

                    var result = _contractRefactorService.Save(contractModel);

                    var contractGuid = result.Message["contractGuid"];

                    //create folder template ..
                    var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                    _folderService.CreateFolderTemplate(contractModel.ContractGuid.ToString(), contractViewModel.BasicContractInfo.ProjectNumber, ResourceType.Contract.ToString(), new Guid(contractGuid.ToString()), userGuid);

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), ResourceType.Contract.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractGuid);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Contract", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModel.BasicContractInfo, resource, (Guid)contractGuid, UserHelper.GetHostedIp(HttpContext), "Contract Added", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                    //end of log..

                    if (contractModel.FarContractTypeGuid != null && contractModel.FarContractTypeGuid != Guid.Empty)
                    {
                        SaveFarContract((Guid)contractGuid, contractModel.FarContractTypeGuid);
                    }
                    SaveAndNotifyRevenueRepresentative(contractModel, (Guid)contractGuid);

                    //after adding new  contract send notification..
                    var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.Contract.ToString(),
                        EnumGlobal.CrudType.Create.ToString());
                    var redirectUrl = string.Format($@"/Contract/Details/{contractGuid}");

                    var parameter = new
                    {
                        redirectUrl = redirectUrl,
                        key = key,
                        cameFrom = "Contract Management",
                        resourceName = contractModel.ProjectNumber,
                        resourceDisplayName = "Contract",
                        resourceId = contractGuid,
                    };

                    //generate link for notification...
                    var notificationLink = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "Notification", "Index", parameter);

                    var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys("Base", (Guid)contractGuid);


                    var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                    if (currentUser == null)
                        return RedirectToAction("Details", new { id = contractGuid });
                    var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                    var jsonObject = new
                    {
                        status = true,
                        notificationLink = notificationLink,
                        resourceId = contractGuid,
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
                //ModelState.AddModelError("", e.Message);
                ModelState.AddModelError("", " Some error occurred !!");
            }

            return new JsonResult(new
            {
                status = false,
                Errors = ModelState
            });

            // in any case return this if not success..
            //var customerAttributeValuesViewModel = InitialLoad();

            //contractViewModel.BasicContractInfo.IsIDIQContract = true;
            //contractViewModel.BasicContractInfo.IsPrimeContract = true;
            //contractViewModel.BasicContractInfo.CPAREligible = true;
            //contractViewModel.BasicContractInfo.QualityLevelRequirements = true;

            //contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
            //contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            //contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);

            //contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            //ViewBag.Resourcekey = ResourceType.Contract.ToString();

            //return View(contractViewModel);
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.Edit)]
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return null;
            }
            ContractViewModel contractViewModel = new ContractViewModel();
            var contractEntity = _contractRefactorService.GetDetailById(id);
            contractViewModel = ContractsMapper.MapEntityToModel(contractEntity, false);

            var customerAttributeValuesViewModel = InitialLoad();
            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            FileAttachmentParameter(contractViewModel);
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

        private void FileAttachmentParameter(ContractViewModel contractViewModel)
        {
            var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
            if (currentUser != null)
            {
                var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.DisplayName;
            }

            ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
            ViewBag.ResourceId = contractViewModel.ContractGuid;
            ViewBag.Resourcekey = ResourceType.Contract.ToString();
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.Edit)]
        [HttpPost]
        public IActionResult Edit([FromForm]ContractViewModel contractViewModel)
        {
            try
            {
                var isProjectNumberChanged = false;
                //check validations..
                var IsExistContractNumber =
                   _contractRefactorService.IsExistContractNumber(contractViewModel.BasicContractInfo.ContractNumber, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractNumber))
                {
                    ModelState.AddModelError("", "Contract Number not found!!");
                }
                if (IsExistContractNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Contract Number !!");
                }

                var isExistProjectNumber =
                        _contractRefactorService.IsExistProjectNumber(contractViewModel.BasicContractInfo.ProjectNumber, contractViewModel.ContractGuid);


                if (isExistProjectNumber)
                {
                    ModelState.AddModelError("", " Found Duplicate Project Number !!");
                }

                if (contractViewModel.BasicContractInfo.ORGID == Guid.Empty)
                {
                    ModelState.AddModelError("", "Please select organization Id.");
                }

                var isExistContractTitle =
                    _contractRefactorService.IsExistContractTitle(contractViewModel.BasicContractInfo.ContractTitle, contractViewModel.ContractGuid);

                if (string.IsNullOrEmpty(contractViewModel.BasicContractInfo.ContractTitle))
                {
                    ModelState.AddModelError("", "Contract Title not found!!");
                }
                if (isExistContractTitle)
                {
                    ModelState.AddModelError("", " Found Duplicate Contract Title !!");
                }

                if (ModelState.IsValid)
                {
                    var contractModel = ContractsMapper.MapModelToEntity(contractViewModel);

                    contractModel = ValidateModel(contractModel);

                    contractModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                    var previousAwarddetails = _contractRefactorService.GetAmountById(contractViewModel.ContractGuid);
                    decimal? previousContractAwardAmount = 0.00M;
                    if (previousAwarddetails.RevenueRecognitionGuid != Guid.Empty && previousAwarddetails.RevenueRecognitionGuid != null)
                    {
                        previousContractAwardAmount = previousAwarddetails.AwardAmount;
                    }

                    var existingContract = _contractRefactorService.GetBasicContractById(contractModel.ContractGuid);

                    if (existingContract != null)
                    {
                        if (existingContract.ProjectNumber != contractModel.ProjectNumber)
                        {
                            isProjectNumberChanged = true;
                        }
                    }

                    var result = _contractRefactorService.Save(contractModel);

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), ResourceType.Contract.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModel.ContractGuid);
                    //var additionalInformationWithUri = string.Format("<a href=\"{0}\">{1}</a>", additionalInformationURl, additionalInformation);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.Contract.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Contract", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractViewModel.BasicContractInfo, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Contract Edited", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
                    //end of log..

                    //if (result.StatusName.ToString().ToLower() == "fail")
                    //{
                    //    if (result.StatusName.ToString().ToLower() == "fail")
                    //    {
                    //        foreach (KeyValuePair<string, object> msg in result.Message)
                    //        {
                    //            var ob = (List<string>)msg.Value;
                    //            foreach (var msg1 in ob)
                    //            {
                    //                ModelState.AddModelError("", msg1.ToString());
                    //            }
                    //        }
                    //        throw new ArgumentException();
                    //    }
                    //}

                    if (contractModel.FarContractTypeGuid != null && contractModel.FarContractTypeGuid != Guid.Empty)
                    {
                        SaveFarContract(contractModel.ContractGuid, contractModel.FarContractTypeGuid);
                    }


                    // For Revenue
                    decimal? totalModAwardAmountSum = 0.00M;
                    //sum of mod award amount where the mod amount was not calculated previously.
                    var sumOfModAwardAmount = _contractModificationService.GetTotalAwardAmount(contractModel.ContractGuid);
                    if (sumOfModAwardAmount != null)
                    {
                        totalModAwardAmountSum = (sumOfModAwardAmount.AwardAmount == null ? 0.00M : sumOfModAwardAmount.AwardAmount);
                    }
                    //gets the actual amount or the differenced amount
                    decimal? actualAmount = contractModel.AwardAmount - previousContractAwardAmount;
                    if (actualAmount > 0)
                    {
                        decimal? currentAmount = totalModAwardAmountSum + actualAmount;
                        if (RevenueRecognitionHelper.IsValidForRevenueRecognitionRequest(_configuration, contractModel.ContractType, currentAmount, 0.00M))
                        {
                            contractModel.IsUpdated = true;
                            AddNewRevenueAndUpdateContractRevenueGuid(contractModel, contractModel.ContractGuid);
                        }
                    }




                    ////generate link for notification...
                    //var notificationLink = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "Notification", "Index", parameter);

                    var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys("Base", contractViewModel.ContractGuid);

                    //var uploadPath = string.Format(
                    //  $@"{contractViewModel.BasicContractInfo.ContractNumber}");

                    var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                    var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                    var close = ContractStatus.Closed.ToString();
                    if (contractModel.Status != null)
                    {
                        if (contractModel.Status.ToUpper() == close.ToUpper())
                        {
                            string contractClosekey = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.ContractCloseOut.ToString(),
                            EnumGlobal.CrudType.Notify.ToString());
                            AddNotificationMessage(contractModel, contractModel.ContractGuid, contractClosekey);
                        }
                    }
                    //var jsonObject = new
                    //{
                    //    status = true,
                    //    notificationLink = notificationLink,
                    //    resourceId = contractViewModel.ContractGuid,
                    //    //uploadPath = uploadPath,
                    //    uploadPath = contractResourceFile.FilePath,
                    //    resourceName = contractModel.ProjectNumber,
                    //    updatedBy = users.DisplayName,
                    //    parentId = contractResourceFile.ContractResourceFileGuid,
                    //    updatedOn = CurrentDateTimeHelper.GetCurrentDateTime()
                    //};
                    //return Json(jsonObject);

                    //after updating contract send notification..
                    var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.Contract.ToString(),
                        EnumGlobal.CrudType.Edit.ToString());
                    var redirectUrl = string.Format($@"/Contract/Details/{contractViewModel.ContractGuid}");

                    var parameter = new
                    {
                        redirectUrl = redirectUrl,
                        key = key,
                        cameFrom = "Contract Management",
                        resourceName = contractModel.ProjectNumber,
                        resourceDisplayName = "Contract",
                        resourceId = contractModel.ContractGuid,
                    };

                    //Rename Document root foldername
                    if (isProjectNumberChanged)
                    {
                        _documentManagementService.RenameRootFolder("Contract", contractModel.ProjectNumber, contractModel.ContractGuid, UserHelper.CurrentUserGuid(HttpContext));
                    }

                    return RedirectToAction("Index", "Notification", parameter);
                }
            }
            //catch (ArgumentException)
            //{
            //}
            catch (Exception e)
            {
                //ModelState.AddModelError("", e.Message);
                BadRequestFormatter.BadRequest(this, e);
                ModelState.AddModelError("", " Some error occurred !!");
            }

            // in any case return this if not success..
            var customerAttributeValuesViewModel = InitialLoad();

            contractViewModel.BasicContractInfo.IsIDIQContract = true;
            contractViewModel.BasicContractInfo.IsPrimeContract = true;
            contractViewModel.BasicContractInfo.CPAREligible = true;
            contractViewModel.BasicContractInfo.QualityLevelRequirements = true;

            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);
            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            contractViewModel.CountrySelectListItems = _countryService.GetCountryList().ToDictionary(x => x.CountryId, x => x.CountryName);

            contractViewModel.CustomerAttributeValuesModel = customerAttributeValuesViewModel;

            FileAttachmentParameter(contractViewModel);
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
            customerAttributeValuesViewModel.radioCPAREligible = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioIsIDIQContract = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioIsPrimeContract = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioQualityLevelRequirements = KeyValueHelper.GetYesNo();
            customerAttributeValuesViewModel.radioSBA = KeyValueHelper.GetYesNo();

            customerAttributeValuesViewModel.ddlBillingFormula = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "BillingFormula");
            customerAttributeValuesViewModel.ddlRevenueFormula = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "RevenueFormula");
            customerAttributeValuesViewModel.ddlStatus = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "Status");
            customerAttributeValuesViewModel.ddlFarContractType = _farContractTypeService.GetAllList().ToDictionary(x => x.FarContractTypeGuid, x => x.Title);
            return customerAttributeValuesViewModel;
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.Delete)]
        [HttpPost]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _contractRefactorService.Delete(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractRefactorService.GetContractEntityByContractId(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), ResourceType.Contract.ToString().ToLower());
                    //var resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.Contract.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Contract", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Contract  Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
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

        [Secure(ResourceType.Contract, ResourceActionPermission.Edit)]
        [HttpPost]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _contractRefactorService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractRefactorService.GetContractEntityByContractId(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), ResourceType.Contract.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModel.ContractGuid);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.Contract.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Contract", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Contract Disabled", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
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

        [Secure(ResourceType.Contract, ResourceActionPermission.Edit)]
        [HttpPost]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _contractRefactorService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModel = _contractRefactorService.GetContractEntityByContractId(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), ResourceType.Contract.ToString().ToLower());
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModel.ContractGuid);
                    //var resource = string.Format("{0} </br> GUID:{1} </br> Contract No:{2} </br> Project No:{3} </br> Contract Title:{4}", ResourceType.Contract.ToString(), contractModel.ContractGuid, contractModel.ContractNumber, contractModel.ProjectNumber, contractModel.ContractTitle);
                    var resource = string.Format("{0} </br> Project No :{1} Title:{2}", "Contract", contractModel.ProjectNumber, contractModel.ContractTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), id, resource, contractModel.ContractGuid, UserHelper.GetHostedIp(HttpContext), "Contract Enabled", Guid.Empty, "Successful", "", additionalInformation, additionalInformationURl);
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

        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        [HttpPost]
        public IActionResult SetAsFavouriteContract([FromBody] Guid[] ids)
        {
            try
            {
                var currentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                var activity = new RecentActivity()
                {
                    RecentActivityGuid = Guid.NewGuid(),
                    CreatedBy = currentUserGuid,
                    CreatedOn = DateTime.Now,
                    Entity = "Contract",
                    EntityGuid = ids.FirstOrDefault(),
                    UserAction = EnumGlobal.ActivityType.MyFavorite.ToString(),
                    IsDeleted = false,
                    UpdatedBy = currentUserGuid,
                    UpdatedOn = DateTime.Now,
                    UserGuid = currentUserGuid,
                };
                _recentActivityService.InsertRecentActivity(activity);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully added as favourite !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [Secure(ResourceType.Contract, ResourceActionPermission.List)]
        [HttpPost]
        public IActionResult RemoveAsFavouriteContract([FromBody] Guid[] ids)
        {
            try
            {
                _recentActivityService.RemoveFromActivity(ids.FirstOrDefault(), "Contract");
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Removed as favourite !!" });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value); ;
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

                var stateWithGrtOrNot = states.Select(x => new
                { Keys = x.StateId, Values = x.GRT == true ? x.StateName + " (GRT)" : x.StateName });
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

        [HttpGet]
        [Authorize]
        public IActionResult GetSelectedStatesByStateIds(string statesIds)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            try
            {
                if (!string.IsNullOrEmpty(statesIds))
                {
                    var statesIdStringArray = statesIds.Split(',');
                    var states = _stateService.GetStatesByStateIds(statesIdStringArray);
                    var stateWithGrtOrNot = states.Select(x => new
                    { Keys = x.StateId, Values = x.GRT == true ? x.StateName + " (GRT)" : x.StateName });

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
                var listData = _contractRefactorService.GetOrganizationData(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Infrastructure.Helpers.FormatHelper.FormatAutoCompleteData(ob.Name, ob.Title);
                    model.label = result.Trim();
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
        public IActionResult GetNAICSCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _naicsService.GetNaicsList(searchText);
                List<AutoCompleteReturnModel> multiSelectReturnModels = new List<AutoCompleteReturnModel>();
                foreach (var ob in listData)
                {
                    AutoCompleteReturnModel model = new AutoCompleteReturnModel();
                    var result = Infrastructure.Helpers.FormatHelper.FormatAutoCompleteData(ob.Code, ob.Title);
                    model.label = result.Trim();
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
        public IActionResult GetPSCCodeData([FromBody] string searchText)
        {
            try
            {
                var listData = _pscService.GetPscList(searchText);
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
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllContactByCustomer(Guid customerId)
        {
            try
            {
                var contractRepresentative = _customerContactService.GetAllContactByCustomer(customerId, ContactType.ContractRepresentative.ToString());
                var technicalContractRepresentative = _customerContactService.GetAllContactByCustomer(customerId, ContactType.TechnicalContractRepresentative.ToString());
                var joinContactResult = contractRepresentative.Union(technicalContractRepresentative).ToList();
                return Json(new { contractRepresentative = joinContactResult, technicalContractRepresentative = joinContactResult });
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
                var result = _contractRefactorService.GetCompanyRegionAndOfficeNameByCode(entityCode);
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
                if (contact != null)
                {
                    if (string.IsNullOrWhiteSpace(contact.EmailAddress))
                        contact.EmailAddress = "N/A";
                    if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
                        contact.PhoneNumber = "N/A";
                }

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
                if (agency != null)
                {
                    if (string.IsNullOrWhiteSpace(agency.PrimaryEmail))
                        agency.PrimaryEmail = "N/A";
                    if (string.IsNullOrWhiteSpace(agency.PrimaryPhone))
                        agency.PrimaryPhone = "N/A";
                }
                return Json(agency);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new Customer());
            }
        }

        [Authorize]
        public IActionResult DownloadDocument(string filePath, string fileName)
        {
            try
            {
                var fullFilePath = _configuration.GetSection("Document").GetSection("DocumentRoot").Value + filePath;
                return _fileService.GetFile(fileName, fullFilePath);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        public IActionResult DownloadSampleDocument(string filePath, string fileName)
        {
            try
            {
                var fullPath = filePath + "/" + fileName;
                return _fileService.GetFile(fileName, fullPath);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IActionResult ContractBrief(Guid contractGuid)
        {
            var contract = _contractRefactorService.GetContractEntityByContractId(contractGuid);
            ViewBag.ProjectNumber = contract.ProjectNumber;
            ViewBag.ProjectNumber = contract.ProjectNumber;
            ViewBag.Currnecy = contract.Currency;
            ViewBag.IsContract = (contract.ParentContractGuid == Guid.Empty || contract.ParentContractGuid == null) ? true : false;
            return View(contractGuid);
        }

        public IActionResult GetContractBriefData(Guid contractGuid)
        {
            try
            {
                var contract = _contractRefactorService.GetContractEntityByContractId(contractGuid);
                var orgInfo = _contractRefactorService.GetOrganizationByOrgId(contract.ORGID);
                var companyCode = orgInfo.Name.Split(".")[0];

                var companyInfo = _companyRepository.GetCompanyByCode(companyCode);

                ContractBriefViewModel contractBriefViewModel = new ContractBriefViewModel();

                contractBriefViewModel.ContractorName = companyInfo.CompanyName;
                contractBriefViewModel.ContractNumber = contract.ContractNumber;
                contractBriefViewModel.DateOfAward = contract.POPStart?.ToString("MM/dd/yyyy");
                contractBriefViewModel.ContractJobNumber = contract.ProjectNumber;
                contractBriefViewModel.IsSubContract = !contract.IsPrimeContract ? true : false;
                contractBriefViewModel.BriefStatementOfScopeOfWork = contract.Description;

                var projectCP = _projectServiceCP.GetCostPointProjectByProjectNumber(contract.ProjectNumber);
                var projectModCP = _projectModServiceCP.GetCostPointProjectModsByProjectNumber(contract.ProjectNumber);
                var projectModCPEntity = _projectModServiceCP.GetBriefedThroughModNo(contract.ProjectNumber);

                //get questions and answers..
                var questionaires = _contractQuestionariesService.GetContractQuestionaries(ResourceType.FarContract.ToString(), "Edit", contractGuid).ToList();

                //get selected far contract..
                Guid farContractType = _contractRefactorService.GetFarContractTypeGuidById(contractGuid);
                var selectedFars = _farContractService.GetSelectedData(contractGuid, farContractType);

                var jsonObject = new
                {
                    ProjectCp = projectCP,
                    ProjectModCP = projectModCP,
                    ProjectModCPEntity = projectModCPEntity,
                    ProjectCms = contractBriefViewModel,
                    Questionaires = questionaires,
                    SelectedFars = selectedFars
                };
                return Json(jsonObject);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        public IActionResult checkServerSideValidation(Guid contractId, string controlId, string val)
        {

            string result = string.Empty;
            if (controlId.ToLower().Contains("contractnumber"))
            {
                var IsExistContractNumber =
                  _contractRefactorService.IsExistContractNumber(val, contractId);

                if (string.IsNullOrEmpty(val))
                {
                    result = "Contract Number not found!!";
                }
                if (IsExistContractNumber)
                {
                    result = " Found Duplicate Contract Number !!";
                }
            }
            else if (controlId.ToLower().Contains("projectnumber"))
            {
                var isExistProjectNumber =
                       _contractRefactorService.IsExistProjectNumber(val, contractId);

                if (isExistProjectNumber)
                {
                    result = " Found Duplicate Project Number !!";
                }
            }
            else if (controlId.ToLower().Contains("contracttitle"))
            {
                var isExistContractTitle =
                    _contractRefactorService.IsExistContractTitle(val, contractId);

                if (string.IsNullOrEmpty(val))
                {
                    result = "Contract Title not found!!";
                }
                if (isExistContractTitle)
                {
                    result = " Found Duplicate Contract Title !!";
                }
            }

            if (!string.IsNullOrEmpty(result))
                return Json(new { Status = "Fail", result = result });

            return Json(new { Status = "Ok", result = result });
        }

        #region private

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
            var key = "";
            if (contractModel.IsUpdated)
            {
                key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                        EnumGlobal.ResourceAction.ContractUpdate.ToString());
            }
            else
            {
                key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                           EnumGlobal.ResourceAction.ContractCreate.ToString());
            }
            if (AddNotificationMessage(contractModel, contractGuid, key))
            {
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
                    _contractRefactorService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, contractGuid);
                    _contractModificationService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, contractGuid);
                }
                return isSaved;
            }
            return false;
        }

        private bool AddNotificationMessage(Contracts contractModel, Guid contractGuid, string key)
        {
            try
            {
                var notificationModel = new GenericNotificationViewModel();
                var notificationTemplatesDetails = new NotificationTemplatesDetail();
                var userList = new List<User>();
                var receiverInfo = new User();
                Guid? receiverGuid = Guid.Empty;
                decimal thresholdAmount = 0.00M;

                notificationModel.ResourceId = contractModel.ContractGuid;
                notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/contract/Details/" + contractModel.ContractGuid);
                notificationModel.NotificationTemplateKey = key;
                notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                notificationModel.SendEmail = true;


                var keyPersonnels = _contractRefactorService.GetKeyPersonnelByContractGuid(contractModel.ContractGuid);

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

        private ContractViewModel CheckAuthorizationByRole(ContractViewModel model)
        {
            var accountdetail = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                                .FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative);
            if (accountdetail != null)
            {
                Guid accountingRepresentative = accountdetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, accountingRepresentative))
                {
                    model.IsAccountingRepresentative = true;
                }
            }
            var contractRepresentativeDetail = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                               .FirstOrDefault(x => x.UserRole == ContractUserRole._contractRepresentative);
            if (contractRepresentativeDetail != null)
            {
                Guid contractRepresentative = contractRepresentativeDetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, contractRepresentative))
                {
                    model.IsContractRepresentative = true;
                }
            }

            var projectManagerDetail = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                               .FirstOrDefault(x => x.UserRole == ContractUserRole._projectManager);
            if (projectManagerDetail != null)
            {
                Guid projectManagerGuid = projectManagerDetail.UserGuid;
                if (UserHelper.IsAuthorizedRepresentative(HttpContext, projectManagerGuid))
                {
                    model.IsProjectManager = true;
                }
            }

            var projectControlsDetail = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid)
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

        private void CreateFolderTemplate(string contractGuid, string projectNumber, string resourceType, Guid resourceId)
        {
            var UserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var structureApplied = _documentManagementService.HasDefaultStructure(resourceType, resourceId);
            if (!structureApplied)
            {
                var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
                if (masterData.Count() > 0)
                {
                    var templateFolders = _folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid);
                    _documentManagementService.ManageDefaultStructure(contractGuid, projectNumber, templateFolders, resourceType, resourceId, UserGuid);
                }
            }
        }

        private bool IsAuthorizedForResource(EnumGlobal.ResourceType resourceType, EnumGlobal.ResourceActionPermission action)
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
            ViewBag.RevenueRecognitionAdd = IsAuthorizedForResource(ResourceType.RevenueRecognition, ResourceActionPermission.Add);
            ViewBag.RevenueRecognitionEdit = IsAuthorizedForResource(ResourceType.RevenueRecognition, ResourceActionPermission.Edit);
            ViewBag.RevenueRecognitionList = IsAuthorizedForResource(ResourceType.RevenueRecognition, ResourceActionPermission.List);

            ///Job Request
            ViewBag.JobRequestAdd = IsAuthorizedForResource(ResourceType.JobRequest, ResourceActionPermission.Add);
            ViewBag.JobRequestEdit = IsAuthorizedForResource(ResourceType.JobRequest, ResourceActionPermission.Edit);
            ViewBag.JobRequestList = IsAuthorizedForResource(ResourceType.JobRequest, ResourceActionPermission.List);
            ViewBag.JobRequestDetail = IsAuthorizedForResource(ResourceType.JobRequest, ResourceActionPermission.Details);

            ///Wbs
            ViewBag.WbsAdd = IsAuthorizedForResource(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Add);
            ViewBag.WbsEdit = IsAuthorizedForResource(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Edit);
            ViewBag.WbsDetail = IsAuthorizedForResource(ResourceType.WorkBreakDownStructure, ResourceActionPermission.Details);

            ///Employee Billing Rates
            ViewBag.EmployeeBillingRatesAdd = IsAuthorizedForResource(ResourceType.EmployeeBillingRates, ResourceActionPermission.Add);
            ViewBag.EmployeeBillingRatesEdit = IsAuthorizedForResource(ResourceType.EmployeeBillingRates, ResourceActionPermission.Edit);
            ViewBag.EmployeeBillingRatesDetail = IsAuthorizedForResource(ResourceType.EmployeeBillingRates, ResourceActionPermission.Details);

            ///Labor Category Rates
            ViewBag.LaborCategoryRatesAdd = IsAuthorizedForResource(ResourceType.LaborCategoryRates, ResourceActionPermission.Add);
            ViewBag.LaborCategoryRatesEdit = IsAuthorizedForResource(ResourceType.LaborCategoryRates, ResourceActionPermission.Edit);
            ViewBag.LaborCategoryRatesDetail = IsAuthorizedForResource(ResourceType.LaborCategoryRates, ResourceActionPermission.Details);


            ///Contract Close
            ViewBag.ContractCloseAdd = IsAuthorizedForResource(ResourceType.ContractCloseOut, ResourceActionPermission.Add);
            ViewBag.ContractCloseDetail = IsAuthorizedForResource(ResourceType.ContractCloseOut, ResourceActionPermission.Details);

            ///Contract Notice
            ViewBag.ContractNoticeAdd = IsAuthorizedForResource(ResourceType.ContractNotice, ResourceActionPermission.Add);
            ViewBag.ContractNoticeEdit = IsAuthorizedForResource(ResourceType.ContractNotice, ResourceActionPermission.Edit);
            ViewBag.ContractNoticeList = IsAuthorizedForResource(ResourceType.ContractNotice, ResourceActionPermission.List);
            ViewBag.ContractNoticeDetail = IsAuthorizedForResource(ResourceType.ContractNotice, ResourceActionPermission.Details);

            ///Contract Clause
            ViewBag.ContractClauseAdd = IsAuthorizedForResource(ResourceType.ContractClauses, ResourceActionPermission.Add);
            ViewBag.ContractClauseEdit = IsAuthorizedForResource(ResourceType.ContractClauses, ResourceActionPermission.Edit);
            ViewBag.ContractClauseList = IsAuthorizedForResource(ResourceType.ContractClauses, ResourceActionPermission.List);
            ViewBag.ContractClauseDetail = IsAuthorizedForResource(ResourceType.ContractClauses, ResourceActionPermission.Details);

            ///Document Manager
            ViewBag.DocumentManagerManage = IsAuthorizedForResource(ResourceType.DocumentManager, ResourceActionPermission.Manage);
            ViewBag.ViewDocument = IsAuthorizedForResource(ResourceType.DocumentManager, ResourceActionPermission.View);
        }
        #endregion
    }
}
