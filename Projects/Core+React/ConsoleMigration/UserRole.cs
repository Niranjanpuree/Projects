using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleMigration
{
    public class UserRole
    {
        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly IGroupPermissionService _groupPermissionService;
        private readonly IGroupUserService _groupUserService;
        private readonly IResourceService _resourceService;
        private string sysAdmin = "SYSADMIN";
        public UserRole(IUserService userService,IGroupService groupService,IGroupPermissionService groupPermissionService,
            IGroupUserService groupUserService,IResourceService resourceService)
        {
            _userService = userService;
            _groupService = groupService;
            _groupPermissionService = groupPermissionService;
            _groupUserService = groupUserService;
            _resourceService = resourceService;
        }

        //assign super admin role to specific users
        public void AssignRoleToUser()
        {
            AddSARoleToGroup();
            List<string> usernameList = new List<string>(new string[] {
                "ashish.pokhrel",
                "santosh.aryal",
                "saroj.ghorasaini",
                "shishir.baral"
            });

            var userList = _userService.GetUsersByUsername(usernameList);
            var groupList = _groupService.GetGroupByName(sysAdmin);
            bool isSAGroup = false;
            foreach (var user in userList)
            {
                var userGroup = _groupUserService.GetGroupUserByUserGUID(user.UserGuid);
                foreach (var group in userGroup)
                {
                    if (groupList != null && groupList.Count() > 0) {
                        isSAGroup = group.GroupGUID == groupList.FirstOrDefault().GroupGuid ? true : false;
                        break;
                    }
                }

                if (!isSAGroup && groupList != null && groupList.Count() > 0)
                {
                    var groupUser = new GroupUser() {
                        GroupGUID = groupList.FirstOrDefault().GroupGuid,
                        UserGUID = user.UserGuid
                    };
                    _groupUserService.Insert(groupUser);
                }
            }
        }

        private void AddSARoleToGroup()
        {
            var groupList = _groupService.GetGroupByName(sysAdmin);
            if (groupList == null || groupList.Count() == 0)
            {
                var groupGuid = Guid.NewGuid();
                var group = new Group()
                {
                    GroupGuid = groupGuid,
                    GroupName = sysAdmin,
                    CN = sysAdmin,
                    Description = "Super Admin Group"
                };
                _groupService.Insert(group);
                AddGroupPermission(groupGuid);
            }
            
        }

        private void AddGroupPermission(Guid groupGuid)
        {
            var resourceList = _resourceService.GetAll();
            var groupPermission = new GroupPermission();
            foreach (var resource in resourceList)
            {
                groupPermission.GroupGuid = groupGuid;
                groupPermission.ResourceGuid = resource.ResourceGuid;
                var resourceActionList = _resourceService.GetResourceAction(resource.ResourceGuid);
                foreach (var action in resourceActionList)
                {
                    groupPermission.ResourceActionGuid = action.ActionGuid;
                    _groupPermissionService.InsertGroupPermission(groupPermission);
                }
            }
        }
    }
}
