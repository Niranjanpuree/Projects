using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Northwind.Core.Specifications
{
    public class SearchVM
    {
        public string AttributeName { get; set; }
        public string Operator { get; set; }
        public string FirstValue { get; set; }
        public string ValueToCompare { get; set; }
    }

    public class LstSearch
    {
        public List<SearchVM> SearchVms { get; set; }
    }

}
