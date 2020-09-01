using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Specifications
{
    public class SearchSpec
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string Dir { get; set; }
    }
}
