using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Data
{
    public class NotificationTemplatesRepository : INotificationTemplatesRepository
    {
        public IDatabaseContext _context;

        public NotificationTemplatesRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public NotificationTemplate GetByKey(string key)
        {
            string sql = $@"select  * from NotificationTemplate where keys = @key";
            var result = _context.Connection.QueryFirstOrDefault<NotificationTemplate>(sql, new { key = key });
            return result;
        }

        public IEnumerable<EmailMessageModel> GetUsersForEmail(Guid moduleGuid)
        {
            string sql = $@"select  users.WorkEmail,
                                    users.Displayname,
                                    NotificationMessage.NotificationMessageGuid,
                                    NotificationTemplates.Message,
                                    NotificationTemplates.Status,
                                    NotificationTemplates.Subjects 
                                    from NotificationBatch , NotificationMessage ,NotificationTemplates,Users
                                    where NotificationBatch.NotificationGuid = NotificationMessage.NotificationBatchGuid
                                    and NotificationTemplates.Keys = CONCAT(NotificationBatch.ResourceType,NotificationBatch.ResourceAction) 
                                    and NotificationBatch.ModuleGuid = @moduleGuid
                                    and users.UserGuid =  NotificationMessage.UserGuid
                                    and NotificationMessage.Status=0";
            var result = _context.Connection.Query<EmailMessageModel>(sql, new { moduleGuid = moduleGuid });
            return result;
        }

        public bool Add(NotificationTemplate model)
        {
            var sql = @"INSERT INTO NotificationTemplate (NotificationTemplateGuid
                                                   ,Keys
                                                   ,NotificationTypeGuid
                                                   ,Subject
                                                   ,Message
                                                   ,IsActive
                                                   ,Priority
                                                   ,IsRecurring
                                                   ,RecurringInterval
                                                   ,CreatedOn
                                                   ,CreatedBy )
                                           VALUES
                                                   (@NotificationTemplateGuid
                                                    ,@Keys
                                                    ,@NotificationTypeGuid
                                                    ,@Subject
                                                    ,@Message
                                                    ,@IsActive
                                                    ,@Priority
                                                    ,@IsRecurring
                                                    ,@RecurringInterval
                                                    ,@CreatedOn
                                                    ,@CreatedBy )";
            _context.Connection.Execute(sql, model);
            return true;
        }

        public NotificationTemplate GetNotificationTemplateAsResource(NotificationTemplatesDetail notificationTemplatesDetail, string resourcekey)
        {
            var notificationTemplate = GetByKey(resourcekey);
            string revenueRecognition = (EnumGlobal.ResourceType.RevenueRecognition.ToString()).ToUpper();
            string contractClose = (EnumGlobal.ResourceType.ContractCloseOut.ToString()).ToUpper();
            var key = resourcekey.Split('.')[0].ToUpper();
            if (key == revenueRecognition)
            {
                string message = revenueEmailtemplates(notificationTemplatesDetail, notificationTemplate);

                notificationTemplate.Message = message;
                StringBuilder content = new StringBuilder(notificationTemplate.Subject);
                notificationTemplate.Subject = content.Replace("{PROJECT_NUMBER}", notificationTemplatesDetail.ProjectNumber).ToString();
                return notificationTemplate;
            }
            else if (key == contractClose)
            {
                string message = contractCloseEmailtemplates(notificationTemplatesDetail, notificationTemplate);

                notificationTemplate.Message = message;
                StringBuilder content = new StringBuilder(notificationTemplate.Subject);
                content.Replace("{PROJECT_NUMBER}", notificationTemplatesDetail.ProjectNumber).ToString();
                content.Replace("{CONTRACT_NUMBER}", notificationTemplatesDetail.ContractNumber).ToString();
                content.Replace("{CONTRACT_CONTRACT_TYPE}", notificationTemplatesDetail.ContractType).ToString();
                content.Replace("{SUBMITTED_NAME}", notificationTemplatesDetail.SubmittedByName);
                content.Replace("{CONTRACT_DESCRIPTION}", notificationTemplatesDetail.Description);
                content.Replace("{CONTRACT_TITLE}", notificationTemplatesDetail.ContractTitle);
                notificationTemplate.Subject = content.ToString();
                return notificationTemplate;
            }
            return null;
        }

        private string revenueEmailtemplates(NotificationTemplatesDetail detail, NotificationTemplate notificationTemplate)
        {
            var additionalMsg = string.IsNullOrEmpty(detail.AdditionalMessage) ? "No additional message was added" : detail.AdditionalMessage;
            StringBuilder content = new StringBuilder(notificationTemplate.Message);
            content.Replace("{REQUESTEDBY_NAME}", detail.ReceiverDisplayName);
            content.Replace("{CONTRACT_NUMBER}", detail.ContractNumber);
            content.Replace("{PROJECT_NUMBER}", detail.ProjectNumber);
            content.Replace("{TITLE}", detail.Title);
            content.Replace("{CONTRACT_NUMBER}", detail.ContractNumber);
            content.Replace("{CONTRACT_TYPE}", detail.ContractType);
            content.Replace("{TASKORDER_NUMBER}", detail.TaskOrderNumber);
            content.Replace("{TASKORDER_TITLE}", detail.ContractTitle);
            content.Replace("{ThresholdAward_Amount}", detail.ThresholdAmount.ToString());
            content.Replace("{THRESHOLD_AMOUNT}", detail.ThresholdAmount.ToString());
            content.Replace("{MOD_NUMBER}", detail.ModNumber);
            content.Replace("{NOTIFY_OTHER}", detail.NotifyOther);
            content.Replace("{ADDITIONAL_MESSAGE}", additionalMsg);
            content.Replace("{CONTRACT_TITLE}", detail.ContractTitle);
            content.Replace("{Redirect_Url}", detail.RedirectUrlPath);
            content.Replace("{REDIRECT_URL}", detail.RedirectUrlPath);
            content.Replace("{CONTRACT_DESCRIPTION}", detail.Description);
            return content.ToString();
        }

        private string contractCloseEmailtemplates(NotificationTemplatesDetail detail, NotificationTemplate notificationTemplate)
        {
            var additionalMsg = string.IsNullOrEmpty(detail.AdditionalMessage) ? "No additional message was added" : detail.AdditionalMessage;
            StringBuilder content = new StringBuilder(notificationTemplate.Message);
            content.Replace("{REQUESTEDBY_NAME}", detail.ReceiverDisplayName);
            content.Replace("{CONTRACT_NUMBER}", detail.ContractNumber);
            content.Replace("{PROJECT_NUMBER}", detail.ProjectNumber);
            content.Replace("{TITLE}", detail.Title);
            content.Replace("{CONTRACT_TYPE}", detail.ContractType);
            content.Replace("{CONTRACT_TITLE}", detail.ContractTitle);
            content.Replace("{Redirect_Url}", detail.RedirectUrlPath);
            content.Replace("{REDIRECT_URL}", detail.RedirectUrlPath);
            content.Replace("{SUBMITTED_NAME}", detail.SubmittedByName);
            content.Replace("{ADDITIONAL_MESSAGE}", additionalMsg);
            content.Replace("{NOTIFY_OTHER}", detail.NotifyOther);
            content.Replace("{CONTRACT_DESCRIPTION}", detail.Description);
            return content.ToString();
        }

        public NotificationTemplate GetNotificationTemplateByKey(string resourcekey)
        {
            var notificationTemplate = GetByKey(resourcekey);
            return notificationTemplate;
        }
    }
}
