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
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Models.ViewModels.Questionaire;
using static Northwind.Core.Entities.EnumGlobal;
using static Northwind.Core.Entities.GenericNotification;
using EnumGlobal = Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    public class ContractCloseOutController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQuestionaireUserAnswerService _questionaireUserAnswerService;
        private readonly IContractsService _contractService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IQuestionaireMasterService _questionaireMasterService;
        private readonly IContractCloseApprovalService _contractCloseApprovalService;
        private readonly IQuestionaireManagerTypeService _questionaireManagerTypeService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IUserService _userService;
        private readonly IGenericNotificationService _genericNotificationService;
        private readonly IConfiguration _configuration;
        private readonly Logger _eventLogger;
        private readonly Logger _logger;

        public ContractCloseOutController(IQuestionaireUserAnswerService questionaireUserAnswerService,
                                    IQuestionaireMasterService questionaireMasterService,
                                    IQuestionaireManagerTypeService questionaireManagerTypeService,
                                    IContractCloseApprovalService contractCloseApprovalService,
                                    IResourceAttributeValueService resourceAttributeValueService,
                                    INotificationTemplatesService notificationTemplatesService,
                                    INotificationBatchService notificationBatchService,
                                    IContractsService contractService,
                                    IUserService userService,
                                    IGenericNotificationService genericNotificationService,
                                    INotificationMessageService notificationMessageService,
                                    IConfiguration configuration,
                                    IMapper mapper)
        {
            _questionaireUserAnswerService = questionaireUserAnswerService;
            _questionaireMasterService = questionaireMasterService;
            _configuration = configuration;
            _contractCloseApprovalService = contractCloseApprovalService;
            _questionaireManagerTypeService = questionaireManagerTypeService;
            _contractService = contractService;
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
            _notificationBatchService = notificationBatchService;
            _notificationTemplatesService = notificationTemplatesService;
            _userService = userService;
            _genericNotificationService = genericNotificationService;
            _notificationMessageService = notificationMessageService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _mapper = mapper;
        }

        [HttpGet]
        [Secure(ResourceType.ContractCloseOut, ResourceActionPermission.Add)]
        public IActionResult Add(Guid id)
        {
            try
            {
                var viewModel = Get(id);
                ViewBag.Resourcekey = ResourceType.ContractCloseOut.ToString();
                ViewBag.ProjectNumber = _contractService.GetProjectNumberById(id);

                Guid parentContractGuid = _contractService.GetParentContractGuidByContractGuid(id) ?? Guid.Empty;
                if (parentContractGuid != Guid.Empty)
                {
                    ViewBag.ParentProjectNumber = _contractService.GetProjectNumberById(parentContractGuid);
                    viewModel.ParentContractGuid = parentContractGuid;
                }
                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.DisplayName;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");

                return View(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        [HttpGet]
        [Secure(ResourceType.ContractCloseOut, ResourceActionPermission.Details)]
        public IActionResult Detail(Guid id)
        {
            try
            {
                var viewModel = Get(id);
                if (viewModel.RepresentativeType == string.Empty)
                    viewModel.RepresentativeType = _configuration.GetSection("ContractRepresentatives").GetValue<string>("AccountRepresentative");
                ViewBag.Resourcekey = ResourceType.ContractCloseOut.ToString();
                ViewBag.ProjectNumber = _contractService.GetProjectNumberById(id);

                Guid parentContractGuid = _contractService.GetParentContractGuidByContractGuid(id) ?? Guid.Empty;
                if (parentContractGuid != Guid.Empty)
                {
                    ViewBag.ParentProjectNumber = _contractService.GetProjectNumberById(parentContractGuid);
                    viewModel.ParentContractGuid = parentContractGuid;
                }
                var currentUser = _userService.GetUserByUserGuid(UserHelper.CurrentUserGuid(HttpContext));
                var users = Models.ObjectMapper<User, Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel>.Map(currentUser);
                ViewBag.UpdatedBy = users.DisplayName;
                ViewBag.UpdatedOn = CurrentDateTimeHelper.GetCurrentDateTime().ToString("MM/dd/yyyy");

                return View(viewModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }


        [HttpPost]
        [Secure(ResourceType.ContractCloseOut, ResourceActionPermission.Add)]
        public IActionResult Add(ContractCloseOutViewModel viewModel)
        {
            try
            {
                Guid resourceId = Post(viewModel);
                var contractNumber = _contractService.GetContractNumberById(viewModel.ContractGuid);
                var uploadPath = string.Format($@"{contractNumber}\ContractClose");
                var fileParentDetails = _contractService.GetFilesByContractGuid(viewModel.ContractGuid,
                    EnumGlobal.ResourceAction.Closeout.ToString());
                bool isTaskOrder = viewModel.ParentContractGuid != Guid.Empty ? true : false;
                var parentId = fileParentDetails != null ? fileParentDetails.ContractResourceFileGuid : Guid.Empty;
                var jsonObject = new
                {
                    status = true,
                    resourceId = resourceId,
                    uploadPath = uploadPath,
                    contractGuid = viewModel.ContractGuid,
                    representativeType = viewModel.RepresentativeType,
                    parentId = parentId,
                    taskOrder = isTaskOrder
                };
                return Json(jsonObject);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        #region Private

        private ContractCloseOutViewModel Get(Guid id)
        {
            var viewModel = new ContractCloseOutViewModel();
            var listModel = new List<QuestionaryAnswerViewModel>();

            var projectManager = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ProjectManager");
            var contractRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ContracRepresentative");
            var accountingRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("AccountRepresentative");

            Guid currentUser = UserHelper.CurrentUserGuid(HttpContext);
            var savedAnswers = _questionaireUserAnswerService.GetByContractAndUserGuid(id, currentUser);

            var representativeTypeName = GetRepresentativeType(id, currentUser, savedAnswers);
            var closeOut = EnumGlobal.ResourceType.ContractCloseOut.ToString().ToUpper();
            var questions = _questionaireMasterService.GetAll(closeOut).ToList();

            foreach (var item in questions)
            {
                var mappedModel = Models.ObjectMapper<QuestionaireMaster, QuestionaryAnswerViewModel>.Map(item);
                if (savedAnswers.Count() > 0)
                {
                    var answer = savedAnswers.FirstOrDefault(x => x.QuestionaireMasterGuid == mappedModel.QuestionaireMasterGuid);
                    if (answer != null)
                    {
                        mappedModel.Answer = answer.Answer;
                        mappedModel.RepresentativeType = answer.RepresentativeType;
                        mappedModel.Notes = answer.Notes;
                    }
                    else
                    {
                        var representative = _questionaireManagerTypeService.GetNameByGuid(item.QuestionaireManagerTypeGuid);
                        mappedModel.RepresentativeType = representative;
                    }
                }
                else
                {
                    var representative = _questionaireManagerTypeService.GetNameByGuid(item.QuestionaireManagerTypeGuid);
                    mappedModel.RepresentativeType = representative;
                }
                listModel.Add(mappedModel);
            }
            viewModel.ProjectManagerQuestions = listModel.Where(x => x.ParentMasterGuid == Guid.Empty && x.RepresentativeType.ToUpper() == projectManager).OrderByDescending(x => x.OrderNumber).ToList();
            viewModel.ContractRepresentativeQuestions = listModel.Where(x => x.ParentMasterGuid == Guid.Empty && x.RepresentativeType.ToUpper() == contractRepresentative).OrderByDescending(x => x.OrderNumber).ToList();
            viewModel.AccountingRepresentativeQuestions = listModel.Where(x => x.ParentMasterGuid == Guid.Empty && x.RepresentativeType.ToUpper() == accountingRepresentative).OrderByDescending(x => x.OrderNumber).ToList();
            viewModel.SubQuestions = listModel.Where(x => x.ParentMasterGuid != Guid.Empty).OrderByDescending(x => x.OrderNumber).ToList();
            viewModel.ContractGuid = id;
            viewModel.RepresentativeType = representativeTypeName;

            // For Files Attachments
            if (savedAnswers.Count() > 0)
            {
                var projectManagerData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == projectManager);
                var contractRepresentativeData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == contractRepresentative);
                var accountingRepresentativeData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == accountingRepresentative);

                if (projectManagerData != null)
                {
                    var files = _contractService.GetFileListByContractGuid(projectManagerData.QuestionaireUserAnswerGuid);
                    var fileDetails = new List<FileUploadModel>();
                    foreach (var item in files)
                    {
                        var file = new FileUploadModel();
                        file.UploadFileName = item.UploadFileName;
                        file.UploadedFileGuid = item.ContractResourceFileGuid;
                        file.Description = item.Description;
                        fileDetails.Add(file);
                    }
                    viewModel.ProjetcManagerFiles = fileDetails;
                }

                if (contractRepresentativeData != null)
                {
                    var files = _contractService.GetFileListByContractGuid(contractRepresentativeData.QuestionaireUserAnswerGuid);
                    var fileDetails = new List<FileUploadModel>();
                    foreach (var item in files)
                    {
                        var file = new FileUploadModel();
                        file.UploadFileName = item.UploadFileName;
                        file.UploadedFileGuid = item.ContractResourceFileGuid;
                        file.Description = item.Description;
                        fileDetails.Add(file);
                    }
                    viewModel.ContractRepresentativeFiles = fileDetails;
                }

                if (accountingRepresentativeData != null)
                {
                    var files = _contractService.GetFileListByContractGuid(accountingRepresentativeData.QuestionaireUserAnswerGuid);
                    var fileDetails = new List<FileUploadModel>();
                    foreach (var item in files)
                    {
                        var file = new FileUploadModel();
                        file.UploadFileName = item.UploadFileName;
                        file.UploadedFileGuid = item.ContractResourceFileGuid;
                        file.Description = item.Description;
                        fileDetails.Add(file);
                    }
                    viewModel.AccountingRepresentativeFiles = fileDetails;
                }

            }

            return viewModel;
        }

        private Guid Post(ContractCloseOutViewModel viewModel)
        {
            Guid currentUser = UserHelper.CurrentUserGuid(HttpContext);
            Guid createdBy = UserHelper.CurrentUserGuid(HttpContext);
            Guid updatedBy = UserHelper.CurrentUserGuid(HttpContext);
            DateTime createdOn = CurrentDateTimeHelper.GetCurrentDateTime();
            DateTime updatedOn = CurrentDateTimeHelper.GetCurrentDateTime();
            var modelData = new List<QuestionaryAnswerViewModel>();
            var modelSubData = new List<QuestionaryAnswerViewModel>();
            var projectManager = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ProjectManager");
            var contractRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ContracRepresentative");
            var accountingRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("AccountRepresentative");

            if (viewModel.ProjectManagerQuestions != null && viewModel.RepresentativeType == projectManager)
            {
                modelData.AddRange(viewModel.ProjectManagerQuestions.Where(x => !string.IsNullOrEmpty(x.Answer)));
            }
            if (viewModel.ContractRepresentativeQuestions != null && viewModel.RepresentativeType == contractRepresentative)
            {
                modelData.AddRange(viewModel.ContractRepresentativeQuestions.Where(x => !string.IsNullOrEmpty(x.Answer)));
            }
            if (viewModel.AccountingRepresentativeQuestions != null && viewModel.RepresentativeType == accountingRepresentative)
            {
                modelData.AddRange(viewModel.AccountingRepresentativeQuestions.Where(x => !string.IsNullOrEmpty(x.Answer)));
            }
            if (viewModel.SubQuestions != null && viewModel.RepresentativeType == projectManager)
            {
                modelSubData.AddRange(viewModel.SubQuestions.Where(x => !string.IsNullOrEmpty(x.Answer)));
            }


            foreach (var item in modelData)
            {
                string approvalStatus = "";
                if (item.Answer == "No")
                {
                    if (item.OrderNumber == 1 && item.RepresentativeType == projectManager)
                    {
                        approvalStatus = ApprovalStatus.APPROVED.ToString();
                        modelSubData = null;
                    }
                    else
                    {
                        approvalStatus = ApprovalStatus.UNAPPROVED.ToString();
                    }
                }
                else
                {
                    approvalStatus = ApprovalStatus.APPROVED.ToString();
                }
                var contractCloseApproval = _contractCloseApprovalService.GetByNormalizedValue(approvalStatus);
                var model = new QuestionaireUserAnswer
                {
                    Answer = item.Answer,
                    ContractCloseApprovalGuid = contractCloseApproval.ContractCloseApprovalGuid,
                    ContractGuid = viewModel.ContractGuid,
                    ManagerUserGuid = currentUser,
                    QuestionaireMasterGuid = item.QuestionaireMasterGuid,
                    Questions = item.Questions,
                    RepresentativeType = item.RepresentativeType,
                    Status = contractCloseApproval.NormalizedValue,
                    Notes = item.Notes,
                    CreatedOn = createdOn,
                    CreatedBy = createdBy,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                };
                _questionaireUserAnswerService.Add(model);
            }
            if (modelSubData != null)
            {
                if (modelSubData.Count() > 0)
                {
                    foreach (var item in modelSubData.Where(x => x.Answer != null))
                    {
                        string approvalStatus = "";
                        if (item.Answer == "No")
                        {
                            approvalStatus = ApprovalStatus.UNAPPROVED.ToString();
                        }
                        else
                        {
                            approvalStatus = ApprovalStatus.APPROVED.ToString();
                        }
                        var contractCloseApproval = _contractCloseApprovalService.GetByNormalizedValue(approvalStatus);
                        var model = new QuestionaireUserAnswer
                        {
                            Answer = item.Answer,
                            ContractCloseApprovalGuid = contractCloseApproval.ContractCloseApprovalGuid,
                            ContractGuid = viewModel.ContractGuid,
                            ManagerUserGuid = currentUser,
                            QuestionaireMasterGuid = item.QuestionaireMasterGuid,
                            Questions = item.Questions,
                            RepresentativeType = item.RepresentativeType,
                            Status = contractCloseApproval.NormalizedValue,
                            CreatedOn = createdOn,
                            CreatedBy = createdBy,
                            UpdatedBy = updatedBy,
                            UpdatedOn = updatedOn
                        };
                        _questionaireUserAnswerService.Add(model);
                    }
                }
            }

            //For Notification 
            AddNotification(viewModel);

            var savedAnswers = _questionaireUserAnswerService.GetByContractGuid(viewModel.ContractGuid);
            var resourceid = Guid.Empty;
            if (savedAnswers.Count() > 0)
            {
                if (viewModel.RepresentativeType == projectManager)
                {
                    var projectManagerData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == projectManager);
                    resourceid = projectManagerData != null ? projectManagerData.QuestionaireUserAnswerGuid : Guid.Empty;
                    Guid[] ids = new Guid[1];
                    ids[0] = viewModel.ContractGuid;
                    _contractService.Disable(ids);
                }
                else if (viewModel.RepresentativeType == contractRepresentative)
                {
                    var contractRepresentativeData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == contractRepresentative);
                    resourceid = contractRepresentativeData != null ? contractRepresentativeData.QuestionaireUserAnswerGuid : Guid.Empty;
                }
                else if (viewModel.RepresentativeType == accountingRepresentative)
                {
                    var accountingRepresentativeData = savedAnswers.FirstOrDefault(x => x.RepresentativeType == accountingRepresentative);
                    if (accountingRepresentativeData != null)
                    {
                        resourceid = accountingRepresentativeData.QuestionaireUserAnswerGuid;
                        _contractService.CloseContractStatus(viewModel.ContractGuid);
                    }
                }
            }
            return resourceid;
        }

        private string GetRepresentativeType(Guid contractGuid, Guid currentUser, IEnumerable<QuestionaireUserAnswer> savedAnswers)
        {
            var userRole = _contractService.GetContractRoleByUserGuid(contractGuid, currentUser);
            var representativeType = string.Empty;
            if (userRole.Count() > 0)
            {
                var projectManager = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ProjectManager");

                var projectManagerRole = userRole.FirstOrDefault(x => x.ToUpper() == projectManager);

                if (savedAnswers.Count() > 0)
                {
                    var contractRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ContracRepresentative");
                    var accountingRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("AccountRepresentative");

                    var contractRepresentativeRole = userRole.FirstOrDefault(x => x.ToUpper() == contractRepresentative);
                    var accountingRepresentativeRole = userRole.FirstOrDefault(x => x.ToUpper() == accountingRepresentative);

                    if (contractRepresentativeRole != null)
                    {
                        representativeType = contractRepresentative;
                    }
                    else
                    {
                        representativeType = "NotAccessable";
                    }
                    if (accountingRepresentativeRole != null)
                    {
                        if (savedAnswers.Any(x => x.RepresentativeType == contractRepresentative))
                        {
                            representativeType = accountingRepresentative;
                        }
                        if (savedAnswers.Any(x => x.RepresentativeType == accountingRepresentative))
                        {
                            representativeType = string.Empty;
                        }
                    }

                }
                else
                {
                    if (projectManagerRole != null)
                    {
                        representativeType = projectManager;
                    }
                    else
                    {
                        representativeType = "NotAccessable";
                    }
                }
            }
            return representativeType;
        }

        #region notification

        private bool AddNotification(ContractCloseOutViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.RepresentativeType))
            {
                var projectManager = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ProjectManager");
                var contractRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("ContracRepresentative");
                var accountingRepresentative = _configuration.GetSection("ContractRepresentatives").GetValue<string>("AccountRepresentative");

                string contractClosekey = Infrastructure.Helpers.FormatHelper.ConcatResourceTypeAndAction(EnumGlobal.ResourceType.ContractCloseOut.ToString(),
                                            EnumGlobal.CrudType.Notify.ToString());
                var contractdetails = _contractService.GetOnlyRequiredContractData(viewModel.ContractGuid);
                var representativeToNotify = string.Empty;
                if (viewModel.RepresentativeType.ToUpper() == projectManager)
                {
                    representativeToNotify = contractRepresentative;
                }
                else if (viewModel.RepresentativeType.ToUpper() == contractRepresentative)
                {
                    representativeToNotify = accountingRepresentative;
                }
                else
                {
                    representativeToNotify = string.Empty;
                }
                if (!string.IsNullOrEmpty(representativeToNotify))
                {
                    var notification = new ContractNotificationModel
                    {
                        ContractGuid = viewModel.ContractGuid,
                        ContractNumber = contractdetails.ContractNumber,
                        ProjectNumber = contractdetails.ProjectNumber,
                        ContractRepresentative = viewModel.RepresentativeType,
                        ContractTitle = contractdetails.ContractTitle,
                        ContractType = contractdetails.ContractType,
                        key = contractClosekey,
                        Description = contractdetails.Description
                    };
                    AddNotificationMessage(notification);
                }
            }
            return true;
        }

        private bool AddNotificationMessage(ContractNotificationModel contractModel)
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
                notificationModel.NotificationTemplateKey = contractModel.key;
                notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                notificationModel.SendEmail = true;


                var keyPersonnels = _contractService.GetKeyPersonnelByContractGuid(contractModel.ContractGuid);

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

        #endregion

        #endregion
    }
}
