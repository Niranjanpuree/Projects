using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.Sync
{
    public interface ISyncBatchService
    {
        SyncBatch GetSyncBatchByGUID(Guid guid);
        IEnumerable<Group> GetLastBatch(Guid BatchGUID);
        Guid Insert(SyncBatch syncBatch);
        void Update(SyncBatch syncBatch);
    }
}
