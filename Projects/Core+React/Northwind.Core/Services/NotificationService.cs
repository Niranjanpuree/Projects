using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Core.Models;
using Northwind.Core.Interfaces.ContractRefactor;
using static Northwind.Core.Entities.EnumGlobal;
using Northwind.Core.Entities.ContractRefactor;
using System.Text;

namespace Northwind.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationMessageService _notificationMessageService;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly IJobRequestService _jobRequestService;
        private readonly IContractsService _contractRefactorService;
        private readonly INotificationTemplatesService _notificationTemplatesService;

        public NotificationService(
            INotificationMessageService notificationMessageService,
            IUserService userService,
            IContractsService contractsService,
            IJobRequestService jobRequestService,
            INotificationTemplatesService notificationTemplateService,
            IEmailSender emailSender)
        {
            _notificationMessageService = notificationMessageService;
            _userService = userService;
            _emailSender = emailSender;
            _contractRefactorService = contractsService;
            _jobRequestService = jobRequestService;
            _notificationTemplatesService = notificationTemplateService;
        }
        public bool NotifyUsers(Guid notificationBatchGuid)
        {
            /*
                * // For Email Message
                *
                * After saved distribution user in db, then again notify them  accordingly....
                * can fetch from db
                * or take directly from component //todo
                */


            //email notification..

            NotifyAccordingToNotificationType(notificationBatchGuid, EnumGlobal.NotificationType.EmailNotification.ToString());

           /*
            * user notification..
            * will add by default so no need to take further action..
            */
             
           // NotifyAccordingToNotificationType(notificationBatchGuid, EnumGlobal.NotificationType.UserNotification.ToString());

            return true;
        }

        private void NotifyAccordingToNotificationType(Guid notificationBatchGuid, string notificationTypeName)
        {
            var notificationMessages =
                _notificationMessageService.GetByNotificationBatchIdAndNotificationType(notificationBatchGuid, notificationTypeName);

            var notifyMessage =
                _notificationMessageService.GetByNotificationBatchIdAndNotificationType(notificationBatchGuid, notificationTypeName).FirstOrDefault();

            List<string> emailList = new List<string>();
            foreach (var notificationMessage in notificationMessages)
            {
                var ob = _userService.GetUserByUserGuid(notificationMessage.UserGuid).WorkEmail;
                emailList.Add(ob);
            }
            EmailMessageModel usersToNotifyWithTemplate = new EmailMessageModel()
            {
                NotificationMessageGuid = notifyMessage.NotificationMessageGuid,
                Message = notifyMessage.Message,
                Subjects = notifyMessage.Subject,
                Status = notifyMessage.Status,
                AdditionalMessage = notifyMessage.AdditionalMessage,
                WorkEmail = string.Join(",", emailList),
                Displayname = _userService.GetUserByUserGuid(notifyMessage.UserGuid).DisplayName
            };

            /*
             * todo   make method GetNotificationTypeByNotificationBatchId -
             * todo   to compare either Email Notification or SMS Notification in future..
             */

            _emailSender.SendEmailAsync(usersToNotifyWithTemplate.WorkEmail, usersToNotifyWithTemplate.Displayname, usersToNotifyWithTemplate.Subjects,
                usersToNotifyWithTemplate.Message);
        }
        /*
         * This method will be called after user logged in ..
         */

        private EmailMessageModel SendEmailToRespectivePersonnel(int status, Guid contractGuid, string baseUrl)
        {
            var jobRequestEntity = _jobRequestService.GetDetailsForJobRequestById(contractGuid);
            var keyPersonnel = _contractRefactorService.GetKeyPersonnelByContractGuid(contractGuid);

            var param = new { id = contractGuid };
            //var link = RouteUrlHelper.GetAbsoluteAction(_urlHelper, "JobRequest", "Detail", param);
            //var urlLink = new UrlHelper(ControllerContext.RequestContext);
            JobRequestEmailModel emailModel = new JobRequestEmailModel();
            emailModel.ContractNumber = jobRequestEntity.BasicContractInfo.ContractNumber;
            emailModel.ProjectNumber = jobRequestEntity.BasicContractInfo.ProjectNumber;
            emailModel.AwardingAgency = jobRequestEntity.CustomerInformation.AwardingAgencyOfficeName;
            emailModel.FundingAgency = jobRequestEntity.CustomerInformation.FundingAgencyOfficeName;
            //emailModel.ClickableUrl = link;
            emailModel.Status = "In Progress";
            string emailTo = "gmagar@xylontech.com,sshrestha @xylontech.com";
            string recipientName = string.Empty;
            string subject = string.Empty;

            Guid notifyTo = jobRequestEntity.Contracts.ContractRepresentative.UserGuid;

            var notificationTemplate = _notificationTemplatesService.GetByKey("jobrequest-notify");
            var content = string.Empty;
            var template = string.Empty;
            if (notificationTemplate != null)
                template = notificationTemplate.Message;

            //for filtering the representative to send email
            switch (status)
            {
                case (int)JobRequestStatus.ContractRepresentative:
                    var controlRepresentative = jobRequestEntity.KeyPersonnel.ProjectControls;
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
                    var conManager = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = conManager.Firstname + " " + conManager.Lastname;
                    var submittedBy = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.ContractRepresentative);
                    emailModel.SubmittedBy = submittedBy.Firstname + " " + submittedBy.Lastname;
                    break;
                case (int)JobRequestStatus.ProjectControl:
                    var projectRepresentative = jobRequestEntity.KeyPersonnel.ProjectManager;
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
                    var manager = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.ProjectManager);
                    emailModel.NotifiedTo = manager.Firstname + " " + manager.Lastname;

                    var submittedByProject = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.ProjectControls);
                    emailModel.SubmittedBy = submittedByProject.Firstname + " " + submittedByProject.Lastname;
                    break;
                case (int)JobRequestStatus.ProjectManager:
                    var managerRepresentative = jobRequestEntity.KeyPersonnel.AccountingRepresentative;
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
                    var submittedByManager = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.ProjectManager);
                    emailModel.SubmittedBy = submittedByManager.Firstname + " " + submittedByManager.Lastname;
                    break;
                case (int)JobRequestStatus.Accounting:
                    var accountUser = _userService.GetUserByUserGuid(notifyTo);
                    if (accountUser != null)
                    {
                        //emailTo = accountUser.WorkEmail;
                        recipientName = accountUser.DisplayName;
                        emailModel.ReceipentName = recipientName;
                        subject = "Job Request has been set to done contract: " + emailModel.ContractNumber;
                    }
                    var submittedByAccount = _userService.GetUserByUserGuid(jobRequestEntity.KeyPersonnel.AccountingRepresentative);
                    emailModel.SubmittedBy = submittedByAccount.Firstname + " " + submittedByAccount.Lastname;
                    emailModel.Status = "Done";
                    break;
                default:
                    break;
            }

            content = GetContentForJobRequestNotify(template, keyPersonnel, emailModel);
            //_emailSender.SendEmailAsync(emailTo, recipientName, subject, content);

            var messageModel = new EmailMessageModel();
            messageModel.Message = content;
            messageModel.WorkEmail = emailTo;
            messageModel.Displayname = recipientName;
            messageModel.Subjects = subject;
            return messageModel;
        }

        private string GetContentForJobRequestNotify(string template, IEnumerable<ContractUserRole> keyPersonnelList, JobRequestEmailModel emailModel)
        {
            StringBuilder content = new StringBuilder(template);
            var keyList = "<ul>";
            foreach (var person in keyPersonnelList)
            {
                keyList += "<li>" + person.User.DisplayName + " (" + person.UserRole + ")" + "</li>";
            }
            keyList += "</li>";

            content.Replace("<<receiver-displayName>>", emailModel.ReceipentName);
            content.Replace("<<submitted-displayName>>", emailModel.SubmittedBy);
            content.Replace("<<contract-number>>", emailModel.ContractNumber);
            content.Replace("<<awarding-agency>>", emailModel.AwardingAgency);
            content.Replace("<<funding-agency>>", emailModel.FundingAgency);
            content.Replace("<<task-orderNumber>>", emailModel.TaskOrderNumber);
            content.Replace("<<project-number>>", emailModel.ProjectNumber);
            content.Replace("<<submitted-by>>", emailModel.SubmittedBy);
            content.Replace("<<status>>", emailModel.Status);
            content.Replace("<<key-personnelList>>", keyList);
            content.Replace("<<jobrequest-Url>>", emailModel.ClickableUrl);
            content.Replace("<<notify-to>>", emailModel.NotifiedTo);
            content.Replace("<<notify-other>>", emailModel.NotifyOther);
            return content.ToString();
        }
    }
}
