using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System.Linq;
using Northwind.Core.Interfaces.Sync;

namespace Northwind.Core.Services
{
    public class UserSyncService : IUserSyncService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISyncStatusService _syncStatusService;
        private readonly IManagerUserService _managerUserService;

        public UserSyncService(IUserRepository userRepository, ISyncStatusService syncStatusService, IManagerUserService managerUserService)
        {
            _userRepository = userRepository;
            _syncStatusService = syncStatusService;
            _managerUserService = managerUserService;
        }

        public void SyncUsersFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID)
        {
            var listOfUsers = adContext.UserRepository.GetUsers();
            var listOfUsernames = listOfUsers.Select(x => x.Username).ToList();
            var usersToUpdate = _userRepository.GetUsersByUsername(listOfUsernames);
            foreach (var user in listOfUsers)
            {
                if (usersToUpdate.FirstOrDefault(x => String.Equals(x.Username, user.Username, StringComparison.OrdinalIgnoreCase)) != null)
                {
                    var userToUpdate = usersToUpdate.FirstOrDefault(x => String.Equals(x.Username, user.Username, StringComparison.OrdinalIgnoreCase));
                    UpdateUser(user, userToUpdate, syncBatchGUID);
                }
                else
                {
                    InsertUser(user, syncBatchGUID);
                }

                if (user.ManagerUser != null)
                {
                    var u = _userRepository.GetUsersByUsername(new List<string> { user.Username });
                    var m = _userRepository.GetUsersByUsername(new List<string> { user.ManagerUser.Username });
                    if (u.Count() > 0 && m.Count() > 0)
                    {
                        if (!_managerUserService.IsManagerExists(u.SingleOrDefault().UserGuid, m.SingleOrDefault().UserGuid))
                        {
                            try
                            {
                                _managerUserService.Insert(new ManagerUser { ManagerGUID = m.SingleOrDefault().UserGuid, UserGUID = u.SingleOrDefault().UserGuid });
                                var syncStatus = new SyncStatus
                                {
                                    ObjectGUID = u.SingleOrDefault().UserGuid,
                                    ObjectName = u.SingleOrDefault().Username,
                                    ObjectType = "ManagerUser",
                                    SyncBatchGUID = syncBatchGUID,
                                    SyncStatusText = "INSERT"
                                };
                                _syncStatusService.Insert(syncStatus);
                            }
                            catch (Exception ex)
                            {
                                var syncStatus = new SyncStatus
                                {
                                    ObjectGUID = user.UserGuid,
                                    ObjectName = user.Username,
                                    ObjectType = "User",
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
                                ObjectGUID = user.UserGuid,
                                ObjectName = user.Username,
                                ObjectType = "User",
                                SyncBatchGUID = syncBatchGUID,
                                SyncStatusText = "INVALID",
                                ErrorMessage = "Unable to find manager to update"
                            };
                            _syncStatusService.Insert(syncStatus);
                        }
                    }

                }
                else
                {
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = user.UserGuid,
                        ObjectName = user.Username,
                        ObjectType = "User",
                        SyncBatchGUID = syncBatchGUID,
                        SyncStatusText = "INVALID",
                        ErrorMessage = "No Manager found for this user"
                    };
                    _syncStatusService.Insert(syncStatus);
                }
            }
        }

        private void UpdateUser(User u, User userToUpdate, Guid syncBatchGUID)
        {
            userToUpdate = u;
            var userValidationResult = ValidateUser(userToUpdate);

            if (userValidationResult.IsValid)
            {
                try
                {
                    _userRepository.Update(u);
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = u.UserGuid,
                        ObjectName = u.Username,
                        ObjectType = "User",
                        SyncBatchGUID = syncBatchGUID,
                        SyncStatusText = "UPDATE"
                    };
                    _syncStatusService.Insert(syncStatus);
                }
                catch (Exception ex)
                {
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = u.UserGuid,
                        ObjectName = u.Username,
                        ObjectType = "User",
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
                    ObjectGUID = u.UserGuid,
                    ObjectName = u.Username,
                    ObjectType = "User",
                    SyncBatchGUID = syncBatchGUID,
                    SyncStatusText = "ERROR",
                    ErrorMessage = DictionaryToString(userValidationResult.Errors)
                };
                _syncStatusService.Insert(syncStatus);
            }

        }

        private void InsertUser(User u, Guid syncBatchGUID)
        {
            var userSyncResult = new UserSyncResult
            {
                Username = u.Username
            };
            var userValidationResult = ValidateUser(u);

            if (userValidationResult.IsValid)
            {
                try
                {
                    _userRepository.Insert(u);
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = u.UserGuid,
                        ObjectName = u.Username,
                        ObjectType = "User",
                        SyncBatchGUID = syncBatchGUID,
                        SyncStatusText = "INSERT"
                    };
                    _syncStatusService.Insert(syncStatus);
                }
                catch (Exception ex)
                {
                    var syncStatus = new SyncStatus
                    {
                        ObjectGUID = u.UserGuid,
                        ObjectName = u.Username,
                        ObjectType = "User",
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
                    ObjectGUID = u.UserGuid,
                    ObjectName = u.Username,
                    ObjectType = "User",
                    SyncBatchGUID = syncBatchGUID,
                    SyncStatusText = "ERROR",
                    ErrorMessage = DictionaryToString(userValidationResult.Errors)
                };
                _syncStatusService.Insert(syncStatus);
            }

        }

        private UserSyncValidationResult ValidateUser(User u)
        {
            //run validation logic here.
            return new UserSyncValidationResult();
        }

        private string DictionaryToString(Dictionary<string, object> dic)
        {
            string result = "";
            foreach (var key in dic.Keys)
            {
                result += dic[key].ToString() + "\r\n";
            }
            return result;
        }
    }
}
