using System.Collections.Generic;
using Dapper;
using Northwind.CostPoint.Interfaces;
using Northwind.CostPoint.Entities;
using Northwind.Core.Models;
using System;
using Northwind.Costpoint.Entities;
using System.Linq;

namespace Northwind.Costpoint.Data
{
    public class PORepositoryCP : IPORepositoryCP
    {
        IPFSDBContext _context;
        public PORepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public POCP GetById(long Id)
        {
            var sql = "select * from Costs Where POCommitmentId=@Id";
            var result = _context.Connection.Query<POCP>(sql, new { Id });
            if (result.AsList().Count > 0)
                return result.AsList<POCP>()[0];
            else
                return null;
        }

        public IEnumerable<POCP> GetPO(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var orderBy = queryBuilder.OrderBy(searchSpec);
            var offSet = queryBuilder.OffsetQuery(searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select * from PO {where} {orderBy} {offSet}";
            return _context.Connection.Query<POCP>(sql, parameters);
        }

        public int GetCount(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select count(*) from PO {where}";
            return _context.Connection.ExecuteScalar<int>(sql, parameters);
        }

        private string WhereAnd(string rawSqlAndConditions, SearchSpecCP searchSpec)
        {
            string searchParam = "";
            if (!string.IsNullOrEmpty(searchSpec.SearchText))
            {
                searchParam = " AND ( VendorName LIKE @searchValue or po_id LIKE @searchValue or PaymentTerms LIKE @searchValue)";
            }
            var str = $@" WHERE projectNumber = @projectNumber {searchParam}
                      {rawSqlAndConditions}";
            return str;
        }
    }
}
