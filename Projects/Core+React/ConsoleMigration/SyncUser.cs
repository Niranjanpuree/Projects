using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMigration
{
    public class SyncUser
    {
        private readonly IUserSyncService _userSyncService;
        private readonly IADGroupSyncService _groupSyncService;
        private readonly IActiveDirectoryContext _adContext;
        private readonly ISyncBatchService _syncBatchService;

        public SyncUser(IUserSyncService userSyncService, IADGroupSyncService groupSyncService, 
            ISyncBatchService syncBatchService, IActiveDirectoryContext adContext)
        {
            _userSyncService = userSyncService;
            _groupSyncService = groupSyncService;
            _syncBatchService = syncBatchService;
            _adContext = adContext;
        }
        public void SyncUserFromActiveDirectory()
        {
            var syncBatch = new SyncBatch();
            syncBatch.BatchStart = DateTime.Now;
            Guid batchGuid = _syncBatchService.Insert(syncBatch);

            _userSyncService.SyncUsersFromActiveDirectory(_adContext, batchGuid);
        }


    }
}
