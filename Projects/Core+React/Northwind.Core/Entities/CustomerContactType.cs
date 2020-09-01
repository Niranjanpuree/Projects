using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class CustomerContactType:BaseModel
    {
        public Guid ContactTypeGuid { get; set; } 
        public string ContactTypeName { get; set; } 
    }
}
