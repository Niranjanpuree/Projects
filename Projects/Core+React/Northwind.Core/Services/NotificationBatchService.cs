using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;

namespace Northwind.Core.Services
{
    public class NotificationBatchService : INotificationBatchService
    {
        private readonly INotificationBatchRepository _notificationBatchRepository;
        public NotificationBatchService(INotificationBatchRepository notificationBatchRepository)
        {
            _notificationBatchRepository = notificationBatchRepository;
        }
        public Guid Add(NotificationBatch model)
        {
            return _notificationBatchRepository.Add(model);
        }

        public NotificationBatch GetByResourceId(Guid resourceId)
        {
           return _notificationBatchRepository.GetByResourceId(resourceId);
        }

        public Guid GetExistsGuid(Guid notificationTemplatesGuid, Guid resourceId)
        {
            return _notificationBatchRepository.GetExistsGuid(notificationTemplatesGuid,resourceId);
        }

        //public Guid GetResourceIdByNotificationMessageUserGuid(Guid userGuid)
        //{
        //    return _notificationBatchRepository.GetResourceIdByNotificationMessageUserGuid(userGuid);
        //}
    }
}
