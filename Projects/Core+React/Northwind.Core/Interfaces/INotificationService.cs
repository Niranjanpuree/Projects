using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;

namespace Northwind.Core.Interfaces
{
    public interface INotificationService
    {
        bool NotifyUsers(Guid notificationBatchGuid);
    }
}
