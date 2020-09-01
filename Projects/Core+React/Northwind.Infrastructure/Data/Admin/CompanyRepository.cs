using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Infrastructure.Data.Admin
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDatabaseContext _context;

        public CompanyRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Add(Company company)
        {
            string insertQuery = @"INSERT INTO [dbo].Company
                                                                   (
                                                                    CompanyGuid,
                                                                    CompanyName, 
                                                                    IsActive,
                                                                    IsDeleted, 
                                                                    CreatedOn, 
                                                                    UpdatedOn, 
                                                                    CreatedBy,
                                                                    UpdatedBy,
                                                                    CompanyCode,
																	President,
																	Abbreviation,
																	Description
                                                                    )
                                  VALUES (
                                                                    @CompanyGuid,
                                                                    @CompanyName, 
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy,
                                                                    @CompanyCode,
                                                                    @President,
                                                                    @Abbreviation,
                                                                    @Description
                                                                )";
            return _context.Connection.Execute(insertQuery, company);
        }

        public int Delete(Guid[] companyGuids)
        {
            foreach (var companyGuid in companyGuids)
            {
                var company = new
                {
                    companyGuid = companyGuid
                };
                string deleteQuery = @"Update Company set 
                                                                    IsDeleted   = 1
                                                                   
                                            where companyGuid =@companyGuid ";
                _context.Connection.Execute(deleteQuery, company);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Disable(Guid[] companyGuids)
        {
            foreach (var companyGuid in companyGuids)
            {
                var company = new
                {
                    companyGuid = companyGuid
                };
                string deleteQuery = @"Update Company set 
                                                                    IsActive   = 0
                                                                   
                                            where companyGuid =@companyGuid ";
                _context.Connection.Execute(deleteQuery, company);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Enable(Guid[] companyGuids)
        {
            foreach (var companyGuid in companyGuids)
            {
                var company = new
                {
                    companyGuid = companyGuid
                };
                string deleteQuery = @"Update Company set 
                                                                    IsActive   = 1
                                                                   
                                            where companyGuid =@companyGuid ";
                _context.Connection.Execute(deleteQuery, company);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }


        public Company GetbyId(Guid id)
        {
            string sql = "SELECT * FROM Company WHERE CompanyGuid = @CompanyGuid;";
            var result = _context.Connection.QuerySingle<Company>(sql, new { CompanyGuid = id });
            return result;
        }

        public Company GetDetailsById(Guid id)
        {
            string sql = @"select company.CompanyGuid,
                                    company.CompanyName,
                                    company.CompanyCode,
                                    company.President,
                                    users.Displayname PresidentName,
                                    company.Abbreviation,
                                    company.Description,
                                    company.IsActive 
                                    from Company company 
                                    left join
                                    Users users on
                                    users.UserGuid = company.President WHERE CompanyGuid = @CompanyGuid;";
            var result = _context.Connection.QuerySingle<Company>(sql, new { CompanyGuid = id });
            return result;
        }

        public ICollection<KeyValuePairModel<Guid, string>> GetUserList()
        {
            var model = new List<KeyValuePairModel<Guid, string>>();
            var data = _context.Connection.Query<User>("SELECT * FROM users order by Displayname asc");
            foreach (var item in data)
            {
                model.Add(new KeyValuePairModel<Guid, string> { Keys = item.UserGuid, Values = item.DisplayName });
            }
            return model;
        }

        public int TotalRecord(string searchValue)
        {
            var searchString = string.Empty;
            var where = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(CompanyName like @searchValue or CompanyCode like @searchValue )";
            }
            string sql = $"SELECT Count(1) FROM Company WHERE IsDeleted = 0 {where}";
            var result = _context.Connection.QuerySingle<int>(sql, new { searchValue = searchString });
            return result;
        }

        public int Edit(Company company)
        {
            string updateQuery = @"update Company set 
                                                                CompanyName     =   @CompanyName,
                                                                CompanyCode     =   @CompanyCode,
                                                                President       =   @President,
                                                                Abbreviation    =   @Abbreviation,
                                                                Description     =   @Description,
                                                                UpdatedBy       =   @UpdatedBy,
                                                                UpdatedOn       =   @UpdatedOn
                                                                where CompanyGuid = @CompanyGuid ";
            return _context.Connection.Execute(updateQuery, company);
        }

        public int CheckDuplicates(Company company)
        {
            string sql = $"SELECT Count(1) FROM Company WHERE (CompanyCode = @CompanyCode or CompanyName = @CompanyName) and CompanyGuid != @CompanyGuid and IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { CompanyCode = company.CompanyCode, CompanyName = company.CompanyName, CompanyGuid = company.CompanyGuid });
            return result;
        }

        public Company GetCompanyByCode(string companyCode)
        {
            string query = @"SELECT TOP(1) * 
                            FROM Company 
                            WHERE CompanyCode = @companyCode";
            return _context.Connection.QueryFirstOrDefault<Company>(query, new { companyCode = companyCode });
        }

        public Company GetCompanyByName(string companyName)
        {
            string query = @"SELECT TOP(1) * 
                            FROM Company 
                            WHERE CompanyName = @companyName";
            return _context.Connection.QueryFirstOrDefault<Company>(query, new { companyName = companyName });
        }

        public IEnumerable<Company> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField, sortDirection);
            var where = "";
            var searchString = "";
            var rowNum = skip + pageSize;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(CompanyName like @searchValue or CompanyCode like @searchValue )";
            }
            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                        (SELECT ROW_NUMBER() OVER (ORDER BY {orderingQuery}) AS RowNum, 
                                                                                        company.CompanyGuid,
                                                                                        company.CompanyName,
                                                                                        company.CompanyCode,
                                                                                        company.President,
                                                                                        users.Displayname PresidentName,
                                                                                        company.Description,
                                                                                        company.Abbreviation,
                                                                                        company.IsActive,
                                                                                        company.CreatedOn,
                                                                                        company.CreatedBy,
                                                                                        company.UpdatedOn,
                                                                                        company.UpdatedBy 
                                                                                        from Company company 
                                                                                        left join
                                                                                        Users users on company.President = users.UserGuid
                                                                                        where company.IsDeleted = 0 
                                                                                        { where }) 
                                                                                        AS Paged 
                                                                                        WHERE   
                                                                                        RowNum > @skip 
                                                                                        AND RowNum <= @rowNum  
                                                                                    ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Company>(pagingQuery, new { searchValue = searchString, skip = skip, rowNum = rowNum });
            return pagedData;
        }

        public IEnumerable<Company> GetAllWithOrg(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField, sortDirection);
            var where = "";
            var searchString = "";
            var rowNum = skip + pageSize;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(CompanyName like @searchValue or CompanyCode like @searchValue )";
            }
            var pagingQuery = string.Format($@"Select * from (SELECT [CompanyGuid]
                                              ,[CompanyName]
                                              ,[CompanyCode]
                                              ,[President]
                                              ,[Abbreviation]
                                              ,[Description]
                                              ,[IsActive]
                                              ,[IsDeleted]
                                              ,[CreatedOn]
                                              ,[UpdatedOn]
                                              ,[CreatedBy]
                                              ,[UpdatedBy]
                                          FROM [Company]

                                          union

                                          select 
	                                        OrgIDGuid as [CompanyGuid],
                                            [Title] as CompanyName,
	                                        [Name] as [CompanyCode],	                                        
	                                        NEWID() as [President],
	                                        '' as [Abbreviation],
	                                        [Description],
	                                        1 as [IsActive],
	                                        0 as [IsDeleted],
	                                        NULL as [CreatedOn],
                                            NULL as [UpdatedOn],
                                            NEWID() as [CreatedBy],
                                            NEWID() as [UpdatedBy]
                                          from OrgID) as paged {where} ORDER BY {sortField} {sortDirection}  OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY");

            var pagedData = _context.Connection.Query<Company>(pagingQuery, new { searchValue = searchString, skip, rowNum });
            return pagedData;
        }

        public string GetOrderByColumn(string sortField, string sortDirection)
        {
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
                switch (sortField.ToUpper())
                {
                    case "ISACTIVESTATUS":
                        sortBy = "isActive" + sortDirection;
                        break;
                    case "COMPANYNAME":
                        sortBy = "COMPANYNAME" + sortDirection;
                        break;
                    case "PRESIDENTNAME":
                        sortBy = "Displayname" + sortDirection;
                        break;
                    case "ABBREVIATION":
                        sortBy = "ABBREVIATION" + sortDirection;
                        break;
                    case "DESCRIPTION":
                        sortBy = "DESCRIPTION" + sortDirection;
                        break;
                    case "UPDATEDON":
                        sortBy = "UPDATEDON" + sortDirection;
                        break;
                    default:
                        sortBy = "COMPANYCODE" + sortDirection;
                        break;
                }
            }
            return sortBy;
        }

        public Company GetCompanyByCodeOrName(string code, string name)
        {
            string query = @"SELECT TOP(1) * 
                            FROM Company 
                            WHERE CompanyName = @companyName
                            OR CompanyCode = @companyCode";
            return _context.Connection.QueryFirstOrDefault<Company>(query, new { companyName = name, companyCode = code });
        }

        public IEnumerable<Company> GetCompanyList()
        {
            var sql = @"SELECT CompanyName,CompanyCode
                        FROM Company
                        WHERE IsActive = 1
                        ORDER BY CompanyName";
            return _context.Connection.Query<Company>(sql);
        }

        #region organization
        public Organization GetOrganizationByName(string orgIDName)
        {
            var query = @"SELECT * 
                        FROM OrgID
                        WHERE Name = @orgIDName";
            return _context.Connection.QueryFirstOrDefault<Organization>(query, new { OrgIDName = orgIDName });
        }

        public int DeleteById(Guid id)
        {
            string deleteQuery = @"Update Company set 
                                                                    IsDeleted   = 1
                                                                   
                                            where companyGuid =@CompanyGuid ";
            return _context.Connection.Execute(deleteQuery, new { CompanyGuid  = id});
        }
        #endregion organization
    }
}
