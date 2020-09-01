
using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Internationalization;
using Newtonsoft.Json;
using System.Linq;
using Northwind.Core.Specifications;

namespace Northwind.Infrastructure.Data.Fakes
{
    public class FakeUserRepository : IUserRepository
    {
        public void Delete(List<Guid> userGuids)
        {
            throw new NotImplementedException();
        }

        public void DeleteByUserId(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> Find(UserSearchSpec spec)
        {
            throw new NotImplementedException();
        }

        public User GetEmployeeDirectoryByUserGuid(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public int GetEmployeeDirectoryCount(string searchValue, string filterBy)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetEmployeeDirectoryList(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            throw new NotImplementedException();
        }

        public User GetUserByDisplayName(string displayName)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUserGuid(Guid userGuid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUserByUserGuid(List<Guid> userGuid)
        {
            throw new NotImplementedException();
        }

        public int GetUserCount(string searchValue)
        {
            throw new NotImplementedException();
        }

        public int GetUserCount(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            throw new NotImplementedException();
        }

        public ICollection<KeyValuePairModel<Guid, string>> GetUserList()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            
            var userList = JsonConvert.DeserializeObject<IEnumerable<User>>(Resources.SimpleUserList);
            return userList;
        }

        public IEnumerable<User> GetUsers(string searchValue, int take, int skip, string sortField, string dir)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersByUsername(List<string> usernames)
        {
            throw new NotImplementedException();
        }

        public int GetUsersCountPerson(string searchValue)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsersPerson(string searchValue, int take, int skip, string sortField, string dir)
        {
            throw new NotImplementedException();
        }

        public void Insert(User u)
        {
            throw new NotImplementedException();
        }

        public void Update(User u)
        {
            throw new NotImplementedException();
        }

        Guid IUserRepository.Insert(User u)
        {
            throw new NotImplementedException();
        }
        public User GetUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public User GetUserByFirstAndLastName(string firstNamestring, string lastName)
        {
            throw new NotImplementedException();
        }

        public User GetUserByWorkEmail(string workEmail)
        {
            throw new NotImplementedException();
        }
    }
}
