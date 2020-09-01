using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;
using static Northwind.Core.Entities.GenericNotification;

namespace Northwind.Core.Interfaces
{
    public interface IGenericNotificationService
    {
        bool AddNotificationMessage(GenericNotificationViewModel notificationViewModel);
    }
}
