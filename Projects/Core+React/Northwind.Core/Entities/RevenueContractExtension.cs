using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class RevenueContractExtension:BaseModel
    {
        public Guid RevenueGuid { get; set; }
        public Guid ContractExtensionGuid { get; set; }
        public DateTime? ExtensionDate { get; set; }
    }
}
