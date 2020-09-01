using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IProjectServiceCP
    {
        IEnumerable<ProjectCP> GetProjects(Guid UserGuid, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue);
        int GetProjectCount(Guid UserGuid, string searchValue, List<AdvancedSearchRequest> postValue);
        ProjectCP GetProjectByProjectNumber(string projectNumber);
        ProjectCP GetCostPointProjectByProjectNumber(string projectNumber);
        void UpdateContractGuidByProjectNumber(string projectNumber, Guid contractGuid);
    }
}
