using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IPORepositoryCP
    {
        IEnumerable<POCP> GetPO(SearchSpecCP searchSpec);
        int GetCount(SearchSpecCP searchSpec);
        POCP GetById(long Id);
    }
}
