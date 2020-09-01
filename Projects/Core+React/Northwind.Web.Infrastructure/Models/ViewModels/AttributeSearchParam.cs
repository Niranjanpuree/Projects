using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Models.ViewModels
{
    public class AttributeSearchParam
    {
        public Guid AttributeId { get; set; }
        public bool IsEntityLookup { get; set; }
        public string Entity { get; set; }
        public OperatorName QueryOperator { get; set; }
    }
}
