using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Core.Models
{
    public class AdvancedSearchRequest
    {
        public AdvancedSearchAttribute Attribute { get; set; }
        public AdvancedSearchOperator Operator { get; set; }
        public bool IsEntity { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
    }

    public class AdvanceSearch
    {
        public string id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }

    public class AdvancedSearchAttribute
    {
        public string AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeTitle { get; set; }
        public int AttributeType { get; set; }
    }

    public class AdvancedSearchOperator
    {
        public string OperatorId { get; set; }
        public string OperatorTitle { get; set; }
        public int OperatorName { get; set; }
        public int OperatorType { get; set; }
    }
}
