using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IDistributionUserRepository
   {
        IEnumerable<User> GetUsersByDistributionListGuid(Guid distributionListGuid, string searchValue, int take, int skip, string sortField, string dir);

        int GetSelectedUsersCount(Guid distributionListGuid, string searchValue);

        IEnumerable<User> GetUsersExceptDistributionUser(Guid? distributionListGuid, string searchValue, int take, int skip,
           string sortField, string dir);
       int GetUserCountExceptDistributionUser(Guid distributionListGuid, string searchValue);
       int GetDistributionUserCountByDistributionListId(Guid distributionListGuid);
       int RemoveMemberFromDistributionList(Guid DistributioinListId, Guid UserId);
    }
}
