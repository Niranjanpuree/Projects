using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface INotificationMessageService
    {
        bool Add(NotificationMessage model);
        IEnumerable<NotificationMessage> GetByNotificationBatchId(Guid notificationBatchGuid);
        bool GetByNotificationBatchIdAndUserId(Guid notificationBatchGuid,Guid userGuid);
        IEnumerable<NotificationMessage> GetByNotificationBatchIdAndNotificationType(Guid notificationBatchGuid,string notificationTypeName);
        IEnumerable<NotificationMessage> GetDesktopNotification(Guid notificationBatchGuid,string searchValue);
        NotificationMessage GetLatestDesktopNotification(Guid notificationBatchGuid);
        NotificationMessage GetByNotificationMessageId(Guid notificationMessageGuid);
        IEnumerable<NotificationMessage> GetUnReadDesktopNotification(Guid notificationBatchGuid);
        int EditUserResponseByNotificationMessageId(Guid notificationMessageGuid);
    }
}
