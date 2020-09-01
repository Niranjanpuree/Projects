using Northwind.Core.Models;
using Northwind.Costpoint.Entities;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface ICostRepositoryCP
    {
        IEnumerable<CostCP> GetCosts(SearchSpecCP searchSpec);
        IEnumerable<ChartModel> GetCostForPieChart(SearchSpecCP searchSpec);
        IEnumerable<ChartModel> GetCostForBarChart(SearchSpecCP searchSpec);
        int GetCount(SearchSpecCP searchSpec);
        CostCP GetById(long Id);
    }
}
