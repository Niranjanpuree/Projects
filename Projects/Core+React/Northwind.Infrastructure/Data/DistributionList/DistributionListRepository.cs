using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.DistributionUser
{
    public class DistributionListRepository : IDistributionListRepository
    {
        private readonly IDatabaseContext _context;

        public DistributionListRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<DistributionList> Get(Guid loggedUser, string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var where = "";
            var searchString = "";
            var sql = string.Empty;
            if (string.IsNullOrEmpty(orderBy))
            {
                orderBy = "title";
            }
            else if (orderBy.ToLower().Contains("updatedon"))
            {
                orderBy = "UpdatedOn";
            }
            else if (orderBy.ToLower().Contains("ispublic"))
            {
                orderBy = "IsPublic";
            }
            else if (orderBy.ToLower().Contains("createdby"))
            {
                orderBy = "Users.FirstName";
            }
            if (take == 0)
            {
                take = 10;
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (title LIKE @searchValue AND isDeleted = 0)";
            }
            if (orderBy.ToLower().Contains("membercount"))
            {

                orderBy = "DistributionListWithMemberCount.MemberCount";

                sql = @"
                 select DistributionListWithMemberCount.* from
					  (select a.DistributionListGuid,a.Title,a.IsActive,a.CreatedBy,a.CreatedOn,a.IsDeleted,a.UpdatedBy,a.UpdatedOn,a.IsPublic,count(u.DistributionListGuid) MemberCount 
                                from DistributionList a  left join DistributionUser  u 
                                on a.DistributionListGuid = u.DistributionListGuid
                                group by a.Title,a.DistributionListGuid,u.DistributionListGuid,a.Title,a.IsActive,a.CreatedBy,a.CreatedOn,a.IsDeleted,a.UpdatedBy,a.UpdatedOn,a.IsPublic
                        ) as DistributionListWithMemberCount
                   join users on DistributionListWithMemberCount.CreatedBy = Users.UserGuid where isDeleted = 0 and ((CreatedBy = @loggedUser and IsPublic = 0) or IsPublic = 1)";
            }
            else
            {
                sql = @"
                  select * from DistributionList join users on DistributionList.CreatedBy = Users.UserGuid where isDeleted = 0 and ((CreatedBy = @loggedUser and IsPublic = 0) or IsPublic = 1) ";
            }

            sql += $"{ where }";
            sql += $" ORDER BY {orderBy} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<DistributionList>(sql, new { loggedUser = loggedUser, searchValue = searchString });
        }

        public IEnumerable<DistributionList> GetDistributionListByLoggedUser(Guid loggedUser, string searchValue)
        {
            var sql = $@"
                  select * from DistributionList where isDeleted = 0 and ((CreatedBy = @loggedUser and IsPublic = 0) or IsPublic = 1)  ";
            //            var sql = $@"
            //                  select * from DistributionList where isDeleted = 0 and ((CreatedBy = @loggedUser and IsPublic = 0) or IsPublic = 1) and title like '%@searchValue%' ";
            return _context.Connection.Query<DistributionList>(sql, new { loggedUser = loggedUser, searchValue = searchValue });
        }

        public int GetCount(Guid loggedUser, string searchValue)
        {
            var where = "";
            var searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                where = " AND ";
                where += " (title LIKE @searchValue AND isDeleted = 0)";
            }

            var sql = @"
                  select count(1) from DistributionList where isDeleted = 0 and (CreatedBy = @loggedUser and IsPublic = 0) or IsPublic = 1 ";
            sql += $"{ where }";
            return _context.Connection.ExecuteScalar<int>(sql, new { loggedUser = loggedUser, searchValue = searchString });
        }

        public int DeleteDistributionUserByDistributionListId(Guid distributionListGuid)
        {
            var sql = "DELETE FROM DistributionUser WHERE distributionListGuid = @distributionListGuid";
            return _context.Connection.Execute(sql, new { distributionListGuid = distributionListGuid });
        }

        public bool HasDistributionList(Guid loggedUser)
        {
            var sql = "Select count(1) from  DistributionList WHERE CreatedBy = @loggedUser and IsDeleted = 0";
            var result = _context.Connection.ExecuteScalar<int>(sql, new { loggedUser = loggedUser });

            if (result > 0)
            {
                return true;
            }
            else return false;
        }

        public bool IsDuplicateTitle(string distributionTitle)
        {
            var sql = "Select count(1) from  DistributionList WHERE LOWER(Title) = @title and IsDeleted = 0";
            var result = _context.Connection.ExecuteScalar<int>(sql, new { title = distributionTitle.Trim().ToLower() });

            if (result > 0)
            {
                return true;
            }
            else return false;
        }

        public int Add(DistributionList d)
        {
            var sql = @"INSERT INTO dbo.[DistributionList]
           (DistributionListGuid        ,
           Name                         ,
           Title                        ,
           IsPublic                     ,
           IsActive                     ,
           IsDeleted                    ,
           CreatedOn                    ,
           UpdatedOn                    ,
           CreatedBy                    ,
           UpdatedBy)
            VALUES
              (@DistributionListGuid     ,
               @Name                     ,
               @Title                    ,
               @IsPublic                 ,
               @IsActive                 ,
               @IsDeleted                ,
               @CreatedOn                ,
               @UpdatedOn                ,
               @CreatedBy                ,
               @UpdatedBy)";
            return _context.Connection.Execute(sql, d);
        }

        public int Edit(DistributionList d)
        {
            var sql = @"UPDATE DistributionList
                        SET Title = @Title, IsPublic =@IsPublic,UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy  
                         WHERE DistributionListGuid = @distributionListGuid";
            return _context.Connection.Execute(sql, d);
        }

        public DistributionList GetDistributionListById(Guid id)
        {
            var sql = @"select * from distributionList where distributionListGuid = @distributionListGuid";
            return _context.Connection.QueryFirstOrDefault<DistributionList>(sql, new { distributionListGuid = id });
        }

        public IEnumerable<Core.Entities.DistributionUser> GetDistributionUsersById(Guid id)
        {
            var sql = @"select * from distributionUser where distributionListGuid = @distributionListGuid";
            return _context.Connection.Query<Core.Entities.DistributionUser>(sql, new { distributionListGuid = id });

        }

        public int GetUserCount(string searchValue)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " WHERE U.[Username] LIKE @Username OR U.[Lastname] LIKE @Username OR U.[Firstname] LIKE @Username OR U.[Givenname] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[Company] LIKE @Username OR Manager LIKE @Username  OR Department LIKE @Username OR u.[Group] LIKE @Username ";
            }

            return _context.Connection.ExecuteScalar<int>(@"Select Count(1) from (SELECT U.[UserGuid]
           ,U.[Username]
           ,U.[Lastname]
           ,U.[Firstname]
           ,U.[Givenname]
           ,U.[Displayname]
           ,U.[UserStatus]
           ,U.[WorkEmail]
           ,U.[PersonalEmail]
           ,U.[WorkPhone]
           ,U.[HomePhone]
           ,U.[JobStatus]
           ,Manager.[Displayname] As Manager
           ,U.[MobilePhone]
           ,U.[JobTitle]
           ,U.[Company]
           ,STUFF((SELECT ', ' + gp.CN 
              FROM [Group] gp left join GroupUser gu on gu.UserGUID = U.UserGUID
              WHERE gp.GroupGUID = gu.GroupGUID
              ORDER BY gp.CN
              FOR XML PATH('')), 1, 1, '') as [Group]
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U " +
                                                          $" {searchParam} ", new { Username = searchString });
        }

        public int AddDistributionUser(Core.Entities.DistributionUser du)
        {
            var sql = @"INSERT INTO dbo.DistributionUser
                           (DistributionUserGuid   ,
                           DistributionListGuid    ,
                           UserGuid                ,
                           CreatedOn               ,
                           CreatedBy              )
                            VALUES
                               (@DistributionUserGuid   ,
                                @DistributionListGuid   ,
                                @UserGuid               ,
                                @CreatedOn              ,
                                @CreatedBy              )";
            return _context.Connection.Execute(sql, du);
        }

        public int Delete(Guid distributionListGuid)
        {
            var sql = "update DistributionList set isDeleted = 1 WHERE distributionListGuid = @distributionListGuid";
            return _context.Connection.Execute(sql, new { distributionListGuid = distributionListGuid });
        }
    }
}
