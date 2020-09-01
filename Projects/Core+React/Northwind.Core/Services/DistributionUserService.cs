using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class DistributionUserService : IDistributionUserService
    {
        private readonly IDistributionUserRepository _distributionUserRepository;
        public DistributionUserService(IDistributionUserRepository distributionUserRepository)
        {
            _distributionUserRepository = distributionUserRepository;
        }
        public IEnumerable<User> GetUsersByDistributionListGuid(Guid distributionListGuid, string searchValue, int take, int skip, string sortField, string dir)
        {
            return _distributionUserRepository.GetUsersByDistributionListGuid(distributionListGuid,searchValue,take,skip, sortField,dir);
        }

        public int GetSelectedUsersCount(Guid distributionListGuid, string searchValue)
        {
            return _distributionUserRepository.GetSelectedUsersCount(distributionListGuid, searchValue);
        }

        public IEnumerable<User> GetUsersExceptDistributionUser(Guid? distributionListGuid, string searchValue, int take, int skip,
            string sortField, string dir)
        {
            return _distributionUserRepository.GetUsersExceptDistributionUser(distributionListGuid, searchValue, take, skip, sortField, dir);
        }

        public int GetUserCountExceptDistributionUser(Guid distributionListGuid, string searchValue)
        {
            return _distributionUserRepository.GetUserCountExceptDistributionUser(distributionListGuid, searchValue);
        }

        public int GetDistributionUserCountByDistributionListId(Guid distributionListGuid)
        {
            return _distributionUserRepository.GetDistributionUserCountByDistributionListId(distributionListGuid);
        }

        public int RemoveMemberFromDistributionList(Guid DistributioinListId, Guid UserId)
        {
            return _distributionUserRepository.RemoveMemberFromDistributionList(DistributioinListId,UserId);

        }
    }
}
