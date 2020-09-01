using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface INotificationTypeService
    {
        NotificationType GetByNotificationTypeName(string notificationTypeName);
    }
}
