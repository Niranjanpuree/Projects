using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.CrossSiteInterface
{
    public interface IContractServiceCrossSite
    {
        Guid GetContractGuidByProjectNumber(string projectNumber);
        IEnumerable<Contracts> GetProjects(string searchValue, int pageSize, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);
        Contracts GetProject(string projectNumber);
        int GetProjectCount(string searchValue, List<AdvancedSearchRequest> postValue, Guid userGuid, string additionalFilter, bool isTaskOrder);
    }
}
