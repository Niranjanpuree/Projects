using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class SortCriteria
    {
        public string AttributeName { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
