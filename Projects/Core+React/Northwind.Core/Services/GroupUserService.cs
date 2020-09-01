using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class GroupUserService : IGroupUserService
    {
        IGroupUserRepository _groupUserRepository;

        public GroupUserService(IGroupUserRepository groupUserRepository)
        {
            _groupUserRepository = groupUserRepository;
        }

        public void DeleteByGroupGuidUserGuid(Guid groupGUID, Guid userGuid)
        {
            _groupUserRepository.DeleteByGroupGuidUserGuid(groupGUID, userGuid);
        }

        public void DeleteByGroupId(Guid groupGuid)
        {
            _groupUserRepository.DeleteByGroupId(groupGuid);
        }

        public void DeleteByGroupUserId(Guid groupUserGUID)
        {
            _groupUserRepository.DeleteByGroupUserId(groupUserGUID);
        }

        public void DeleteByUserId(Guid userGuid)
        {
            _groupUserRepository.DeleteByUserId(userGuid);
        }

        public IEnumerable<User> GetGroupUnassignedUserToGroup(Guid groupGUID, string searchValue, int take)
        {
            return _groupUserRepository.GetGroupUnassignedUserToGroup(groupGUID, searchValue, take);
        }
        public IEnumerable<Group> GetGroupUnassignedToUser(Guid userGUID, string searchValue, int take)
        {
            return _groupUserRepository.GetGroupUnassignedToUser(userGUID, searchValue, take);
        }

        public IEnumerable<GroupUser> GetGroupUserByGroupGUID(Guid groupGUID)
        {
            return _groupUserRepository.GetGroupUserByGroupGUID(groupGUID);
        }

        public GroupUser GetGroupUserByGroupUserGUID(Guid groupUserGUID)
        {
            return _groupUserRepository.GetGroupUserByGroupUserGUID(groupUserGUID);
        }

        public IEnumerable<GroupUser> GetGroupUserByUserGUID(Guid userGUID)
        {
            return _groupUserRepository.GetGroupUserByUserGUID(userGUID);
        }
        public IEnumerable<GroupPermission> GetGroupUserCountByUserGUID(Guid userGUID)
        {
            return _groupUserRepository.GetGroupUserCountByUserGUID(userGUID);
        }

        public IEnumerable<GroupUser> GetGroupUserByUserGUIDAndGroupGUID(Guid userGUID, Guid groupGUID)
        {
            return _groupUserRepository.GetGroupUserByUserGUIDAndGroupGUID(userGUID, groupGUID);
        }

        public void Insert(GroupUser groupUser)
        {
            _groupUserRepository.Insert(groupUser);
        }

        public void Update(GroupUser groupUser)
        {
            _groupUserRepository.Update(groupUser);
        }

        public bool IsUserInSAGroup(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return false;
            return _groupUserRepository.IsUserInSAGroup(userGuid);
        }
    }
}
