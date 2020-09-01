using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Web.Models.ViewModels.FarClause
{
    public class FarContractTypeClauseViewModel
    {
        public Guid FarContractTypeClauseGuid { get; set; }
        public Guid FarClauseGuid { get; set; }
        public Guid FarContractTypeGuid { get; set; }
        //farcontract Type code
        public string Code { get; set; }
        public string Title { get; set; }
        public string DisplayCode { get { return Code.Replace("&", ""); } }
        public bool IsRequired { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsOptional { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
        public string SelectedValue { get { return IsRequired == true ? "Required" : IsApplicable == true ? "Applicable" : IsOptional == true ? "Optional" : string.Empty; } }
    }
}
