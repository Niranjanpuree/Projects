using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Web.Infrastructure.Models.ViewModels
{
    public class NotificationMessageViewModel
    {
        public Guid NotificationMessageGuid { get; set; }
        public Guid NotificationBatchGuid { get; set; }
        public Guid DistributionListGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string AdditionalMessage { get; set; }
        public bool Status { get; set; }
        public bool UserResponse { get; set; }
        public DateTime NextAction { get; set; }
        public Guid CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnFormatDateTime { get; set; }
    }
}
