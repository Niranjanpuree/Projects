using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class GridFilter
    {
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }

    public class GridFilters
    {
        public List<GridFilter> Filters { get; set; }
        public string Logic { get; set; }
    }

    public class GridSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }
}
