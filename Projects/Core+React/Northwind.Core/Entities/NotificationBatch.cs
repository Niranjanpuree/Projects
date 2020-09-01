using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class NotificationBatch
    {
        public Guid NotificationBatchGuid { get; set; }
        public Guid NotificationTemplateGuid { get; set; }
        public Guid ResourceId { get; set; }
        public string ResourceType { get; set; }
        public string ResourceAction { get; set; }
        public DateTime StartDate { get; set; }
        public string AdditionalMessage { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
