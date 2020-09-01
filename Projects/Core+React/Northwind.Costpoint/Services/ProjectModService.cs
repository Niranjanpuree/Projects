using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using System.Collections.Generic;

namespace Northwind.CostPoint.Services
{
    public class ProjectModServiceCP : IProjectModServiceCP
    {
        IProjectModRepositoryCP _projectModRepository;

        public ProjectModServiceCP(IProjectModRepositoryCP projectModRepository)
        {
            _projectModRepository = projectModRepository;
        }

        public IEnumerable<ProjectModCP> GetCostPointProjectModsByProjectNumber(string ProjectNumber)
        {
            return _projectModRepository.GetCostPointProjectModsByProjectNumber(ProjectNumber);
        }

        public ProjectModCP GetProjectModById(long projectModId)
        {
            return _projectModRepository.GetProjectModById(projectModId);
        }

        public ProjectModCP GetBriefedThroughModNo(string projectNumber)
        {
            return _projectModRepository.GetBriefedThroughModNo(projectNumber);
        }

        public IEnumerable<ProjectModCP> GetProjectMods(string ProjectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue)
        {
            return _projectModRepository.GetProjectMods(ProjectNumber, searchValue, skip, take, orderBy, dir, postValue);
        }

        public int GetProjectModsCount(string ProjectNumber, string searchValue, List<AdvancedSearchRequest> postValue)
        {
            return _projectModRepository.GetProjectModsCount(ProjectNumber, searchValue, postValue);
        }
    }
}
