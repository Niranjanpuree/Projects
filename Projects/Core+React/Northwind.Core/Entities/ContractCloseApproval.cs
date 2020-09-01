using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class ContractCloseApproval
    {
        public Guid ContractCloseApprovalGuid { get; set; }
        public string Title { get; set; }
        public string NormalizedValue { get; set; }
    }
}
