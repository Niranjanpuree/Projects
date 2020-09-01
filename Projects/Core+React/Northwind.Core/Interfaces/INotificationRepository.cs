using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface INotificationRepository
    {
        Guid InsertUserNotification(UserNotification model);
        bool InsertUserNotificationMessage(UserNotificationMessage model);
    }
}
