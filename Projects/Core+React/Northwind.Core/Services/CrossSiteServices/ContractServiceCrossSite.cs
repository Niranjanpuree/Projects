using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services.CrossSiteServices
{
    public class ContractServiceCrossSite : IContractServiceCrossSite
    {
        IContractsRepository _contractRepository;
        IRecentActivityService _recentActivityService;
        IGroupUserService _groupUserService;
        public ContractServiceCrossSite(IContractsRepository contractRepository, IRecentActivityService recentActivityService,
            IGroupUserService groupUserService)
        {
            _contractRepository = contractRepository;
            _recentActivityService = recentActivityService;
            _groupUserService = groupUserService;
        }

        public Guid GetContractGuidByProjectNumber(string projectNumber)
        {
            return _contractRepository.GetContractGuidByProjectNumber(projectNumber);
        }

        public Contracts GetProject(string projectNumber)
        {
            return _contractRepository.GetDetailByContractNumber(projectNumber);
        }

        public IEnumerable<Contracts> GetProjects(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            var isUserInSAGroup = _groupUserService.IsUserInSAGroup(userGuid);
            if (isUserInSAGroup)
                additionalFilter = "all";
            var projectList = _contractRepository.GetProjectListForPFS(searchValue, pageSize, skip, take, orderBy, dir, postValue, userGuid, additionalFilter, isTaskOrder);
            foreach (var project in projectList)
            {
                project.IsFavorite = _recentActivityService.IsFavouriteActivity(project.ContractGuid, "Contract", userGuid);
            }
            return projectList;
        }

        public int GetProjectCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder)
        {
            var isUserInSAGroup = _groupUserService.IsUserInSAGroup(userGuid);
            if (isUserInSAGroup)
                additionalFilter = "all";
            return _contractRepository.GetContractCountForPFS(searchValue, postValue, userGuid, additionalFilter, isTaskOrder);
        }
    }
}
