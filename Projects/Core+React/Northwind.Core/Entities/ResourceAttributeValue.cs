using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class ResourceAttributeValue
    {
        public Guid ResourceAttributeValueGuid { get; set; }
        public Guid ResourceAttributeGuid { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

    }
}
