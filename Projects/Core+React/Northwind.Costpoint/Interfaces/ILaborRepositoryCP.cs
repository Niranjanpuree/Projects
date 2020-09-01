using Northwind.Core.Models;
using Northwind.CostPoint.Entities;
using System;
using System.Collections.Generic;
using Northwind.Costpoint.Entities;
using Northwind.Costpoint.Interfaces;

namespace Northwind.CostPoint.Interfaces
{
    public interface ILaborRepositoryCP
    {
        IEnumerable<LaborCP> GetLabor(SearchSpecCP searchSpec);
        int GetLaborCount(SearchSpecCP searchSpec);
        IEnumerable<ChartModel> GetLaborForPieChart(SearchSpecCP searchSpec);
        IEnumerable<ChartModel> GetLaborForBarChart(SearchSpecCP searchSpec);
    }
}
