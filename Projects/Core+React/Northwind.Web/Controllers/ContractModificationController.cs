using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.Contract;
using static Northwind.Core.Entities.EnumGlobal;
using static Northwind.Core.Entities.GenericNotification;

namespace Northwind.Web.Controllers
{
    public class ContractModificationController : Controller
    {
        private readonly IContractModificationService _contractModificationService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IUserService _userService;
        private readonly IUrlHelper _urlHelper;
        private readonly Logger _eventLogger;
        private readonly IContractsService _contractsService;
        private readonly IResourceAttributeService _resourceAttributeService;
        private readonly IRevenueRecognitionService _revenueRecognitionService;
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IContractsService _contractRefactorService;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IContractResourceFileService _contractResourceFileService;
        private readonly IGenericNotificationService _genericNotificationService;

        public ContractModificationController(
                                  IContractModificationService contractModificationService,
                                  IUrlHelper urlHelper,
                                  INotificationTemplatesService notificationTemplatesService,
                                  INotificationBatchService notificationBatchService,
                                  INotificationMessageService notificationMessageService,
                                  IContractsService contractsService,
                                  IContractsService contractRefactorService,
                                  ICommonService commonService,
                                  IResourceAttributeService resourceAttributeService,
                                  IResourceAttributeValueService resourceAttributeValueService,
                                  IConfiguration configuration,
                                  IUserService userService,
                                  IRevenueRecognitionService revenueRecognitionService,
                                  IMapper mapper,
                                  IContractResourceFileService contractResourceFileService,
                                  IGenericNotificationService genericNotificationService,
                                  IFileService fileService)
        {
            _contractModificationService = contractModificationService;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationBatchService = notificationBatchService;
            _notificationMessageService = notificationMessageService;
            _urlHelper = urlHelper;
            _resourceAttributeService = resourceAttributeService;
            _contractsService = contractsService;
            _configuration = configuration;
            _commonService = commonService;
            _userService = userService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _revenueRecognitionService = revenueRecognitionService;
            _contractRefactorService = contractRefactorService;
            _fileService = fileService;
            _userService = userService;
            _mapper = mapper;
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _configuration = configuration;
            _logger = LogManager.GetCurrentClassLogger();
            _genericNotificationService = genericNotificationService;
            _contractResourceFileService = contractResourceFileService;
        }
        // GET: Contract Modification
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.List)]
        public ActionResult Index(Guid ContractGuid, string SearchValue)
        {
            var ContractModificationModel = new ContractModificationViewModel();
            ContractModificationModel.SearchValue = SearchValue;
            ContractModificationModel.ContractGuid = ContractGuid;
            return View(ContractModificationModel);
        }

        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.List)]
        public IActionResult Get(Guid ContractGuid, string SearchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            try
            {
                var contractEntity = _contractModificationService.GetAll(ContractGuid, false, SearchValue, pageSize, skip, sortField, sortDirection);
                var contractModel = _mapper.Map<IEnumerable<ContractModificationViewModel>>(contractEntity);
                var totalRecordCount = _contractModificationService.TotalRecord(ContractGuid, false);

                var data = contractModel.Select(x => new
                {
                    ContractModificationGuid = x.ContractModificationGuid,
                    ContractNumber = x.ContractNumber,
                    ProjectNumber = x.ProjectNumber,
                    ModificationNumber = x.ModificationNumber,
                    ContractTitle = x.ContractTitle,
                    ModificationTitle = x.ModificationTitle,
                    EffectiveDate = x.EffectiveDate?.ToString("MM/dd/yyyy"),
                    EnteredDate = x.EnteredDate?.ToString("MM/dd/yyyy"),
                    PopStart = x.POPStart?.ToString("MM/dd/yyyy"),
                    PopEnd = x.POPEnd?.ToString("MM/dd/yyyy"),
                    AwardAmount = x.AwardAmount,
                    IsActiveStatus = x.IsActive == true ? EnumGlobal.ActiveStatus.Active : EnumGlobal.ActiveStatus.Inactive,
                    UpdatedOn = x.UpdatedOn.ToString("MM/dd/yyyy")
                }).ToList();

                return Json(new { total = totalRecordCount, data = data });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region  Crud

        [HttpGet]
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Add)]
        public ActionResult Add(Guid contractGuid)
        {
            var contractModificationModel = new ContractModificationViewModel();
            try
            {

                var contractInfo = _contractsService.GetOnlyRequiredContractData(contractGuid);
                contractModificationModel.ContractGuid = contractGuid;
                contractModificationModel.ContractTitle = contractInfo.ContractTitle;
                contractModificationModel.ProjectNumber = contractInfo.ProjectNumber;
                contractModificationModel.ContractNumber = contractInfo.ContractNumber;
                contractModificationModel.keyValuePairs = new Dictionary<string, string>();
                var contractModification = _resourceAttributeService.GetByResource(ResourceType.ContractModification.ToString());
                if (contractModification != null)
                {
                    var modificationType = contractModification.FirstOrDefault(x => x.Name.ToUpper() == "MODIFICATIONTYPE");
                    if (modificationType != null)
                    {
                        var resourcevalue = _resourceAttributeValueService.GetResourceAttributeOptionsByResourceAttributeGuid(modificationType.ResourceAttributeGuid);
                        if (resourcevalue != null)
                        {
                            contractModificationModel.keyValuePairs = resourcevalue.ToDictionary(x => x.Value, x => x.Name);
                        }
                    }
                }
                ViewBag.Resourcekey = EnumGlobal.ResourceType.ContractModification.ToString();
                return PartialView(contractModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractModificationModel);
            }
        }

        [HttpPost]
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Add)]
        public IActionResult Add([FromBody]ContractModificationViewModel contractModificationModel)
        {
            try
            {
                List<string> filePath = new List<string>();

                string validation = validateModel(contractModificationModel);
                if (validation != YesNoStatus.Yes.ToString())
                {
                    ModelState.AddModelError("", validation);
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    Guid id = Guid.NewGuid();
                    contractModificationModel.ContractModificationGuid = id;
                    contractModificationModel.CreatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModificationModel.CreatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractModificationModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModificationModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);
                    contractModificationModel.IsActive = true;
                    contractModificationModel.IsDeleted = false;
                    contractModificationModel.AwardAmount = contractModificationModel.AwardAmount ?? 0;


                    var contractModificationEntity = _mapper.Map<ContractModification>(contractModificationModel);
                    _contractModificationService.Add(contractModificationEntity);


                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), "Contract Mod");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModificationEntity.ContractGuid);
                    var resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModificationEntity, resource, contractModificationEntity.ContractModificationGuid, UserHelper.GetHostedIp(HttpContext), "Contract Mod Added", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                    //end of log..

                    //for Revenue Recognition
                    bool istriggered = false;

                    var revenueGuid = SaveAndNotifyRevenueRepresentative(contractModificationEntity);
                    if (revenueGuid != Guid.Empty)
                    {
                        istriggered = true;
                    }

                    bool isViewHistory = false;

                    var historyCount = _revenueRecognitionService.DetailListCount(contractModificationEntity.ContractGuid, "");
                    if (historyCount > 0)
                    {
                        isViewHistory = true;
                    }

                    //get file info..
                    var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(ContractResourceFileKey.ContractModification.ToString(), contractModificationModel.ContractGuid);

                    var jsonObject = new
                    {
                        status = ResponseStatus.success.ToString(),
                        message = "Successfully Added !!",
                        revenueGuid = revenueGuid,
                        viewHistory = isViewHistory,
                        istriggered = istriggered,
                        contractGuid = contractModificationEntity.ContractGuid,
                        resourceId = contractModificationEntity.ContractModificationGuid,
                        uploadPath = contractResourceFile.FilePath,
                        parentId = contractResourceFile.ContractResourceFileGuid
                    };

                    return Ok(jsonObject);
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
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Edit)]
        public ActionResult Edit(Guid id)
        {
            var contractModificationModel = new ContractModificationViewModel();
            if (id != Guid.Empty)
            {
                var contractModificationEntity = _contractModificationService.GetDetailById(id);
                contractModificationModel = _mapper.Map<ContractModificationViewModel>(contractModificationEntity);

                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.Displayname;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
                ViewBag.ResourceId = contractModificationModel.ContractModificationGuid;
                ViewBag.Resourcekey = EnumGlobal.ResourceType.ContractModification.ToString();

                contractModificationModel.keyValuePairs = new Dictionary<string, string>();
                var contractModification = _resourceAttributeService.GetByResource(ResourceType.ContractModification.ToString());
                if (contractModification != null)
                {
                    var modificationType = contractModification.FirstOrDefault(x => x.Name.ToUpper() == "MODIFICATIONTYPE");
                    if (modificationType != null)
                    {
                        var resourcevalue = _resourceAttributeValueService.GetResourceAttributeOptionsByResourceAttributeGuid(modificationType.ResourceAttributeGuid);
                        if (resourcevalue != null)
                        {
                            contractModificationModel.keyValuePairs = resourcevalue.ToDictionary(x => x.Value, x => x.Name);
                        }
                    }
                }

            }
            try
            {
                return PartialView(contractModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractModificationModel);
            }
        }


        [HttpPost]
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Edit)]
        public IActionResult Edit([FromBody]ContractModificationViewModel contractModificationModel)
        {
            try
            {
                List<string> filePath = new List<string>();
                bool istriggered = false;
                bool isViewHistory = false;

                Guid revenueGuid = Guid.Empty;
                //check for duplicate modification number..
                //check for duplicate modification number..
                string validation = validateModel(contractModificationModel);
                if (validation != YesNoStatus.Yes.ToString())
                {
                    ModelState.AddModelError("", validation);
                    return BadRequest(ModelState);
                }
                if (ModelState.IsValid)
                {
                    Guid id = Guid.NewGuid();
                    contractModificationModel.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
                    contractModificationModel.UpdatedBy = UserHelper.CurrentUserGuid(HttpContext);

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    //var uploadPath = string.Format(
                    //          $@"{contractModificationModel.ContractNumber}\ContractModification\{contractModificationModel.ModificationNumber}-{contractModificationModel.ContractTitle}");


                    var contractModificationEntity = _mapper.Map<ContractModification>(contractModificationModel);


                    // for revenue and revenue notification
                    decimal? totalPreviousAwardAmount = 0.00M;
                    decimal? actualAmount = 0.00M;
                    decimal? currentAmount = 0.00M;
                    var sumofAwardAmount = _contractModificationService.GetTotalAwardAmount(contractModificationModel.ContractGuid);
                    if (sumofAwardAmount != null)
                    {
                        totalPreviousAwardAmount = (sumofAwardAmount.AwardAmount == null ? 0.00M : sumofAwardAmount.AwardAmount);
                    }
                    var previousAwardAmount = _contractModificationService.getAwardAndFundingAmountbyId(contractModificationModel.ContractModificationGuid);

                    if (previousAwardAmount.RevenueRecognitionGuid != Guid.Empty && previousAwardAmount.RevenueRecognitionGuid != null)
                    {
                        actualAmount = contractModificationModel.AwardAmount - previousAwardAmount.AwardAmount;
                    }
                    else
                    {
                        actualAmount = contractModificationModel.AwardAmount;
                    }
                    if (actualAmount < 0)
                    {
                        actualAmount = actualAmount * (-1);
                    }
                    currentAmount = totalPreviousAwardAmount + actualAmount;


                    string contractType = _contractRefactorService.GetContractType(contractModificationModel.ContractGuid);

                    if (RevenueRecognitionHelper.IsValidForRevenueRecognitionRequest(_configuration, contractType, currentAmount, 0.00M))
                    {
                        _contractModificationService.UpdateRevenueRecognitionGuid(contractModificationEntity.ContractModificationGuid, contractModificationEntity.AwardAmount, contractModificationEntity.FundingAmount);
                        contractModificationEntity.IsUpdated = true;
                        revenueGuid = AddNewRevenueAndUpdateContractModRevenueGuid(contractModificationEntity);
                        istriggered = true;


                        var historyCount = _revenueRecognitionService.DetailListCount(contractModificationEntity.ContractGuid, "");
                        if (historyCount > 0)
                        {
                            isViewHistory = true;
                        }
                    }


                    _contractModificationService.Edit(contractModificationEntity);

                    //audit log..
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), "Contract Mod");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModificationEntity.ContractGuid);
                    var resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModificationEntity, resource, contractModificationEntity.ContractModificationGuid, UserHelper.GetHostedIp(HttpContext), "Contract Mod Edited", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                    //end of log..

                    //get file info..
                    var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(ContractResourceFileKey.ContractModification.ToString(), contractModificationModel.ContractGuid);


                    var jsonObject = new
                    {
                        status = ResponseStatus.success.ToString(),
                        message = "Successfully Updated !!",
                        revenueGuid = revenueGuid,
                        viewHistory = isViewHistory,
                        istriggered = istriggered,
                        contractGuid = contractModificationEntity.ContractGuid,
                        resourceId = contractModificationEntity.ContractModificationGuid,
                        uploadPath = contractResourceFile.FilePath,
                        parentId = contractResourceFile.ContractResourceFileGuid
                    };

                    return Ok(jsonObject);
                }
                else
                {
                    return View(contractModificationModel);
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(_configuration.GetSection("ExceptionErrorMessage").Value);
            }
        }

        [HttpGet]
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Details)]
        public ActionResult Details(Guid id)
        {
            var contractModificationModel = new ContractModificationViewModel();
            try
            {
                var contractModificationEntity = _contractModificationService.GetDetailById(id);
                contractModificationModel = _mapper.Map<ContractModificationViewModel>(contractModificationEntity);

                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.Displayname;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");
                ViewBag.ResourceId = contractModificationModel.ContractModificationGuid;
                ViewBag.Resourcekey = EnumGlobal.ResourceType.ContractModification.ToString();
                //ViewBag.FilePath = string.Format(
                //               $@"{contractModificationModel.ContractNumber}\ContractModification\{contractModificationModel.ModificationNumber}-{contractModificationModel.ContractTitle}");
                //get file info..
                var contractResourceFile = _contractResourceFileService.GetFilePathByResourceIdAndKeys(ContractResourceFileKey.ChangeProposals.ToString(), contractModificationModel.ContractGuid);
                ViewBag.FilePath = contractResourceFile.FilePath;

                var resourceAttributeValues = _resourceAttributeValueService.GetResourceAttributeValueByValue(contractModificationModel.ModificationType);
                if (resourceAttributeValues != null)
                {
                    contractModificationModel.ModificationType = resourceAttributeValues.Name;
                }
                return PartialView(contractModificationModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(contractModificationModel);
            }
        }
        [HttpPost]
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Delete)]
        public IActionResult Delete([FromBody] Guid[] ids)
        {
            try
            {
                _contractModificationService.Delete(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModificationEntity = _contractModificationService.GetDetailById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Deleted.ToString(), "Contract Mod");
                    var resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModificationEntity, resource, contractModificationEntity.ContractModificationGuid, UserHelper.GetHostedIp(HttpContext), "Contract Mod Deleted", Guid.Empty, "Successful", "", additionalInformation, "");
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
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Edit)]
        public IActionResult Disable([FromBody] Guid[] ids)
        {
            try
            {
                _contractModificationService.Disable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModificationEntity = _contractModificationService.GetDetailById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Disabled.ToString(), "Contract Mod");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModificationEntity.ContractGuid);
                    var resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModificationEntity, resource, contractModificationEntity.ContractModificationGuid, UserHelper.GetHostedIp(HttpContext), "Contract Mod Disabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
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
        [Secure(EnumGlobal.ResourceType.Contract, ResourceActionPermission.Edit)]
        public IActionResult Enable([FromBody] Guid[] ids)
        {
            try
            {
                _contractModificationService.Enable(ids);

                //audit log..
                foreach (var id in ids)
                {
                    var contractModificationEntity = _contractModificationService.GetDetailById(id);
                    var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Enabled.ToString(), "Contract Mod");
                    var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractModificationEntity.ContractGuid);
                    var resource = string.Format("{0} </br> Mod No: {1} </br> Mod Title:{2}", "Contract Mod", contractModificationEntity.ModificationNumber, contractModificationEntity.ModificationTitle);
                    AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), contractModificationEntity, resource, contractModificationEntity.ContractModificationGuid, UserHelper.GetHostedIp(HttpContext), "Contract Mod Enabled", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
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
        public IActionResult DownloadDocument(string filePath, string fileName)
        {
            try
            {
                return _fileService.GetFile(fileName, filePath);
            }
            catch (Exception ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        private Guid SaveAndNotifyRevenueRepresentative(ContractModification model)
        {
            try
            {
                var revenuedata = _revenueRecognitionService.GetAwardAmountDetail(model.ContractGuid);
                if (revenuedata != null)
                {
                    bool isRevenueTriggered = RevenueRecognitionHelper.IsValidForRevenueRecognitionRequest(_configuration, revenuedata.ContractType, revenuedata.AwardAmount, revenuedata.FundingAmount);
                    if (isRevenueTriggered)
                    {
                        Guid revenueRecognitionGuid = AddNewRevenueAndUpdateContractModRevenueGuid(model);
                        return revenueRecognitionGuid;
                    }
                }
                return Guid.Empty;
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
                return Guid.Empty;
            }
        }

        private Guid AddNewRevenueAndUpdateContractModRevenueGuid(ContractModification model)
        {
            Guid revenueRecognitionGuid = Guid.NewGuid();
            bool isSaved = _revenueRecognitionService.AddRevenueWithResourceGuid(
               new RevenueRecognition
               {
                   RevenueRecognizationGuid = revenueRecognitionGuid,
                   ResourceGuid = model.ContractModificationGuid,
                   ContractGuid = model.ContractGuid,
                   UpdatedBy = model.UpdatedBy,
                   UpdatedOn = model.UpdatedOn,
                   CreatedBy = model.UpdatedBy,
                   CreatedOn = model.UpdatedOn
               });
            if (isSaved)
            {
                _contractRefactorService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, model.ContractGuid);
                _contractModificationService.InsertRevenueRecognitionGuid(revenueRecognitionGuid, model.ContractGuid);
                AddNotificationMessage(model);
                return revenueRecognitionGuid;
            }
            return Guid.Empty;
        }

        private bool AddNotificationMessage(ContractModification model)
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
                if (model.IsUpdated)
                {
                    key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                            EnumGlobal.ResourceAction.ContractModUpdate.ToString());
                }
                else
                {
                    key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                               EnumGlobal.ResourceAction.ContractModCreate.ToString());
                }

                notificationModel.ResourceId = model.ContractGuid;
                notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/contract/Details/" + model.ContractGuid);
                notificationModel.NotificationTemplateKey = key;
                notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                notificationModel.SendEmail = true;


                var keyPersonnels = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid);

                if (keyPersonnels?.Any() == true)
                {
                    receiverGuid = keyPersonnels.FirstOrDefault(x => x.UserRole == ContractUserRole._contractRepresentative)?.UserGuid;
                    if (receiverGuid != Guid.Empty)
                    {
                        string contractTypeValue = _contractRefactorService.GetContractType(model.ContractGuid);

                        thresholdAmount = RevenueRecognitionHelper.GetAmountByContractType(_configuration, contractTypeValue);

                        receiverInfo = _userService.GetUserByUserGuid(receiverGuid ?? Guid.Empty);

                        var resourcevalue = _resourceAttributeValueService.GetResourceAttributeValueByValue(contractTypeValue);
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

                        notificationTemplatesDetails.ContractNumber = model.ContractNumber;
                        notificationTemplatesDetails.Title = model.ContractTitle;
                        notificationTemplatesDetails.ContractType = contracttype;
                        notificationTemplatesDetails.ContractTitle = model.ContractTitle;
                        notificationTemplatesDetails.ProjectNumber = model.ProjectNumber;
                        notificationTemplatesDetails.AdditionalUser = additionalUser.ToString();
                        notificationTemplatesDetails.ModNumber = model.ModificationNumber;
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

        private string validateModel(ContractModificationViewModel contractModificationViewModel)
        {
            if (_contractModificationService.IsExistModificationNumber(contractModificationViewModel.ContractGuid, contractModificationViewModel.ContractModificationGuid, contractModificationViewModel.ModificationNumber))
            {
                string errormessage = "Found Duplicate Modification Number !!";
                return errormessage;
            }
            if (_contractModificationService.IsExistModificationTitle(contractModificationViewModel.ContractGuid, contractModificationViewModel.ContractModificationGuid, contractModificationViewModel.ModificationTitle))
            {
                string errormessage = "Found Duplicate Modification Title !!";
                return errormessage;
            }
            return YesNoStatus.Yes.ToString();
        }
    }
}
