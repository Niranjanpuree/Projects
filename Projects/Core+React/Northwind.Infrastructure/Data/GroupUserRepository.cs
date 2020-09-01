using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data
{
    public class GroupUserRepository : IGroupUserRepository
    {
        public IDatabaseContext _context;
        private string saGroup = "SYSADMIN";

        public GroupUserRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<GroupUser> GetGroupUserByUserGUIDAndGroupGUID(Guid userGUID, Guid groupGUID)
        {
            return _context.Connection.Query<GroupUser>(@"SELECT
             [GroupUserGUID]
              ,[GroupGUID]
              ,[UserGUID]
           FROM [GroupUser] WHERE [UserGUID] = @UserGUID AND [GroupGUID] = @GroupGUID", new { UserGUID = userGUID, GroupGUID = groupGUID });
        }

        public IEnumerable<GroupUser> GetGroupUserByUserGUID(Guid userGUID)
        {
            return _context.Connection.Query<GroupUser>(@"SELECT
             a.[GroupUserGUID]
              ,a.[GroupGUID]
              ,a.[UserGUID]
              ,[GroupName]
            
           FROM [GroupUser] a join [group] b on a.GroupGUID = b.GroupGUID  WHERE [UserGUID] = @UserGUID", new { UserGUID = userGUID });
        }

        public IEnumerable<GroupPermission> GetGroupUserCountByUserGUID(Guid userGUID)
        {
            return _context.Connection.Query<GroupPermission>(@"select distinct ResourceActionGuid from GroupUser a  join GroupPermission b on a.GroupGuid = b.GroupGuid  WHERE [UserGUID] = @UserGUID", new { UserGUID = userGUID });
        }

        public IEnumerable<GroupUser> GetGroupUserByGroupGUID(Guid groupGUID)
        {
            return _context.Connection.Query<GroupUser>(@"SELECT
             [GroupUserGUID]
              ,[GroupGUID]
              ,[UserGUID]
           FROM [GroupUser] WHERE [GroupGUID] = @GroupGUID", new { GroupGUID = groupGUID });
        }

        public void Insert(GroupUser groupUser)
        {
            groupUser.GroupUserGUID = Guid.NewGuid();
            _context.Connection.Execute(@"INSERT INTO[GroupUser]
               ([GroupUserGUID]
              ,[GroupGUID]
              ,[UserGUID])
               VALUES
               ( @GroupUserGUID
               , @GroupGUID
               , @UserGUID)",
                groupUser
           );
        }

        public void Update(GroupUser groupUser)
        {
            groupUser.GroupUserGUID = Guid.NewGuid();
            _context.Connection.Execute(@"UPDATE [GroupUser]
               SET [GroupGUID] = @GroupGUID, [UserGUID] = @UserGUID
               WHERE [GroupUserGUID] = @GroupUserGUID",
                new
                {
                    groupUser.GroupUserGUID,
                    groupUser.GroupGUID,
                    groupUser.UserGUID
                }
           );
        }

        public void DeleteByUserId(Guid userGuid)
        {
            var sql = "DELETE FROM GroupUser WHERE [UserGUID] = @UserGUID";
            _context.Connection.Execute(sql, new { UserGUID = userGuid });
        }

        public void DeleteByGroupId(Guid groupGuid)
        {
            var sql = "DELETE FROM GroupUser WHERE [GroupGUID] = @GroupGUID";
            _context.Connection.Execute(sql, new { GroupGUID = groupGuid });
        }

        public GroupUser GetGroupUserByGroupUserGUID(Guid groupUserGUID)
        {
            return _context.Connection.QueryFirstOrDefault<GroupUser>(@"SELECT
             [GroupUserGUID]
              ,[GroupGUID]
              ,[UserGUID]
           FROM [GroupUser] WHERE [GroupUserGUID] = @GroupUserGUID", new { GroupUserGUID = groupUserGUID });
        }

        public void DeleteByGroupUserId(Guid groupUserGUID)
        {
            var sql = "DELETE FROM GroupUser WHERE [GroupUserGUID] = @GroupUserGUID";
            _context.Connection.Execute(sql, new { GroupUserGUID = groupUserGUID });
        }

        public IEnumerable<User> GetGroupUnassignedUserToGroup(Guid groupGUID, string searchValue, int take)
        {
            var sql = $@"SELECT u.* FROM Users u WHERE UserGuid Not In (SELECT gu.UserGuid FROM GroupUser gu WHERE gu.GroupGuid=@GroupGuid) AND
                        u.displayName like '%{searchValue}%'
                        ORDER BY u.DisplayName Asc  OFFSET 0 ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<User>(sql, new { GroupGuid = groupGUID });
        }
        public IEnumerable<Group> GetGroupUnassignedToUser(Guid userGUID, string searchValue, int take)
        {
            var sql = $@"SELECT g.* FROM [group] g WHERE GroupGuid Not In (SELECT gu.GroupGuid FROM GroupUser gu WHERE gu.userGuid=@userGUID) AND
                        g.GroupName like '%{searchValue}%'
                        ORDER BY g.GroupName Asc  OFFSET 0 ROWS FETCH NEXT {take} ROWS ONLY";
            return _context.Connection.Query<Group>(sql, new { userGuid = userGUID });
        }

        public void DeleteByGroupGuidUserGuid(Guid groupGUID, Guid userGuid)
        {
            var sql = "DELETE FROM GroupUser WHERE [GroupGUID] = @GroupGUID AND [UserGuid]=@UserGuid";
            _context.Connection.Execute(sql, new { GroupGUID = groupGUID, UserGuid = userGuid });
        }

        public bool IsUserInSAGroup(Guid userGuid)
        {
            var query = $@"SELECT COUNT(gu.UserGuid) 
                          FROM GroupUser gu
                          LEFT JOIN [Group] g
                          ON g.GroupGuid = gu.GroupGuid
                          WHERE g.GroupName = '{saGroup}'
                          AND gu.UserGuid = @userGuid";
            var userCount = _context.Connection.ExecuteScalar<int>(query, new { userGuid = userGuid });
            if (userCount > 0)
                return true;
            return false;
        }
    }
}

