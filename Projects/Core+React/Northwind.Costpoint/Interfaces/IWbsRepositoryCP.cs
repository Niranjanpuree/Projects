using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IWbsRepositoryCP
    {
        IEnumerable<WbsCP> GetWbs(SearchSpecCP searchSpec);
        int GetCount(SearchSpecCP searchSpec);
        WbsCP GetById(Guid Id);
    }
}
