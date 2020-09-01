using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.CrossSiteInterface;
using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Services
{
    public class ProjectServiceCP : IProjectServiceCP
    {
        IProjectRepositoryCP _projectRepository;
        IRecentActivityService _recentActivityService;
        IContractServiceCrossSite _contractsService;
        public ProjectServiceCP(IProjectRepositoryCP projectRepository, IRecentActivityService recentActivityService,IContractServiceCrossSite contractsService)
        {
            _projectRepository = projectRepository;
            _recentActivityService = recentActivityService;
            _contractsService = contractsService;
        }

        public ProjectCP GetCostPointProjectByProjectNumber(string projectNumber)
        {
            return _projectRepository.GetCostPointProjectByProjectNumber(projectNumber);
        }

        public ProjectCP GetProjectByProjectNumber(string projectNumber)
        {
            return _projectRepository.GetProjectByProjectNumber(projectNumber);
        }

        public int GetProjectCount(Guid UserGuid, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            return _projectRepository.GetProjectCount(UserGuid, searchValue, postValue);
        }

        public IEnumerable<ProjectCP> GetProjects(Guid UserGuid, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue)
        {
            var projectList = _projectRepository.GetProjects(UserGuid, searchValue, skip, take, orderBy, dir, postValue);
            foreach (var project in projectList)
            {
                if (project.ContractGuid != Guid.Empty)
                {
                    project.IsFavorite = _recentActivityService.IsFavouriteActivity(project.ContractGuid ?? Guid.Empty, "PFS-Project", UserGuid);
                }
            }
            return projectList;
        }
        
        public void UpdateContractGuidByProjectNumber(string projectNumber, Guid contractGuid)
        {   
            if(!string.IsNullOrWhiteSpace(projectNumber) && contractGuid != Guid.Empty)
                _projectRepository.UpdateContractGuidByProjectNumber(projectNumber,contractGuid);
        }

    }
}
