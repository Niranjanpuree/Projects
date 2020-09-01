using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class GroupService: IGroupService
    {
        IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public void AssignPolicyToGroup(Guid groupGuid, Guid policyGuid)
        {
            _groupRepository.AssignPolicyToGroup(groupGuid, policyGuid);
        }

        public void Delete(Guid groupGuid)
        {
            _groupRepository.Delete(groupGuid);
        }

        public void Delete(List<Guid> groupGuid)
        {
            _groupRepository.Delete(groupGuid);
        }
        public void Delete(List<Guid> userGuid,string groupGuid)
        {
            _groupRepository.Delete(userGuid,groupGuid);
        }
        public void DeleteGroup(List<Guid> groupGuid, string userGuid)
        {
            _groupRepository.DeleteGroup(groupGuid, userGuid);
        }

        public IEnumerable<PolicyEntity> GetAssignedPolicyToGroup(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            return _groupRepository.GetAssignedPolicyToGroup(groupGuid, searchValue, take, skip, sortField, dir);
        }

        public int GetAssignedPolicyToGroupCount(Guid groupGuid, string searchValue)
        {
            return _groupRepository.GetAssignedPolicyToGroupCount(groupGuid, searchValue);
        }

        public IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(Guid groupGuid, Guid resourceGuid)
        {
            return _groupRepository.GetGroupResourceActions(groupGuid, resourceGuid);
        }

        public Group GetGroupByGUID(Guid guid)
        {
            return _groupRepository.GetGroupByGUID(guid);
        }

        public IEnumerable<Group> GetGroupByGUID(List<Guid> guids)
        {
            return _groupRepository.GetGroupByGUID(guids);
        }

        public IEnumerable<Group> GetGroupByName(string groupName)
        {
            return _groupRepository.GetGroupByName(groupName);
        }

        public IEnumerable<GroupResourcePermission> GetGroupResources()
        {
            return _groupRepository.GetGroupResources();
        }

        public IEnumerable<Group> GetGroups()
        {
            return _groupRepository.GetGroups();
        }

        public IEnumerable<Group> GetGroups(string searchValue, int take, int skip, string sortField, string dir, List<AdvancedSearchRequest> postValue)
        {
            return _groupRepository.GetGroups(searchValue, take, skip, sortField, dir, postValue);
        }

        public int GetTotalRows(string searchValue, int take, int skip, List<AdvancedSearchRequest> postValue)
        {
            return _groupRepository.GetTotalRows(searchValue, take, skip, postValue);
        }

        public IEnumerable<PolicyEntity> GetUnassignedPolicyToGroup(Guid groupGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            return _groupRepository.GetUnassignedPolicyToGroup(groupGuid, searchValue, take, skip, sortField, dir);
        }

        public void Insert(Group group)
        {
            _groupRepository.Insert(group);
        }

        public void UnassignPolicyToGroup(Guid groupGuid, Guid policyGuid)
        {
            _groupRepository.UnassignPolicyToGroup(groupGuid, policyGuid);
        }

        public void Update(Group group)
        {
            _groupRepository.Update(group);
        }

        public void AssignGroupPermission(Guid groupGuid, List<string> actionGuid)
        {
            _groupRepository.AssignGroupPermission(groupGuid, actionGuid);
        }

        public void RemoveUserGroup(Guid userGuid, Guid groupGuid)
        {
            _groupRepository.RemoveUserGroup(userGuid, groupGuid);
        }

        public Group GetGroupByUser(Guid userGuid)
        {
            return _groupRepository.GetGroupByUser(userGuid);
        }

        public IEnumerable<GroupResourceActionPermission> GetGroupResourceActions(List<Guid> groupGuids, Guid resourceGuid)
        {
            return _groupRepository.GetGroupResourceActions(groupGuids, resourceGuid);
        }

        public IEnumerable<GroupResourcePermission> GetGroupResources(Guid userGuid)
        {
            return _groupRepository.GetGroupResources(userGuid);
        }
    }
}
