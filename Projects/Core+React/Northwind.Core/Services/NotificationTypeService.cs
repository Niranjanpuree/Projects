using System;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class NotificationTypeService : INotificationTypeService
    {
        private INotificationTypeRepository _notificationTypeRepository;

        public NotificationTypeService(INotificationTypeRepository notificationTypeRepository)
        {
            _notificationTypeRepository = notificationTypeRepository;
        }
        public NotificationType GetByNotificationTypeName(string notificationTypeName)
        {
            return _notificationTypeRepository.GetByNotificationTypeName(notificationTypeName);
        }
    }
}
