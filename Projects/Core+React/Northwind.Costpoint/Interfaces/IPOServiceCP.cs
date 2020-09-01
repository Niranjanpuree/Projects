using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IPOServiceCP
    {
        IEnumerable<POCP> GetPO(string projectNumber, string searchValue, int skip, int take, string orderBy, string dir, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate);
        int GetCount(string projectNumber, string searchValue, List<AdvancedSearchRequest> postValue, DateTime? startDate, DateTime? endDate);
        POCP GetById(long Id);
    }
}
