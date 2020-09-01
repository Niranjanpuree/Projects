using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class QueryOperator
    {
        public Guid QueryOperatorGuid { get; set; }
        public OperatorName Name { get; set; }
        public string Title { get; set; }
        public ResourceAttributeType Type { get; set; }

    }
}
