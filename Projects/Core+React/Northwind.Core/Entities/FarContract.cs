using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class FarContract
    {
        public Guid FarContractGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid FarContractTypeGuid { get; set; }
        public Guid FarContractTypeClauseGuid { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public string DisplayName { get; set; }
    }
    public class FarContractDetail
    {
        public Guid FarContractGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid FarContractTypeClauseGuid { get; set; }
        public string FarClauseTitle { get; set; }
        public string FarClauseNumber { get; set; }
        public string FarClauseParagraph { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
