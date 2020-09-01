using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class GridHeaderModel
    {
        public Guid ContractGuid { get; set; }
        public Guid ProjectGuid { get; set; }

        public string WBSCode { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ContractType { get; set; }
        public string InvoiceAtThisLevel { get; set; }

        public string SubContractor { get; set; }
        public string LaborCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Rate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
}
