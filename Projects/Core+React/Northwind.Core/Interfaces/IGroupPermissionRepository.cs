using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Core.Interfaces
{
    public interface IGroupPermissionRepository
    {
        void UpdateGroupPermission(List<GroupPermission> permissions);
        void DeleteGroupPermission(List<GroupPermission> permissions);
        void DeleteGroupPermission(GroupPermission permission);
        IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid);
        IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid, Guid ResourceGuid);
        GroupPermission GetGroupPermission(Guid GroupGuid, Guid ResourceGuid, Guid ResourceActionGuid);
        bool IsUserPermitted(Guid userGuid, ResourceType resourceType, ResourceActionPermission permission);
        bool IsUserPermitted(Guid userGuid, string resourceType, string permission);
        void InsertGroupPermission(GroupPermission groupPermission);
    }
}
