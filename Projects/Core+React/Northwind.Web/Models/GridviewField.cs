using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class GridviewField
    {
        private string type = "text";

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string format = "{0:%s}";

        public string Format
        {
            get { return format; }
            set { format = value; }
        }


        public string FieldName { get; set; }
        public string FieldLabel { get; set; }
        public bool IsSortable { get; set; }
        public bool IsFilterable { get; set; }
        public int OrderIndex { get; set; }
        public bool IsDefaultSortField { get; set; }
        public bool Clickable { get; set; }
        public bool visibleToGrid { get; set; }
        public string GridColumnCss { get; set; }
    }
}
