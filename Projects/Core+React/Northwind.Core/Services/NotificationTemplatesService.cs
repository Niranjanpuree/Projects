using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;

namespace Northwind.Core.Services
{
    public class NotificationTemplateService : INotificationTemplatesService
    {
        private readonly INotificationTemplatesRepository _emailTemplatesRepository;
        public NotificationTemplateService(INotificationTemplatesRepository emailTemplatesRepository)
        {
            _emailTemplatesRepository = emailTemplatesRepository;
        }

        public bool Add(NotificationTemplate model)
        {
            return _emailTemplatesRepository.Add(model);
        }

        public NotificationTemplate GetByKey(string key)
        {
            return _emailTemplatesRepository.GetByKey(key);
        }

        public IEnumerable<EmailMessageModel> GetUsersForEmail(Guid moduleGuid)
        {
            return _emailTemplatesRepository.GetUsersForEmail(moduleGuid);
        }

        public NotificationTemplate GetNotificationTemplateAsResource(NotificationTemplatesDetail notificationTemplatesDetail, string resourcekey)
        {
            return _emailTemplatesRepository.GetNotificationTemplateAsResource(notificationTemplatesDetail,resourcekey);
        }

        public NotificationTemplate GetNotificationTemplateByKey(string resourcekey)
        {
            return _emailTemplatesRepository.GetNotificationTemplateByKey(resourcekey);
        }
    }
}
