using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.Sync
{
    public interface ISyncStatusRepository
    {
        IEnumerable<SyncStatus> GetSyncStatusBySyncBatchGUID(Guid guid);
        void Insert(SyncStatus syncStatus);

    }
}
