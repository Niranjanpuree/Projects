using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractListViewModel
    {
        public Guid ContractGuid { get; set; }
        public bool IsIDIQContract { get; set; }
        public bool IsPrimeContract { get; set; }
        public string ContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public Guid ORGID { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        public Guid? OfficeContractTechnicalRepresent { get; set; }

        public Guid CountryOfPerformance { get; set; }
        public string PlaceOfPerformanceSelectedIds { get; set; }

        public Guid? FundingAgencyOffice { get; set; }

        public string SetAside { get; set; }
        public decimal? SelfPerformancePercent { get; set; }
        public string PlaceOfPerformance { get; set; }
        public DateTime? POPStart { get; set; }
        public DateTime? POPEnd { get; set; }
        public Guid NAICSCode { get; set; }
        public Guid PSCCode { get; set; }
        public bool CPAREligible { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }
        public bool SBA { get; set; }
        public string Competition { get; set; }
        public string ContractType { get; set; }
        public decimal? OverHead { get; set; }
        public decimal? GAPercent { get; set; }
        public decimal? FeePercent { get; set; }
        public string Currency { get; set; }
        public decimal? BlueSkyAwardAmount { get; set; }
        public decimal? AwardAmount { get; set; }
        public decimal? FundingAmount { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string AppWageDetermineDavisBaconAct { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal? RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }

        public string Description { get; set; }
        public string AppWageDetermineServiceContractAct { get; set; }
        public Guid FundingOfficeContractRepresentative { get; set; }
        public Guid FundingOfficeContractTechnicalRepresent { get; set; }
        public Guid? ParentContractGuid { get; set; }
        public int ProjectCounter { get; set; }

        public Guid? AwardingAgencyOffice { get; set; }
        public Guid? OfficeContractRepresentative { get; set; }

        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public string SearchValue { get; set; }

        public ContractUserViewModel ContractRepresentative { get; set; }
        public ContractUserViewModel RegionalManager { get; set; }
        public ContractUserViewModel ProjectManager { get; set; }
        public ContractUserViewModel AccountRepresentative { get; set; }
        public ContractUserViewModel CompanyPresident { get; set; }
        
    }
}
