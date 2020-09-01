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
    public class CostRepositoryCP : ICostRepositoryCP
    {
        IPFSDBContext _context;
        public CostRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public CostCP GetById(long Id)
        {
            var sql = "select * from Cost Where CostId=@Id";
            var result = _context.Connection.Query<CostCP>(sql, new { Id });
            if (result.AsList().Count > 0)
                return result.AsList<CostCP>()[0];
            else
                return null;
        }

        public IEnumerable<CostCP> GetCosts(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            string searchParam = "";
            string searchString = "";
            if (startDate.HasValue && endDate.HasValue)
            {
                int stYear = startDate.Value.Year;
                int stMonth = startDate.Value.Month;
                int enYear = endDate.Value.Year;
                int enMonth = endDate.Value.Month;
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "FiscalYear", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stYear, Value2 = enYear });
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "Period", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stMonth, Value2 = enMonth });
                
            }
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }
            var where = selector.RawSql.Replace("WHERE", " AND ");
            var parameters = (Dapper.DynamicParameters)selector.Parameters;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND ( Description LIKE @searchValue or VendorName LIKE @searchValue or PO_ID like @searchValue)";
            }
            parameters.Add("ProjectNumber", projectNumber);
            parameters.Add("searchValue", searchString);

            var sql = $"select * from Cost WHERE projectNumber=@projectNumber {searchParam} {where} ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<CostCP>(sql, parameters);
        }

        public IEnumerable<dynamic> GetCostForPieChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            string searchParam = "";
            string searchString = "";
            if (startDate.HasValue && endDate.HasValue)
            {
                int stYear = startDate.Value.Year;
                int stMonth = startDate.Value.Month;
                int enYear = endDate.Value.Year;
                int enMonth = endDate.Value.Month;
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "FiscalYear", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stYear, Value2 = enYear });
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "Period", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stMonth, Value2 = enMonth });
            }
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }
            var where = selector.RawSql.Replace("WHERE", " AND ");
            var parameters = (Dapper.DynamicParameters)selector.Parameters;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND ( Description LIKE @searchValue or VendorName LIKE @searchValue or PO_ID like @searchValue)";
            }
            parameters.Add("ProjectNumber", projectNumber);
            parameters.Add("searchValue", searchString);

            var sql = $"select Description, Sum(Amount) as Amount from Cost WHERE projectNumber=@projectNumber {searchParam} {where} GROUP BY Description ORDER BY Description";
            return _context.Connection.Query<CostCP>(sql, parameters);
        }

        public IEnumerable<dynamic> GetCostForBarChart(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            string searchParam = "";
            string searchString = "";
            if (startDate.HasValue && endDate.HasValue)
            {
                int stYear = startDate.Value.Year;
                int stMonth = startDate.Value.Month;
                int enYear = endDate.Value.Year;
                int enMonth = endDate.Value.Month;
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "FiscalYear", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stYear, Value2 = enYear });
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "Period", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stMonth, Value2 = enMonth });
            }
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }
            var where = selector.RawSql.Replace("WHERE", " AND ");
            var parameters = (Dapper.DynamicParameters)selector.Parameters;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND ( Description LIKE @searchValue or VendorName LIKE @searchValue or PO_ID like @searchValue)";
            }
            parameters.Add("ProjectNumber", projectNumber);
            parameters.Add("searchValue", searchString);

            var sql = $"select Period, Sum(Amount) as Amount from Cost WHERE projectNumber=@projectNumber {searchParam} {where} GROUP BY Period ORDER BY Cast(Period as int)";
            return _context.Connection.Query<CostCP>(sql, parameters);
        }

        public int GetCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate)
        {
            string searchParam = "";
            string searchString = "";
            if (startDate.HasValue && endDate.HasValue)
            {
                int stYear = startDate.Value.Year;
                int stMonth = startDate.Value.Month;
                int enYear = endDate.Value.Year;
                int enMonth = endDate.Value.Month + 1;
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "FiscalYear", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stYear, Value2 = enYear });
                postValue.Add(new AdvancedSearchRequest { Attribute = new AdvancedSearchAttribute { AttributeName = "Period", AttributeType = 4 }, Operator = new AdvancedSearchOperator { OperatorName = 13, OperatorType = 4 }, Value = stMonth, Value2 = enMonth });
            }
            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }
            var where = selector.RawSql.Replace("WHERE", " AND ");
            var parameters = (Dapper.DynamicParameters)selector.Parameters;

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND ( Description LIKE @searchValue or VendorName LIKE @searchValue or PO_ID like @searchValue)";
            }
            parameters.Add("ProjectNumber", projectNumber);
            parameters.Add("searchValue", searchString);

            var sql = $"select Count(1) from Cost WHERE projectNumber=@projectNumber {searchParam} {where}";
            return _context.Connection.ExecuteScalar<int>(sql, parameters);
        }

        public IEnumerable<CostCP> GetCosts(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var orderBy = queryBuilder.OrderBy(searchSpec);
            var offSet = queryBuilder.OffsetQuery(searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select * from Cost {where} {orderBy} {offSet}";
            return _context.Connection.Query<CostCP>(sql, parameters);
        }

        public IEnumerable<ChartModel> GetCostForPieChart(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select Description as Name, Sum(Amount) as Amount from Cost {where}  GROUP BY Description ORDER BY Description";
            return _context.Connection.Query<ChartModel>(sql, parameters);
        }

        public IEnumerable<ChartModel> GetCostForBarChart(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select Period as Name, Sum(Amount) as Amount from Cost {where} Group by Period ORDER BY Cast(Period as int)";
            return _context.Connection.Query<ChartModel>(sql, parameters);
        }

        public int GetCount(SearchSpecCP searchSpec)
        {
            var queryBuilder = new AdvancedSearchQueryBuilder(searchSpec.AdvancedSearchCriteria.ToList());
            var where = WhereAnd(queryBuilder.RawSql, searchSpec);
            var parameters = (DynamicParameters)queryBuilder.Parameters;
            parameters.Add("ProjectNumber", searchSpec.ProjectNumber);
            parameters.Add("searchValue", "%" + searchSpec.SearchText + "%");
            var sql = $"select count(1) from Cost {where}";
            return _context.Connection.ExecuteScalar<int>(sql, parameters);
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
    }
}
