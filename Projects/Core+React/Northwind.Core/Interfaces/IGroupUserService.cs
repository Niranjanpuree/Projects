using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IGroupUserService
    {
        IEnumerable<GroupUser> GetGroupUserByUserGUIDAndGroupGUID(Guid userGUID, Guid groupGUID);
        IEnumerable<GroupUser> GetGroupUserByUserGUID(Guid userGUID);
        IEnumerable<GroupPermission> GetGroupUserCountByUserGUID(Guid userGUID);
        IEnumerable<GroupUser> GetGroupUserByGroupGUID(Guid groupGUID);
        IEnumerable<User> GetGroupUnassignedUserToGroup(Guid groupGUID, string searchValue, int take);
        IEnumerable<Group> GetGroupUnassignedToUser(Guid userGUID, string searchValue, int take);
        GroupUser GetGroupUserByGroupUserGUID(Guid groupUserGUID);
        void Insert(GroupUser groupUser);
        void Update(GroupUser groupUser);
        void DeleteByUserId(Guid userGuid);
        void DeleteByGroupId(Guid groupGuid);
        void DeleteByGroupUserId(Guid groupUserGUID);
        void DeleteByGroupGuidUserGuid(Guid groupGUID, Guid userGuid);
        bool IsUserInSAGroup(Guid userGuid);
    }
}
