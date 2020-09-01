using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Infrastructure.Data.FarClauseRepo
{
    public class FarClauseRepository : IFarClauseRepository
    {
        private readonly IDatabaseContext _context;

        public FarClauseRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(FarClause farClause)
        {
            string insertQuery = $@"INSERT INTO [dbo].[FarClause]
                                                                   ( 
                                                                        FarClauseGuid,
                                                                        Number	   ,
                                                                        Title	   ,
                                                                        Paragraph  ,			       
                                                                        UpdatedBy  ,			       
                                                                        IsDeleted  			       
                                                                    )
                                  VALUES (
                                                                        @FarClauseGuid	    ,
                                                                        @Number	    ,
                                                                        @Title	    ,
                                                                        @Paragraph	,			                                             
                                                                        @UpdatedBy	,			                                             
                                                                        @IsDeleted				                                             
                                                                )";
            _context.Connection.Execute(insertQuery, farClause);
        }

        public int CheckDuplicateFarClauseNumber(FarClause farClause)
        {
            string sql = $"SELECT Count(1) FROM FarClause WHERE Number = @Number and FarClauseGuid != @FarClauseGuid ";
            var result = _context.Connection.QuerySingle<int>(sql, new { Number = farClause.Number, FarClauseGuid = farClause.FarClauseGuid });
            return result;
        }

        public void Delete(Guid id, Guid updatedBy)
        {
            string updateQuery = $@"Update FarClause set isDeleted = 1 , updatedBy = @UpdatedBy where FarClauseGuid = @FarClauseGuid ";
            _context.Connection.Execute(updateQuery, new { FarClauseGuid = id, UpdatedBy = updatedBy });
        }

        public IEnumerable<FarClause> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = $@"{sortField}";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = $@"and (Number LIKE @searchValue or  Title LIKE @searchValue or Paragraph LIKE @searchValue)";
            }
            if (string.IsNullOrEmpty(sortField))
            {
                additionalSql = "Number";
                dir = "asc";
            }
            else if (sortField.ToLower().Equals("updatedby"))
            {
                additionalSql = "DisplayName";
            }

            sql = $@"
                  select f.*,u.Displayname from FarClause f left join users u
				  on f.UpdatedBy = u.UserGuid 
                  where 1=1 
                  and f.IsDeleted = 0  
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            return _context.Connection.Query<FarClause>(sql, new { searchValue = searchString });
        }

        public IEnumerable<FarClause> GetAll()
        {
            var sql = $@"
                  Select * from FarClause where (isDeleted = 0 or IsDeleted is null) order by number asc";

            return _context.Connection.Query<FarClause>(sql);
        }

        public FarClause GetById(Guid FarClauseGuid)
        {
            var sql = @" select *  from FarClause where FarClauseGuid = @FarClauseGuid";
            return _context.Connection.QueryFirstOrDefault<FarClause>(sql, new { FarClauseGuid = FarClauseGuid });
        }

        public int TotalRecord(string searchValue)
        {
            string searchParam = "";
            string searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " And (Number LIKE @searchValue or Title LIKE @searchValue) ";
            }
            var sql = $@" select count(1) from FarClause Where 1=1 and IsDeleted = 0 {searchParam}";

            return _context.Connection.ExecuteScalar<int>(sql, new { searchValue = searchString });
        }

        public void Edit(FarClause farClause)
        {
            string updateQuery = $@"Update FarClause      set 
                                                                    Number					    =     @Number					,
                                                                    Title	                    =     @Title	                ,
                                                                    Paragraph		            =     @Paragraph		        ,
                                                                    UpdatedBy                   =     @UpdatedBy                
                                                                     where FarClauseGuid = @FarClauseGuid ";
            _context.Connection.Execute(updateQuery, farClause);
        }

        public IEnumerable<FarClause> GetByAlphabetFilter(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            string searchParam = "";
            string searchString = "";
            string sortBy = "";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Number";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if (!string.IsNullOrEmpty(filterBy))
            {
                var switchFilter = filterBy.ToLower();
                switch (switchFilter)
                {
                    case "all":
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            searchParam = " Where (Number LIKE @searchValue or Title LIKE @searchValue) ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            searchParam = " Where (Number LIKE @sortBy)  ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            searchParam = " Where  Number LIKE @searchValue   ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    searchParam = " Where Number LIKE @searchValue  ";
                    filterBy = null;
                }
            }
            var orderBy = string.Empty;
            if (take == 1000)
                orderBy += $" ORDER BY {sortField} {dir}";
            else
                orderBy += $" ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var query = $@"Select * from FarClause
             {searchParam}  {orderBy}";
            return _context.Connection.Query<FarClause>(query, new { searchValue = searchString, sortBy = sortBy });
        }

        public FarClause GetFarClauseByNumber(string number)
        {
            var query = @"SELECT * 
                        FROM FarClause
                        WHERE Number = @number";
            return _context.Connection.QueryFirstOrDefault<FarClause>(query,new { number = number});
        }
    }
}
