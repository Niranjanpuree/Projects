using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface INotificationBatchService
    {
        Guid Add(NotificationBatch model);
        NotificationBatch GetByResourceId(Guid resourceId);
        Guid GetExistsGuid(Guid notificationTemplatesGuid, Guid resourceId);

    }
}
