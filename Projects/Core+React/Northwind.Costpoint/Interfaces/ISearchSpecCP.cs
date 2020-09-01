using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;

namespace Northwind.Costpoint.Interfaces
{
    public interface ISearchSpecCP
    {
        string SearchText { get; set; }
        int Skip { get; set; }
        int Take { get; set; }
        string OrderBy { get; set; }
        string Direction { get; set; }
        IEnumerable<AdvancedSearchRequest> AdvancedSearchCriteria { get; set; }
    }
}
