using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Web.Infrastructure.Models
{
    public class EventLog
    {
        public Guid EventGuid { get; set; }
        public string Application { get; set; }
        public DateTime EventDate { get; set; }
        public Guid UserGuid { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string UserName { get; set; }
        public Exception InnerException { get; set; }
    }
}
