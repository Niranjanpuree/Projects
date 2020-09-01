using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Utilities;
using Northwind.Web.Helpers;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models;
using Northwind.Web.Models.ViewModels;
using static Northwind.Core.Entities.GenericNotification;
using static Northwind.Web.Models.ViewModels.EnumGlobal;
using EnumGlobal = Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationBatchService _notificationBatchService;
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IDistributionListService _distributionListService;
        private readonly INotificationTemplatesService _notificationTemplatesService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IJobRequestService _jobRequestService;
        private readonly IContractsService _contractRefactorService;
        private readonly IEmailSender _emailSender;
        private readonly IContractsService _contractsService;
        private readonly IContractModificationService _contractModificationService;
        private readonly IRevenueRecognitionService _revenueRecognitionService;
        private readonly IGenericNotificationService _genericNotificationService;
        public NotificationController(
            INotificationBatchService notificationBatchService,
            INotificationMessageService notificationMessageService,
            IDistributionListService distributionListService,
            INotificationTemplatesService notificationTemplatesService,
            INotificationService notificationService,
            IUserService userService,
            IEmailSender emailSender,
            IJobRequestService jobRequestService,
            IContractsService contractRefactorService,
            IRevenueRecognitionService revenueRecognitionService,
            IConfiguration configuration,
            IResourceAttributeValueService resourceAttributeValueService,
            IContractsService contractsService,
            IGenericNotificationService genericNotificationService,
            IContractModificationService contractModificationService
            )
        {
            _notificationBatchService = notificationBatchService;
            _notificationMessageService = notificationMessageService;
            _distributionListService = distributionListService;
            _notificationTemplatesService = notificationTemplatesService;
            _notificationService = notificationService;
            _userService = userService;
            _configuration = configuration;
            _jobRequestService = jobRequestService;
            _contractRefactorService = contractRefactorService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _emailSender = emailSender;
            _genericNotificationService = genericNotificationService;
            _contractsService = contractsService;
            _revenueRecognitionService = revenueRecognitionService;
            _contractModificationService = contractModificationService;
        }

        //load notification page after contract/project/job request is saved and updated ,works for redirection page.
        public IActionResult Index(string redirectUrl, string key, string cameFrom, string resourceName, string resourceDisplayName, Guid resourceId, string parentContractNumber, string parentRedirection)
        {
            var loggedUser = UserHelper.CurrentUserGuid(HttpContext);
            ViewBag.loggedUser = loggedUser;
            ViewBag.redirectUrl = redirectUrl;
            ViewBag.key = key;
            ViewBag.cameFrom = cameFrom;
            ViewBag.resourceName = resourceName;
            ViewBag.resourceDisplayName = resourceDisplayName;
            ViewBag.resourceId = resourceId;
            ViewBag.parentContractNumber = parentContractNumber;
            ViewBag.parentRedirection = parentRedirection;
            return View();
        }

        //update table Notification Message when user read the message..
        [HttpPost]
        public IActionResult EditUserResponseByNotificationMessageId([FromBody]Guid notificationMessageGuid)
        {
            try
            {
                _notificationMessageService.EditUserResponseByNotificationMessageId(notificationMessageGuid);
                return Ok(new { status = true });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        //get all message of specified logged user ..
        public IActionResult GetDesktopNotification(string searchValue)
        {
            var loggedUser = UserHelper.CurrentUserGuid(HttpContext);

            var notificationMessages = _notificationMessageService.GetDesktopNotification(loggedUser, searchValue);

            //this block is required to get the actual User name of user who created the notifications
            List<NotificationMessageViewModel> notificationMessageViewModels = new List<NotificationMessageViewModel>();
            foreach (var ob in notificationMessages)
            {
                var notificationMessage = Models.ObjectMapper<NotificationMessage, NotificationMessageViewModel>.Map(ob);
                var user = _userService.GetUserByUserGuid(notificationMessage.CreatedBy);

                notificationMessage.CreatedByName =
                    Infrastructure.Helpers.FormatHelper.FormatFullName(user.Firstname, string.Empty, user.Lastname);
                notificationMessage.CreatedOnFormatDateTime = Infrastructure.Helpers.FormatHelper.FormatDateTime(
                    notificationMessage.CreatedOn.ToString("MM/dd/yyyy"),
                    notificationMessage.CreatedOn.ToString("HH:m:s tt zzz"));
                notificationMessageViewModels.Add(notificationMessage);
            }

            return Json(notificationMessageViewModels);
        }

        //add notification and send to the respective selected users of distribution list and may be individual users   from notification page that was loaded from index page..
        [HttpPost]
        public IActionResult Add([FromBody]NotificationViewModel notificationViewModel)
        {
            try
            {
                Guid loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);

                var currentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                bool isJobRequestNotified = false;
                if (notificationViewModel.NotificationTemplateKey == "RevenueRecognition.ContractReview" || notificationViewModel.NotificationTemplateKey == "RevenueRecognition.ContractCompleted")
                {
                    var revenuedetails = _revenueRecognitionService.GetDetailsById(notificationViewModel.ResourceId);
                    if (revenuedetails != null)
                    {
                        var contractEntity =
                              _contractsService.GetOnlyRequiredContractData(revenuedetails.ContractGuid);
                        if (contractEntity != null)
                        {
                            if (contractEntity.ParentContractGuid != null)
                            {
                                if (contractEntity.ParentContractGuid != Guid.Empty)
                                {
                                    if (notificationViewModel.NotificationTemplateKey == "RevenueRecognition.ContractReview")
                                    {
                                        notificationViewModel.NotificationTemplateKey = "RevenueRecognition.TaskOrderReview";
                                    }
                                    else if (notificationViewModel.NotificationTemplateKey == "RevenueRecognition.ContractCompleted")
                                    {
                                        notificationViewModel.NotificationTemplateKey = "RevenueRecognition.TaskOrderCompleted";
                                    }
                                }
                            }
                        }
                    }
                }


                var notificationTemplate = _notificationTemplatesService.GetByKey(notificationViewModel.NotificationTemplateKey);


                #region Add to Notification Batch
                //Add the  Notification Batch .....
                NotificationBatch notificationBatch = new NotificationBatch();
                notificationBatch.NotificationBatchGuid = UserHelper.GetNewGuid();
                notificationBatch.StartDate = currentDate;
                if (!string.IsNullOrEmpty(notificationViewModel.NotificationTemplateKey))
                {
                    notificationBatch.ResourceType = notificationViewModel.NotificationTemplateKey.Split('.')[0];
                    notificationBatch.ResourceAction = notificationViewModel.NotificationTemplateKey.Split('.')[1];
                }
                notificationBatch.AdditionalMessage = notificationViewModel.AdditionalNotes;
                notificationBatch.ResourceId = notificationViewModel.ResourceId;
                notificationBatch.NotificationTemplateGuid = notificationTemplate.NotificationTemplateGuid;
                notificationBatch.CreatedBy = loggedUserGuid;
                notificationBatch.CreatedOn = currentDate;

                Guid notificationBatchGuid = _notificationBatchService.Add(notificationBatch);
                #endregion

                #region Add to Notification Message 

                List<NotificationMessage> notificationMessageList = new List<NotificationMessage>();

                //Add the Notification Message  if Selected all the distribution list ........
                if (notificationViewModel.DistributionSelection.SelectedAll)
                {
                    var distributionAllList =
                       _distributionListService.GetDistributionListByLoggedUser(loggedUserGuid, string.Empty);

                    var distributionExceptExcludeList = distributionAllList.Where(x =>
                        !notificationViewModel.DistributionSelection.ExcludeList.Select(y => y.DistributionListGuid)
                            .Contains(x.DistributionListGuid));

                    if (distributionExceptExcludeList.Count() > 0)
                    {

                        foreach (var distributionList in distributionExceptExcludeList)
                        {
                            var distributionUserList = _distributionListService.GetDistributionUsersById(distributionList.DistributionListGuid);
                            if (distributionUserList.Count() > 0)
                            {
                                if (notificationViewModel.NotificationTemplateKey == "JobRequest.Notify")
                                {
                                    InsertMessageForJobRequest(notificationViewModel, distributionUserList, notificationBatchGuid);
                                    break;
                                }
                                foreach (var distributionUser in distributionUserList)
                                {
                                    var notificationSingleMessage = new NotificationMessage
                                    {
                                        NotificationMessageGuid = UserHelper.GetNewGuid(),
                                        NotificationBatchGuid = notificationBatchGuid,
                                        DistributionListGuid = distributionUser.DistributionListGuid,
                                        UserGuid = distributionUser.UserGuid,
                                        Subject = notificationTemplate.Subject,
                                        Message = notificationTemplate.Message,
                                        AdditionalMessage = notificationViewModel.AdditionalNotes,
                                        Status = false,
                                        UserResponse = false,
                                        NextAction = currentDate,
                                        CreatedOn = currentDate,
                                        CreatedBy = loggedUserGuid,
                                    };

                                    notificationMessageList.Add(notificationSingleMessage);
                                }
                            }
                        }
                    }
                }
                else if (notificationViewModel.NotificationTemplateKey.Split('.')[0] == "RevenueRecognition")
                {
                    List<NotificationDistributionIndividualViewModel> userNotificationModel = new List<NotificationDistributionIndividualViewModel>();
                    foreach (var distributionList in notificationViewModel.DistributionSelection.IncludeList)
                    {
                        var distributionUserList = _distributionListService.GetDistributionUsersById(distributionList.DistributionListGuid);
                        foreach (var user in distributionUserList)
                        {
                            var receiverDetails = _userService.GetUserByUserGuid(user.UserGuid);
                            var data = new NotificationDistributionIndividualViewModel
                            {
                                AdditionalNotes = notificationViewModel.AdditionalNotes,
                                DistributionListGuid = user.DistributionListGuid,
                                NotificationTemplateKey = notificationViewModel.NotificationTemplateKey,
                                ReceiverGuid = receiverDetails.UserGuid,
                                ResourceId = notificationViewModel.ResourceId,
                                ReceiverDisplayName = receiverDetails.DisplayName,
                            };
                            userNotificationModel.Add(data);
                        }
                        //InsertMessageForRevenueRecognition(notificationViewModel, additionalUsers, distributionUserList, notificationBatchGuid);
                    }
                    if (notificationViewModel.IndividualRecipients != null)
                    {
                        foreach (var individualUser in notificationViewModel.IndividualRecipients)
                        {
                            var data = new NotificationDistributionIndividualViewModel
                            {
                                AdditionalNotes = notificationViewModel.AdditionalNotes,
                                DistributionListGuid = Guid.Empty,
                                NotificationTemplateKey = notificationViewModel.NotificationTemplateKey,
                                ReceiverGuid = individualUser.UserGuid,
                                ResourceId = notificationViewModel.ResourceId,
                                ReceiverDisplayName = individualUser.DisplayName,
                            };
                            userNotificationModel.Add(data);
                        }
                    }
                    var list = userNotificationModel.GroupBy(x => x.ReceiverGuid).Select(y => y.First()).Distinct().ToList();
                    string additionalUsers = " ";
                    foreach (var user in list)
                    {
                        additionalUsers += user.ReceiverDisplayName + ", ";
                    }
                    if (additionalUsers.EndsWith(", "))
                    {
                        additionalUsers = additionalUsers.Remove(additionalUsers.Length - 2, 2);
                    }
                    InsertMessageForRevenueRecognition(list, additionalUsers, notificationBatchGuid);
                }
                else //save notification of selected distribution list..
                {
                    if (notificationViewModel.DistributionSelection.IncludeList.Count() > 0)
                    {
                        foreach (var distributionList in notificationViewModel.DistributionSelection.IncludeList)
                        {
                            var distributionUserList = _distributionListService.GetDistributionUsersById(distributionList.DistributionListGuid);
                            if (distributionUserList.Count() > 0)
                            {
                                if (notificationViewModel.NotificationTemplateKey == "JobRequest.Notify")
                                {
                                    InsertMessageForJobRequest(notificationViewModel, distributionUserList, notificationBatchGuid);
                                    break;
                                }
                                foreach (var distributionUser in distributionUserList)
                                {
                                    var notificationSingleMessage = new NotificationMessage
                                    {
                                        NotificationMessageGuid = UserHelper.GetNewGuid(),
                                        NotificationBatchGuid = notificationBatchGuid,
                                        DistributionListGuid = distributionUser.DistributionListGuid,
                                        UserGuid = distributionUser.UserGuid,
                                        Subject = notificationTemplate.Subject,
                                        Message = notificationTemplate.Message,
                                        AdditionalMessage = notificationViewModel.AdditionalNotes,
                                        Status = false,
                                        UserResponse = false,
                                        NextAction = currentDate,
                                        CreatedOn = currentDate,
                                        CreatedBy = loggedUserGuid,
                                    };

                                    notificationMessageList.Add(notificationSingleMessage);
                                }
                            }
                        }

                    }
                }

                //added individual record
                if (notificationViewModel.IndividualRecipients != null)
                {
                    //List<NotificationMessage> notificationMessage = new List<NotificationMessage>();

                    foreach (var user in notificationViewModel.IndividualRecipients)
                    {
                        if (notificationViewModel.NotificationTemplateKey == "JobRequest.Notify")
                        {
                            notificationMessageList = InsertMessageForJobRequestForIndividual(notificationViewModel, notificationViewModel.IndividualRecipients, notificationBatchGuid);
                            isJobRequestNotified = true;
                            break;
                        }
                        if (notificationViewModel.NotificationTemplateKey.Split('.')[0] == "RevenueRecognition")
                        {
                            return Ok(new { status = true });
                        }
                        else
                        {
                            var notificationSingleMessage = new NotificationMessage
                            {
                                NotificationMessageGuid = UserHelper.GetNewGuid(),
                                NotificationBatchGuid = notificationBatchGuid,
                                //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                                UserGuid = user.UserGuid,
                                Subject = notificationTemplate.Subject,
                                Message = notificationTemplate.Message,
                                AdditionalMessage = notificationViewModel.AdditionalNotes,
                                Status = false,
                                UserResponse = false,
                                NextAction = currentDate,
                                CreatedOn = currentDate,
                                CreatedBy = loggedUserGuid,
                            };
                            notificationMessageList.Add(notificationSingleMessage);
                        }
                    }
                }

                // make unique notification message because we have distribution list and individual recipient some time users could be selected same in  both ..

                notificationMessageList = notificationMessageList.GroupBy(p => p.UserGuid).Select(g => g.FirstOrDefault()).ToList();

                //If template not found for any user , or no user has been selected either in individual or in distribution list..
                if (notificationMessageList.Count() == 0 && isJobRequestNotified == false)
                {
                    throw new ArgumentException("Either distribution list has no member(s) or individual recipient has not been selected.");
                }

                //format all the raw template message into formatted message template  with data


                List<string> otherRecipients = new List<string>();
                foreach (var notificationMessage in notificationMessageList)
                {
                    var ob = _userService.GetUserByUserGuid(notificationMessage.UserGuid).DisplayName;
                    otherRecipients.Add(ob);
                }

                if (!isJobRequestNotified)
                {
                    foreach (var notificationMessage in notificationMessageList)
                    {
                        var formattedNotificationMessage = FillDataInTemplateAccordingToTheResourceTypeAndResourceId(notificationBatch, notificationMessage, otherRecipients);

                        // finally save the notification message 
                        _notificationMessageService.Add(formattedNotificationMessage);
                    }
                }

                #endregion

                #region Now Notify Users 
                // For Email Message..
                _notificationService.NotifyUsers(notificationBatch.NotificationBatchGuid);
                #endregion

                return Ok(new { status = true });
            }
            catch (ArgumentException ex)
            {
                return BadRequestFormatter.BadRequest(this, ex);
            }
            catch (Exception e)
            {
                return BadRequestFormatter.BadRequest(this, e);
            }
        }

        public bool InsertMessageForJobRequest(NotificationViewModel notificationViewModel, IEnumerable<DistributionUser> userList, Guid notificationBatchGuid)
        {
            Guid loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var currentDate = CurrentDateTimeHelper.GetCurrentDateTime();

            //var otherUser = string.Join(", ",userList.Select(x => x))
            string users = " ";
            foreach (var user in userList)
            {
                var receiverDetails = _userService.GetUserByUserGuid(user.UserGuid);
                users += receiverDetails.DisplayName + ", ";
            }
            if (users.EndsWith(", "))
            {
                users = users.Remove(users.Length - 2, 2);
            }
            var message = SendEmailToRespectivePersonnel(notificationViewModel.ResourceId, notificationViewModel.AdditionalNotes, users);
            var notificationList = new List<NotificationMessage>();
            notificationList.Add(new NotificationMessage
            {
                NotificationMessageGuid = Northwind.Web.Infrastructure.Helpers.UserHelper.GetNewGuid(),
                NotificationBatchGuid = notificationBatchGuid,
                //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                UserGuid = message.UserGuid,
                Subject = message.Subject,
                Message = message.Message,
                AdditionalMessage = notificationViewModel.AdditionalNotes,
                Status = false,
                UserResponse = false,
                NextAction = currentDate,
                CreatedOn = currentDate,
                CreatedBy = loggedUserGuid,
            });
            foreach (var user in userList)
            {
                var notificationMessage = new NotificationMessage
                {
                    NotificationMessageGuid = UserHelper.GetNewGuid(),
                    NotificationBatchGuid = notificationBatchGuid,
                    //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                    UserGuid = user.UserGuid,
                    Subject = message.Subject,
                    Message = message.Message,
                    AdditionalMessage = notificationViewModel.AdditionalNotes,
                    Status = false,
                    UserResponse = false,
                    NextAction = currentDate,
                    CreatedOn = currentDate,
                    CreatedBy = loggedUserGuid,
                };

                _notificationMessageService.Add(notificationMessage);
            }
            return true;
        }

        public List<NotificationMessage> InsertMessageForJobRequestForIndividual(NotificationViewModel notificationViewModel, IEnumerable<UserViewModel> userList, Guid notificationBatchGuid)
        {
            Guid loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var currentDate = CurrentDateTimeHelper.GetCurrentDateTime();
            string users = " ";
            foreach (var user in userList)
            {
                var receiverDetails = _userService.GetUserByUserGuid(user.UserGuid);
                users += receiverDetails.DisplayName + ", ";
            }
            foreach (var user in notificationViewModel.IndividualRecipients)
            {
                var additionalUser = _userService.GetUserByUserGuid(user.UserGuid);
            }
            if (users.EndsWith(", "))
            {
                users = users.Remove(users.Length - 2, 2);
            }

            var message = SendEmailToRespectivePersonnel(notificationViewModel.ResourceId, notificationViewModel.AdditionalNotes, users);
            var notificationList = new List<NotificationMessage>();
            notificationList.Add(new NotificationMessage
            {
                NotificationMessageGuid = UserHelper.GetNewGuid(),
                NotificationBatchGuid = notificationBatchGuid,
                //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                UserGuid = message.UserGuid,
                Subject = message.Subject,
                Message = message.Message,
                AdditionalMessage = notificationViewModel.AdditionalNotes,
                Status = false,
                UserResponse = false,
                NextAction = currentDate,
                CreatedOn = currentDate,
                CreatedBy = loggedUserGuid,
            });
            foreach (var user in userList)
            {
                var notificationMessage = new NotificationMessage
                {
                    NotificationMessageGuid = UserHelper.GetNewGuid(),
                    NotificationBatchGuid = notificationBatchGuid,
                    //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                    UserGuid = user.UserGuid,
                    Subject = message.Subject,
                    Message = message.Message,
                    AdditionalMessage = notificationViewModel.AdditionalNotes,
                    Status = false,
                    UserResponse = false,
                    NextAction = currentDate,
                    CreatedOn = currentDate,
                    CreatedBy = loggedUserGuid,
                };
                _notificationMessageService.Add(notificationMessage);
            }

            foreach (var user in notificationViewModel.IndividualRecipients)
            {
                var notificationMessage = new NotificationMessage
                {
                    NotificationMessageGuid = UserHelper.GetNewGuid(),
                    NotificationBatchGuid = notificationBatchGuid,
                    //  DistributionListGuid = we don't have distribution list id while adding individual user for notification..,
                    UserGuid = user.UserGuid,
                    Subject = message.Subject,
                    Message = message.Message,
                    AdditionalMessage = notificationViewModel.AdditionalNotes,
                    Status = false,
                    UserResponse = false,
                    NextAction = currentDate,
                    CreatedOn = currentDate,
                    CreatedBy = loggedUserGuid,
                };
                _notificationMessageService.Add(notificationMessage);
            }
            return notificationList;
        }

        private bool InsertMessageForRevenueRecognition(List<NotificationDistributionIndividualViewModel> notificationViewModel, string additionalUsers, Guid notificationBatchGuid)
        {
            var notificationModel = new GenericNotificationViewModel();
            var notificationTemplatesDetails = new NotificationTemplatesDetail();
            var userList = new List<User>();
            string key = string.Empty;
            string additionalNotes = string.Empty;

            var revenuedetail = _revenueRecognitionService.GetInfoForDetailPage(notificationViewModel[0].ResourceId);
            notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/contract/Details/" + revenuedetail.ContractGuid);
            var contractdetails = _contractRefactorService.GetOnlyRequiredContractData(revenuedetail.ContractGuid);
            string parentcontractNumber = contractdetails.ContractNumber;
            if (contractdetails != null)
            {
                if (contractdetails.ParentContractGuid != null)
                {
                    if (contractdetails.ParentContractGuid != Guid.Empty)
                    {
                        var parentcontractEntity =
                            _contractsService.GetOnlyRequiredContractData(contractdetails.ParentContractGuid);
                        parentcontractNumber = parentcontractEntity.ContractNumber;
                        notificationModel.RedirectUrl = _configuration.GetSection("SiteUrl").Value + ("/project/Details/" + revenuedetail.ContractGuid);
                    }
                }
            }
            decimal thresholdAmount = RevenueRecognitionHelper.GetAmountByContractType(_configuration, contractdetails.ContractType);
            var resourcevalue = _resourceAttributeValueService.GetResourceAttributeValueByValue(contractdetails.ContractType);
            string contracttype = contractdetails.ContractType;
            if (resourcevalue != null)
            {
                contracttype = resourcevalue.Name;
            }
            if (notificationViewModel?.Any() == true)
            {
                key = notificationViewModel[0].NotificationTemplateKey;
                additionalNotes = notificationViewModel[0].AdditionalNotes;
                foreach (var item in notificationViewModel)
                {
                    var receiverInfo = _userService.GetUserByUserGuid(item.ReceiverGuid);
                    userList.Add(receiverInfo);
                    notificationModel.IndividualRecipients = userList;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    notificationModel.ResourceId = revenuedetail.RevenueRecognizationGuid;
                    notificationModel.NotificationTemplateKey = key;
                    notificationModel.CurrentDate = CurrentDateTimeHelper.GetCurrentDateTime();
                    notificationModel.CurrentUserGuid = UserHelper.CurrentUserGuid(HttpContext);
                    notificationModel.SendEmail = true;
                    notificationTemplatesDetails.ContractNumber = parentcontractNumber;
                    notificationTemplatesDetails.TaskOrderNumber = contractdetails.ContractNumber;
                    notificationTemplatesDetails.Title = contractdetails.ContractTitle;
                    notificationTemplatesDetails.ContractType = contracttype;
                    notificationTemplatesDetails.ContractTitle = contractdetails.ContractTitle;
                    notificationTemplatesDetails.ProjectNumber = contractdetails.ProjectNumber;
                    notificationTemplatesDetails.AdditionalMessage = additionalNotes;
                    notificationTemplatesDetails.AdditionalUser = additionalUsers;
                    notificationTemplatesDetails.ThresholdAmount = thresholdAmount;
                    notificationTemplatesDetails.Status = "";
                    notificationModel.NotificationTemplatesDetail = notificationTemplatesDetails;

                    _genericNotificationService.AddNotificationMessage(notificationModel);
                }
            }
            return false;
        }

        public NotificationMessage SendEmailToRespectivePersonnel(Guid contractGuid, string additionalMessage, string otherRecipients)
        {
            var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(contractGuid);
            var model = ContractsMapper.MapJobRequestToViewModel(jobRequestEntity);
            var keyPersonnel = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid);

            var status = jobRequestEntity.Contracts.JobRequest.Status;
            var param = new { id = contractGuid };
            var link = _configuration.GetSection("SiteUrl").Value + ("/JobRequest/Detail/" + contractGuid);
            //var link = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "JobRequest", "Detail", param);
            //var urlLink = new UrlHelper(ControllerContext.RequestContext);
            JobRequestEmailModel emailModel = new JobRequestEmailModel();
            emailModel.ContractNumber = model.BasicContractInfo.ContractNumber;
            emailModel.ProjectNumber = model.BasicContractInfo.ProjectNumber;
            emailModel.AwardingAgency = model.CustomerInformation.AwardingAgencyOfficeName;
            emailModel.FundingAgency = model.CustomerInformation.FundingAgencyOfficeName;
            emailModel.ContractTitle = model.BasicContractInfo.ContractTitle;
            emailModel.Description = model.BasicContractInfo.Description;
            emailModel.ClickableUrl = link;
            emailModel.AdditionalMessage = additionalMessage;
            emailModel.Status = "In Progress";
            emailModel.NotifyOther = otherRecipients;

            //string emailTo = "gmagar@xylontech.com,sshrestha @xylontech.com";
            string recipientName = string.Empty;
            string subject = string.Empty;

            Guid notifyTo = UserHelper.CurrentUserGuid(HttpContext);

            var notificationTemplate = _notificationTemplatesService.GetByKey("JobRequest.Notify");
            var content = string.Empty;
            var template = string.Empty;
            if (notificationTemplate != null)
                template = notificationTemplate.Message;



            //for filtering the representative to send email
            switch (status)
            {
                case (int)JobRequestStatus.ProjectControl:
                    var controlRepresentative = model.KeyPersonnel.ProjectControls;
                    if (controlRepresentative != null)
                        notifyTo = controlRepresentative;
                    var projectUser = _userService.GetUserByUserGuid(notifyTo);
                    if (projectUser != null)
                    {
                        //emailTo = contractUser.WorkEmail;
                        recipientName = projectUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var conManager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = conManager.Firstname + " " + conManager.Lastname;
                    var submittedBy = _userService.GetUserByUserGuid(model.KeyPersonnel.ContractRepresentative);
                    if (submittedBy != null)
                    {
                        emailModel.SubmittedBy = submittedBy.Firstname + " " + submittedBy.Lastname;
                    }
                    break;
                case (int)JobRequestStatus.ProjectManager:
                    var projectRepresentative = model.KeyPersonnel.ProjectManager;
                    if (projectRepresentative != null)
                        notifyTo = projectRepresentative;
                    var managerUser = _userService.GetUserByUserGuid(notifyTo);
                    if (managerUser != null)
                    {
                        //emailTo = controlUser.WorkEmail;
                        recipientName = managerUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var manager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = manager.Firstname + " " + manager.Lastname;

                    var submittedByProject = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectControls);
                    if (submittedByProject != null)
                    {
                        emailModel.SubmittedBy = submittedByProject.Firstname + " " + submittedByProject.Lastname;
                    }
                    break;
                case (int)JobRequestStatus.Accounting:
                    var managerRepresentative = model.KeyPersonnel.AccountingRepresentative;
                    if (managerRepresentative != null)
                        notifyTo = managerRepresentative;
                    var accountManager = _userService.GetUserByUserGuid(notifyTo);
                    if (accountManager != null)
                    {
                        //emailTo = projectManager.WorkEmail;
                        recipientName = accountManager.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "A new Job Request Form has been submitted for contract: " + emailModel.ContractNumber;
                    }
                    var submittedByManager = _userService.GetUserByUserGuid(model.KeyPersonnel.ProjectManager);
                    if (submittedByManager != null)
                    {
                        emailModel.SubmittedBy = submittedByManager.Firstname + " " + submittedByManager.Lastname;
                    }
                    break;
                case (int)JobRequestStatus.Complete:
                    var accountUser = _userService.GetUserByUserGuid(notifyTo);
                    if (accountUser != null)
                    {
                        //emailTo = accountUser.WorkEmail;
                        recipientName = accountUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                    }
                    subject = "Job Request has been set to done for contract: " + emailModel.ContractNumber;
                    var guidList = new List<Guid>();
                    guidList.Add(model.KeyPersonnel.ContractRepresentative);
                    guidList.Add(model.KeyPersonnel.ProjectControls);
                    guidList.Add(model.KeyPersonnel.ProjectManager);
                    var userList = _userService.GetUserByUserGuid(guidList);
                    emailModel.NotifiedTo = string.Join(",", userList.ToList().Select(x => x.Firstname + " " + x.Lastname).ToArray());
                    emailModel.ReceipentName = string.Join(", ", userList.Where(x => x.UserGuid != model.KeyPersonnel.AccountingRepresentative).Select(x => x.Firstname + " " + x.Lastname));
                    var submittedByAccount = _userService.GetUserByUserGuid(model.KeyPersonnel.AccountingRepresentative);
                    if (submittedByAccount != null)
                    {
                        emailModel.SubmittedBy = submittedByAccount.Firstname + " " + submittedByAccount.Lastname;
                    }
                    emailModel.Status = "Done";
                    break;
                default:
                    break;
            }
            content = EmailHelper.GetContentForJobRequestNotify(template, keyPersonnel, emailModel);
            //_emailSender.SendEmailAsync(emailTo, recipientName, subject, content);

            var message = new NotificationMessage();
            message.Message = content;
            message.Subject = subject;
            message.UserGuid = model.KeyPersonnel.ProjectControls;

            return message;
            //return RedirectToAction("Details", baseUrl, new { id = contractGuid });
        }

        //Fill raw template from the specified module data 
        private NotificationMessage FillDataInTemplateAccordingToTheResourceTypeAndResourceId(NotificationBatch notificationBatch, NotificationMessage notificationMessage, List<string> otherRecipients)
        {
            Guid loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            var receiverUser = _userService.GetUserByUserGuid(notificationMessage.UserGuid);
            var receiverUserFullName = Infrastructure.Helpers.FormatHelper.FormatFullName(receiverUser.Firstname, "", receiverUser.Lastname);
            var loggedUser = _userService.GetUserByUserGuid(loggedUserGuid);
            var loggedUserFullName = Infrastructure.Helpers.FormatHelper.FormatFullName(loggedUser.Firstname, "", loggedUser.Lastname);

            if (notificationBatch.ResourceType.Equals(EnumGlobal.ResourceType.Contract.ToString()))
            {
                var contractEntity =
                    _contractsService.GetDetailById(notificationBatch.ResourceId);
                string contractNumber = contractEntity.ContractNumber;

                notificationMessage.Subject =
           notificationMessage.Subject.Replace("{CONTRACT_NUMBER}", contractNumber);

                var param = new { id = notificationBatch.ResourceId };
                var link = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + notificationBatch.ResourceId);
                notificationMessage.Message = notificationMessage.Message.Replace("{RECEIVER_DISPLAY_NAME}", receiverUserFullName).
                                                                      Replace("{SUBMITTED_NAME}", loggedUserFullName).
                                                                      Replace("{CONTRACT_NUMBER}", contractNumber).
                                                                      Replace("{PROJECT_NUMBER}", contractEntity.ProjectNumber).
                                                                      Replace("{AWARDING_AGENCY}", contractEntity.CustomerInformation.AwardingAgencyOfficeName).
                                                                      Replace("{FUNDING_AGENCY}", contractEntity.CustomerInformation.FundingAgencyOfficeName).
                                                                      Replace("{LINK}", link).
                                                                      Replace("{CONTRACT_TITLE}", contractEntity.BasicContractInfo.ContractTitle).
                                                                      Replace("{CONTRACT_DESCRIPTION}", contractEntity.BasicContractInfo.Description).
                                                                      Replace("{ADDITIONAL_MESSAGE}", notificationBatch.AdditionalMessage).
                                                                      Replace("{ADDITIONAL_RECIPIENT}", string.Join(" , ", otherRecipients)).
                                                                      Replace("{STATUS}", contractEntity.IsActive == true ? "Active" : "Inactive");
            }
            else if (notificationBatch.ResourceType.Equals(EnumGlobal.ResourceType.ContractModification.ToString()))
            {
                var contractMod = _contractModificationService.GetDetailById(notificationBatch.ResourceId);
                var contractEntity =
                    _contractsService.GetDetailById(contractMod.ContractGuid);
                var param = new { id = contractEntity.ContractGuid };
                notificationMessage.Subject =
                    notificationMessage.Subject.Replace("{CONTRACT_NUMBER}", contractEntity.ContractNumber);
                var link = _configuration.GetSection("SiteUrl").Value + ("/Contract/Details/" + contractEntity.ContractGuid);
                notificationMessage.Message = notificationMessage.Message.Replace("{RECEIVER_DISPLAY_NAME}", receiverUserFullName).
                    Replace("{SUBMITTED_NAME}", loggedUserFullName).
                    Replace("{CONTRACT_NUMBER}", contractEntity.ContractNumber).
                    Replace("{PROJECT_NUMBER}", contractEntity.ProjectNumber).
                    Replace("{AWARDING_AGENCY}", contractEntity.CustomerInformation.AwardingAgencyOfficeName).
                    Replace("{FUNDING_AGENCY}", contractEntity.CustomerInformation.FundingAgencyOfficeName).
                    Replace("{MODIFICATION_NUMBER}", contractMod.ModificationNumber).
                    Replace("{AWARD_AMOUNT}", Convert.ToString(contractMod.AwardAmount)).
                    Replace("{LINK}", link).
                    Replace("{CONTRACT_TITLE}", contractEntity.BasicContractInfo.ContractTitle).
                    Replace("{CONTRACT_DESCRIPTION}", contractEntity.BasicContractInfo.Description).
                    Replace("{ADDITIONAL_MESSAGE}", notificationBatch.AdditionalMessage).
                    Replace("{ADDITIONAL_RECIPIENT}", string.Join(" , ", otherRecipients)).
                    Replace("{MODIFICATION_TITLE}", contractMod.ModificationTitle).
                    Replace("{FUNDING_AMOUNT}", contractMod.FundingAmount.ToString()).
                    Replace("{STATUS}", contractEntity.IsActive == true ? "Active" : "Inactive");

            }
            else if (notificationBatch.ResourceType.Equals(EnumGlobal.ResourceType.Project.ToString()))
            {
                var contractEntity =
                    _contractsService.GetDetailById(notificationBatch.ResourceId);

                var param = new { id = notificationBatch.ResourceId };
                string contractNumber = contractEntity.ContractNumber;
                string taskOrderNumber = contractEntity.ContractNumber;
                string taskorderTitle = contractEntity.ContractTitle;
                if (contractEntity.ParentContractGuid != null)
                {
                    if (contractEntity.ParentContractGuid != Guid.Empty)
                    {
                        var parentcontractEntity =
                       _contractsService.GetOnlyRequiredContractData(contractEntity.ParentContractGuid ?? Guid.Empty);
                        contractNumber = parentcontractEntity.ContractNumber;
                    }
                }
                notificationMessage.Subject =
                    notificationMessage.Subject.Replace("{CONTRACT_NUMBER}", contractNumber);
                var link = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + notificationBatch.ResourceId);
                notificationMessage.Message = notificationMessage.Message.Replace("{RECEIVER_DISPLAY_NAME}", receiverUserFullName).
                    Replace("{SUBMITTED_NAME}", loggedUserFullName).
                    Replace("{TASKORDER_NUMBER}", taskOrderNumber).
                    Replace("{TASKORDER_TITLE}", taskorderTitle).
                    Replace("{CONTRACT_NUMBER}", contractNumber).
                    Replace("{PROJECT_NUMBER}", contractEntity.ProjectNumber).
                    Replace("{AWARDING_AGENCY}", contractEntity.CustomerInformation.AwardingAgencyOfficeName).
                    Replace("{FUNDING_AGENCY}", contractEntity.CustomerInformation.FundingAgencyOfficeName).
                    Replace("{CONTRACT_TITLE}", contractEntity.BasicContractInfo.ContractTitle).
                    Replace("{CONTRACT_DESCRIPTION}", contractEntity.BasicContractInfo.Description).
                    Replace("{LINK}", link).
                    Replace("{CONTRACT_TITLE}", contractEntity.BasicContractInfo.ContractTitle).
                    Replace("{CONTRACT_DESCRIPTION}", contractEntity.BasicContractInfo.Description).
                    Replace("{ADDITIONAL_MESSAGE}", notificationBatch.AdditionalMessage).
                    Replace("{ADDITIONAL_RECIPIENT}", string.Join(" , ", otherRecipients)).
                    Replace("{STATUS}", contractEntity.IsActive == true ? "Active" : "Inactive");
            }
            else if (notificationBatch.ResourceType.Equals(EnumGlobal.ResourceType.ProjectModification.ToString()))
            {
                var contractMod = _contractModificationService.GetDetailById(notificationBatch.ResourceId);
                var contractEntity =
                    _contractsService.GetDetailById(contractMod.ContractGuid);
                var param = new { id = contractEntity.ContractGuid };
                string contractNumber = contractEntity.ContractNumber;
                string taskOrderNumber = contractEntity.ContractNumber;
                string taskorderTitle = contractEntity.ContractTitle;
                if (contractEntity.ParentContractGuid != null)
                {
                    if (contractEntity.ParentContractGuid != Guid.Empty)
                    {
                        var parentcontractEntity =
                       _contractsService.GetOnlyRequiredContractData(contractEntity.ParentContractGuid ?? Guid.Empty);
                        contractNumber = parentcontractEntity.ContractNumber;
                    }
                }

                notificationMessage.Subject =
                    notificationMessage.Subject.Replace("{CONTRACT_NUMBER}", contractNumber);
                var link = _configuration.GetSection("SiteUrl").Value + ("/Project/Details/" + contractEntity.ContractGuid);
                notificationMessage.Message = notificationMessage.Message.Replace("{RECEIVER_DISPLAY_NAME}", receiverUserFullName).
                    Replace("{SUBMITTED_NAME}", loggedUserFullName).
                    Replace("{CONTRACT_NUMBER}", contractNumber).
                    Replace("{TASKORDER_NUMBER}", taskOrderNumber).
                    Replace("{TASKORDER_TITLE}", taskorderTitle).
                    Replace("{PROJECT_NUMBER}", contractEntity.ProjectNumber).
                    Replace("{AWARDING_AGENCY}", contractEntity.CustomerInformation.AwardingAgencyOfficeName).
                    Replace("{FUNDING_AGENCY}", contractEntity.CustomerInformation.FundingAgencyOfficeName).
                    Replace("{MODIFICATION_NUMBER}", contractMod.ModificationNumber).
                    Replace("{AWARD_AMOUNT}", Convert.ToString(contractMod.AwardAmount)).
                    Replace("{LINK}", link).
                    Replace("{CONTRACT_TITLE}", contractEntity.BasicContractInfo.ContractTitle).
                    Replace("{CONTRACT_DESCRIPTION}", contractEntity.BasicContractInfo.Description).
                    Replace("{ADDITIONAL_MESSAGE}", notificationBatch.AdditionalMessage).
                    Replace("{ADDITIONAL_RECIPIENT}", string.Join(" , ", otherRecipients)).
                    Replace("{MODIFICATION_TITLE}", contractMod.ModificationTitle).
                    Replace("{FUNDING_AMOUNT}", contractMod.FundingAmount.ToString()).
                    Replace("{STATUS}", contractEntity.IsActive == true ? "Active" : "Inactive");
            }
            else if (notificationBatch.ResourceType.Equals(EnumGlobal.ResourceType.JobRequest.ToString()))
            {
                var users = string.Join(", ", otherRecipients);
                var notification = SendEmailToRespectivePersonnel(notificationBatch.ResourceId, notificationBatch.AdditionalMessage, users);
                notification.NotificationBatchGuid = notificationBatch.NotificationBatchGuid;
                return notification;
            }
            return notificationMessage;
        }
    }
}