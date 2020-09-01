using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Northwind.Core.Models;

namespace Northwind.Infrastructure.Data
{
    public class ESSGroupRepository : IGroupRepository
    {
        public IDatabaseContext _context;

        public ESSGroupRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void AssignPolicyToGroup(Guid groupGuid, Guid policyGuid)
        {
            var sql = @"INSERT INTO GroupPolicy(GroupPolicyGuid, GroupGuid, PolicyGuid) VALUES (@GroupPolicyGuid, @GroupGuid, @PolicyGuid)";
            _context.Connection.Execute(sql, new { GroupPolicyGuid=Guid.NewGuid(), GroupGuid = groupGuid, PolicyGuid = policyGuid });
        }

        public void Delete(Guid groupGuid)
        {
            _context.Connection.Execute(@"DELETE
           FROM [GroupPermission] WHERE [GroupGUID]=@GroupGUID", new { GroupGUID = groupGuid });
            _context.Connection.Execute(@"DELETE
           FROM [GroupUser] WHERE [GroupGUID]=@GroupGUID", new { GroupGUID = groupGuid });
            _context.Connection.Execute(@"DELETE
           FROM [Group] WHERE [GroupGUID]=@GroupGUID", new { GroupGUID = groupGuid });
        }

        public void Delete(List<Guid> groupGuid)
        {
            _context.Connection.Execute(@"DELETE
           FROM [Group] WHERE [GroupGUID] in @GroupGUID", new { GroupGUID = groupGuid });
        }

        public void Delete(List<Guid> userGuid,string groupGuid)
        {
            _context.Connection.Execute(@"DELETE
           FROM [GroupUser] WHERE [userGUID] in @userGuid and [GroupGuid] = @groupGuid", new { GroupGUID = groupGuid , userGuid= userGuid });
        }

        public void DeleteGroup(List<Guid> groupGuid, string userGuid)
        {
            _context.Connection.Execute(@"DELETE
           FROM [GroupUser] WHERE [GroupGuid] in @groupGuid and [userGuid] = @userGuid", new { GroupGUID = groupGuid, UserGuid = userGuid });
        }

        public IEnumerable<PolicyEntity> GetAssignedPolicyToGroup(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            if (string.IsNullOrEmpty(sortField))
                sortField = "p.name";
            string searchQuery = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
                searchQuery = " AND (p.name like @SearchValue OR p.title like @SearchValue OR p.description like @SearchValue)";
            }
            var sql = $@"SELECT p.* FROM Policy p inner join GroupPolicy gp on gp.policyGuid = p.policyGuid WHERE gp.groupGuid=@GroupGuid {searchQuery} ORDER BY {sortField} {dir} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            if (!string.IsNullOrEmpty(searchValue))
            {
                return _context.Connection.Query<PolicyEntity>(sql, new { GroupGuid = groupGuid, SearchValue = searchValue });
            }
            else
            {
                return _context.Connection.Query<PolicyEntity>(sql, new { GroupGuid = groupGuid });
            }
            
        }

        public int GetAssignedPolicyToGroupCount(Guid groupGuid, string searchValue)
        {
            string searchQuery = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchQuery = " AND (p.name like '%@SearchValue%' OR p.title like '%@SearchValue%' OR p.description like '%@SearchValue%')";
            }
            var sql = $@"SELECT COUNT(1) FROM Policy p inner join GroupPolicy gp on gp.policyGuid = p.policyGuid WHERE gp.groupGuid=@GroupGuid {searchQuery}";
            if (!string.IsNullOrEmpty(searchValue))
            {
                return _context.Connection.ExecuteScalar<int>(sql, new { GroupGuid = groupGuid, SearchValue = searchValue });
            }
            else
            {
                return _context.Connection.ExecuteScalar<int>(sql, new { GroupGuid = groupGuid });
            }
        }

        public IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(Guid groupGuid, Guid resourceGuid)
        {
            var sql = @"select RA.ActionGuid, RA.Title, RA.Name, Cast(count(GP.GroupGuid) as Bit) as Selected from ResourceActions RA 
                 left join GroupPermission GP on GP.ResourceActionGuid = RA.ActionGuid and GP.ResourceGuid=RA.ResourceGuid  and GP.GroupGuid=@GroupGuid
                 where RA.ResourceGuid=@ResourceGuid group by ActionGuid, Title, Name order by Title";
            return _context.Connection.Query<GroupResourceActionPermission>(sql, new { GroupGuid = groupGuid, ResourceGuid = resourceGuid });
        }

        public Group GetGroupByGUID(Guid guid)
        {
            return _context.Connection.QueryFirstOrDefault<Group>(@"SELECT
            [GroupGUID]
           ,[ParentGUID]
           ,[GroupName]
           ,[CN]
           ,[Description]
           FROM [Group] WHERE GroupGUID LIKE @GroupGuid", new { GroupGuid = guid });
        }

        public IEnumerable<Group> GetGroupByGUID(List<Guid> guids)
        {
            return _context.Connection.Query<Group>(@"SELECT
            [GroupGUID]
           ,[ParentGUID]
           ,[GroupName]
           ,[CN]
           ,[Description]
           FROM [Group] WHERE GroupGUID in @GroupGUID", new { GroupGUID = guids });
        }

        public IEnumerable<Group> GetGroupByName(string groupName)
        {
            return _context.Connection.Query<Group>(@"SELECT
            [GroupGUID]
           ,[ParentGUID]
           ,[GroupName]
           ,[CN]
           ,[Description]
           FROM [Group] WHERE GroupName LIKE @GroupName", new { GroupName = groupName});
        }

        public IEnumerable<GroupResourcePermission> GetGroupResources()
        {
            var sql = "select * from Resources R where name <> 'All' and (Select count(1) from ResourceActions RA where RA.ResourceGuid=R.ResourceGuid) > 0 order by Title";
            return _context.Connection.Query<GroupResourcePermission>(sql);
        }

        public IEnumerable<Group> GetGroups()
        {
            return _context.Connection.Query<Group>(@"SELECT
            [GroupGUID]
           ,[ParentGUID]
           ,[GroupName]
           ,[CN]
           ,[Description]
           FROM [Group]");
        }

        public IEnumerable<Group> GetGroups(string searchValue, int take, int skip, string sortField, string dir, List<AdvancedSearchRequest> postValue)
        {
            string searchString = "";

            var queryBuilder = new AdvancedSearchQueryBuilder(postValue);
            var query = queryBuilder.getQuery();
            var _builder = new SqlBuilder();
            
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "CN";
            }
            if (string.IsNullOrEmpty(dir))
            {
                dir = "ASC";
            }
            if(!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
               
                searchString = "%" + searchValue + "%";
                _builder.Where("[GroupName] LIKE @GroupName", new { GroupName = searchString });
                _builder.OrWhere("[CN] LIKE @GroupName", new { GroupName = searchString });
                _builder.OrWhere("[Description] LIKE @GroupName", new { GroupName = searchString });
            }
            var selector = _builder.AddTemplate(" /**where**/");
            foreach (dynamic d in query)
            {
                _builder.Where(d.sql, d.value);
            }

            return _context.Connection.Query<Group>("SELECT [GroupGUID] " +
           ",[ParentGUID]"+
           ",[GroupName]"+
           ",[CN]"+
           ",[Description] " +
           $"FROM [group] {selector.RawSql} " +
           $"ORDER BY {sortField} {dir} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY", selector.Parameters);
        }

        public int GetTotalRows(string searchValue, int take, int skip, List<AdvancedSearchRequest> postValue)
        {
            string searchParam = "";
            string searchString = "";
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = " WHERE [GroupName] LIKE @GroupName OR [CN] LIKE @GroupName OR [Description] LIKE @GroupName ";
            }            
            return _context.Connection.ExecuteScalar<int>(@"SELECT Count(1) " +
           $" FROM [group] {searchParam} ", new { GroupName = searchString });
        }

        public IEnumerable<PolicyEntity> GetUnassignedPolicyToGroup(Guid GroupGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            if (string.IsNullOrEmpty(sortField))
                sortField = "p.name";
            string searchQuery = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = "%" + searchValue + "%";
                searchQuery = " AND (p.name like @SearchValue OR p.title like @SearchValue OR p.description like @SearchValue)";
            }
            var sql = $@"SELECT p.* FROM Policy p WHERE p.policyGuid not in (SELECT policyGuid FROM GroupPolicy gp WHERE gp.groupGuid=@GroupGuid) {searchQuery} ORDER BY {sortField} {dir} OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY";
            if (!string.IsNullOrEmpty(searchValue))
            {
                return _context.Connection.Query<PolicyEntity>(sql, new { GroupGuid, SearchValue = searchValue });
            }
            else
            {
                return _context.Connection.Query<PolicyEntity>(sql, new { GroupGuid });
            }
        }

        public void Insert(Group group)
        {
            _context.Connection.Execute(@"INSERT INTO[Group]
               ([GroupGUID]
               ,[ParentGUID]
               ,[GroupName]
               ,[CN]
               ,[Description])
               VALUES
               (@GroupGuid
               , @ParentGuid
               , @GroupName
               , @CN
               , @Description)",
                new {
                    group.GroupGuid,
                    group.ParentGuid,
                    group.GroupName,
                    group.CN,
                    group.Description
                }
           );
        }

        public void UnassignPolicyToGroup(Guid groupGuid, Guid policyGuid)
        {
            var sql = @"DELETE FROM GroupPolicy WHERE GroupGuid = @GroupGuid AND PolicyGuid = @PolicyGuid";
            _context.Connection.Execute(sql, new { GroupGuid = groupGuid, PolicyGuid = policyGuid });
        }

        public void Update(Group group)
        {
            _context.Connection.Execute(@"UPDATE [Group]
                    SET [ParentGUID] = @ParentGuid
                        ,[GroupName] = @GroupName
                        ,[CN] = @CN
                        ,[Description] = @Description
                    WHERE GroupGUID=@GroupGUID",
                new
                {
                    group.GroupGuid,
                    group.ParentGuid,
                    group.GroupName,
                    group.CN,
                    group.Description
                }
           );
        }

        public void AssignGroupPermission(Guid groupGuid, List<string> actionGuid)
        {
            var sql = "DELETE FROM GroupPermission WHERE GroupGuid=@GroupGuid";
            _context.Connection.Execute(sql, new { GroupGuid = groupGuid });

            foreach(var g in actionGuid)
            {
                sql = "INSERT INTO GroupPermission (GroupGuid, ResourceGuid, ResourceActionGuid) Values(@GroupGuid, (SELECT top(1) ResourceGuid FROM ResourceActions WHERE ActionGuid=@ActionGuid), @ActionGuid)";
                _context.Connection.Execute(sql, new { GroupGuid = groupGuid, ActionGuid = g });
            }
        }

        public void RemoveUserGroup(Guid userid, Guid groupGuid)
        {
            var sql = "DELETE FROM GroupUser WHERE GroupGuid=@GroupGuid and userGuid =@userid";
            _context.Connection.Execute(sql, new { GroupGuid = groupGuid, @userid = @userid });
           
        }

        public Group GetGroupByUser(Guid userGuid)
        {
            var sql = @"select distinct g.* from [group] g
                        inner join GroupPermission gp on gp.GroupGuid=g.GroupGuid
                        inner join GroupUser gu on gu.GroupGuid=g.GroupGuid
                        Where gu.UserGuid=@UserGuid";
            return _context.Connection.QuerySingle<Group>(sql, new { UserGuid = userGuid });
        }

        public IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(List<Guid> groupGuids, Guid resourceGuid)
        {
            var sql = @"select RA.ActionGuid, RA.Title, RA.Name, Cast(count(GP.GroupGuid) as Bit) as Selected from ResourceActions RA 
                 left join GroupPermission GP on GP.ResourceActionGuid = RA.ActionGuid and GP.ResourceGuid=RA.ResourceGuid  and GP.GroupGuid in @GroupGuid
                 where RA.ResourceGuid=@ResourceGuid group by ActionGuid, Title, Name order by Title";
            return _context.Connection.Query<GroupResourceActionPermission>(sql, new { GroupGuid = groupGuids, ResourceGuid = resourceGuid });
        }

        public IEnumerable<GroupResourcePermission> GetGroupResources(Guid userGuid)
        {
            var sql = "select * from Resources R where name <> 'All' and (Select count(1) from ResourceActions RA where RA.ResourceGuid=R.ResourceGuid) > 0 and R.ResourceGuid in (SELECT ResourceGuid From GroupPermission GP, GroupUser GU Where GP.GroupGuid=GU.GroupGuid and GU.UserGuid=@UserGuid) order by Title";
            return _context.Connection.Query<GroupResourcePermission>(sql, new { UserGuid = userGuid });
        }
    }
}
