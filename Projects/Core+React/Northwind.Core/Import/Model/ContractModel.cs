using Northwind.Core.Import.Validation;
using Northwind.Core.Interfaces.ContractRefactor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class ContractModel
    {
        [ContractDuplicateValidator(ErrorMessage ="Duplicate Contract")]
        [Required]
        public string ContractNumber { get; set; }
        [Required]
        [TaskOrderNumberValidator]
        public string TaskOrderNumber { get; set; }
        public string ProjectNumber { get; set; }
        [Required]
        public bool IsIDIQ { get; set; }
        public bool IsTaskOrder { get; set; }
        [Required]
        public bool IsPrime { get; set; }
        public string SubContractNumber { get; set; }
        [Required]
        public string OrgID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal AwardAmount { get; set; }
        public decimal FundedAmount { get; set; }
        [Required]
        public DateTime POPStart { get; set; }
        [Required]
        public DateTime POPEnd { get; set; }
        [Required]
        public string ContractCountry { get; set; }
        [Required]
        public string ContractState { get; set; }
        [Required]
        public string NAICS { get; set; }
        [Required]
        public string PSC { get; set; }
        [Required]
        public bool CPAREligible { get; set; }
        [Required]
        public string QualityLevel { get; set; }
        public bool QualityLevelRequirement { get; set; }
        public string AcquisitionType { get; set; }
        public string Competition { get; set; }
        public string ContractType { get; set; }
        public string Currency { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public decimal OverHeadFeePercent { get; set; }
        public decimal GAFeePercent { get; set; }
        public decimal FeePercent { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string ApplicableWageDetermination { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public string RevenueRecognitionPercent { get; set; }
        public string OHOnSite { get; set; }
        public string OHOffSite { get; set; }
        [Required]
        public string ContractRepresentative { get; set; }
        public string ProjectControlRepresentative { get; set; }
        public string ProjectManager { get; set; }
        public string AccountingRepresentative { get; set; }
        [Required]
        public string AwardingAgency { get; set; }
        [Required]
        public string AwardAgencyContractRepresentative { get; set; }
        [Required]
        public string AwardingAgencyTechnicalRepresentative { get; set; }
        [Required]
        public string FundingAgency { get; set; }
        [Required]
        public string FundingAgencyContractRepresentative { get; set; }
        [Required]
        public string FundingAgencyTechnicalRepresentative { get; set; }
        public string Action { get; set; }

        public Guid ContractGuid { get; set; }
        public Guid CountryGuid { get; set; }
        public Guid NAICSGuid { get; set; }
        public Guid PSCGuid { get; set; }
        public Guid OrgIDGuid { get; set; }

        public Guid ProjectManagerGuid { get; set; }
        public Guid RegionalManagerGuid { get; set; }
        public Guid ProjectControlsGuid { get; set; }
        public Guid ContractRepresentativeGuid { get; set; }
        public Guid CompanyPresidentGuid { get; set; }
        public Guid AccountRepresentativeGuid { get; set; }

        public Guid FundingAgencyGuid { get; set; }
        public Guid FundingAgencyContractRepresentativeGuid { get; set; }
        public Guid FundingAgencyTechnicalRepresentativeGuid { get; set; }
        public Guid AwardingAgencyGuid { get; set; }
        public Guid AwardingAgencyContractRepresentativeGuid { get; set; }
        public Guid AwardingAgencyTechnicalRepresentativeGuid { get; set; }
    }
}
