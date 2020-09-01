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
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models.ViewModels.RevenueRecognition;
using static Northwind.Core.Entities.EnumGlobal;
using static Northwind.Core.Entities.GenericNotification;

namespace Northwind.Web.Controllers
{
    public class RevenueRecognitionController : Controller
    {
        private readonly IRevenueRecognitionService _revenueRecognitionService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IUrlHelper _urlHelper;
        private readonly ICommonService _commonService;
        private readonly IUserService _userService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IMapper _mapper;
        private readonly Logger _eventLogger;
        private readonly IContractsService _contractRefactorService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IConfiguration _configuration;
        private readonly Logger _logger;
        private readonly IGenericNotificationService _genericNotificationService;

        public RevenueRecognitionController(
            IRevenueRecognitionService revenueRecognitionService,
            ICommonService commonService,
            IMapper mapper,
            IContractsService contratRefactorService,
            INotificationTemplatesService notificationTemplatesService,
            INotificationMessageService notificationMessageService,
            IUrlHelper urlHelper,
            IUserService userService,
            IContractsService contractRefactorService,
            INotificationBatchService notificationBatchService,
            IContractModificationService contractModificationService,
            IConfiguration configuration,
            IGenericNotificationService genericNotificationService,
            IResourceAttributeValueService resourceAttributeValueService)
        {
            _revenueRecognitionService = revenueRecognitionService;
            _commonService = commonService;
            _contractRefactorService = contractRefactorService;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationMessageService = notificationMessageService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _notificationBatchService = notificationBatchService;
            _urlHelper = urlHelper;
            _userService = userService;
            _configuration = configuration;
            _genericNotificationService = genericNotificationService;
            _mapper = mapper;
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _contractRefactorService = contractRefactorService;
            _contractModificationService = contractModificationService;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Add)]
        public IActionResult Add(Guid id)
        {
            try
            {
                var viewModel = GetRevenueById(id);
                viewModel.CrudType = CrudType.Create;
                return PartialView(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(new { responseJSON = ModelState });
            }
        }

        [HttpPost]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Add)]
        public ActionResult SaveRevenueRecognition(RevenueRecognitionViewModel model)
        {
            try
            {
                var loggedUser = new Guid(HttpContext.User.Identity.Name);
                string userName = "";
                var loggedUserDetails = _userService.GetUserByUserGuid(loggedUser);
                if (loggedUserDetails != null)
                {
                    userName = loggedUserDetails.DisplayName;
                }
                DateTime currentdatetime = CurrentDateTimeHelper.GetCurrentDateTime();
                string date = currentdatetime.ToString("MM/dd/yyyy");
                var recognitionEntity = _mapper.Map<RevenueRecognition>(model);
                var contractExtensionEntityList = _mapper.Map<List<RevenueContractExtension>>(model.ListContractExtension);
                var obligationEntityList = _mapper.Map<List<RevenuePerformanceObligation>>(model.ListRevenuePerformanceObligation);
                switch (model.CrudType)
                {
                    case CrudType.Create:
                        recognitionEntity.CreatedOn = currentdatetime;
                        recognitionEntity.CreatedBy = loggedUser;
                        recognitionEntity.UpdatedOn = currentdatetime;
                        recognitionEntity.UpdatedBy = loggedUser;
                        recognitionEntity.IsActive = true;
                        recognitionEntity.IsDeleted = false;
                        model.CrudType = CrudType.Edit;
                        _revenueRecognitionService.UpdateRevenueRecognition(recognitionEntity);

                        //audit log..
                        var additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Added.ToString(), "Revenue Recognition");
                        var additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + recognitionEntity.ContractGuid);
                        var resource = string.Format("{0} </br> GUID:{1}", "Revenue Recognition", recognitionEntity.RevenueRecognizationGuid);
                        AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), recognitionEntity, resource, recognitionEntity.RevenueRecognizationGuid, UserHelper.GetHostedIp(HttpContext), "Revenue Recognition Added", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                        //end of log..

                        return Ok(new { model.CrudType, updatedby = userName, updatedon = date, revenueGuid = recognitionEntity.RevenueRecognizationGuid, isnotify = false, CurrentStage = model.CurrentStage, contractGuid = model.ContractGuid });
                    case CrudType.Edit:
                        var recognitionEntitydata = _revenueRecognitionService.GetDetailsById(model.RevenueRecognizationGuid);
                        recognitionEntity.CreatedBy = recognitionEntitydata.CreatedBy;
                        recognitionEntity.CreatedOn = recognitionEntitydata.CreatedOn;
                        recognitionEntity.UpdatedBy = loggedUser;
                        recognitionEntity.UpdatedOn = currentdatetime;
                        if (!CheckAuthorization(recognitionEntity.ContractGuid, model.IsAccountRepresentive))
                        {
                            throw new Exception("Not an authorized user!!");
                        }
                        switch (model.CurrentStage)
                        {
                            case "#tab_5":
                                recognitionEntity.IsNotify = true;
                                recognitionEntity.IsCompleted = true;
                                _contractRefactorService.InsertRevenueRecognitionGuid(recognitionEntity.RevenueRecognizationGuid, recognitionEntity.ContractGuid);
                                _contractModificationService.InsertRevenueRecognitionGuid(recognitionEntity.RevenueRecognizationGuid, recognitionEntity.ContractGuid);
                                break;
                            case "#tab_4":
                                var notificationbatch = _notificationBatchService.GetByResourceId(recognitionEntity.RevenueRecognizationGuid);
                                if (notificationbatch == null)
                                {
                                    SaveAndNotifyAccountingRepresentative(recognitionEntity);
                                }
                                recognitionEntity.IsNotify = true;
                                _revenueRecognitionService.UpdateIsNotify(recognitionEntity.RevenueRecognizationGuid);
                                break;
                        }
                        _revenueRecognitionService.UpdateRevenueRecognition(recognitionEntity);

                        //audit log..
                        if (!string.IsNullOrEmpty(model.CurrentStage))
                        {
                            if (model.CurrentStage.Equals("#tab_5") || model.CurrentStage.Equals("#tab_4"))
                            {
                                additionalInformation = string.Format("{0} {1} the {2}", User.FindFirst("fullName").Value, CrudTypeForAdditionalLogMessage.Edited.ToString(), "Revenue Recognition");
                                additionalInformationURl = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + recognitionEntity.ContractGuid);
                                resource = string.Format("{0} </br> GUID:{1}", "Revenue Recognition", recognitionEntity.RevenueRecognizationGuid);
                                AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), recognitionEntity, resource, recognitionEntity.RevenueRecognizationGuid, UserHelper.GetHostedIp(HttpContext), "Revenue Recognition Edited", Guid.Empty, "Successful", "", additionalInformationURl, additionalInformationURl);
                            }
                        }
                        //end of log..

                        if (recognitionEntity.IsContractTermExpansion)
                        {
                            SaveContractExtension(contractExtensionEntityList, recognitionEntity.RevenueRecognizationGuid);
                        }
                        SaveObligationEntity(obligationEntityList, recognitionEntity.RevenueRecognizationGuid);
                        return Ok(new { model.CrudType, revenueGuid = model.RevenueRecognizationGuid, updatedby = userName, updatedon = date, isnotify = true, CurrentStage = model.CurrentStage, contractGuid = model.ContractGuid });
                }
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Added !!", model.CrudType });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Edit)]
        public IActionResult Edit(Guid id)
        {
            try
            {
                var viewModel = GetRevenueById(id);
                viewModel.CrudType = CrudType.Edit;
                if (viewModel.IsNotify)
                    viewModel.IsAccountRepresentive = true;
                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.List)]
        public IActionResult Detail(Guid id)
        {
            try
            {
                var model = _revenueRecognitionService.GetInfoForDetailPage(id);
                var basicContract = _contractRefactorService.GetBasicContractById(model.ContractGuid);
                var keyPersonnel = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid);
                var viewModel = _mapper.Map<RevenueRecognitionViewModel>(model);
                if (viewModel != null)
                {
                    if (viewModel.IsFile)
                    {
                        var user = _userService.GetUserByUserGuid(viewModel.UpdatedBy);
                        viewModel.UpdatedByName = user.DisplayName;
                        var fileDetails = _contractRefactorService.GetFileByResourceGuid(viewModel.RevenueRecognizationGuid, EnumGlobal.ResourceType.RevenueRecognition.ToString());
                        if (fileDetails != null)
                        {
                            viewModel.ContractResourceFileGuid = fileDetails.ContractResourceFileGuid;

                        }
                        return PartialView(viewModel);
                    }
                }
                var extensionEntityList = _revenueRecognitionService.GetContractExtension(viewModel.RevenueRecognizationGuid);
                var contractExtensionMapping = _mapper.Map<List<RevenueContractExtensionViewModel>>(extensionEntityList);
                viewModel.ListContractExtension = contractExtensionMapping;
                var obligationEntityList = _revenueRecognitionService.GetPerformanceObligation(viewModel.RevenueRecognizationGuid);
                var obligationModelMapping = _mapper.Map<List<RevenuePerformanceObligationViewModel>>(obligationEntityList);
                viewModel.ListRevenuePerformanceObligation = obligationModelMapping;
                if (!string.IsNullOrEmpty(viewModel.IdentityContract))
                {
                    viewModel.IdentityContract = _resourceAttributeValueService.GetResourceAttributeValueByValue(viewModel.IdentityContract).Name;
                }
                if (!string.IsNullOrEmpty(viewModel.ContractType))
                {
                    viewModel.ContractType = _resourceAttributeValueService.GetResourceAttributeValueByValue(viewModel.ContractType).Name;
                }
                var exportDetail = GetContractDetailForExport(model.ContractGuid);

                viewModel.BasicContractInfoModel = exportDetail.BasicContractInfoModel;
                viewModel.AccountingRepresentativeName = exportDetail.AccountingRepresentativeName;
                viewModel.ProjectManagerName = exportDetail.ProjectManagerName;

                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpGet]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.List)]
        public IActionResult DetailList(Guid id)
        {
            ViewBag.ParentGuid = id;
            return PartialView();
        }

        [HttpPost]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Delete)]
        public IActionResult Delete([FromBody] Guid ids)
        {
            try
            {
                var model = _revenueRecognitionService.GetDetailsById(ids);
                _revenueRecognitionService.DeleteRevenueRecognition(ids);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!", model });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Delete)]
        public IActionResult DeleteRevenueExtensionPeriod([FromBody] Guid[] id)
        {
            try
            {
                _revenueRecognitionService.DisableRevenueExtensionPeriod(id);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!", id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.Delete)]
        public IActionResult DeleteRevenueObligation([FromBody] Guid[] id)
        {
            try
            {
                _revenueRecognitionService.DisableRevenueObligation(id);
                return Ok(new { status = ResponseStatus.success.ToString(), message = "Successfully Deleted !!" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        private RevenueRecognitionViewModel GetRevenueById(Guid id)
        {
            var recognitionEntity = _revenueRecognitionService.GetDetailsById(id);
            var viewModel = _mapper.Map<RevenueRecognitionViewModel>(recognitionEntity);
            var contractdetails = _contractRefactorService.GetDetailById(recognitionEntity.ContractGuid);
            if (contractdetails.ParentContractGuid != null)
            {
                if (contractdetails.ParentContractGuid != Guid.Empty)
                {
                    viewModel.isTaskOrder = true;
                }
                else
                {
                    viewModel.isTaskOrder = false;
                }
            }
            else
            {
                viewModel.isTaskOrder = false;
            }
            if (!CheckAuthorization(recognitionEntity.ContractGuid, viewModel.IsNotify))
            {
                throw new Exception("Not an authorized user!!");
            }
            var revenueContractExtensionEntity = _revenueRecognitionService.GetContractExtension(id);
            var contractModelList = _mapper.Map<List<RevenueContractExtensionViewModel>>(revenueContractExtensionEntity);

            var revenuePerformanceObligationEntityList = _revenueRecognitionService.GetPerformanceObligation(id);
            var obligationModelList = _mapper.Map<List<RevenuePerformanceObligationViewModel>>(revenuePerformanceObligationEntityList);

            viewModel.ListRevenuePerformanceObligation = obligationModelList;
            viewModel.ListContractExtension = contractModelList;
            viewModel.IdentifyContract = _resourceAttributeValueService.GetDropDownByResourceType("RevenueRecognition", "IdentifyContract");
            viewModel.PriceArrangementtype = _resourceAttributeValueService.GetDropDownByResourceType("Contract", "ContractType");
            var isIDIQContract = _contractRefactorService.IsIDIQContract(id);
            viewModel.IsIDIQContract = isIDIQContract;
            return viewModel;
        }

        private int SaveContractExtension(List<RevenueContractExtension> contractExtensions, Guid RevenueRecognizationGuid)
        {
            var saveContractExtension = contractExtensions.Where(x => x.ExtensionDate.HasValue).ToList();
            if (saveContractExtension.Count() > 0)
            {
                var loggedUser = new Guid(HttpContext.User.Identity.Name);
                DateTime currentdatetime = CurrentDateTimeHelper.GetCurrentDateTime();
                saveContractExtension.ForEach(x =>
                {
                    x.RevenueGuid = RevenueRecognizationGuid;
                    x.ContractExtensionGuid = new Guid();
                    x.UpdatedOn = currentdatetime;
                    x.UpdatedBy = loggedUser;
                    x.IsDeleted = false;
                    x.IsActive = true;
                    x.UpdatedOn = currentdatetime;
                    x.CreatedBy = loggedUser;
                    x.CreatedOn = currentdatetime;
                });
                return _revenueRecognitionService.UpdateContractExtension(saveContractExtension);
            }
            return 1;
        }

        private int SaveObligationEntity(List<RevenuePerformanceObligation> obligationEntityList, Guid RevenueRecognizationGuid)
        {
            var saveObligationEntity = obligationEntityList.Where(x => x.RevenueStreamIdentifier != null).ToList();
            if (saveObligationEntity.Count() > 0)
            {
                var loggedUser = new Guid(HttpContext.User.Identity.Name);
                DateTime currentdatetime = CurrentDateTimeHelper.GetCurrentDateTime();
                saveObligationEntity.ForEach(x =>
                {
                    x.RevenueGuid = RevenueRecognizationGuid;
                    x.PerformanceObligationGuid = new Guid();
                    x.UpdatedBy = loggedUser;
                    x.IsDeleted = false;
                    x.IsActive = true;
                    x.UpdatedOn = currentdatetime;
                    x.CreatedBy = loggedUser;
                    x.CreatedOn = currentdatetime;
                    if (x.RevenueOverTimePointInTime == null)
                        x.RevenueOverTimePointInTime = "";
                    if (x.SatisfiedOverTime == null)
                        x.SatisfiedOverTime = "";
                });
                return _revenueRecognitionService.UpdatePerformanceObligation(saveObligationEntity);
            }
            return 1;
        }

        private bool CheckAuthorization(Guid contractGuid, bool isAccountingRepresentative)
        {
            if (isAccountingRepresentative)
            {
                Guid accountingRepresentative = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid)
                                     .FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative).UserGuid;
                if (!UserHelper.IsAuthorizedRepresentative(HttpContext, accountingRepresentative))
                {
                    return false;
                }
            }
            else
            {
                Guid contractRepresentative = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid)
                                         .FirstOrDefault(x => x.UserRole == ContractUserRole._contractRepresentative).UserGuid;
                if (!UserHelper.IsAuthorizedRepresentative(HttpContext, contractRepresentative))
                {
                    return false;
                }
            }
            return true;
        }

        private bool SaveAndNotifyAccountingRepresentative(RevenueRecognition model)
        {
            try
            {
                var userToNotifydetail = _contractRefactorService.GetKeyPersonnelByContractGuid(model.ContractGuid)
                                           .FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative);
                if (userToNotifydetail != null)
                {
                    Guid userToNotify = userToNotifydetail.UserGuid;
                    var contractType = _contractRefactorService.GetContractType(model.ContractGuid);
                    var key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                               EnumGlobal.CrudType.Notify.ToString());
                    var updatedByName = _userService.GetUserByUserGuid(model.UpdatedBy);
                    var receiverUserName = _userService.GetUserByUserGuid(userToNotify).DisplayName;
                    return AddNotificationMessage(model);
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

        private bool AddNotificationMessage(RevenueRecognition model)
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
                string contractNumber = string.Empty;
                string taskorderNumber = string.Empty;

                var contractdetails = _contractRefactorService.GetOnlyRequiredContractData(model.ContractGuid);
                notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/contract/Details/" + model.ContractGuid);
                if (contractdetails != null)
                {
                    if (contractdetails.ParentContractGuid != null)
                    {
                        if (contractdetails.ParentContractGuid != Guid.Empty)
                        {
                            key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                EnumGlobal.ResourceAction.TaskOrderReview.ToString());
                            var parentcontractEntity = _contractRefactorService.GetOnlyRequiredContractData(contractdetails.ParentContractGuid);
                            contractNumber = parentcontractEntity.ContractNumber;
                            taskorderNumber = contractdetails.ContractNumber;
                            notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/project/Details/" + model.ContractGuid);

                        }
                        else
                        {
                            key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                 EnumGlobal.ResourceAction.ContractReview.ToString());
                        }
                    }
                    else
                    {
                        key = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.RevenueRecognition.ToString(),
                                 EnumGlobal.ResourceAction.ContractReview.ToString());
                    }

                    notificationModel.ResourceId = model.ContractGuid;
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
                            thresholdAmount = RevenueRecognitionHelper.GetAmountByContractType(_configuration, model.ContractType);

                            receiverInfo = _userService.GetUserByUserGuid(receiverGuid ?? Guid.Empty);

                            var resourcevalue = _resourceAttributeValueService.GetResourceAttributeValueByValue(model.ContractType);
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

                            notificationTemplatesDetails.ContractNumber = contractNumber;
                            notificationTemplatesDetails.TaskOrderNumber = taskorderNumber;
                            notificationTemplatesDetails.Title = contractdetails.ContractTitle;
                            notificationTemplatesDetails.ContractType = contracttype;
                            notificationTemplatesDetails.ContractTitle = contractdetails.ContractTitle;
                            notificationTemplatesDetails.ProjectNumber = contractdetails.ProjectNumber;
                            notificationTemplatesDetails.AdditionalUser = additionalUser.ToString();
                            notificationTemplatesDetails.ThresholdAmount = thresholdAmount;
                            notificationTemplatesDetails.Status = "";
                            notificationModel.NotificationTemplatesDetail = notificationTemplatesDetails;
                            _genericNotificationService.AddNotificationMessage(notificationModel);
                            return true;
                        }
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


        [HttpGet]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.List)]
        public IActionResult GetDetailList(Guid id, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            try
            {
                var model = _revenueRecognitionService.DetailList(id, searchValue, pageSize, skip, take, sortField, dir);
                var count = _revenueRecognitionService.DetailListCount(id, searchValue);

                List<RevenueRecognitionViewModel> result = new List<RevenueRecognitionViewModel>();
                foreach (var item in model)
                {
                    var mapping = _mapper.Map<RevenueRecognitionViewModel>(item);

                    var extensionEntityList = _revenueRecognitionService.GetContractExtension(mapping.RevenueRecognizationGuid);

                    var extensionModelList = _mapper.Map<List<RevenueContractExtensionViewModel>>(extensionEntityList);

                    var obligationEntityList = _revenueRecognitionService.GetPerformanceObligation(mapping.RevenueRecognizationGuid);

                    var obligationModelList = _mapper.Map<List<RevenuePerformanceObligationViewModel>>(obligationEntityList);

                    mapping.ListRevenuePerformanceObligation = obligationModelList;

                    mapping.ListContractExtension = extensionModelList;

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

        [HttpPost]
        [Secure(ResourceType.RevenueRecognition, ResourceActionPermission.List)]
        public IActionResult ExportDetailList([FromBody]Guid[] id)
        {
            try
            {
                List<RevenueRecognitionViewModel> result = new List<RevenueRecognitionViewModel>();
                foreach (var revenueGuid in id)
                {
                    var viewModel = GetDetails(revenueGuid);
                    result.Add(viewModel);
                }
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        private RevenueRecognitionViewModel GetDetails(Guid id)
        {
            var model = _revenueRecognitionService.GetInfoForDetailPage(id);
            var result = _mapper.Map<RevenueRecognitionViewModel>(model);
            if (model != null)
            {
                if (model.IsFile)
                {
                    var user = _userService.GetUserByUserGuid(model.UpdatedBy);
                    result.UpdatedByName = user.DisplayName;
                    var fileDetails = _contractRefactorService.GetFileByResourceGuid(model.RevenueRecognizationGuid, EnumGlobal.ResourceType.RevenueRecognition.ToString());
                    if (fileDetails != null)
                    {
                        var fileName = fileDetails.FilePath.Split("/").Last();
                        if (fileName != null)
                        {
                            result.FileName = fileName;
                        }
                        result.ContractResourceFileGuid = fileDetails.ContractResourceFileGuid;
                    }
                    return result;
                }
            }
            var extensionEntityList = _revenueRecognitionService.GetContractExtension(result.RevenueRecognizationGuid);
            var contractExtensionMapping = _mapper.Map<List<RevenueContractExtensionViewModel>>(extensionEntityList);
            result.ListContractExtension = contractExtensionMapping;
            var obligationEntityList = _revenueRecognitionService.GetPerformanceObligation(result.RevenueRecognizationGuid);
            var obligationModelMapping = _mapper.Map<List<RevenuePerformanceObligationViewModel>>(obligationEntityList);
            result.ListRevenuePerformanceObligation = obligationModelMapping;
            if (!string.IsNullOrEmpty(result.IdentityContract))
            {
                result.IdentityContract = _resourceAttributeValueService.GetResourceAttributeValueByValue(result.IdentityContract).Name;
            }
            if (!string.IsNullOrEmpty(result.ContractType))
            {
                result.ContractType = _resourceAttributeValueService.GetResourceAttributeValueByValue(result.ContractType).Name;
            }
            var exportDetail = GetContractDetailForExport(model.ContractGuid);

            result.BasicContractInfoModel = exportDetail.BasicContractInfoModel;
            result.AccountingRepresentativeName = exportDetail.AccountingRepresentativeName;
            result.ProjectManagerName = exportDetail.ProjectManagerName;

            return result;
        }

        private RevenueRecognitionViewModel GetContractDetailForExport(Guid id)
        {
            var result = new RevenueRecognitionViewModel();
            var basicContract = _contractRefactorService.GetBasicContractById(id);
            var keyPersonnel = _contractRefactorService.GetKeyPersonnelByContractGuid(id);

            if (basicContract != null)
            {
                var entityCode = new EntityCode();
                if (basicContract.ORGID != null && basicContract.ORGID != Guid.Empty)
                {
                    var orgName = _contractRefactorService.GetOrgNameById(basicContract.ORGID);
                    if (orgName != null)
                    {
                        if (orgName.Contains("."))
                        {
                            var splitedValue = orgName.Split(".");
                            entityCode.CompanyCode = splitedValue[0];
                            entityCode.OfficeCode = splitedValue[1];
                            entityCode.RegionCode = splitedValue[2];
                            var basicInfo = _contractRefactorService.GetCompanyRegionAndOfficeNameByCode(entityCode);
                            basicContract.CompanyName = basicInfo.CompanyName;
                        }
                    }
                }
                basicContract.CompanyName = string.IsNullOrEmpty(basicContract.CompanyName) ? "N/A" : basicContract.CompanyName;
                basicContract.ContractNumber = string.IsNullOrEmpty(basicContract.CompanyName) ? "N/A" : basicContract.ContractNumber;
                basicContract.ContractTitle = string.IsNullOrEmpty(basicContract.CompanyName) ? "N/A" : basicContract.ContractTitle;
                basicContract.ProjectNumber = string.IsNullOrEmpty(basicContract.CompanyName) ? "N/A" : basicContract.ProjectNumber;
                result.BasicContractInfoModel = basicContract;
            }
            else
            {
                var basicContractData = new BasicContractInfoModel();
                basicContractData.CompanyName = "N/A";
                basicContractData.ContractNumber = "N/A";
                basicContractData.ContractTitle = "N/A";
                basicContractData.ProjectNumber = "N/A";
                result.BasicContractInfoModel = basicContractData;
            }

            if (keyPersonnel != null)
            {
                var projectManager = keyPersonnel.FirstOrDefault(x => x.UserRole == ContractUserRole._projectManager);
                var accountingManager = keyPersonnel.FirstOrDefault(x => x.UserRole == ContractUserRole._accountRepresentative);

                if (projectManager != null)
                {
                    var user = _userService.GetUserByUserGuid(projectManager.UserGuid);
                    result.ProjectManagerName = user.DisplayName;
                }
                else
                {
                    result.ProjectManagerName = "N/A";
                }
                if (accountingManager != null)
                {
                    var user = _userService.GetUserByUserGuid(accountingManager.UserGuid);
                    result.AccountingRepresentativeName = user.DisplayName;
                }
                else
                {
                    result.AccountingRepresentativeName = "N/A";
                }
            }
            else
            {
                result.ProjectManagerName = "N/A";
                result.AccountingRepresentativeName = "N/A";
            }
            return result;
        }

    }
}