using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class GroupPermissionService : IGroupPermissionService
    {
        IGroupPermissionRepository _repository;
        public GroupPermissionService(IGroupPermissionRepository repository)
        {
            _repository = repository;
        }

        public void DeleteGroupPermission(List<GroupPermission> permissions)
        {
            _repository.DeleteGroupPermission(permissions);
        }

        public void DeleteGroupPermission(GroupPermission permission)
        {
            _repository.DeleteGroupPermission(permission);
        }

        public IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid)
        {
            return _repository.GetGroupPermission(groupGuid);
        }

        public IEnumerable<GroupPermission> GetGroupPermission(Guid groupGuid, Guid resourceGuid)
        {
            return _repository.GetGroupPermission(groupGuid, resourceGuid);
        }

        public GroupPermission GetGroupPermission(Guid groupGuid, Guid resourceGuid, Guid resourceActionGuid)
        {
            return _repository.GetGroupPermission(groupGuid, resourceGuid, resourceActionGuid);
        }

        public bool IsUserPermitted(Guid userGuid, EnumGlobal.ResourceType resourceType, EnumGlobal.ResourceActionPermission permission)
        {
            return _repository.IsUserPermitted(userGuid, resourceType, permission);
        }

        public bool IsUserPermitted(Guid userGuid, string resourceType, string permission)
        {
            return _repository.IsUserPermitted(userGuid, resourceType, permission);
        }

        public void UpdateGroupPermission(List<GroupPermission> permissions)
        {
            _repository.UpdateGroupPermission(permissions);
        }

        public void InsertGroupPermission(GroupPermission groupPermission)
        {
            _repository.InsertGroupPermission(groupPermission);
        }
    }
}
