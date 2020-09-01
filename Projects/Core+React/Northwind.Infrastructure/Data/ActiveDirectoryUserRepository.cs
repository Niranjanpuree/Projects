using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Infrastructure.Data;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;

namespace Northwind.Infrastructure
{
    public class ActiveDirectoryUserRepository : IActiveDirectoryUserRepository
    {
        private ActiveDirectoryContext _context;

        public ActiveDirectoryUserRepository(ActiveDirectoryContext context)
        {
            _context = context;
        }

        public bool validateUser(string username, String password)
        {
            try
            {
             var connection = new LdapConnection(new LdapDirectoryIdentifier(_context.LdapPath.ToLower().Replace("ldap://", "").Replace("/", ""), 389));
               connection.Bind(new System.Net.NetworkCredential(username, password));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            var listOfUsers = new List<User>();
            try
            {
                var rootEntry = new DirectoryEntry(_context.LdapPath + _context.LdapDomainController, _context.LdapUsername, _context.LdapPassword);
                var filter = "(&(objectClass=person)(|(userAccountControl=512)(userAccountControl=2)(userAccountControl=66048)))";               
                var propertiesToLoad = new string[] {"givenName", "department", "sn", "telephoneNumber", "mail", "company", "displayName", "mobile", "sAMAccountName", "title", "userPrincipalName", "pager","extensionAttribute4", "manager" };
                var directorySearcher = new DirectorySearcher(rootEntry, filter, propertiesToLoad)
                {
                    PageSize = 1001
                };
                var searchResult = directorySearcher.FindAll();
                foreach (SearchResult u in searchResult)
                {
                    var user = new User
                    {
                        UserGuid = u.GetDirectoryEntry().Guid,
                        Firstname = GetPropertyFromSearchResult(u,"givenName"),
                        Lastname = GetPropertyFromSearchResult(u,"sn"),
                        Givenname = GetPropertyFromSearchResult(u,"givenName"),
                        DisplayName = GetPropertyFromSearchResult(u, "DisplayName"),
                        Username = GetPropertyFromSearchResult(u, "sAMAccountName"),
                        WorkPhone = GetPropertyFromSearchResult(u, "telephoneNumber"),
                        MobilePhone = GetPropertyFromSearchResult(u,"mobile"),
                        WorkEmail = GetPropertyFromSearchResult(u, "mail"),
                        Company = GetPropertyFromSearchResult(u, "company"),
                        JobTitle = GetPropertyFromSearchResult(u,"title"),
                        Department = GetPropertyFromSearchResult(u, "department"),
                        Extension = GetPropertyFromSearchResult(u, "pager"),
                        JobStatus = GetPropertyFromSearchResult(u, "extensionAttribute4"),
                        UserStatus = GetPropertyFromSearchResult(u, "userAccountControl") == "2" ? "DISABLED": "ENABLED",
                        Manager = GetPropertyFromSearchResult(u, "manager"),
                    };
                    listOfUsers.Add(user);
                }

                foreach(var user in listOfUsers)
                {
                    try
                    {
                        if (user.Manager != null)
                        {
                            user.ManagerUser = GetUserByCN(rootEntry, user.Manager);
                            if (user.ManagerUser != null)
                            {
                                user.Manager = user.ManagerUser.DisplayName;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                    }
                }
                rootEntry.Close();

            }

            catch (DirectoryServicesCOMException)
            {
                return listOfUsers;
            }

            catch (Exception ex)
            {
                string message = ex.Message;
                return listOfUsers;
            }
            return listOfUsers;

        }

        public User GetUserByCN(DirectoryEntry rootEntry, string manager)
        {
            var arrManager = manager.Split(",");
            var cn = arrManager[0].TrimStart().TrimEnd();
            cn = cn.Replace("/", "");
            cn = cn.Replace(@"\", "");
            var filter = $"(&(objectClass=user)({cn}))";
            var propertiesToLoad = new string[] { "givenName", "department", "sn", "telephoneNumber", "mail", "company", "displayName", "mobile", "sAMAccountName", "title", "userPrincipalName", "pager", "extensionAttribute4", "manager" };
            var directorySearcher = new DirectorySearcher(rootEntry, filter, propertiesToLoad)
            {
                PageSize = 1001
            };
            var searchResult = directorySearcher.FindAll();
            foreach (SearchResult u in searchResult)
            {
                return new User
                {
                    UserGuid = u.GetDirectoryEntry().Guid,
                    Firstname = GetPropertyFromSearchResult(u, "givenName"),
                    Lastname = GetPropertyFromSearchResult(u, "sn"),
                    Givenname = GetPropertyFromSearchResult(u, "givenName"),
                    DisplayName = GetPropertyFromSearchResult(u, "DisplayName"),
                    Username = GetPropertyFromSearchResult(u, "sAMAccountName"),
                    WorkPhone = GetPropertyFromSearchResult(u, "telephoneNumber"),
                    MobilePhone = GetPropertyFromSearchResult(u, "mobile"),
                    WorkEmail = GetPropertyFromSearchResult(u, "mail"),
                    Company = GetPropertyFromSearchResult(u, "company"),
                    JobTitle = GetPropertyFromSearchResult(u, "title"),
                    Department = GetPropertyFromSearchResult(u, "department"),
                    Extension = GetPropertyFromSearchResult(u, "pager"),
                    JobStatus = GetPropertyFromSearchResult(u, "extensionAttribute4"),
                    UserStatus = GetPropertyFromSearchResult(u, "userAccountControl") == "2" ? "DISABLED" : "ENABLED",
                };
            }
            return null;
        }

        public AdUserGroupAndManager GetUserAdUserByUsername(string username)
        {
            try
            {
                var rootEntry = new DirectoryEntry(_context.LdapPath + _context.LdapDomainController, _context.LdapUsername, _context.LdapPassword);
                var filter = $"(&(objectClass=person)(|(userAccountControl=512)(userAccountControl=2)(userAccountControl=66048))(sAMAccountName={username}))";
                var propertiesToLoad = new string[] { "givenName", "department", "sn", "manager", "memberOf" };
                var directorySearcher = new DirectorySearcher(rootEntry, filter, propertiesToLoad)
                {
                    PageSize = 1001
                };
                var searchResult = directorySearcher.FindAll();
                if (searchResult.Count > 0)
                {
                    var u = searchResult[0];
                    var user = new User
                    {
                        UserGuid = u.GetDirectoryEntry().Guid,
                        Firstname = GetPropertyFromSearchResult(u, "givenName"),
                        Lastname = GetPropertyFromSearchResult(u, "sn"),
                        Givenname = GetPropertyFromSearchResult(u, "givenName"),
                        DisplayName = GetPropertyFromSearchResult(u, "DisplayName"),
                        Username = GetPropertyFromSearchResult(u, "sAMAccountName"),
                        WorkPhone = GetPropertyFromSearchResult(u, "telephoneNumber"),
                        MobilePhone = GetPropertyFromSearchResult(u, "mobile"),
                        WorkEmail = GetPropertyFromSearchResult(u, "mail"),
                        Company = GetPropertyFromSearchResult(u, "company"),
                        JobTitle = GetPropertyFromSearchResult(u, "title"),
                        Department = GetPropertyFromSearchResult(u, "department"),
                        Extension = GetPropertyFromSearchResult(u, "pager"),
                        JobStatus = GetPropertyFromSearchResult(u, "extensionAttribute4"),
                        UserStatus = GetPropertyFromSearchResult(u, "userAccountControl") == "2" ? "DISABLED" : "ENABLED",
                    };
                    return new AdUserGroupAndManager
                    {
                        User = user,
                        Manager = GetPropertyFromSearchResult(u, "manager"),
                        MemberOf = GetPropertyFromSearchResult(u, "memberOf"),
                    };                    
                }                    

            }

            catch (DirectoryServicesCOMException)
            {
                return null;
            }

            catch (Exception)
            {
                return null;
            }
            return null;

        }

        public AdUserGroupAndManager GetUserAdUserByCN(string username)
        {
            try
            {
                var rootEntry = new DirectoryEntry(_context.LdapPath + username, _context.LdapUsername, _context.LdapPassword);                
                if (rootEntry != null)
                {
                    var u = rootEntry;
                    var user = new User
                    {
                        UserGuid = rootEntry.Guid,
                        Firstname = GetPropertyFromLDAP(u, "givenName"),
                        Lastname = GetPropertyFromLDAP(u, "sn"),
                        Givenname = GetPropertyFromLDAP(u, "givenName"),
                        DisplayName = GetPropertyFromLDAP(u, "DisplayName"),
                        Username = GetPropertyFromLDAP(u, "sAMAccountName"),
                        WorkPhone = GetPropertyFromLDAP(u, "telephoneNumber"),
                        MobilePhone = GetPropertyFromLDAP(u, "mobile"),
                        WorkEmail = GetPropertyFromLDAP(u, "mail"),
                        Company = GetPropertyFromLDAP(u, "company"),
                        JobTitle = GetPropertyFromLDAP(u, "title"),
                        Department = GetPropertyFromLDAP(u, "department"),
                        Extension = GetPropertyFromLDAP(u, "pager"),
                        JobStatus = GetPropertyFromLDAP(u, "extensionAttribute4"),
                        UserStatus = GetPropertyFromLDAP(u, "userAccountControl") == "2" ? "DISABLED" : "ENABLED",
                    };
                    return new AdUserGroupAndManager
                    {
                        User = user,
                        Manager = GetPropertyFromLDAP(u, "manager"),
                        MemberOf = GetPropertyFromLDAP(u, "memberOf"),
                    };
                }

            }

            catch (DirectoryServicesCOMException)
            {
                return null;
            }

            catch (Exception)
            {
                return null;
            }
            return null;

        }


        private string GetPropertyFromLDAP(DirectoryEntry directoryEntry, string propertyName)
        {
            List<string> myString = new List<string>();

            if (directoryEntry.Properties.Contains(propertyName))
            {
               return directoryEntry.Properties[propertyName][0].ToString();
            }

            return null;
        }

        private string GetPropertyFromSearchResult(SearchResult r, string propertyName)
        {
            if (r.Properties.Contains(propertyName))
            {
                return r.Properties[propertyName][0].ToString();
            }
            return null;
        }

    }
 }
