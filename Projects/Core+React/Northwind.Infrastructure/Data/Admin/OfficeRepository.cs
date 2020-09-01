using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Text;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Infrastructure.Data.Admin
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IDatabaseContext _context;
        public OfficeRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public int Add(Office officeModel)
        {
            string insertQuery = @"INSERT INTO [dbo].[Office]
                                                                   (
                                                                    OfficeGuid,
                                                                    OfficeName, 
                                                                    IsActive,
                                                                    IsDeleted, 
                                                                    CreatedOn, 
                                                                    UpdatedOn, 
                                                                    CreatedBy,
                                                                    UpdatedBy,
                                                                    PhysicalAddress,
                                                                    PhysicalAddressLine1,
                                                                    MailingAddress,
                                                                    MailingAddressLine1,
                                                                    PhysicalCity,
                                                                    PhysicalStateId,
                                                                    PhysicalZipCode,
                                                                    PhysicalCountryId,
                                                                    MailingCity,
                                                                    MailingStateId,
                                                                    MailingZipCode,
                                                                    MailingCountryId,
                                                                    OfficeCode,
                                                                    Phone,
                                                                    OperationManagerGuid,
                                                                    Fax
                                                                    )
                                  VALUES (
                                                                    @OfficeGuid,
                                                                    @OfficeName, 
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy,
                                                                    @PhysicalAddress,
                                                                    @PhysicalAddressLine1,
                                                                    @MailingAddress,
                                                                    @MailingAddressLine1,
                                                                    @PhysicalCity,
                                                                    @PhysicalStateId,
                                                                    @PhysicalZipCode,
                                                                    @PhysicalCountryId,
                                                                    @MailingCity,
                                                                    @MailingStateId,
                                                                    @MailingZipCode,
                                                                    @MailingCountryId,
                                                                    @OfficeCode,
                                                                    @Phone,
                                                                    @OperationManagerGuid,
                                                                    @Fax
                                                                )";
            return _context.Connection.Execute(insertQuery, officeModel);
        }
        public int Edit(Office officeModel)
        {
            string updateQuery = @"Update Office set 
                                                                    OfficeName                      =       @OfficeName,
                                                                    UpdatedOn                       =       @UpdatedOn,
                                                                    UpdatedBy                       =       @UpdatedBy,
                                                                    PhysicalAddress                 =       @PhysicalAddress,
                                                                    PhysicalAddressLine1            =       @PhysicalAddressLine1,
                                                                    MailingAddress                  =       @MailingAddress,
                                                                    MailingAddressLine1             =       @MailingAddressLine1,
                                                                    PhysicalCity                    =       @PhysicalCity,
                                                                    PhysicalStateId                 =       @PhysicalStateId,
                                                                    PhysicalZipCode                 =       @PhysicalZipCode,
                                                                    PhysicalCountryId               =       @PhysicalCountryId, 
                                                                    MailingCity                     =       @MailingCity,
                                                                    MailingStateId                  =       @MailingStateId,
                                                                    MailingZipCode                  =       @MailingZipCode,
                                                                    MailingCountryId                =       @MailingCountryId,
                                                                    OfficeCode                      =       @OfficeCode,
                                                                    Phone                           =       @Phone,
                                                                    OperationManagerGuid            =       @OperationManagerGuid,
                                                                    Fax                             =       @Fax

                                                                    where OfficeGuid =@OfficeGuid ";
            return _context.Connection.Execute(updateQuery, officeModel);
        }
        public int Delete(Guid[] officeGuidIds)
        {
            foreach (var officeGuid in officeGuidIds)
            {
                var office = new
                {
                    OfficeGuid = officeGuid
                };
                string disableQuery = @"Update Office set 
                                            IsDeleted   = 1
                                            where OfficeGuid =@OfficeGuid ";
                _context.Connection.Execute(disableQuery, office);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int TotalRecord(string searchValue)
        {
            var searchString = string.Empty;
            var where = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(OfficeName like @searchValue or OfficeCode like @searchValue )";
            }
            string sql = $"SELECT Count(1) FROM Office WHERE IsDeleted = 0 {where}";
            var result = _context.Connection.QuerySingle<int>(sql, new { searchValue = searchString });
            return result;
        }
        public Office GetById(Guid officeGuid)
        {

            string sql = "SELECT * FROM Office WHERE OfficeGuid = @OfficeGuid;";
            var result = _context.Connection.QuerySingle<Office>(sql, new { OfficeGuid = officeGuid });
            return result;
        }
        public int Disable(Guid[] officeGuidIds)
        {
            foreach (var officeGuid in officeGuidIds)
            {
                var office = new
                {
                    OfficeGuid = officeGuid
                };
                string disableQuery = @"Update Office set 
                                            IsActive   = 0
                                            where OfficeGuid =@OfficeGuid ";
                _context.Connection.Execute(disableQuery, office);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public int Enable(Guid[] officeGuidIds)
        {
            foreach (var officeGuid in officeGuidIds)
            {
                var office = new
                {
                    OfficeGuid = officeGuid
                };
                string disableQuery = @"Update Office set 
                                            IsActive   = 1
                                            where OfficeGuid =@OfficeGuid ";
                _context.Connection.Execute(disableQuery, office);
            }

            return 1;// 1 is success action..    0 for some error occurred..
        }
        public Office GetDetailById(Guid id)
        {
            string sql = @"select 
                                                                                        Office.OfficeGuid,
                                                                                        OfficeName,
																						Users.Displayname OperationManagerName,
																						OperationManagerGuid,
                                                                                        Phone,
                                                                                        Fax,
                                                                                        Office.IsActive,
                                                                                        Office.PhysicalAddress,
                                                                                        Office.MailingAddress,
                                                                                        Office.OfficeCode OfficeCode,
                                                                                        Office.UpdatedOn,
                                                                                        Office.CreatedOn,
                                                                                        Office.UpdatedBy,
                                                                                        Office.CreatedBy,
                                                                                        PhysicalState.StateName PhysicalStateName,
                                                                                        PhysicalCountry.CountryName PhysicalCountryName,
                                                                                        PhysicalCity,
                                                                                        PhysicalZipCode,
                                                                                        PhysicalCountry.CountryId,
                                                                                        MailingState.StateName MailingStateName,
                                                                                        MailingCountry.CountryName MailingCountryName,
                                                                                        MailingZipCode,
                                                                                        MailingCity
                                                                                        from Office
                                                                                        left
                                                                                        join Country PhysicalCountry
                                                                                        on PhysicalCountry.CountryId = Office.PhysicalCountryId
                                                                                        left
                                                                                        join State PhysicalState
                                                                                        on PhysicalState.StateId = Office.PhysicalStateId
                                                                                        left join Country MailingCountry
                                                                                        on MailingCountry.CountryId = Office.MailingCountryId
                                                                                        left
                                                                                        join State MailingState
                                                                                        on MailingState.StateId = Office.MailingStateId
                                                                                        left
																						join Users
																						on Users.UserGuid = office.OperationManagerGuid
																						where Office.IsDeleted = 0
                                                                                        and Office.OfficeGuid = @id";

            var result = _context.Connection.QuerySingle<Office>(sql, new { id = id });
            return result;
        }
        public int CheckDuplicate(Office office)
        {
            string sql = $"SELECT Count(1) FROM Office WHERE (OfficeCode = @OfficeCode or OfficeName = @OfficeName) and OfficeGuid != @OfficeGuid and IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { OfficeCode = office.OfficeCode, OfficeName = office.OfficeName, OfficeGuid = office.OfficeGuid });
            return result;
        }

        public string GetOfficeCodeByContractGuid(Guid contractGuid)
        {
            string officeCodeSql = $@"select SUBSTRING(OrgID.Name,7,2) 
                                    from Contract 
                                    left join OrgID 
                                    on Contract.ORGID = OrgID.OrgIDGuid 
                                    WHERE ContractGuid = '{contractGuid}';";
            var officeCode = _context.Connection.Query<string>(officeCodeSql).FirstOrDefault();
            return officeCode;
        }

        public Office GetOfficeByCode(string officeCode)
        {
            string officeNameSql = $@"select Office.OfficeName 
                                    from Office 
                                    where OfficeCode = '{officeCode}' 
                                    and IsDeleted = 0";
            return _context.Connection.Query<Office>(officeNameSql).FirstOrDefault();
        }

        public IEnumerable<Office> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField, sortDirection);
            var rowNum = skip + pageSize;
            var where = "";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(OfficeName like @searchValue or OfficeCode like @searchValue )";
            }
            var pagingQuery = string.Format($@"Select * 
                                                    FROM 
                                                        (SELECT ROW_NUMBER() OVER (Order By {orderingQuery}) AS RowNum, 
                                                                                        Office.OfficeGuid,
                                                                                        Office.OfficeName,
																						Users.Displayname OperationManagerName,
                                                                                        Office.Phone,
                                                                                        Office.Fax,
                                                                                        Office.IsActive,
                                                                                        Office.PhysicalAddress,
                                                                                        Office.PhysicalAddressLine1,
                                                                                        Office.PhysicalCity,
                                                                                        Office.PhysicalCountryId,
                                                                                        Office.PhysicalZipCode,
                                                                                        Office.MailingAddress,
                                                                                        Office.MailingCountryId,
                                                                                        Office.MailingAddressLine1,
                                                                                        Office.MailingCity,
                                                                                        Office.MailingZipCode,
                                                                                        Office.OfficeCode OfficeCode,
                                                                                        PhysicalState.StateName PhysicalStateName,
                                                                                        PhysicalCountry.CountryName PhysicalCountryName,
																						MailingState.StateName MailingStateName,
                                                                                        MailingCountry.CountryName MailingCountryName,
                                                                                        Office.UpdatedOn
                                                                                        from Office
                                                                                        left
                                                                                        join Country PhysicalCountry
                                                                                        on PhysicalCountry.CountryId = Office.PhysicalCountryId
                                                                                        left
                                                                                        join State PhysicalState
                                                                                        on PhysicalState.StateId = Office.PhysicalStateId
                                                                                        left
                                                                                        join Country MailingCountry
                                                                                        on MailingCountry.CountryId = Office.MailingCountryId
                                                                                        left
                                                                                        join State MailingState
                                                                                        on MailingState.StateId = Office.MailingStateId
                                                                                        left
																						join Users
																						on Users.UserGuid = office.OperationManagerGuid                                                                                        
                                                                                        where Office.IsDeleted = 0
                                                                                        { where }
                                      ) AS Paged 
                                            WHERE   
                                            RowNum > @skip
                                            AND RowNum <= @rowNum  
                                        ORDER BY RowNum");

            var pagedData = _context.Connection.Query<Office>(pagingQuery, new { searchValue = searchString, skip = skip, rowNum = rowNum });
            return pagedData;
        }

        //method to get list of office to display in web
        public IEnumerable<Office> GetOfficeListForUser(string searchValue, string filterBy, int pageSize, int skip, string sortField, string sortDirection)
        {
            var orderingQuery = GetOrderByColumn(sortField, sortDirection);
            var rowNum = skip + pageSize;
            var where = "";
            var searchString = "";
            var sortBy = "";
            if (!string.IsNullOrEmpty(filterBy))
            {
                var switchFilter = filterBy.ToLower();
                switch (switchFilter)
                {
                    case "all":
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            where += " and o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            where += " and ((o.OfficeName LIKE @sortBy or o.OfficeCode like @sortBy )  and (o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue)  ) ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            where += " and o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue  ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    where = " AND ";
                    where += "(o.OfficeName LIKE @searchValue or o.OfficeCode like @searchValue )";
                }
            }

            if (string.IsNullOrWhiteSpace(sortField))
                sortField = "o.OfficeName";
            if (pageSize == 1000)
                where += $" ORDER BY {sortField} {sortDirection}";
            else
                where += $" ORDER BY {sortField} {sortDirection}  OFFSET {skip} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            var pagingQuery = string.Format($@"SELECT 
                                                   o.*,physicalCountry.CountryName as PhysicalCountryName,mailingCountry.CountryName as MailingCountryName,
												   physicalState.StateName as PhysicalStateName, mailingState.StateName as MailingStateName
                                                    FROM Office o
                                                    LEFT JOIN Country physicalCountry ON o.PhysicalCountryId = physicalCountry.CountryId
                                                    LEFT JOIN Country mailingCountry ON o.MailingCountryId = mailingCountry.CountryId
                                                    LEFT JOIN State physicalState ON o.PhysicalStateId = physicalState.StateId
                                                    LEFT JOIN State mailingState ON o.MailingStateId = mailingState.StateId
                                                    WHERE o.IsDeleted = 0
                                                    { where }");


            var pagedData = _context.Connection.Query<Office>(pagingQuery, new { searchValue = searchString, skip = skip, rowNum = rowNum, sortBy = sortBy });
            return pagedData;
        }

        public int GetTotalCountForUser(string searchValue, string filterBy)
        {
            var searchString = "";
            var where = "";
            var sortBy = "";
            if (!string.IsNullOrEmpty(filterBy))
            {
                var switchFilter = filterBy.ToLower();
                switch (switchFilter)
                {
                    case "all":
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            where += " and o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            where += " and ((o.OfficeName LIKE @sortBy or o.OfficeCode like @sortBy )  and (o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue)  ) ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            where += " and o.OfficeName LIKE @searchValue or o.OfficeCode like  @searchValue  ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    where = " AND ";
                    where += "(o.OfficeName LIKE @searchValue or o.OfficeCode like @searchValue )";
                }
            }
            string sql = $@"SELECT COUNT(1) 
                            FROM Office o
                            WHERE IsDeleted = 0
                            {where}";
            var totalCount = _context.Connection.QueryFirstOrDefault<int>(sql, new { searchValue = searchString, sortBy = sortBy });
            return totalCount;
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
                        sortBy = "Office.isActive" + sortDirection;
                        break;
                    case "OFFICENAME":
                        sortBy = "OFFICENAME" + sortDirection;
                        break;
                    case "PHONE":
                        sortBy = "PHONE" + sortDirection;
                        break;
                    case "FAX":
                        sortBy = "FAX" + sortDirection;
                        break;
                    case "ADDRESS":
                        sortBy = "PHYSICALADDRESS" + sortDirection;
                        break;
                    case "MAILINGADDRESS":
                        sortBy = "MAILINGADDRESS" + sortDirection;
                        break;
                    case "UPDATEDON":
                        sortBy = "UPDATEDON" + sortDirection;
                        break;
                    default:
                        sortBy = "OFFICECODE" + sortDirection;
                        break;
                }
            }
            return sortBy;
        }

        public Office GetOfficeByCodeOrName(string officeCode, string officeName)
        {
            string officeSql = @"SELECT * 
                                FROM Office
                                WHERE OfficeCode = @officeCode
                                OR OfficeName = @officeName";
            return _context.Connection.QueryFirstOrDefault<Office>(officeSql, new { officeCode = officeCode, officeName = officeName });
        }

        public int CheckDuplicateOfficeByName(string officeName, Guid officeGuid)
        {
            string sql = $@"SELECT Count(1) 
                            FROM Office 
                            WHERE OfficeName = @OfficeName 
                            AND OfficeGuid != @OfficeGuid AND IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { OfficeName = officeName, OfficeGuid = officeGuid });
            return result;
        }

        public int DeleteById(Guid id)
        {
            string disableQuery = @"Update Office set 
                                            IsDeleted   = 1
                                            where OfficeGuid =@OfficeGuid ";
            return _context.Connection.Execute(disableQuery, new { OfficeGuid =id });
        }
    }
}
