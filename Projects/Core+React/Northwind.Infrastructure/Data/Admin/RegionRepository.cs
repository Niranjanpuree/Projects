using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System.Text;


namespace Northwind.Infrastructure.Data.Admin
{
    public class RegionRepository : IRegionRepository
    {
        IDatabaseContext _context;

        private string GetOrderByColumn(string sortField, string sortDirection)
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
                        sortBy = "Region.isActive" + sortDirection;
                        break;
                    case "MANAGERName":
                        sortBy = "Displayname" + sortDirection;
                        break;
                    case "UPDATEDON":
                        sortBy = "UpdatedOn" + sortDirection;
                        break;
                    default:
                        sortBy = "RegionCode" + sortDirection;
                        break;
                }
            }
            return sortBy;
        }

        public RegionRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public int Add(Region region)
        {
            string insertQuery = @"INSERT INTO [dbo].Region
                                                                   (
                                                                    RegionGuid,
                                                                    RegionName, 
                                                                    IsActive,
                                                                    IsDeleted, 
                                                                    CreatedOn, 
                                                                    UpdatedOn, 
                                                                    CreatedBy,
                                                                    UpdatedBy,
                                                                    RegionCode
																	
                                                                    )
                                  VALUES (
                                                                    @RegionGuid,
                                                                    @RegionName, 
                                                                    @IsActive,
                                                                    @IsDeleted, 
                                                                    @CreatedOn, 
                                                                    @UpdatedOn, 
                                                                    @CreatedBy,
                                                                    @UpdatedBy,
                                                                    @RegionCode
                                                                   
                                                                )";
            return _context.Connection.Execute(insertQuery, region);
        }

        public int AddUpdateManager(RegionUserRoleMapping Regionsmapping)
        {
            string selectquery = @"select count(*) from [dbo].RegionUserRoleMapping  where roletype = @RoleType and regionguid = @regionGuid";
            var result = _context.Connection.QuerySingle<int>(selectquery, Regionsmapping);
            if (result == 0)
            {

                string insertQuery = @"INSERT INTO [dbo].RegionUserRoleMapping
                                                                   (
                                                                    RegionUserRoleMappingGuid,
                                                                    RegionGuid, 
                                                                    UserGuid,
                                                                    RoleType, 
                                                                    Keys
                                                                    )
                                  VALUES (
                                                                    @RegionUserRoleMappingGuid,
                                                                    @RegionGuid, 
                                                                    @UserGuid,
                                                                    @RoleType, 
                                                                    @Keys 
                                                                    
                                                                )";
                return _context.Connection.Execute(insertQuery, Regionsmapping);
            }
            else
            {
                string updateQuery = @"update [dbo].RegionUserRoleMapping set userguid = @userGuid where roletype = @RoleType and regionguid = @regionGuid";

                if (_context.Connection.Execute(updateQuery, Regionsmapping) > 0) 
                {
                    return UpdateContractUserRole(Regionsmapping);
                }
                return 0;
            }
        }

        public int UpdateContractUserRole(RegionUserRoleMapping Regionsmapping)
        {
            string updateQuery = "Update contractuserrole set userguid = @UserGuid where userrole = @RoleType";
            return _context.Connection.Execute(updateQuery, Regionsmapping);

        }

        public int UpdateManager(RegionUserRoleMapping Regionsmapping)
        {
            string selectquery = @"select count(*) from [dbo].RegionUserRoleMapping  where roletype = @RoleType and regionguid = @regionGuid";
            var result = _context.Connection.QuerySingle<int>(selectquery, Regionsmapping);
            if (result > 0)
            {
                string updateQuery = @"update [dbo].RegionUserRoleMapping set userguid = @userGuid where roletype = @RoleType and regionguid = @regionGuid";

                return _context.Connection.Execute(updateQuery, Regionsmapping);
            }
            else
            {
                string insertQuery = @"INSERT INTO [dbo].RegionUserRoleMapping
                                                                   (
                                                                    RegionUserRoleMappingGuid,
                                                                    RegionGuid, 
                                                                    UserGuid,
                                                                    RoleType, 
                                                                    Keys
                                                                    )
                                  VALUES (
                                                                    @RegionUserRoleMappingGuid,
                                                                    @RegionGuid, 
                                                                    @UserGuid,
                                                                    @RoleType, 
                                                                    @Keys 
                                                                    
                                                                )";
                return _context.Connection.Execute(insertQuery, Regionsmapping);
            }
        }
        public int Delete(Guid[] regionGuids)
        {
            foreach (var regionGuid in regionGuids)
            {
                var region = new
                {
                    regionGuid = regionGuid
                };
                string deleteQuery = @"Update Region set 
                                                                    IsDeleted   = 1
                                                                   
                                            where regionGuid =@regionGuid ";
                _context.Connection.Execute(deleteQuery, region);

                string deleteManagerQuery = @"delete from RegionUserRoleMapping                                                                              where regionGuid =@regionGuid ";
                _context.Connection.Execute(deleteManagerQuery, region);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Disable(Guid[] regionGuids)
        {
            foreach (var regionGuid in regionGuids)
            {
                var region = new
                {
                    regionGuid = regionGuid
                };
                string deleteQuery = @"Update Region set 
                                                                    IsActive   = 0
                                                                   
                                            where regionGuid =@regionGuid ";
                _context.Connection.Execute(deleteQuery, region);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public int Enable(Guid[] regionGuids)
        {
            foreach (var regionGuid in regionGuids)
            {
                var region = new
                {
                    regionGuid = regionGuid
                };
                string deleteQuery = @"Update Region set 
                                                                    IsActive   = 1
                                                                   
                                            where regionGuid =@regionGuid ";
                _context.Connection.Execute(deleteQuery, region);
            }
            return 1;// 1 is success action..    0 for some error occurred..
        }

        public IEnumerable<Region> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            var additionalSql = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "'%" + searchValue + "%'";
                where = " Where ";
                where += $@"(RegionName like {searchString} or RegionCode like {searchString} or BDManagerName like {searchString} or HSManagerName like {searchString} or DeputyManagerName like {searchString} or ManagerName like {searchString} )";
            }
            additionalSql = $@"{sortField}";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "LastUpdatedDate";
                dir = "desc";
                additionalSql = $@"{sortField}";
            }

            string sqlQuery = @"SELECT 
                                                                                        a.RegionGuid,
                                                                                        a.RegionName,
                                                                                        a.RegionCode,
                                                                                        a.IsActive,
                                                                                        a.CreatedOn,
                                                                                        a.CreatedBy,
                                                                                        a.UpdatedOn,
                                                                                        a.UpdatedBy,
                                                                              (select top 1 c.Displayname from regionuserrolemapping b                                              join users c on b.userguid = c.userguid   where                                                 b.regionguid = a.regionguid and b.roletype = 'BD-regional-manager')                                              as BDManagerName, 
                                                                              (select top 1 c.Displayname from regionuserrolemapping b                                              join users c on b.userguid = c.userguid   where                                                 b.regionguid = a.regionguid and b.roletype = 'HS-regional-manager')                                              as HSManagerName,
                                                                              (select top 1 c.Displayname from regionuserrolemapping b                                              join users c on b.userguid = c.userguid   where                                                 b.regionguid = a.regionguid and b.roletype =                                                    'deputy-regional-manager') as DeputyManagerName,
                                                                              (select top 1 c.Displayname from regionuserrolemapping b                                              join users c on b.userguid = c.userguid   where                                                 b.regionguid = a.regionguid and b.roletype =                                                    'regional-manager') as ManagerName
                                                                                        from Region a 
                                                                                        where a.IsDeleted = 0";
            sql = $@"
                   
                      select 
                            *
                  from ({sqlQuery}) A
                  {where}
                  ORDER BY {additionalSql} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            var data = _context.Connection.Query<Region>(sql);
            return data;
           
        }



        public Region GetById(Guid id)
        {
            string sql = "SELECT * FROM Region WHERE RegionGuid = @RegionGuid;";
            var result = _context.Connection.QuerySingle<Region>(sql, new { RegionGuid = id });
            return result;
        }

        public Region GetDetailsById(Guid id)
        {
            string sql = @"select a.RegionGuid,
                                    a.RegionName,
                                    a.RegionCode,
                                    a.IsActive,
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where         b.regionguid = a.regionguid and b.roletype = 'BD-regional-manager') as BusinessDevelopmentRegionalManager, 
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where          b.regionguid = a.regionguid and b.roletype = 'HS-regional-manager') as HSRegionalManager,
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where         b.regionguid = a.regionguid and b.roletype = 'deputy-regional-manager') as DeputyRegionalManager,
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where         b.regionguid = a.regionguid and b.roletype = 'regional-manager') as RegionalManager
                                    from Region a 
                                     WHERE RegionGuid = @RegionGuid;";
            var result = _context.Connection.QuerySingle<Region>(sql, new { RegionGuid = id });
            return result;
        }

        public int GetCount(string searchValue)
        {
            var searchString = string.Empty;
            var where = string.Empty;
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += "(RegionName like @searchValue or RegionCode like @searchValue )";
            }
            string sql = $"SELECT Count(1) FROM Region WHERE IsDeleted = 0 {where}";

            var result = _context.Connection.QuerySingle<int>(sql, new {searchValue= searchString });

            return result;
        }

        public int Edit(Region region)
        {
            string updateQuery = @"update Region set 
                                                                RegionName      =   @RegionName,
                                                                RegionCode      =   @RegionCode,
                                                               
                                                                IsActive        =   @IsActive,
                                                                IsDeleted       =   @IsDeleted,
                                                                UpdatedBy       =   @UpdatedBy,
                                                                UpdatedOn       =   @UpdatedOn
                                                                where RegionGuid =  @RegionGuid ";
            return _context.Connection.Execute(updateQuery, region);
        }

        public int CheckDuplicates(Region region)
        {
            string sql = $"SELECT Count(1) FROM Region WHERE (RegionCode = @RegionCode or RegionName = @RegionName) and RegionGuid != @RegionGuid and IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { RegionCode = region.RegionCode, RegionName = region.RegionName, RegionGuid = region.RegionGuid });
            return result;
        }

        public Region GetRegionByCode(string regionCode)
        {
            string regionSql = $@"select *, (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid                                   where b.regionguid = a.regionguid and b.roletype = 'BD-regional-manager') as BusinessDevelopmentRegionalManager, 
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where          b.regionguid = a.regionguid and b.roletype = 'HS-regional-manager') as HSRegionalManager,
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where         b.regionguid = a.regionguid and b.roletype = 'deputy-regional-Manager') as DeputyRegionalManager,
                                    (select top 1 c.userguid from regionuserrolemapping b join users c on b.userguid = c.userguid   where         b.regionguid = a.regionguid and b.roletype = 'regional-manager') as RegionalManager
                                    FROM Region a
                                    WHERE a.RegionCode = @regionCode";
            var region = _context.Connection.QueryFirstOrDefault<Region>(regionSql, new { regionCode = regionCode });
            return region;
        }

        public Region GetRegionByCodeOrName(string regionCode, string regionName)
        {
            string regionSql = @"SELECT * 
                                FROM Region
                                WHERE RegionCode = @regionCode
                                OR RegionName = @regionName";
            return _context.Connection.QueryFirstOrDefault<Region>(regionSql, new { regionCode = regionCode, regionName = regionName });
        }

        public int CheckDuplicateRegionByName(string regionName, Guid regionGuid)
        {
            string sql = $@"SELECT Count(1) 
                            FROM Region 
                            WHERE RegionName = @RegionName 
                            AND RegionGuid != @RegionGuid AND IsDeleted != 1 ";
            var result = _context.Connection.QuerySingle<int>(sql, new { RegionName = regionName, RegionGuid = regionGuid });
            return result;
        }

        public IEnumerable<Region> GetRegionList()
        {
            var sql = @"SELECT RegionName, RegionCode
                    FROM Region
                    WHERE IsActive = 1
                    ORDER BY RegionName";
            return _context.Connection.Query<Region>(sql);
        }

        public int DeleteById(Guid id)
        {
            string deleteQuery = @"Update Region set 
                                                                    IsDeleted   = 1
                                                                   
                                            where regionGuid =@RegionGuid ";
            string deleteManagerQuery = @"delete from RegionUserRoleMapping                                                                              where regionGuid =@regionGuid ";
            _context.Connection.Execute(deleteManagerQuery, new { RegionGuid = id });
            return _context.Connection.Execute(deleteQuery, new { RegionGuid = id });
            
        }
    }
}
