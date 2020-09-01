using Northwind.Core.Entities;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services.SyncServices
{
    public class SyncBatchService : ISyncBatchService
    {
        private readonly ISyncBatchRepository _syncBatchRepository;
        public SyncBatchService(ISyncBatchRepository syncBatchRepository)
        {
            _syncBatchRepository = syncBatchRepository;
        }

        public IEnumerable<Group> GetLastBatch(Guid BatchGUID)
        {
            throw new NotImplementedException();
        }

        public SyncBatch GetSyncBatchByGUID(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Guid Insert(SyncBatch syncBatch)
        {
            return _syncBatchRepository.Insert(syncBatch);
        }

        public void Update(SyncBatch syncBatch)
        {
            throw new NotImplementedException();
        }
    }
}
