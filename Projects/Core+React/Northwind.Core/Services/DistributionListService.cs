using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class DistributionListService : IDistributionListService
    {
        private readonly IDistributionListRepository _distributionRepository;

        public DistributionListService(IDistributionListRepository userDistributionRepo)
        {
            _distributionRepository = userDistributionRepo;
        }

        public IEnumerable<DistributionList> Get(Guid loggedUser, string searchValue, int pageSize, int skip, int take, string orderBy, string dir)
        {
            var getAll = _distributionRepository.Get(loggedUser, searchValue, pageSize, skip, take, orderBy, dir);
            return getAll;
        }

        public IEnumerable<DistributionList> GetDistributionListByLoggedUser(Guid loggedUser, string searchValue)
        {
            var getAll = _distributionRepository.GetDistributionListByLoggedUser(loggedUser, searchValue);
            return getAll;
        }

        public int DeleteDistributionUserByDistributionListId(Guid distributionUserGuid)
        {
            return _distributionRepository.DeleteDistributionUserByDistributionListId(distributionUserGuid);
        }

        public bool HasDistributionList(Guid loggedUser)
        {
            return _distributionRepository.HasDistributionList(loggedUser);
        }

        public bool IsOwnerOfDistributionList(Guid loggedUser)
        {
            return _distributionRepository.HasDistributionList(loggedUser);
        }

        public bool IsDuplicateTitle(string distributionTitle)
        {
            return _distributionRepository.IsDuplicateTitle(distributionTitle);
        }

        public int Add(DistributionList distribution)
        {
            return _distributionRepository.Add(distribution);
        }

        public int GetCount(Guid loggedUser, string searchValue)
        {
            return _distributionRepository.GetCount(loggedUser, searchValue);
        }

        public int Edit(DistributionList distributionList)
        {
            return _distributionRepository.Edit(distributionList);
        }

        public DistributionList GetDistributionListById(Guid id)
        {
            return _distributionRepository.GetDistributionListById(id);
        }

        public IEnumerable<DistributionUser> GetDistributionUsersById(Guid id)
        {
            return _distributionRepository.GetDistributionUsersById(id);
        }

        public int GetUserCount(string searchValue)
        {
            return _distributionRepository.GetUserCount(searchValue);
        }

        public int AddDistributionUser(DistributionUser distributionUser)
        {
            return _distributionRepository.AddDistributionUser(distributionUser);

        }

        public int Delete(Guid distributionGuid)
        {
            return _distributionRepository.Delete(distributionGuid);
        }
    }
}
