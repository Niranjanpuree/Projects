using System;

namespace Northwind.Core.AuditLog.Entities
{
    public class EventLogs
    {
        public Guid EventGuid { get; set; }
        public string Application { get; set; }
        public DateTime EventDate { get; set; }
        public string UserName { get; set; }
        public Guid UserGuid { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
       
    }
}
