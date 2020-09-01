using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;

namespace Northwind.Core.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
        IEnumerable<User> GetUsers(string searchValue, int take, int skip, string sortField, string dir);
        int GetUserCount(string searchValue);
        IEnumerable<User> GetUsers(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir);
        int GetUserCount(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir);
        IEnumerable<User> GetUsersByUsername(List<string> usernames);
        IEnumerable<User> Find(UserSearchSpec spec);
        IEnumerable<User> GetUserByUserGuid(List<Guid> userGuid);
        Guid Insert(User u);
        void Update(User u);
        void DeleteByUserId(Guid userGuid);
        User GetUserByUserGuid(Guid userGuid);
        User GetEmployeeDirectoryByUserGuid(Guid userGuid);
        void Delete(List<Guid> userGuids);
        IEnumerable<User> GetUsersData(string searchText);
        IEnumerable<User> GetEmployeeDirectoryList(string searchValue, int take, int skip, string sortField, string dir, string filterBy);
        int GetEmployeeDirectoryCount(string searchValue, string filterBy);
        User GetUserByDisplayName(string displayName);
        int GetUsersCountPerson(string searchValue);
        IEnumerable<User> GetUsersPerson(string searchValue, int take, int skip, string sortField, string dir);

        User GetUserByUsername(string username);

        User GetUserByFirstAndLastName(string firstName, string lastName);
        User GetUserByWorkEmail(string workEmail);
    }
}
