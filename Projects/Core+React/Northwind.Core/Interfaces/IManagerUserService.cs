using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IManagerUserService
    {
        void Insert(ManagerUser managerUser);
        void Delete(ManagerUser managerUser);
        void DeleteByUserId(Guid userGuid);
        void DeleteByManagerId(Guid managerGuid);
        IEnumerable<ManagerUser> GetManagerUser(ManagerUser managerUser);
        IEnumerable<ManagerUser> GetManagerUser(Guid userGUID);
        bool IsManagerExists(Guid userGUID, Guid managerGuid);
    }
}
