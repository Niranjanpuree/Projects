using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Entities
{
    public class Criteria : ICriteria
    {
        public ResourceAttribute Attribute { get; set; }
        public OperatorName Operator { get; set; }
        public List<object> Value { get; set; }
        public string ValueToCompare { get; set; }  // for  this to that compare like ToDate ..
        public Criteria()
        {
            
        }
    }
}
