using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class FarContractType
    {
        public Guid FarContractTypeGuid { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
        public string UpdatedByName { get; set; }
    }
}
