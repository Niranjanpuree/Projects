using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Infrastructure.Data.FarClauseRepo
{
    public class FarContractTypeClauseRepository : IFarContractTypeClauseRepository
    {
        private readonly IDatabaseContext _context;

        public FarContractTypeClauseRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void Add(FarContractTypeClause farContractTypeClause)
        {
            string insertQuery = $@"INSERT INTO [dbo].[FarContractTypeClause]
                                                                   (
                                                                        FarClauseGuid	   ,
                                                                        FarContractTypeGuid,
                                                                        IsRequired	       ,
                                                                        IsApplicable	   ,
                                                                        IsOptional	       ,
                                                                        UpdatedBy          ,	   
                                                                        IsDeleted          	   
                                                                    )
                                  VALUES (
                                                                        @FarClauseGuid	    ,
                                                                        @FarContractTypeGuid,
                                                                        @IsRequired	        ,
                                                                        @IsApplicable	    ,
                                                                        @IsOptional	        ,
                                                                        @UpdatedBy	        ,
                                                                        @IsDeleted	    
                                                                )";
            _context.Connection.Execute(insertQuery, farContractTypeClause);
        }

        public void Delete(Guid id)
        {
            string updateQuery = $@"Delete from farContractTypeClause where farContractTypeClauseGuid = @FarContractTypeClauseGuid ";
            _context.Connection.Execute(updateQuery, new { FarContractTypeClauseGuid = id });
        }

        public IEnumerable<FarContractTypeClause> GetAll(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            string searchParam = "";
            string searchString = "";
            string sortBy = "";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "[farContractTypeCode]";
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
                            searchParam = " Where [farContractTypeCode] LIKE @FarContractTypeCode  ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            searchParam = " Where ([farContractTypeCode] LIKE @sortBy)  ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            searchParam = " Where  [farContractTypeCode] LIKE @FarContractTypeCode   ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    searchParam = " Where [farContractTypeCode] LIKE @FarContractTypeCode  ";
                    filterBy = null;
                }
            }
            var orderBy = string.Empty;
            if (take == 1000)
                orderBy += $" ORDER BY {sortField} {dir}";
            else
                orderBy += $" ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var query = $@"Select * from farContractTypeClause
             {searchParam}  {orderBy}";
            return _context.Connection.Query<FarContractTypeClause>(query, new { FarContractTypeCode = searchString, sortBy = sortBy });
        }

        public FarContractTypeClause GetById(Guid farContractTypeClauseGuid)
        {
            var sql = @" select *  from FarContractTypeClause where FarContractTypeClauseGuid = @FarContractTypeClauseGuid";
            return _context.Connection.QueryFirstOrDefault<FarContractTypeClause>(sql, new { FarContractTypeClauseGuid = farContractTypeClauseGuid });
        }

        public IEnumerable<FarContractTypeClause> GetFarContractTypeByFarClauseId(Guid farClauseId)
        {
            var query = $@"
                            select b.FarContractTypeClauseGuid,
                              b.FarClauseGuid,
                              a.FarContractTypeGuid,
                              b.IsRequired,
                              b.IsApplicable,
                              b.IsOptional,
                              b.IsDeleted,
                              b.UpdatedBy,
							  a.code,a.isdeleted
							 from (select * from FarContractType where isdeleted=0) a 
                                   left join 
                                  (select * from FarContractTypeClause where FarClauseGuid= @farClauseGuid) b 
							 on a.FarContractTypeGuid = b.FarContractTypeGuid
                             order by a.Code";

            return _context.Connection.Query<FarContractTypeClause>(query, new { farClauseGuid = farClauseId });
        }

        public int TotalRecord(string searchValue, string filterBy)
        {
            string searchParam = "";
            string searchString = "";
            string sortBy = "";
            if (!string.IsNullOrEmpty(filterBy))
            {
                var switchFilter = filterBy.ToLower();
                switch (switchFilter)
                {
                    case "all":
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            searchParam = " where [farContractTypeCode] LIKE @FarContractTypeCode  ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            searchParam = " where ([farContractTypeCode] LIKE @sortBy)  ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            searchParam = " where [farContractTypeCode] LIKE @FarContractTypeCode   ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    searchParam = " where [farContractTypeCode] LIKE @FarContractTypeCode  ";
                    filterBy = null;
                }
            }
            var query = $@"Select count(*) from farContractTypeClause
              {searchParam} ";
            return _context.Connection.ExecuteScalar<int>(query, new { FarContractTypeCode = searchString, sortBy = sortBy });
        }

        public void Edit(FarContractTypeClause farContractTypeClause)
        {
            string updateQuery = $@"Update FarContractTypeClause       set 
                                                                    FarClauseGuid					    =     @FarClauseGuid				,
                                                                    FarContractTypeGuid					=     @FarContractTypeGuid			,
                                                                    IsRequired					        =     @IsRequired					,
                                                                    IsApplicable					    =     @IsApplicable					,
                                                                    IsOptional	                        =     @IsOptional	                ,
                                                                    IsDeleted	                        =     @IsDeleted	                ,
                                                                    UpdatedBy	                        =     @UpdatedBy	               
                                                                    where FarContractTypeClauseGuid = @FarContractTypeClauseGuid ";
            _context.Connection.Execute(updateQuery, farContractTypeClause);
        }

        public int CheckDuplicateByFarClauseAndFarContractTypeComposition(FarContractTypeClause farContractTypeClauseModel)
        {
            string sql = $"SELECT Count(1) FROM FarContractTypeClause WHERE FarClauseGuid = @FarClauseGuid and FarContractTypeGuid = @FarContractTypeGuid";
            var result = _context.Connection.QuerySingle<int>(sql, farContractTypeClauseModel);
            return result;
        }

        public FarContractTypeClause GetByFarClauseFarContractTypeGuid(Guid farClauseGuid, Guid farContractTypeGuid)
        {
            var sql = @" select *  from FarContractTypeClause where FarClauseGuid = @FarClauseGuid and FarContractTypeGuid = @FarContractTypeGuid ";
            return _context.Connection.QueryFirstOrDefault<FarContractTypeClause>(sql, new { FarClauseGuid = farClauseGuid, FarContractTypeGuid = farContractTypeGuid, });
        }
    }
}
