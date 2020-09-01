using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Northwind.Infrastructure.Data.FarClauseRepo
{
    public class FarContractTypeRepository : IFarContractTypeRepository
    {
        private readonly IDatabaseContext _context;

        public FarContractTypeRepository(IDatabaseContext context)
        {
            _context = context;
        }

        private string GetOrderByColumn(string sortField, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortDirection))
                sortDirection = " asc ";
            switch (sortDirection.ToUpper())
            {
                case "DESC":
                    sortDirection = " Desc";
                    break;
                default:
                    sortDirection = " Asc";
                    break;
            }
            var sortBy = "";
            if (!string.IsNullOrEmpty(sortField))
            {
                switch (sortField.ToLower())
                {
                    case "title":
                        sortBy = "Title" + sortDirection;
                        break;
                    case "updated by":
                        sortBy = "u.Displayname" + sortDirection;
                        break;
                    default:
                        sortBy = "Code" + sortDirection;
                        break;
                }
            }
            else
            {
                sortBy = "Code" + sortDirection;
            }
            return sortBy;
        }

        public void Add(FarContractType farContractType)
        {

            string insertQuery = $@"INSERT INTO [dbo].[FarContractType]
                                                                   (
                                                                        FarContractTypeGuid	        ,
                                                                        Code	        ,
                                                                        IsDeleted	    ,
                                                                        UpdatedBy	    ,
                                                                        Title	   
                                                                    )
                                  VALUES (
                                                                        @FarContractTypeGuid	        ,
                                                                        @Code	        ,
                                                                        @IsDeleted	    ,
                                                                        @UpdatedBy	    ,
                                                                        @Title	    
                                                                )";
            _context.Connection.Execute(insertQuery, farContractType);
        }

        public void Delete(Guid id)
        {
            string updateQuery = $@"Update FarContractType       set 
                                                                    IsDeleted	                    =     @IsDeleted	               
                                                                     where FarContractTypeGuid = @FarContractTypeGuid ";
            _context.Connection.Execute(updateQuery, new { IsDeleted= true, FarContractTypeGuid = id });
        }

        public IEnumerable<FarContractType> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = " Where isdeleted = 0 ";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where += " and (Code LIKE @searchValue or Title like @searchValue)";
            }

            if (take == 0)
            {
                take = 10;
            }
            var orderBy = GetOrderByColumn(sortField, dir);

            string sql = @"select f.*,
                            u.Displayname UpdatedByName
                            from FarContractType f
                            left join Users u
                            on u.UserGuid = f.UpdatedBy";
            sql += $"{ where }";
            sql += $" ORDER BY {orderBy} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var list = _context.Connection.Query<FarContractType>(sql, new { searchValue = searchString }).ToList();
            return list;
        }

        public IEnumerable<FarContractType> GetAll()
        {
            var sql = $@"
                  Select * from FarContractType where (isDeleted = 0 or IsDeleted is null) order by code asc";

            return _context.Connection.Query<FarContractType>(sql);
        }

        public IEnumerable<FarContractType> GetAllList()
        {
            var data = _context.Connection.Query<FarContractType>("SELECT * FROM FarContractType where (isDeleted = 0 or isDeleted is null) order by Title asc");
            return data;
        }

        public int TotalRecord(string searchValue)
        {
            var where = " Where isdeleted = 0 ";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where += " and (title LIKE @searchValue or Code like @searchValue)";
            }
            string sql = @"select count(*) from FarContractType";
            sql += $"{ where }";
            var result = _context.Connection.QuerySingle<int>(sql, new { searchValue = searchString });
            return result;
        }

        public void Update(FarContractType farContractType)
        {
            string updateQuery = $@"Update FarContractType       set 
                                                                    Code					    =     @Code					,
                                                                    UpdatedBy				    =     @UpdatedBy					,
                                                                    Title	                    =     @Title	               
                                                                     where FarContractTypeGuid = @FarContractTypeGuid ";
            _context.Connection.Execute(updateQuery, farContractType);
        }

        public FarContractType GetById(Guid id)
        {
            string selectQuery = $@"SELECT * FROM FarContractType  where FarContractTypeGuid = @FarContractTypeGuid ";
            var data = _context.Connection.QueryFirstOrDefault<FarContractType>(selectQuery, new { FarContractTypeGuid = id });
            return data;
        }

        public int CheckDuplicate(FarContractType farContractType)
        {
            string selectQuery = $@"SELECT count(1) FROM FarContractType  where Code = @Code and FarContractTypeGuid != @FarContractTypeGuid and IsDeleted = 0";
            var data = _context.Connection.QueryFirstOrDefault<int>(selectQuery, new { FarContractTypeGuid = farContractType.FarContractTypeGuid, Code = farContractType.Code });
            return data;
        }

        public FarContractType GetByCode(string code)
        {
            string selectQuery = $@"SELECT * FROM FarContractType  where replace(code,'&','') = @FarContractTypeCode ";
            var data = _context.Connection.QueryFirstOrDefault<FarContractType>(selectQuery, new { FarContractTypeCode = code });
            return data;
        }
    }
}
