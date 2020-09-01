using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IDistributionListService
    {
        IEnumerable<DistributionList> Get(Guid loggedUser, string searchValue, int pageSize, int skip, int take, string orderBy, string dir);
        IEnumerable<DistributionList> GetDistributionListByLoggedUser(Guid loggedUser, string searchValue);
        int Add(DistributionList distribution);
        int GetCount(Guid loggedUser,string searchValue);
        int Edit(DistributionList groupUser);
        DistributionList GetDistributionListById(Guid id);
        IEnumerable<DistributionUser> GetDistributionUsersById(Guid id);
        int GetUserCount(string searchValue);
        int AddDistributionUser(DistributionUser distributionUser);
        int Delete(Guid distributionGuid);
        int DeleteDistributionUserByDistributionListId(Guid distributionUserGuid);
        bool HasDistributionList(Guid loggedUser);
        bool IsOwnerOfDistributionList(Guid loggedUser);
        bool IsDuplicateTitle(string distributionTitle);
    }
}
