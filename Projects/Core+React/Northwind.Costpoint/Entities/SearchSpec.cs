using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Models;
using Northwind.Costpoint.Interfaces;

namespace Northwind.Costpoint.Entities
{
    public class SearchSpecCP: ISearchSpecCP
    {
        public string ProjectNumber { get; set; }
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string OrderBy { get; set; }
        public string Direction { get; set; }
        public IEnumerable<AdvancedSearchRequest> AdvancedSearchCriteria { get; set; } 
        public SearchSpecCP()
        {
            AdvancedSearchCriteria = new List<AdvancedSearchRequest>();
        }

    }
}
