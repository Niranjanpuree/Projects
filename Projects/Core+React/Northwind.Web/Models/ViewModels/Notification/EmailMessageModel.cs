using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Web.Models.ViewModels
{
    public class EmailMessageModel
    {
        public Guid NotificationMessageGuid { get; set; }
        public string WorkEmail { get; set; }
        public string Displayname { get; set; }
        public string Message { get; set; }
        public string Subjects { get; set; }
        public bool Status { get; set; }
    }
}
