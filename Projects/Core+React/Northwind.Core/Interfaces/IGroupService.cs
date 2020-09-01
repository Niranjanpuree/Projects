using Northwind.Core.Entities;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IGroupService
    {
        void Insert(Group group);
        void Update(Group group);
        void Delete(Guid groupGuid);
        void Delete(List<Guid> groupGuid);
        void Delete(List<Guid> userGuid,string groupGuid);
        void DeleteGroup(List<Guid> groupGuid, string userGuid);
        IEnumerable<Group> GetGroupByName(string groupName);
        IEnumerable<Group> GetGroups();
        IEnumerable<Group> GetGroups(string searchValue, int take, int skip, string sortField, string dir, List<AdvancedSearchRequest> postValue);
        Group GetGroupByGUID(Guid guid);
        IEnumerable<Group> GetGroupByGUID(List<Guid> guids);
        int GetTotalRows(string searchValue, int take, int skip, List<AdvancedSearchRequest> postValue);
        IEnumerable<PolicyEntity> GetAssignedPolicyToGroup(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir);
        int GetAssignedPolicyToGroupCount(Guid groupGuid, string searchValue);
        IEnumerable<PolicyEntity> GetUnassignedPolicyToGroup(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir);
        void AssignPolicyToGroup(Guid groupGuid, Guid policyGuid);
        void UnassignPolicyToGroup(Guid groupGuid, Guid policyGuid);
        IEnumerable<GroupResourcePermission> GetGroupResources();
        IEnumerable<GroupResourcePermission> GetGroupResources(Guid UserGuid);
        IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(Guid groupGuid, Guid resourceGuid);
        IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(List<Guid> groupGuids, Guid resourceGuid);
        void AssignGroupPermission(Guid groupGuid, List<string> actionGuid);
        Group GetGroupByUser(Guid userGuid);
        void RemoveUserGroup(Guid userGuid,Guid groupGuid);
    }
}
