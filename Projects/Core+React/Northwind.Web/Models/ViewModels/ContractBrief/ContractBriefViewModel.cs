using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;
using Northwind.Core.Utilities;
using Northwind.CostPoint.Entities;

namespace Northwind.Web.Models.ViewModels.ContractBrief
{
    public class ContractBriefViewModel
    {
        public Guid ContractGuid { get; set; }
        public string ContractorName { get; set; }
        public string ContractNumber { get; set; }
        public string DateOfAward { get; set; }
        public string ContractJobNumber { get; set; }
        public DateTime BriefedDated { get; set; }
        public string ContractType { get; set; }
        public bool IsSubContract { get; set; }
        public string BriefStatementOfScopeOfWork { get; set; }
        public string PrimeContractor { get; set; }
        public string PrimeContractType { get; set; }
        public string PrimeAddress { get; set; }
        public string PrimePointOfContact { get; set; }
        public string PrimePhone { get; set; }
        public string CognizantDCAAOffice { get; set; }
        public string AcquisitionAgency { get; set; }
        public string AcquisitionAgencyAddress { get; set; }
        public string AcquisitionAgencyPointOfContact { get; set; }
        public string AcquisitionAgencyPhone { get; set; }
        public string AdministrativeContractOffice { get; set; }
        public string AdministrativeContractOfficeAddress { get; set; }
        public string AdministrativeContractOfficePointOfContact { get; set; }
        public string AdministrativeContractOfficePhone { get; set; }
        public string Currnecy { get; set; }
    }
}
