using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class ManagerUserService: IManagerUserService
    {
        private readonly IManagerUserRepository _managerUserRepository;
        public ManagerUserService(IManagerUserRepository managerUserRepository)
        {
            _managerUserRepository = managerUserRepository;
        }

        public void Insert(ManagerUser managerUser)
        {
            _managerUserRepository.Insert(managerUser);
        }

        public IEnumerable<ManagerUser> GetManagerUser(ManagerUser managerUser)
        {
            return _managerUserRepository.GetManagerUser(managerUser);
        }

        public IEnumerable<ManagerUser> GetManagerUser(Guid userGUID)
        {
            return _managerUserRepository.GetManagerUser(userGUID);
        }

        public void Delete(ManagerUser managerUser)
        {
            _managerUserRepository.Delete(managerUser);
        }

        public void DeleteByUserId(Guid userGuid)
        {
            _managerUserRepository.DeleteByUserId(userGuid);
        }

        public void DeleteByManagerId(Guid managerGuid)
        {
            _managerUserRepository.DeleteByManagerId(managerGuid);
        }

        public bool IsManagerExists(Guid userGUID, Guid managerGuid)
        {
            return _managerUserRepository.IsManagerExists(userGUID, managerGuid);
        }
    }
}
