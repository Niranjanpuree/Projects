using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractAndProjectView
    {
        public Guid ContractGuid { get; set; }
        public bool IsIDIQContract { get; set; }
        public bool IsPrimeContract { get; set; }
        public string IsPrimeContractStatus
        {
            get
            {
                if (IsPrimeContract == true)
                    return "Yes";
                return "No";
            } 
        }
        public string IdiqContract {
            get {
                if (IsIDIQContract)
                    return "Yes";
                return "No";
            }
        }

        public string IsContractString
        {
            get {
                if (IsContract)
                    return "Yes";
                return "No";
            }
        }
        public string ContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public Guid? OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrganizationName { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        public string CompanyCode { get; set; }
        public Guid? CompanyPresident { get; set; }
        public string CompanyPresidentName { get; set; }
        public Guid? RegionalManager { get; set; }
        public string RegionalManagerName { get; set; }
        public Guid? ProjectManager { get; set; }
        public string ProjectManagerName { get; set; }
        public Guid? ProjectControls { get; set; }
        public string ProjectControlsName { get; set; }
        public Guid? AccountingRepresentative { get; set; }
        public string AccountingRepresentativeName { get; set; }
        public Guid? ContractRepresentative { get; set; }
        public string ContractRepresentativeName { get; set; }
        public Guid? SubContractAdministrator { get; set; }
        public string SubContractAdministratorName { get; set; }
        public Guid? PurchasingRepresentative { get; set; }
        public string PurchasingRepresentativeName { get; set; }
        public Guid? HumanResourceRepresentative { get; set; }
        public string HumanResourceRepresentativeName { get; set; }
        public Guid? QualityRepresentative { get; set; }
        public string QualityRepresentativeName { get; set; }
        public Guid? SafetyOfficer { get; set; }
        public string SafetyOfficerName { get; set; }
        public Guid? OperationManager { get; set; }
        public string OperationManagerName { get; set; }

        public Guid? CountryOfPerformance { get; set; }
        public string CountryOfPerformanceName { get; set; }
        public string PlaceOfPerformance { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        //[DataType(DataType.Date)]
        public string POPStart { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public string POPEnd { get; set; }
        public Guid? NAICSCode { get; set; }
        public string NAICSCodeName { get; set; }
        public Guid? PSCCode { get; set; }
        public string PSCCodeName { get; set; }
        public bool CPAREligible { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }

        public string AwardingAgencyOfficeName { get; set; }
        public string Office_ContractRepresentative { get; set; }
        public string Office_ContractTechnicalRepresent { get; set; }

        public string FundingAgencyOfficeName { get; set; }
        public Guid? FundingOffice_ContractRepresentative { get; set; }
        public string FundingOffice_ContractRepresentativeName { get; set; }
        public Guid? FundingOffice_ContractTechnicalRepresent { get; set; }
        public string FundingOffice_ContractTechnicalRepresentName { get; set; }

        public string SetAside { get; set; }
        public decimal SelfPerformance_Percent { get; set; }
        public bool SBA { get; set; }
        public string Competition { get; set; }
        public string ContractType { get; set; }
        public decimal OverHead { get; set; }
        public decimal GAPercent { get; set; }
        public decimal FeePercent { get; set; }
        public string Currency { get; set; }
        public decimal BlueSkyAwardAmount { get; set; }
        public decimal AwardAmount { get; set; }
        public decimal FundingAmount { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string AppWageDetermine_DavisBaconAct { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal RevenueRecognitionEAC_Percent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }
        public string Description { get; set; }
        public string AppWageDetermine_ServiceContractAct { get; set; }
       
        public bool IsActive { get; set; }
        public string IsActiveStatus { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public Guid ProjectGuid { get; set; }
        public Guid ORGID { get; set; }
        public string ORGIDName { get; set; }
        public string ProjectTitle { get; set; }
        public Guid ProjectRepresentative { get; set; }
        public string ProjectRepresentativeName { get; set; }
        public string Office_ProjectRepresentative { get; set; }
        public string Office_ProjectTechnicalRepresent { get; set; }
        public string InvoiceSubmissionMethodName { get; set; }
        public string PaymentTermsName { get; set; }
        public string AppWageDetermine_ServiceProjectAct { get; set; }
        public Guid FundingOffice_ProjectRepresentative { get; set; }
        public string FundingOffice_ProjectRepresentativeName { get; set; }
        public Guid FundingOffice_ProjectTechnicalRepresent { get; set; }
        public string FundingOffice_ProjectTechnicalRepresentName { get; set; }
        public int ProjectCounter { get; set; }
        public bool IsContract { get; set; }
        public string IsContractType { get; set; }
        public string ApplicableWageDetemination { get; set; }
        public bool IsFavorite { get; set; }
        public string Status { get; set; }

    }
}
