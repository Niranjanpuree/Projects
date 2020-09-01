using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class NotificationTemplate
    {
        public Guid NotificationTemplateGuid { get; set; }
        public string Keys { get; set; }
        public Guid NotificationTypeGuid { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public bool Priority { get; set; }
        public bool IsRecurring { get; set; }
        public bool UserInteraction { get; set; }
        public int RecurringInterval { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
    public class NotificationTemplatesDetail
    {
        public string ReceiverDisplayName { get; set; }
        public string ResourceName { get; set; }
        public string ResourceTitle { get; set; }
        public string SubmittedByName { get; set; }
        public string RedirectUrlPath { get; set; }
        public string ContractType { get; set; }
        public string ContractTitle { get; set; }
        public string Title { get; set; }
        public string ContractNumber { get; set; }
        public string TaskOrderNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string ModNumber { get; set; }
        public decimal ThresholdAmount { get; set; }
        public string NotifyOther { get; set; }
        public string AdditionalMessage { get; set; }
        public string AwardingAgency { get; set; }
        public string FundingAgency { get; set; }
        public string Status { get; set; }
        public string AdditionalUser { get; set; }
        public string Description { get; set; }
    }
}
