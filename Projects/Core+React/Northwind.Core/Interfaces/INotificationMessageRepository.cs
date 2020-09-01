using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface INotificationMessageRepository
    {
        bool Add(NotificationMessage model);
        IEnumerable<NotificationMessage> GetByNotificationBatchId(Guid notificationBatchGuid);
        IEnumerable<NotificationMessage> GetByNotificationBatchIdAndNotificationType(Guid notificationBatchGuid, string notificationTypeName);
        IEnumerable<NotificationMessage> GetDesktopNotification(Guid userGuid, string searchValue);
        NotificationMessage GetByNotificationMessageId(Guid notificationMessageGuid);

        NotificationMessage GetLatestDesktopNotification(Guid userGuid);
        IEnumerable<NotificationMessage> GetUnReadGetDesktopNotification(Guid userGuid);
        int EditUserResponseByNotificationMessageId(Guid notificationMessageGuid);
        bool GetByNotificationBatchIdAndUserId(Guid notificationBatchGuid, Guid userGuid);
    }
}
