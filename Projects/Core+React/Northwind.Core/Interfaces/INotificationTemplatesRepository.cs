using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;

namespace Northwind.Core.Interfaces
{
    public interface INotificationTemplatesRepository
    {
        bool Add(NotificationTemplate model);
        NotificationTemplate GetByKey(string key);
        IEnumerable<EmailMessageModel> GetUsersForEmail(Guid moduleGuid);
        NotificationTemplate GetNotificationTemplateAsResource(NotificationTemplatesDetail notificationTemplatesDetail,string resourcekey);
        NotificationTemplate GetNotificationTemplateByKey(string resourcekey);
    }
}
