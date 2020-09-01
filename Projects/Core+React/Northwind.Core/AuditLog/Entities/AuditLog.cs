using System;

namespace Northwind.Core.AuditLog.Entities
{
    public class AuditLog
    {
        public Guid AuditLogGuid { get; set; }
        public string RawData { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Resource { get; set; }
        public Guid ResourceId { get; set; }
        public string Actor { get; set; }
        public Guid ActorId { get; set; }
        public string IpAddress { get; set; }
        public string Action { get; set; }
        public Guid ActionId { get; set; }
        public string ActionResult { get; set; }
        public string ActionResultReason { get; set; }
        public string AdditionalInformation { get; set; }
        public string AdditionalInformationURl { get; set; }
    }
}
