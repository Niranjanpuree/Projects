using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class GenericNotification
    {
        public class GenericNotificationViewModel
        {
            public string AdditionalNotes { get; set; }
            public Guid ResourceId { get; set; }
            public string NotificationTemplateKey { get; set; }
            public DateTime CurrentDate { get; set; }
            public Guid CurrentUserGuid { get; set; }
            public IEnumerable<User> IndividualRecipients { get; set; }
            public string RedirectUrl { get; set; }
            public NotificationTemplatesDetail NotificationTemplatesDetail { get; set; }
            public bool SendEmail { get; set; }
        }
    }
}
