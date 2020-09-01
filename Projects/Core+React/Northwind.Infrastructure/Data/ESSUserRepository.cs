using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;
using Northwind.Core.Specifications;
using System.Linq;
using System.Dynamic;

namespace Northwind.Infrastructure.Data
{
    public class ESSUserRepository : IUserRepository
    {
        public IDatabaseContext _context;

        public ESSUserRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Find(UserSearchSpec spec)
        {
            var o = new Dictionary<string, object>();
            var sql = BuildSql(spec, out o);
            if (sql == string.Empty) throw new Exception("Empty Sql");
            return _context.Connection.Query<User>(sql, new DynamicParameters(o));

        }

        private string BuildSql(UserSearchSpec spec, out Dictionary<string, object> o)
        {
            var sql = string.Empty;
            var listOfAttributes = string.Join(",", spec.AttributesToReturn);
            var stringBuilder = new StringBuilder();
            var dict = new Dictionary<string, object>();
            stringBuilder.AppendLine($"SELECT {listOfAttributes} FROM dbo.Users ");
            if (spec.Criteria.Count > 0)
            {
                stringBuilder.AppendLine(" WHERE ");
            }
            foreach (var criteria in spec.Criteria)
            {

                switch (criteria.Operator)
                {
                    case OperatorName.StringEquals:

                        stringBuilder.AppendLine("(");
                        var criteriaBuilder = new List<string>();
                        var i = 0;
                        foreach (var value in criteria.Value)
                        {
                            criteriaBuilder.Add($"{criteria.Attribute.Name}=@{criteria.Attribute.Name}{i}");
                            dict.Add($"@{criteria.Attribute.Name}{i}", value.ToString());
                            ++i;
                        }
                        stringBuilder.Append(string.Join(" OR ", criteriaBuilder));
                        stringBuilder.AppendLine(")");

                        break;
                    default:
                        break;
                }
            }
            o = dict;
            return stringBuilder.ToString();

        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Connection.Query<User>(@"SELECT [UserGuid]
           ,[Username]
           ,[Lastname]
           ,[Firstname]
           ,[Givenname]
           ,[Displayname]
           ,[UserStatus]
           ,[WorkEmail]
           ,[PersonalEmail]
           ,[WorkPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[JobTitle]
           ,[Company]
           ,[Department] FROM dbo.[Users] where Displayname is not null  ORDER BY Displayname");
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            string searchParam = "";
            string searchString = "";
            if (!string.IsNullOrEmpty(searchText))
            {
                searchString = "%" + searchText + "%";
                searchParam = "and FirstName like @searchString or LastName like @searchString";
            }
            var usersQuery = string.Format($@"
               select * from Users
                where 1= 1 {searchParam} and Users.UserStatus = 'ENABLED' and Users.Username is not null and Users.LastName is not null
                order by Firstname asc");
            var usersData = _context.Connection.Query<User>(usersQuery, new { searchString = searchString }).ToList();
            return usersData;
        }

        public IEnumerable<User> GetUsersByUsername(List<string> usernames)
        {

            return _context.Connection.Query<User>(@"SELECT [UserGuid]
           ,[Username]
           ,[Lastname]
           ,[Firstname]
           ,[Givenname]
           ,[Displayname]
           ,[UserStatus]
           ,[WorkEmail]
           ,[PersonalEmail]
           ,[WorkPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[JobTitle]
           ,[Company]
           ,[Department]  FROM dbo.[Users] WHERE Username in @usernames", new { usernames = usernames });
        }

        public Guid Insert(User u)
        {
            var UserGuid = u.UserGuid == Guid.Empty ? Guid.NewGuid() : u.UserGuid;

            _context.Connection.Execute(@"INSERT INTO dbo.[Users]
           ([UserGuid]
           ,[Username]
           ,[Lastname]
           ,[Firstname]
           ,[Givenname]
           ,[Displayname]
           ,[UserStatus]
           ,[WorkEmail]
           ,[PersonalEmail]
           ,[WorkPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[JobTitle]
           ,[JobStatus]
           ,[Company]
           ,[Department])
            VALUES
           (@UserGuid
           ,@Username
           ,@Lastname
           ,@Firstname
           ,@Givenname 
           ,@Displayname
           ,@UserStatus
           ,@WorkEmail 
           ,@PersonalEmail
           ,@WorkPhone 
           ,@HomePhone 
           ,@MobilePhone
           ,@JobTitle 
           ,@JobStatus 
           ,@Company 
           ,@Department) ",
           new
           {
               UserGuid,
               u.Username,
               u.Lastname,
               u.Firstname,
               u.Givenname,
               u.DisplayName,
               u.UserStatus,
               u.WorkEmail,
               u.PersonalEmail,
               u.WorkPhone,
               u.HomePhone,
               u.MobilePhone,
               u.JobTitle,
               u.JobStatus,
               u.Department,
               u.Company
           });
            return UserGuid;
        }

        public void Update(User u)
        {
            var userid = u.UserGuid;
            if (userid == Guid.Empty)
            {
                _context.Connection.Execute(@"UPDATE [dbo].[Users]
           SET 
              [Lastname] = @Lastname
              ,[Firstname] = @Firstname
              ,[Givenname] = @Givenname
              ,[Displayname] = @Displayname
              ,[UserStatus] = @UserStatus
              ,[WorkEmail] = @WorkEmail
              ,[PersonalEmail] = @PersonalEmail
              ,[WorkPhone] = @WorkPhone
              ,[HomePhone] = @HomePhone
              ,[MobilePhone] = @MobilePhone
              ,[JobTitle] = @JobTitle
              ,[Company] = @Company
              ,[JobStatus] = @JobStatus
              ,[Department] = @Department
         WHERE Username = @Username", new
                {

                    u.Lastname,
                    u.Firstname,
                    u.Givenname,
                    u.DisplayName,
                    u.UserStatus,
                    u.WorkEmail,
                    u.PersonalEmail,
                    u.WorkPhone,
                    u.HomePhone,
                    u.MobilePhone,
                    u.JobTitle,
                    u.Company,
                    u.Department,
                    u.Username,
                    u.JobStatus
                });
            }
            else
            {
                _context.Connection.Execute(@"UPDATE [dbo].[Users]
           SET 
              UserGuid = @UserGuid
              ,[Lastname] = @Lastname
              ,[Firstname] = @Firstname
              ,[Givenname] = @Givenname
              ,[Displayname] = @Displayname
              ,[UserStatus] = @UserStatus
              ,[WorkEmail] = @WorkEmail
              ,[PersonalEmail] = @PersonalEmail
              ,[WorkPhone] = @WorkPhone
              ,[HomePhone] = @HomePhone
              ,[MobilePhone] = @MobilePhone
              ,[JobTitle] = @JobTitle
              ,[JobStatus] = @JobStatus
              ,[Company] = @Company
              ,[Department] = @Department
         WHERE [Username] = @Username", new
                {
                    u.UserGuid,
                    u.Lastname,
                    u.Firstname,
                    u.Givenname,
                    u.DisplayName,
                    u.UserStatus,
                    u.WorkEmail,
                    u.PersonalEmail,
                    u.WorkPhone,
                    u.HomePhone,
                    u.MobilePhone,
                    u.JobTitle,
                    u.Company,
                    u.Department,
                    u.Username,
                    u.JobStatus
                });
            }
        }

        public IEnumerable<User> GetUsers(string searchValue, int take, int skip, string sortField, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "[FirstName]";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " and U.[Username] LIKE @Username OR U.[Lastname] LIKE @Username OR U.[Firstname] LIKE @Username OR U.[Givenname] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[Company] LIKE @Username OR Manager LIKE @Username  OR Department LIKE @Username OR u.[Group] LIKE @Username ";
            }

            var query = $@"Select * from (SELECT U.[UserGuid]
           ,U.[Username]
           ,U.[LastName]
           ,U.[FirstName]
           ,U.[GivenName]
           ,U.[DisplayName]
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
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U
            where 1 = 1 {searchParam} and U.UserStatus = 'ENABLED' and U.WorkEmail is not null and U.FirstName is not null ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(query, new { Username = searchString });
        }

        public int GetUserCount(string searchValue)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " and U.[Username] LIKE @Username OR U.[Lastname] LIKE @Username OR U.[Firstname] LIKE @Username OR U.[Givenname] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[Company] LIKE @Username OR Manager LIKE @Username  OR Department LIKE @Username OR u.[Group] LIKE @Username ";
            }


            return _context.Connection.ExecuteScalar<int>($@"Select Count(1) from (SELECT U.[UserGuid]
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
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U 
            where 1 = 1 {searchParam} and U.UserStatus = 'ENABLED' and U.WorkEmail is not null and U.FirstName is not null ", new { Username = searchString });
        }

        public User GetUserByUserGuid(Guid userGuid)
        {
            return _context.Connection.Query<User>(@"SELECT [UserGuid]
           ,[Username]
           ,[Lastname]
           ,[Firstname]
           ,[Givenname]
           ,[Displayname]
           ,[UserStatus]
           ,[WorkEmail]
           ,[PersonalEmail]
           ,[WorkPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[JobTitle]
           ,[Company]
           ,[JobStatus]
           ,[Department] FROM [Users] WHERE UserGuid=@UserGuid", new { UserGuid = userGuid }).SingleOrDefault();
        }
        public User GetEmployeeDirectoryByUserGuid(Guid userGuid)
        {
            var sql = $@"Select * from (SELECT U.[UserGuid]
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
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U
            where U.UserGuid=@UserGuid";

            return _context.Connection.Query<User>(sql, new { UserGuid = userGuid }).SingleOrDefault();
        }

        public void DeleteByUserId(Guid userGuid)
        {
            var sql = "DELETE FROM Users WHERE [UserGuid]=@UserGuid";
            _context.Connection.Execute(sql, new { UserGuid = userGuid });
        }

        public IEnumerable<User> GetUserByUserGuid(List<Guid> userGuid)
        {
            return _context.Connection.Query<User>(@"SELECT [UserGuid]
           ,[Username]
           ,[Lastname]
           ,[Firstname]
           ,[Givenname]
           ,[Displayname]
           ,[UserStatus]
           ,[WorkEmail]
           ,[PersonalEmail]
           ,[WorkPhone]
           ,[HomePhone]
           ,[MobilePhone]
           ,[JobTitle]
           ,[Company]
           ,[JobStatus]
           ,[Department] FROM [Users] WHERE UserGuid IN @UserGuid", new { UserGuid = userGuid });
        }

        public void Delete(List<Guid> userGuids)
        {
            _context.Connection.Execute(@"DELETE FROM [Users] WHERE UserGuid in @UserGuid ", new { UserGuid = userGuids });
        }

        public IEnumerable<User> GetUsers(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "[Firstname]";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND (U.[Username] LIKE @Username OR U.[Lastname] LIKE @Username OR U.[Firstname] LIKE @Username OR U.[Givenname] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[Company] LIKE @Username OR Manager LIKE @Username  OR Department LIKE @Username OR u.[Group] LIKE @Username) ";
            }
            var query = @"Select *,Firstname + ' ' + Lastname Name  from (SELECT U.[UserGuid]
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
           $" WHERE U.[UserGuid] in @UserGuids {searchParam} and U.UserStatus = 'ENABLED' ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(query, new { UserGuids = userGuid, Username = searchString });
        }

        public int GetUserCount(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "CN";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND (U.[Username] LIKE @Username OR U.[Lastname] LIKE @Username OR U.[Firstname] LIKE @Username OR U.[Givenname] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[WorkEmail] LIKE @Username OR U.[Company] LIKE @Username OR Manager LIKE @Username  OR Department LIKE @Username OR u.[Group] LIKE @Username) ";
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
           $" WHERE U.[UserGuid] in @UserGuids {searchParam} and U.UserStatus = 'ENABLED' ", new { UserGuids = userGuid, Username = searchString });
        }

        public IEnumerable<User> GetEmployeeDirectoryList(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            string searchParam = "";
            string searchString = "";
            string sortBy = "";
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "[Firstname]";
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
                            searchParam = "  U.[WorkEmail] LIKE @Username or U.PersonalEmail like  @Username or U.[Firstname] LIKE @Username or U.DisplayName like  @Username and ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            searchParam = "  (U.[Firstname] LIKE @sortBy and (U.[Firstname] LIKE @Username or U.DisplayName like  @Username or U.[WorkEmail] LIKE @Username or U.PersonalEmail like  @Username)  ) and ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            searchParam = "  (U.[Firstname] LIKE @Username or U.DisplayName like  @Username) and ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    searchParam = " and U.[Firstname] LIKE @Username  ";
                    filterBy = null;
                }
            }
            var orderBy = string.Empty;
            if (take == 1000)
                orderBy += $" ORDER BY {sortField} {dir}";
            else
                orderBy += $" ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";

            var query = $@"Select * from (SELECT U.[UserGuid]
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
            ,U.[Extension]
           ,STUFF((SELECT ', ' + gp.CN 
              FROM [Group] gp left join GroupUser gu on gu.UserGUID = U.UserGUID
              WHERE gp.GroupGUID = gu.GroupGUID
              ORDER BY gp.CN
              FOR XML PATH('')), 1, 1, '') as [Group]
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U
            where {searchParam}  U.UserStatus = 'ENABLED' and U.FirstName is not null {orderBy}";
            return _context.Connection.Query<User>(query, new { Username = searchString, sortBy = sortBy });
        }

        public int GetEmployeeDirectoryCount(string searchValue, string filterBy)
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
                            searchParam = "  U.[WorkEmail] LIKE @Username or U.PersonalEmail like  @Username or U.[Firstname] LIKE @Username or U.DisplayName like  @Username and ";
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(searchValue))
                        {
                            searchString = "%" + searchValue + "%";
                            sortBy = filterBy + "%";
                            searchParam = "  (U.[Firstname] LIKE @sortBy and (U.[Firstname] LIKE @Username or U.DisplayName like  @Username or U.[WorkEmail] LIKE @Username or U.PersonalEmail like  @Username)  ) and ";
                        }
                        else
                        {
                            searchString = filterBy + "%";
                            searchParam = "  (U.[Firstname] LIKE @Username or U.DisplayName like  @Username) and ";
                        }
                        break;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchString = "%" + searchValue + "%";
                    searchParam = "  U.[Firstname] LIKE @Username or U.DisplayName like  @Username and  ";
                    filterBy = null;
                }
            }
            var query = $@"Select count(*) from (SELECT U.[UserGuid]
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
            ,U.[Extension]
           ,STUFF((SELECT ', ' + gp.CN 
              FROM [Group] gp left join GroupUser gu on gu.UserGUID = U.UserGUID
              WHERE gp.GroupGUID = gu.GroupGUID
              ORDER BY gp.CN
              FOR XML PATH('')), 1, 1, '') as [Group]
           ,U.[Department] FROM [Users] U LEFT JOIN ManagerUser MU ON MU.UserGUID = U.UserGUID LEFT JOIN [Users] Manager ON Manager.UserGUID=MU.ManagerGUID) as U
            where  {searchParam}  U.UserStatus = 'ENABLED' and U.FirstName is not null";
            return _context.Connection.ExecuteScalar<int>(query, new { Username = searchString, sortBy = sortBy });
        }

        public User GetUserByDisplayName(string displayName)
        {
            var sql = @"SELECT * 
                        FROM [Users]
                        WHERE DisplayName = @displayName";
            return _context.Connection.QueryFirstOrDefault<User>(sql, new { displayName = displayName });
        }

        public int GetUsersCountPerson(string searchValue)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND (U.[Username] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR Department LIKE @Username) ";
            }
            var query = @"Select Count(1) as CNT FROM [Users] U " +
            $" WHERE U.DisplayName <> '' AND U.FirstName <> '' AND U.LastName <> '' and U.UserStatus = 'ENABLED' {searchParam}";
            return _context.Connection.ExecuteScalar<int>(query, new { Username = searchString });
        }

        public IEnumerable<User> GetUsersPerson(string searchValue, int take, int skip, string sortField, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "[Displayname]";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " AND (U.[Username] LIKE @Username OR U.[Displayname] LIKE @Username OR U.[WorkEmail] LIKE @Username OR Department LIKE @Username) ";
            }
            var query = @"Select Distinct [Username],[Lastname],[Firstname],[Displayname],[WorkEmail],[WorkPhone],[JobTitle],[Department] from (SELECT U.[UserGuid]
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
           ,U.[MobilePhone]
           ,U.[JobTitle]
           ,U.[Company]
           ,U.[Department] FROM [Users] U) as U " +
           $" WHERE U.DisplayName <> '' AND U.FirstName <> '' AND U.LastName <> '' and U.UserStatus = 'ENABLED' {searchParam} ORDER BY {sortField} {dir}  OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(query, new { Username = searchString });
        }
        public User GetUserByUsername(string username)
        {
            var sql = @"SELECT *
                        FROM [Users]
                        WHERE Username = @username";
            return _context.Connection.QueryFirstOrDefault<User>(sql, new { username = username });
        }

        public User GetUserByFirstAndLastName(string firstName, string lastName)
        {
            var query = @"SELECT * 
                        FROM Users
                        WHERE (Firstname = @firstName and Lastname = @lastName) 
                        OR (Firstname = @lastName and Lastname = @firstName)";
            return _context.Connection.QueryFirstOrDefault<User>(query, new { firstName = firstName, lastName = lastName });
        }

        public User GetUserByWorkEmail(string workEmail)
        {
            var query = @"SELECT * 
                        FROM Users
                        WHERE WorkEmail = @workEmail";
            return _context.Connection.QueryFirstOrDefault<User>(query, new { workEmail = workEmail });
        }
    }
}
