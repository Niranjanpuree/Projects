using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace Northwind.Infrastructure.Data
{
    public class GroupPermissionRepository : IGroupPermissionRepository
    {
        IDatabaseSingletonContext _context;

        public GroupPermissionRepository(IDatabaseSingletonContext context)
        {
            _context = context;
        }

        public void DeleteGroupPermission(List<GroupPermission> permissions)
        {
            foreach(var g in permissions)
            {
                DeleteGroupPermission(g);
            }
        }

        public void DeleteGroupPermission(GroupPermission permission)
        {
            var sql = "DELETE FROM GroupPermission WHERE GroupPermissionGuid=@GroupPermissionGuid";
            _context.Connection.Execute(sql, permission);
        }

        public IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid)
        {
            var sql = "SELECT * FROM GroupPermission WHERE GroupGuid=@GroupGuid";
            return _context.Connection.Query<GroupPermission>(sql, new { GroupGuid = groupGuid });
        }

        public IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid, Guid resourceGuid)
        {
            var sql = "SELECT * FROM GroupPermission WHERE GroupGuid=@GroupGuid AND ResourceGuid=@ResourceGuid";
            return _context.Connection.Query<GroupPermission>(sql, new { GroupGuid = groupGuid, ResourceGuid = resourceGuid });
        }

        public GroupPermission GetGroupPermission(Guid groupGuid, Guid resourceGuid, Guid resourceActionGuid)
        {
            var sql = "SELECT * FROM GroupPermission WHERE GroupGuid=@GroupGuid AND ResourceGuid=@ResourceGuid AND ResourceActionGuid=@ResourceActionGuid";
            return _context.Connection.QuerySingle<GroupPermission>(sql, new { GroupGuid = groupGuid, ResourceGuid = resourceGuid, ResourceActionGuid = resourceActionGuid });
        }

        public bool IsUserPermitted(Guid userGuid, EnumGlobal.ResourceType resourceType, EnumGlobal.ResourceActionPermission permission)
        {
            //Todo: Due to singleton issue, need to research
            
            using(var conn = new ESSSingletonDbContext(_context.ConnectionString))
            {
                var sql = @"select gp.* from GroupPermission gp
                        inner join Resources r on r.ResourceGuid=gp.ResourceGuid
                        inner join ResourceActions ra on ra.ResourceGuid=gp.ResourceGuid and ra.ActionGuid=gp.ResourceActionGuid
                        inner join [Group] g on g.GroupGuid=gp.GroupGuid
                        inner join [GroupUser] gu on g.GroupGuid=gu.GroupGuid
                        Where gu.UserGuid=@UserGuid and r.Name=@ResourceType and ra.ResourceAction=@ResourceAction";
                var result = conn.Connection.Query<GroupPermission>(sql, new { UserGuid = userGuid, ResourceType = resourceType.ToString(), ResourceAction = permission.ToString() });
                if (result.AsList().Count > 0)
                {
                    conn.Connection.Close();
                    return true;
                }
                else
                {
                    conn.Connection.Close();
                    return false;
                }
            }            
            
           // return true;
        }

        public bool IsUserPermitted(Guid userGuid, string resourceType, string permission)
        {
            //Todo: Due to singleton issue, need to research

            var sql = @"select gp.* from GroupPermission gp
                        inner join Resources r on r.ResourceGuid=gp.ResourceGuid
                        inner join ResourceActions ra on ra.ResourceGuid=gp.ResourceGuid and ra.ActionGuid=gp.ResourceActionGuid
                        inner join [Group] g on g.GroupGuid=gp.GroupGuid
                        inner join [GroupUser] gu on g.GroupGuid=gu.GroupGuid
                        Where gu.UserGuid=@UserGuid and r.Name=@ResourceType and ra.ResourceAction=@ResourceAction";
            var result = _context.Connection.Query<GroupPermission>(sql, new { UserGuid = userGuid, ResourceType = resourceType, ResourceAction = permission });
            if (result.AsList().Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            // return true;
        }

        public void UpdateGroupPermission(List<GroupPermission> permissions)
        {
            var sql = "Delete from GroupPermission WHERE GroupGuid=@GroupGuid AND ResourceGuid=@ResourceGuid AND ResourceActionGuid=@ResourceActionGuid";
            _context.Connection.Execute(sql);
            foreach(var permission in permissions)
            {
                sql = @"INSERT INTO [dbo].[GroupPermission]
                                   ([GroupGuid]
                                   ,[ResourceGuid]
                                   ,[ResourceActionGuid])
                             VALUES
                                   (@GroupGuid
                                   ,@ResourceGuid
                                   ,@ResourceActionGuid)";
                _context.Connection.Execute(sql, permission);
            }
        }

        public void InsertGroupPermission(GroupPermission groupPermission)
        {
            var sql = @"INSERT INTO [dbo].[GroupPermission]
                                   ([GroupGuid]
                                   ,[ResourceGuid]
                                   ,[ResourceActionGuid])
                             VALUES
                                   (@GroupGuid
                                   ,@ResourceGuid
                                   ,@ResourceActionGuid)";
            _context.Connection.Execute(sql, groupPermission);
        }
    }
}
