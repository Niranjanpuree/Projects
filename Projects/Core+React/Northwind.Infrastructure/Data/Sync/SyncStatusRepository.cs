using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.Sync
{
    public class SyncStatusRepository: ISyncStatusRepository
    {
        public IDatabaseContext _context;

        public SyncStatusRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<SyncStatus> GetSyncStatusBySyncBatchGUID(Guid guid)
        {
            return _context.Connection.Query<SyncStatus>(@"SELECT
             [SyncGUID]
              ,[SyncBatchGUID]
              ,[SyncStatusText]
              ,[ErrorMessage]
              ,[ObjectType]
              ,[ObjectName]
              ,[ObjectGUID]
           FROM [SyncStatus] WHERE [SyncBatchGUID] = @SyncBatchGUID", new { SyncBatchGUID = guid });
        }

        public void Insert(SyncStatus syncStatus)
        {
            syncStatus.SyncGUID = Guid.NewGuid();
            _context.Connection.Execute(@"INSERT INTO[SyncStatus]
               ([SyncGUID]
              ,[SyncBatchGUID]
              ,[SyncStatusText]
              ,[ErrorMessage]
              ,[ObjectType]
              ,[ObjectName]
              ,[ObjectGUID])
               VALUES
               ( @SyncGUID
               , @SyncBatchGUID
               , @SyncStatusText
               , @ErrorMessage
               , @ObjectType
               , @ObjectName
               , @ObjectGUID)",
                new
                {
                    syncStatus.SyncGUID,
                    syncStatus.SyncBatchGUID,
                    syncStatus.SyncStatusText,
                    syncStatus.ErrorMessage,
                    syncStatus.ObjectType,
                    syncStatus.ObjectName,
                    syncStatus.ObjectGUID
                }
           );
        }
    }
}
