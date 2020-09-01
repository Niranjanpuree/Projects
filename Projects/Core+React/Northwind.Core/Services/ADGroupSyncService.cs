using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Northwind.Core.Interfaces.Sync;

namespace Northwind.Core.Services
{
    public class ADGroupSyncService : IADGroupSyncService
    {
        private readonly IGroupService _groupService;
        private readonly IUserService _userService;
        private readonly ISyncStatusService _syncStatusService;
        private readonly IGroupUserService _groupUserService;
        private readonly IManagerUserService _managerUserService;

        public ADGroupSyncService(IGroupService groupService, IManagerUserService managerUserService, IUserService userService, IGroupUserService groupUserService, ISyncStatusService syncStatusService)
        {
            _groupService = groupService;
            _syncStatusService = syncStatusService;
            _userService = userService;
            _managerUserService = managerUserService;
            _groupUserService = groupUserService;
        }

        public void SyncGroupsFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID)
        {
            IEnumerable<Group> adGroups = adContext.GroupRepository.GetGroups();
            var dbGroups = _groupService.GetGroups();
            foreach (var adGroup in adGroups)
            {
                var groups = dbGroups.Where(x => x.GroupGuid == adGroup.GroupGuid).ToList();
                if (groups.Count == 0)
                {
                    try
                    {
                        _groupService.Insert(adGroup);
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = adGroup.GroupGuid,
                            ObjectName = adGroup.GroupName,
                            ObjectType = "Group",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "INSERT"
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                    catch (Exception ex)
                    {
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = adGroup.GroupGuid,
                            ObjectName = adGroup.GroupName,
                            ObjectType = "Group",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "ERROR",
                            ErrorMessage = ex.StackTrace
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                }
                else
                {
                    try
                    {
                        _groupService.Update(adGroup);
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = adGroup.GroupGuid,
                            ObjectName = adGroup.GroupName,
                            ObjectType = "Group",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "UPDATE"
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                    catch (Exception ex)
                    {
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = adGroup.GroupGuid,
                            ObjectName = adGroup.GroupName,
                            ObjectType = "Group",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "ERROR",
                            ErrorMessage = ex.StackTrace
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                }
            }

        }

        public void SyncGroupUsersAndManagerFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID)
        {
            var lstUsers = _userService.GetUsers();
            foreach(var user in lstUsers)
            {
                try
                {
                    var result = adContext.UserRepository.GetUserAdUserByUsername(user.Username);
                    if (result != null)
                    {
                        string manager = result.Manager;
                        List<string> lstGroupsCN = result.MemberOf.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                        AdUserGroupAndManager adUser = adContext.UserRepository.GetUserAdUserByCN(manager);
                        if (adUser != null && adUser.User != null)
                        {
                            try
                            {
                                var dbUsers = _userService.GetUsersByUsername(new List<string> { adUser.User.Username });
                                if (dbUsers.Count() > 0)
                                {
                                    var managerUser = new ManagerUser
                                    {
                                        ManagerUserGUID = Guid.NewGuid(),
                                        UserGUID = user.UserGuid,
                                        ManagerGUID = dbUsers.FirstOrDefault().UserGuid
                                    };
                                    SyncUserManager(adContext, syncBatchGUID, user, managerUser);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }

                        }
                        if(adUser.MemberOf != null)
                        {
                            SyncUserGroups(adContext, syncBatchGUID, user, adUser.MemberOf.Split(";").ToList());
                        }
                        else
                        {
                            var syncStatus = new SyncStatus
                            {
                                ObjectGUID = user.UserGuid,
                                ObjectName = user.Username,
                                ObjectType = "ManagerUser",
                                SyncBatchGUID = syncBatchGUID,
                                SyncStatusText = "ERROR",
                                ErrorMessage = "Unable to find group"
                            };
                            _syncStatusService.Insert(syncStatus);
                        }
                        
                    }
                    else
                    {
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = user.UserGuid,
                            ObjectName = user.Username,
                            ObjectType = "ManagerUser",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "ERROR",
                            ErrorMessage = "Unable to find manager"
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                }
                catch (Exception ex)
                {
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = user.UserGuid,
                        ObjectName = user.Username,
                        ObjectType = "ManagerUser",
                        SyncBatchGUID = syncBatchGUID,
                        SyncStatusText = "ERROR",
                        ErrorMessage = ex.StackTrace
                    };
                    _syncStatusService.Insert(syncStatus);
                }
            }
        }

        private void SyncUserGroups(IActiveDirectoryContext adContext, Guid syncBatchGUID, User user, List<string> groups)
        {
            foreach(string groupCN in groups)
            {
                var adGroups = adContext.GroupRepository.GetGroupByCN(groupCN);
                foreach(var adGroup in adGroups)
                {
                    var dbGroup = _groupService.GetGroupByGUID(adGroup.GroupGuid);
                    if(dbGroup != null)
                    {
                        try
                        {
                            var lstUserGroup = _groupUserService.GetGroupUserByUserGUIDAndGroupGUID(user.UserGuid, adGroup.GroupGuid);
                            if(lstUserGroup.Count() == 0)
                            {
                                _groupUserService.Insert(new GroupUser
                                {
                                    GroupGUID = adGroup.GroupGuid,
                                    UserGUID = user.UserGuid
                                });
                                var syncStatus = new SyncStatus
                                {
                                    ObjectGUID = user.UserGuid,
                                    ObjectName = user.Username,
                                    ObjectType = "GroupUser",
                                    SyncBatchGUID = syncBatchGUID,
                                    SyncStatusText = "INSERT"
                                };
                                _syncStatusService.Insert(syncStatus);
                            }
                            else
                            {
                                var syncStatus = new SyncStatus
                                {
                                    ObjectGUID = user.UserGuid,
                                    ObjectName = user.Username,
                                    ObjectType = "GroupUser",
                                    SyncBatchGUID = syncBatchGUID,
                                    SyncStatusText = "NOACTION",
                                    ErrorMessage = "User already assigned to group " + adGroup.CN
                                };
                                _syncStatusService.Insert(syncStatus);
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            var syncStatus = new SyncStatus
                            {
                                ObjectGUID = user.UserGuid,
                                ObjectName = user.Username,
                                ObjectType = "GroupUser",
                                SyncBatchGUID = syncBatchGUID,
                                SyncStatusText = "ERROR",
                                ErrorMessage = ex.StackTrace
                            };
                            _syncStatusService.Insert(syncStatus);
                        }
                    }
                    else
                    {
                        var syncStatus = new SyncStatus
                        {
                            ObjectGUID = adGroup.GroupGuid,
                            ObjectName = adGroup.GroupName,
                            ObjectType = "GroupUser",
                            SyncBatchGUID = syncBatchGUID,
                            SyncStatusText = "ERROR",
                            ErrorMessage = "Unable to find group"
                        };
                        _syncStatusService.Insert(syncStatus);
                    }
                }
            }
        }
        

        private void SyncUserManager(IActiveDirectoryContext adContext, Guid syncBatchGUID, User user, ManagerUser managerUser)
        {
            
            var managerUsers = _managerUserService.GetManagerUser(managerUser);
            if (managerUsers.Count() > 0)
            {
                var syncStatus = new SyncStatus
                {
                    ObjectGUID = user.UserGuid,
                    ObjectName = user.Username,
                    ObjectType = "ManagerUser",
                    SyncBatchGUID = syncBatchGUID,
                    SyncStatusText = "NOACTION",
                    ErrorMessage = "Manager already exists"
                };
                _syncStatusService.Insert(syncStatus);
            }
            else
            {
                _managerUserService.Insert(managerUser);
                var syncStatus = new SyncStatus
                {
                    ObjectGUID = user.UserGuid,
                    ObjectName = user.Username,
                    ObjectType = "ManagerUser",
                    SyncBatchGUID = syncBatchGUID,
                    SyncStatusText = "INSERT"
                };
                _syncStatusService.Insert(syncStatus);
            }
           //TODO: If user has manager other than specified, may need to be deleted.
        }
    }
}
