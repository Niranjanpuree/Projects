using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IProjectModRepositoryCP
    {
        IEnumerable<ProjectModCP> GetProjectMods(string ProjectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue);
        IEnumerable<ProjectModCP> GetCostPointProjectModsByProjectNumber(string ProjectNumber);
        ProjectModCP GetBriefedThroughModNo(string projectNumber);
        int GetProjectModsCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue);
        ProjectModCP GetProjectModById(long projectModId);
    }
}
