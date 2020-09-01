using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface ICriteria
    {
        ResourceAttribute Attribute { get; set; }
        OperatorName Operator { get; set; }
        List<object> Value { get; set; }
        string ValueToCompare { get; set; }
    }
}
