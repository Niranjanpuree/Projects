using Northwind.Core.Models;
using Dapper;
using System.Collections.Generic;
using System;
using Northwind.CostPoint.Interfaces;
using Northwind.CostPoint.Entities;
using Northwind.Costpoint.Entities;
using System.Linq;

namespace Northwind.Costpoint.Data
{
    public class WbsRepositoryCP : IWbsRepositoryCP
    {
        IPFSDBContext _context;
        public WbsRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public WbsCP GetById(Guid Id)
        {
            var sql = "select * from Wbs Where WbsGuid=@Id";
            var result = _context.Connection.Query<WbsCP>(sql, new { Id = Id });
            if (result.AsList().Count > 0)
                return result.AsList<WbsCP>()[0];
            else
                return null;
        }

        public IEnumerable<WbsCP> GetWbs(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var orderBy = queryBuilder.OrderBy(searchSpec);
            var offSet = queryBuilder.OffsetQuery(searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select * from Wbs {where} {orderBy} {offSet}";
            return _context.Connection.Query<WbsCP>(sql, parameters);
        }

        public int GetCount(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var orderBy = queryBuilder.OrderBy(searchSpec);
            var offSet = queryBuilder.OffsetQuery(searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select Count(1) from Wbs {where}";
            return _context.Connection.ExecuteScalar<int>(sql, parameters);
        }
        
        private string WhereAnd(string rawSqlAndConditions, SearchSpecCP searchSpec)
        {
            string searchParam = "";
            if (!string.IsNullOrEmpty(searchSpec.SearchText))
            {
                searchParam = " AND ( Wbs LIKE @searchValue or Description LIKE @searchValue)";
            }
            var str = "";
            if(searchSpec.ProjectNumber != null)
            {
                str = $@" WHERE projectNumber = @projectNumber {searchParam}
                      {rawSqlAndConditions}";
            }
            else
            {
                str = $@" WHERE 1=1 {searchParam}
                      {rawSqlAndConditions}";
            }
            
            return str;
        }
    }
}
