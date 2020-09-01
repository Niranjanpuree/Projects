using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.Sync
{
    public class SyncBatchRepository: ISyncBatchRepository
    {
        public IDatabaseContext _context;

        public SyncBatchRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public SyncBatch GetSyncBatchByGUID(Guid guid)
        {
            return _context.Connection.QueryFirstOrDefault<SyncBatch>(@"SELECT
            [SyncBatchGUID]
           ,[BatchStart]
           ,[BatchEnd]
           FROM [SyncBatch] WHERE [SyncBatchGUID] = @SyncBatchGUID", new { SyncBatchGUID = guid });
        }

        public IEnumerable<Group> GetLastBatch(Guid BatchGUID)
        {
            return _context.Connection.Query<Group>(@"SELECT Top 1
            [SyncBatchGUID]
           ,[BatchStart]
           ,[BatchEnd]
           FROM [SyncBatch] ORDER BY [BatchStart] DESC");
        }

        public Guid Insert(SyncBatch syncBatch)
        {
            var guid = Guid.NewGuid();
            _context.Connection.Execute(@"INSERT INTO[SyncBatch]
               ([SyncBatchGUID]
               ,[BatchStart])
               VALUES
               (@SyncBatchGUID
               , @BatchStart)",
                new
                {
                    SyncBatchGUID = guid,
                    syncBatch.BatchStart
                }
           );
            return guid;
        }

        public void Update(SyncBatch syncBatch)
        {
            _context.Connection.Execute(@"UPDATE [SyncBatch]
                    SET [BatchStart] = @BatchStart
                        ,[BatchEnd] = @BatchEnd                       
                    WHERE SyncBatchGUID=@SyncBatchGUID",
                new
                {
                    syncBatch.BatchStart,
                    syncBatch.BatchEnd,
                    syncBatch.SyncBatchGUID
                }
           );
        }
    }
}
