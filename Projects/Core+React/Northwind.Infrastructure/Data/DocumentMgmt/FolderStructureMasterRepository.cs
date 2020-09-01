using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data.DocumentMgmt
{
    public class FolderStructureMasterRepository : IFolderStructureMasterRepository
    {
        IDatabaseSingletonContext _context;

        public FolderStructureMasterRepository(IDatabaseSingletonContext context)
        {
            _context = context;
        }

        public IEnumerable<FolderStructureMaster> GetActive(string module, string resourceType)
        {
            var sql = $@"select * from FolderStructureMaster Where module=@Module AND ResourceType=@ResourceType and status=1";
            return _context.Connection.Query<FolderStructureMaster>(sql, new { Module = module, ResourceType = resourceType });
        }

        public IEnumerable<FolderStructureMaster> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
