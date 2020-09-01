using Northwind.Core.Entities;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services.SyncServices
{
    public class SyncStatusService : ISyncStatusService
    {
        private readonly ISyncStatusRepository _syncStatusRepository;
        public SyncStatusService(ISyncStatusRepository syncStatusRepository)
        {
            _syncStatusRepository = syncStatusRepository;
        }

        public IEnumerable<SyncStatus> GetSyncStatusBySyncBatchGUID(Guid guid)
        {
            return _syncStatusRepository.GetSyncStatusBySyncBatchGUID(guid);
        }

        public void Insert(SyncStatus syncStatus)
        {
            _syncStatusRepository.Insert(syncStatus);
        }
    }
}
