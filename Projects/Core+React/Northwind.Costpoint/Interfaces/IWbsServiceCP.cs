using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IWbsServiceCP
    {
        IEnumerable<WbsCP> GetWbs(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue);
        int GetCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue);
        WbsCP GetById(Guid Id);
    }
}
