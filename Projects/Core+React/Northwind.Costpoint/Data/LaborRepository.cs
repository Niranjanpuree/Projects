using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.Costpoint.Interfaces;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;

namespace Northwind.Costpoint.Data
{
    public class LaborRepositoryCP : ILaborRepositoryCP
    {
        private readonly IPFSDBContext _context;
        public LaborRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public IEnumerable<LaborCP> GetLabor(SearchSpecCP searchSpec)
        {
           var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
           var where = WhereAnd(queryBuilder.RawSql,searchSpec);
           var orderBy = queryBuilder.OrderBy(searchSpec);
           var offSet = queryBuilder.OffsetQuery(searchSpec);
           var parameters = (DynamicParameters)queryBuilder.Parameters;
           parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
           parameters.Add("searchValue", "%"+searchSpec.SearchText+"%");
           var sql = $"select * from Labor {where} {orderBy} {offSet}"; 
           return _context.Connection.Query<LaborCP>(sql, parameters);

        }

        private string WhereAnd(string rawSqlAndConditions, SearchSpecCP searchSpec)
        {
            string searchParam = "";
            if (!string.IsNullOrEmpty(searchSpec.SearchText))
            {
                searchParam = " AND ( Name LIKE @searchValue or Wbs LIKE @searchValue)";
            }
            var str = $@" WHERE projectNumber = @projectNumber {searchParam}
                      {rawSqlAndConditions}";
            return str;
        }

        public int GetLaborCount(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select count(*) from Labor {where}";
            return _context.Connection.ExecuteScalar<int>(sql, parameters);
        }

        public IEnumerable<ChartModel> GetLaborForPieChart(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select Title as Name, Sum(TotalAmount) as Amount from Labor {where} Group by Title ";
            return _context.Connection.Query<ChartModel>(sql, parameters);
        }

        public IEnumerable<ChartModel> GetLaborForBarChart(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select Right('0'+Cast(Month(TransactionDate) as Varchar),2) as Name, Sum(TotalAmount) as Amount from Labor {where} Group By TransactionDate";
            return _context.Connection.Query<ChartModel>(sql, parameters);
        }
    }
}
