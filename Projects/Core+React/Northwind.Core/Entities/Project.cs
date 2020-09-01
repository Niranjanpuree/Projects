using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Utilities;

namespace Northwind.Core.Entities
{
    public class ProjectForList
    {
        public Guid ContratGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public string ProjectNumber { get; set; }
        public Guid ORGID { get; set; }
        public string ORGIDName { get; set; }
        public string ContratTitle { get; set; }
        public Guid CompanyPresident { get; set; }
        public string CompanyPresidentName { get; set; }
        public Guid RegionalManager { get; set; }
        public string RegionalManagerName { get; set; }
        public Guid ContratManager { get; set; }
        public string ContratManagerName { get; set; }
        public Guid ContratControls { get; set; }
        public string ContratControlsName { get; set; }
        public Guid AccountingRepresentative { get; set; }
        public string AccountingRepresentativeName { get; set; }
        public Guid ContratRepresentative { get; set; }
        public string ContratRepresentativeName { get; set; }
        public Guid CountryOfPerformance { get; set; }
        public string CountryOfPerformanceName { get; set; }
        public string PlaceOfPerformance { get; set; }
        public DateTime? POPStart { get; set; }
        public DateTime? POPEnd { get; set; }
        public Guid NAICSCode { get; set; }
        public string NAICSCodeName { get; set; }
        public Guid PSCCode { get; set; }
        public string PSCCodeName { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }
        public string AwardingAgencyOffice { get; set; }
        public string Office_ContratRepresentative { get; set; }
        public string Office_ContratTechnicalRepresent { get; set; }
        public string FundingAgencyOffice { get; set; }
        public string SetAside { get; set; }
        public decimal SelfPerformance_Percent { get; set; }
        public bool SBA { get; set; }
        public string Competition { get; set; }
        public string ContratType { get; set; }
        public decimal OverHead { get; set; }
        public decimal G_A_Percent { get; set; }
        public decimal Fee_Percent { get; set; }
        public string Currency { get; set; }
        public decimal BlueSkyAward_Amount { get; set; }
        public decimal AwardAmount { get; set; }
        public decimal FundingAmount { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public string InvoiceSubmissionMethod{ get; set; }
        public string InvoiceSubmissionMethodName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentTermsName { get; set; }
        public string AppWageDetermine_DavisBaconAct { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal RevenueRecognitionEAC_Percent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string AppWageDetermine_ServiceContratAct { get; set; }
        public Guid FundingOffice_ContratRepresentative { get; set; }
        public string FundingOffice_ContratRepresentativeName { get; set; }
        public Guid FundingOffice_ContratTechnicalRepresent { get; set; }
        public string FundingOffice_ContratTechnicalRepresentName { get; set; }
        public int ContratCounter { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}