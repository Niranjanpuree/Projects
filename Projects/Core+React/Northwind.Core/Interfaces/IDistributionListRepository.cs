using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IDistributionListRepository
    {
        IEnumerable<DistributionList> Get(Guid loggedUser, string searchValue, int pageSize, int skip, int take, string orderBy, string dir);
        IEnumerable<DistributionList> GetDistributionListByLoggedUser(Guid loggedUser, string searchValue);
        int GetCount(Guid loggedUser, string searchValue);
        int Add(DistributionList distribution);
        int Edit(DistributionList distribution);
        DistributionList GetDistributionListById(Guid id);
        IEnumerable<DistributionUser> GetDistributionUsersById(Guid id);
        int GetUserCount(string searchValue);
        int AddDistributionUser(DistributionUser distributionUser);
        int Delete(Guid distributionGuid);
        int DeleteDistributionUserByDistributionListId(Guid distributionListGuid);
        bool HasDistributionList(Guid loggedUser);
        bool IsDuplicateTitle(string distributionTitle);
    }
}
