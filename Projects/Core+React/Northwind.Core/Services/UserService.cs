using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;

namespace Northwind.Core.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IDatabaseContext _context;
        public UserService(IUserRepository userRepo, IDatabaseContext context)
        {
            _userRepo = userRepo;
            _context = context;
        }

        public User Authenticate(IActiveDirectoryContext adContext, string username, string password)
        {
            if(adContext.UserRepository.validateUser(username, password))
            {
                var users = _userRepo.GetUsersByUsername(new List<string> { username });
                foreach (User user in users)
                {
                    return user;
                }
            }

            return null;
        }

        public IEnumerable<User> Find(UserSearchSpec spec)
        {
            return _userRepo.Find(spec);
        }

        public Guid Insert(User user)
        {
            return _userRepo.Insert(user);
        }

        public IEnumerable<User> GetUsers()
        {
           return  _userRepo.GetUsers();
        }

        public IEnumerable<User> GetUsersByUsername(List<string> usernames)
        {
            return _userRepo.GetUsersByUsername(usernames);
        }

        public void Update(User user)
        {
            _userRepo.Update(user);
        }

        public IEnumerable<User> GetUsers(string searchValue, int take, int skip, string sortField, string dir)
        {
            return _userRepo.GetUsers(searchValue, take, skip, sortField, dir);
        }

        public int GetUserCount(string searchValue)
        {
            return _userRepo.GetUserCount(searchValue);
        }

        public User GetUserByUserGuid(Guid userGuid)
        {
            return _userRepo.GetUserByUserGuid(userGuid);
        }

        public void DeleteByUserId(Guid userGuid)
        {
            _userRepo.DeleteByUserId(userGuid);
        }

        public IEnumerable<User> GetUserByUserGuid(List<Guid> userGuid)
        {
            return _userRepo.GetUserByUserGuid(userGuid);
        }

        public void Delete(List<Guid> userGuids)
        {
            _userRepo.Delete(userGuids);
        }

        public IEnumerable<User> GetUsers(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            return _userRepo.GetUsers(userGuid, searchValue, take, skip, sortField, dir);
        }

        public int GetUserCount(List<Guid> userGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            return _userRepo.GetUserCount(userGuid, searchValue, take, skip, sortField, dir);
        }

        public IEnumerable<User> GetUsersData(string searchText)
        {
            return _userRepo.GetUsersData(searchText);
        }

        public IEnumerable<User> GetEmployeeDirectoryList(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            return _userRepo.GetEmployeeDirectoryList(searchValue, take, skip, sortField, dir, filterBy);
        }

        public int GetEmployeeDirectoryCount(string searchValue, string filterBy)
        {
            return _userRepo.GetEmployeeDirectoryCount(searchValue, filterBy);
        }

        public User GetEmployeeDirectoryByUserGuid(Guid userGuid)
        {
            return _userRepo.GetEmployeeDirectoryByUserGuid(userGuid);
        }

        public User GetUserByDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
                return null;
            return _userRepo.GetUserByDisplayName(displayName.Trim());
        }

        public int GetUsersCountPerson(string searchValue)
        {
            return _userRepo.GetUsersCountPerson(searchValue);
        }

        public IEnumerable<User> GetUsersPerson(string searchValue, int take, int skip, string sortField, string dir)
        {
            return _userRepo.GetUsersPerson(searchValue, take, skip, sortField, dir);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return _userRepo.GetUserByUsername(username);
        }

        public User GetUserByFirstAndLastName(string firstName, string lastName)
        {
            return _userRepo.GetUserByFirstAndLastName(firstName,lastName);
        }

        public User GetUserByWorkEmail(string workEmail)
        {
            if (string.IsNullOrWhiteSpace(workEmail))
                return null;
            return _userRepo.GetUserByWorkEmail(workEmail);
        }
    }
}
