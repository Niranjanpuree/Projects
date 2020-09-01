using System;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class NotificationMessageService : INotificationMessageService
    {
        private readonly INotificationMessageRepository _notificationMessageRepository;
        public NotificationMessageService(INotificationMessageRepository notificationMessageRepository)
        {
            _notificationMessageRepository = notificationMessageRepository;
        }
        public bool Add(NotificationMessage model)
        {
            return _notificationMessageRepository.Add(model);
        }
        public IEnumerable<NotificationMessage> GetByNotificationBatchId(Guid notificationBatchGuid)
        {
            return _notificationMessageRepository.GetByNotificationBatchId(notificationBatchGuid);
        }

        public IEnumerable<NotificationMessage> GetByNotificationBatchIdAndNotificationType(Guid notificationBatchGuid, string notificationTypeName)
        {
            return _notificationMessageRepository.GetByNotificationBatchIdAndNotificationType(notificationBatchGuid, notificationTypeName);
        }

        public IEnumerable<NotificationMessage> GetDesktopNotification(Guid userGuid, string searchValue)
        {
            return _notificationMessageRepository.GetDesktopNotification(userGuid, searchValue);
        }

        public NotificationMessage GetLatestDesktopNotification(Guid userGuid)
        {
            return _notificationMessageRepository.GetLatestDesktopNotification(userGuid);
        }

        public NotificationMessage GetByNotificationMessageId(Guid notificationMessageGuid)
        {
            return _notificationMessageRepository.GetByNotificationMessageId(notificationMessageGuid);
        }

        public IEnumerable<NotificationMessage> GetUnReadDesktopNotification(Guid userGuid)
        {
            return _notificationMessageRepository.GetUnReadGetDesktopNotification(userGuid);
        }

        public int EditUserResponseByNotificationMessageId(Guid notificationMessageGuid)
        {
            return _notificationMessageRepository.EditUserResponseByNotificationMessageId(notificationMessageGuid);
        }

        public bool GetByNotificationBatchIdAndUserId(Guid notificationBatchGuid, Guid userGuid)
        {
            return _notificationMessageRepository.GetByNotificationBatchIdAndUserId(notificationBatchGuid, userGuid);
        }
    }
}
