using System;
using Dapper;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
namespace Northwind.Infrastructure.Data
{
    public class ContractResourceFileRepository : IContractResourceFileRepository
    {
        private readonly IDatabaseContext _context;
        public ContractResourceFileRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public ContractResourceFile GetFilePathByResourceIdAndKeys(string resourceKey, Guid resourceId)
        {
            string sql = @"select *  from ContractResourceFile where Keys = @Keys and resourceGuid = @ResourceId and isFile = 0";
            var result = _context.Connection.QueryFirstOrDefault<ContractResourceFile>(sql, new { Keys = resourceKey, ResourceId = resourceId });
            return result;
        }
    }
}
