using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IActiveDirectoryUserRepository
    {
        bool validateUser(string username, String password);
        IEnumerable<User> GetUsers();
        AdUserGroupAndManager GetUserAdUserByUsername(string username);
        AdUserGroupAndManager GetUserAdUserByCN(string username);
    }
}
