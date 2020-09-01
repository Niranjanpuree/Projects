using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;

namespace Northwind.Core.Interfaces
{
    public interface IUserService
    {
        User Authenticate(IActiveDirectoryContext adContext, string username, string password);
        Guid Insert(User user);
        void Update(User user);
        void Delete(List<Guid> userGuids);
        void DeleteByUserId(Guid userGuid);
        IEnumerable<User> GetUsersByUsername(List<string> usernames);
        IEnumerable<User> GetUsers();
        User GetUserByUserGuid(Guid userGuid);
        User GetEmployeeDirectoryByUserGuid(Guid userGuid);
        IEnumerable<User> GetUserByUserGuid(List<Guid> userGuid);
        IEnumerable<User> GetUsers(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir);
        int GetUserCount(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir);
        IEnumerable<User> GetUsers(string searchValue, int take, int skip, string sortField, string dir);
        IEnumerable<User> GetEmployeeDirectoryList(string searchValue, int take, int skip, string sortField, string dir,string filterBy);
        int GetUserCount(string searchValue);
        int GetEmployeeDirectoryCount(string searchValue,string filterBy);
        IEnumerable<User> Find(UserSearchSpec spec);
        IEnumerable<User> GetUsersData(string searchText);
        User GetUserByDisplayName(string displayName);
        int GetUsersCountPerson(string searchValue);
        IEnumerable<User> GetUsersPerson(string searchValue, int take, int skip, string sortField, string dir);

        User GetUserByUsername(string username);

        User GetUserByFirstAndLastName(string firstName,string lastName);

        User GetUserByWorkEmail(string workEmail);
    }
}
