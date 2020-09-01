using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.QustionaireContractClose
{
    public class ContractCloseApprovalRepository : IContractCloseApprovalRepository
    {
        IDatabaseContext _context;

        public ContractCloseApprovalRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public ContractCloseApproval GetByNormalizedValue(string normalizedValue)
        {
            string sql = $@"SELECT *
                            from ContractCloseApproval
                            where IsActive = 1
                            and  NormalizedValue= @NormalizedValue";
            var result = _context.Connection.QueryFirstOrDefault<ContractCloseApproval>(sql, new { NormalizedValue = normalizedValue });
            return result;
        }
    }
}
