using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.DistributionUser
{
    public class DistributionUserRepository : IDistributionUserRepository
    {
        private readonly IDatabaseContext _context;

        public DistributionUserRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<User> GetUsersByDistributionListGuid(Guid distributionListGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "userName";
                dir = "asc";
            }
            string searchParam = "";
            string searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = $@" AND  users.Username     LIKE @searchValue 
                                    OR users.Lastname     LIKE @searchValue 
                                    OR users.Firstname    LIKE @searchValue 
                                    OR users.DisplayName  LIKE @searchValue 
                                    OR users.WorkEmail    LIKE @searchValue 
                                    OR users.WorkEmail    LIKE @searchValue";
            }
            var sql = $@"SELECT distinct users.* from users inner join DistributionUser 
                        on users.UserGuid = DistributionUser.UserGuid 
				        where DistributionListGuid = @distributionListGuid
                        {searchParam} and users.UserStatus = 'ENABLED'
                        order by users.{sortField} {dir}
                        OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(sql, new { searchValue = searchString, distributionListGuid = distributionListGuid });
        }

        public int GetSelectedUsersCount(Guid distributionListGuid, string searchValue)
        {
            string searchParam = "";
            string searchString = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = $@" AND  users.Username     LIKE @searchValue 
                                    OR users.Lastname     LIKE @searchValue 
                                    OR users.Firstname    LIKE @searchValue 
                                    OR users.DisplayName  LIKE @searchValue 
                                    OR users.WorkEmail    LIKE @searchValue 
                                    OR users.WorkEmail    LIKE @searchValue";
            }

            var sql = $@"SELECT Count(distinct users.WorkEmail) from users inner join DistributionUser 
                        on users.UserGuid = DistributionUser.UserGuid 
				        where DistributionListGuid = @distributionListGuid
                        {searchParam} and users.UserStatus = 'ENABLED'";
            return _context.Connection.ExecuteScalar<int>(sql, new { searchValue = searchString, distributionListGuid = distributionListGuid });
        }

        public IEnumerable<User> GetUsersExceptDistributionUser(Guid? distributionListGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            string searchParam = "";
            string searchString = "";

            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "Username";
                dir = "Asc";
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = $@" and ( Username    LIKE @searchValue 
                                    OR Lastname     LIKE @searchValue 
                                    OR Firstname    LIKE @searchValue 
                                    OR GivenName    LIKE @searchValue 
                                    OR DisplayName  LIKE @searchValue 
                                    OR WorkEmail    LIKE @searchValue )";
            }

            var query = $@"
                           select * from users where userGuid not in 
                                    ( select userGuid from distributionUser where distributionListGuid = @distributionListGuid)
                                    {searchParam} and  WorkEmail is not null and FirstName is not null
                                    ORDER BY {sortField}  {dir} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(query,
                new { searchValue = searchString, distributionListGuid = distributionListGuid });
        }

        public int GetUserCountExceptDistributionUser(Guid distributionListGuid, string searchValue)
        {
            string searchParam = "";
            string searchString = "";

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = $@" AND  Username     LIKE @searchValue 
                                    OR Lastname     LIKE @searchValue 
                                    OR Firstname    LIKE @searchValue 
                                    OR GivenName    LIKE @searchValue 
                                    OR DisplayName  LIKE @searchValue 
                                    OR WorkEmail    LIKE @searchValue 
                                    OR WorkEmail    LIKE @searchValue";
            }

            var query = $@" select count(1) from users where userGuid not in 
                                    ( select userGuid from distributionUser where distributionListGuid = @distributionListGuid)
                         {searchParam} ";
            return _context.Connection.ExecuteScalar<int>(query,
                new { searchValue = searchString, distributionListGuid = distributionListGuid });
        }

        public int GetDistributionUserCountByDistributionListId(Guid distributionListGuid)
        {
            var sql = $@"Select count(1) from DistributionUser where DistributionListGuid = @distributionListGuid";
            return _context.Connection.ExecuteScalar<int>(sql,
                new { distributionListGuid = distributionListGuid });
        }

        public int RemoveMemberFromDistributionList(Guid DistributioinListId, Guid UserId)
        {
            var sql = $@"Delete from DistributionUser where DistributionListGuid = @distributionListGuid and UserGuid = @UserId";
            return _context.Connection.ExecuteScalar<int>(sql,
                new { distributionListGuid = DistributioinListId, UserId = UserId });
        }
    }
}
