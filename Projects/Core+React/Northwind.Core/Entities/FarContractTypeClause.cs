using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class FarContractTypeClause
    {
        public Guid FarContractTypeClauseGuid { get; set; }
        public Guid FarClauseGuid { get; set; }
        public Guid FarContractTypeGuid { get; set; }
        public bool IsRequired { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsOptional { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
