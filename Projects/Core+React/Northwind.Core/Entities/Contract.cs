using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Northwind.Core.Utilities;

namespace Northwind.Core.Entities
{
    public class Contract : BaseModel
    {
        //Basic contract info..
        public Guid ContractGuid { get; set; }
        public Guid? ParentContractGuid { get; set; }
        public bool IsIDIQContract { get; set; }
        public bool IsPrimeContract { get; set; }
        public string ContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public string Description { get; set; }
        public Guid ORGID { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        //public Guid? CountryOfPerformance { get; set; }
        public string CountryOfPerformance { get; set; }
        public string PlaceOfPerformance { get; set; }
        public string PlaceOfPerformanceSelectedIds { get; set; }
        public DateTime? POPStart { get; set; }
        public DateTime? POPEnd { get; set; }
        public Guid NAICSCode { get; set; }
        public Guid PSCCode { get; set; }
        public bool CPAREligible { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }

        //Key personal
        public Guid? CompanyPresident { get; set; }
        public Guid? RegionalManager { get; set; }
        public Guid? ProjectManager { get; set; }
        public Guid? ProjectControls { get; set; }
        public Guid? AccountingRepresentative { get; set; }
        public Guid? ContractRepresentative { get; set; }

        //later added property related to key personal from view model
        public string CompanyPresidentName { get; set; }
        public string RegionalManagerName { get; set; }
        public string ProjectManagerName { get; set; }
        public string ProjectControlName { get; set; }
        public string AccountingRepresentativeName { get; set; }
        public string ContractRepresentativeName { get; set; }


        //customer info
        public Guid? AwardingAgencyOffice { get; set; }
        public Guid? OfficeContractRepresentative { get; set; }
        public Guid? OfficeContractTechnicalRepresent { get; set; }
        public Guid? FundingAgencyOffice { get; set; }
        public Guid? FundingOfficeContractRepresentative { get; set; }
        public Guid? FundingOfficeContractTechnicalRepresent { get; set; }

        //later added property related to customer info from view model
        public string AwardingAgencyOfficeName { get; set; }
        public string FundingAgencyOfficeName { get; set; }

        //financial info..
        public string SetAside { get; set; }
        public decimal? SelfPerformancePercent { get; set; }
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
        public string AppWageDetermineServiceContractAct { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal? RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }
        public int? ProjectCounter { get; set; }

        //later added property related to financial info from view model
        public string ContractTypeName { get; set; }
        public string CurrencyName { get; set; }

        //later added property based on viewmodel
        public int IdFilterList { get; set; }
        public bool ShowHideTaskOrder { get; set; }
        public bool IsPermissionJobRequest { get; set; }
        public string UpdatedByName { get; set; }
        public string IsContract { get; set; }
        public string OrganizationName { get; set; }
        public string NAICSCodeName { get; set; }
        public string PSCCodeName { get; set; }

        public Guid? PreviousProjectGuid { get; set; }
        public Guid? NextProjectGuid { get; set; }

        public BasicContractInfoModel BasicContractInfo { get; set; }
        public KeyPersonnelModel KeyPersonnel { get; set; }
        public CustomerInformationModel CustomerInformation { get; set; }
        public FinancialInformationModel FinancialInformation { get; set; }
        public CustomerAttributeValuesModel CustomerAttributeValuesModel { get; set; }
        public ContractQuestionaire DbContractQuestionaire { get; set; }
        public ContractQuestionaire ContractQuestionaire { get; set; }
        public ContractWBS ContractWBS { get; set; }
        public EmployeeBillingRates EmployeeBillingRates { get; set; }
        public LaborCategoryRates LaborCategoryRates { get; set; }
        public RevenueRecognition RevenueRecognition { get; set; }
        public JobRequest JobRequest { get; set; }
        public EnumGlobal.ModuleType ModuleType { get; set; }
        public DateTime Periodofperformancestart { get; set; }
        public DateTime Periodofperformanceend { get; set; }

        public virtual ICollection<KeyValuePairModel<Guid, string>> CountrySelectListItems { get; set; }
    }
    public class BasicContractInfoModel
    {
        public bool IsIDIQContract { get; set; }
        
        public bool IsPrimeContract { get; set; }
        [Required]
        public string ContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public string Description { get; set; }
        public Guid ORGID { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        public Guid CountryOfPerformance { get; set; }
        public string PlaceOfPerformance { get; set; }
        public string PlaceOfPerformanceSelectedIds { get; set; }
        public DateTime? POPStart { get; set; }
        public DateTime? POPEnd { get; set; }
        public Guid NAICSCode { get; set; }
        public Guid PSCCode { get; set; }
        public bool CPAREligible { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }

        //later added property from view model
        public string IDIQContract { get; set; }
        public string PrimeContract { get; set; }
        public string OrganizationName { get; set; }
        public string CountryOfPerformanceSelected { get; set; }
        public string PlaceOfPerformanceSelected { get; set; }
        public string NAICSCodeName { get; set; }
        public string PSCCodeName { get; set; }
        public string CPAREligibleString { get; set; }
        public string QualityLevelRequirementsString { get; set; }
        public string QualityLevelName { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string OfficeName { get; set; }
    }
    public class KeyPersonnelModel
    {
        public Guid CompanyPresident { get; set; }
        public Guid RegionalManager { get; set; }
        public Guid ProjectManager { get; set; }
        public Guid ProjectControls { get; set; }
        public Guid AccountingRepresentative { get; set; }
        public Guid ContractRepresentative { get; set; }

        //later added property from view model
        public string CompanyPresidentName { get; set; }
        public string RegionalManagerName { get; set; }
        public string ProjectManagerName { get; set; }
        public string ProjectControlName { get; set; }
        public string AccountingRepresentativeName { get; set; }
        public string ContractRepresentativeName { get; set; }
    }
    public class CustomerInformationModel
    {
        public Guid AwardingAgencyOffice { get; set; }
        public Guid OfficeContractRepresentative { get; set; }
        public Guid OfficeContractTechnicalRepresent { get; set; }
        public Guid FundingAgencyOffice { get; set; }
        public Guid FundingOfficeContractRepresentative { get; set; }
        public Guid FundingOfficeContractTechnicalRepresent { get; set; }

        //later added property from view model
        public string AwardingAgencyOfficeName { get; set; }
        public string AwardingAgencyCustomerTypeName { get; set; }
        public Guid? OfficeContractRepresentativeHiddenGuid { get; set; }
        public string OfficeContractRepresentativeName { get; set; }
        public Guid OfficeContractTechnicalRepresentHiddenGuid { get; set; }
        public string OfficeContractTechnicalRepresentName { get; set; }
        public string FundingAgencyOfficeName { get; set; }
        public string FundingAgencyCustomerTypeName { get; set; }
        public Guid? FundingOfficeContractRepresentativeHiddenGuid { get; set; }
        public string FundingOfficeContractRepresentativeName { get; set; }
        public string FundingOfficeContractTechnicalRepresentName { get; set; }

        public string ContractRepresentativePhoneNumber { get; set; }
        public string ContractRepresentativeEmailAddress { get; set; }
        public string ContractRepresentativeAltEmailAddress { get; set; }
        public string ContractRepresentativeAltPhoneNumber { get; set; }
        public string ContractTechnicalRepresentativePhoneNumber { get; set; }
        public string ContractTechnicalRepresentativeAltPhoneNumber { get; set; }
        public string ContractTechnicalRepresentativeEmailAddress { get; set; }
        public string ContractTechnicalRepresentativeAltEmailAddress { get; set; }
        public string OfficeContractRepresentativePhoneNumber { get; set; }
        public string OfficeContractRepresentativeEmailAddress { get; set; }
        public string OfficeContractRepresentativeAltEmailAddress { get; set; }
        public string OfficeContractRepresentativeAltPhoneNumber { get; set; }
        public string OfficeContractTechnicalRepresentativePhoneNumber { get; set; }
        public string OfficeContractTechnicalRepresentativeAltPhoneNumber { get; set; }
        public string OfficeContractTechnicalRepresentativeEmailAddress { get; set; }
        public string OfficeContractTechnicalRepresentativeAltEmailAddress { get; set; }

    }
    public class FinancialInformationModel
    {
        public string SetAside { get; set; }
        public decimal SelfPerformancePercent { get; set; }
        public bool SBA { get; set; }
        public string Competition { get; set; }
        public string ContractType { get; set; }
        public decimal OverHead { get; set; }
        public decimal GAPercent { get; set; }
        public decimal FeePercent { get; set; }
        public string Currency { get; set; }
        public decimal BlueSkyAwardAmount { get; set; }
        public decimal? AwardAmount { get; set; }
        public decimal FundingAmount { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string AppWageDetermineDavisBaconAct { get; set; }
        public string AppWageDetermineServiceContractAct { get; set; }
        public string ApplicableWageDetermination { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public string BillingFormulaName { get; set; }
        public string RevenueFormulaName { get; set; }
        public decimal RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }

        //later added property from view model
        public string SetAsideName { get; set; }
        public string SBAString { get; set; }
        public string CompetitionType { get; set; }
        public string ContractTypeName { get; set; }
        public string CurrencyName { get; set; }
        public string FormattedBlueSkyAwardAmount { get; set; }
        public string FormattedAwardAmount { get; set; }
        public string BillingFrequencyName { get; set; }
        public string InvoiceSubmissionMethodName { get; set; }
        public string PaymentTermsName { get; set; }
        public string AppWageDetermineDavisBaconActType { get; set; }
        public string AppWageDetermineServiceContractActType { get; set; }
        public decimal CumulativeAwardAmount { get; set; }
        public decimal CumulativeFundingAmount { get; set; }

    }
    public class CustomerAttributeValuesModel
    {
        public IDictionary<string, string> DDLSetAside { get; set; }
        public IDictionary<string, string> DDLContractType { get; set; }
        public IDictionary<string, string> DDLCurrency { get; set; }
        public IDictionary<string, string> DDLBillingFrequency { get; set; }
        public IDictionary<string, string> DDLInvoiceSubmissionMethod { get; set; }
        public IDictionary<string, string> DDLQualityLevel { get; set; }
        public IDictionary<string, string> DDLPaymentTerms { get; set; }
        public IDictionary<string, string> RadioAppWageDetermineServiceContractAct { get; set; }
        public IDictionary<string, string> RadioAppWageDetermineDavisBaconAct { get; set; }
        public IDictionary<string, string> RadioCompetition { get; set; }
        public IDictionary<bool, string> RadioIsIDIQContract { get; set; }
        public IDictionary<bool, string> RadioIsPrimeContract { get; set; }
        public IDictionary<bool, string> RadioQualityLevelRequirements { get; set; }
        public IDictionary<bool, string> RadioCPAREligible { get; set; }
        public IDictionary<bool, string> RadioSBA { get; set; }

        //later added property from view model
        public IDictionary<string, string> DDLProjectType { get; set; }
        public IDictionary<string, string> RadioAppWageDetermineServiceProjectAct { get; set; }
        public IDictionary<bool, string> RadioIsPrimeProject { get; set; }

    }
    public class AutoCompleteReturnModel
    {
        public string label { get; set; }
        public string value { get; set; }
    }
    #region later added model from view model
    public class AssociateUserList
    {
        public User CompanyPresident { get; set; }
        public User RegionManager { get; set; }
        public User DeputyRegionManager { get; set; }
        public User HealthAndSafetyRegionManager { get; set; }
        public User BusinessDevelopmentRegionManager { get; set; }
        public User OperationManager { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string OfficeName { get; set; }
    }
    public class EntityCode
    {
        public string CompanyCode { get; set; }
        public string RegionCode { get; set; }
        public string OfficeCode { get; set; }
    }
    //public class AutoCompleteReturnModel
    //{
    //    public string Label { get; set; }
    //    public string Value { get; set; }
    //}
    #endregion

    public class ContractForList
    {
        public Guid ContractGuid { get; set; }
        public bool IsIDIQContract { get; set; }
        public bool IsPrimeContract { get; set; }
        public string ContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public Guid? OrgId { get; set; }
        public string OrgName { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle{ get; set; }
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
        public Guid? CountryOfPerformance { get; set; }
        public string CountryOfPerformanceName { get; set; }
        public string PlaceOfPerformance { get; set; }
        public DateTime? POPStart { get; set; }
        public DateTime? POPEnd { get; set; }
        public Guid? NAICSCode { get; set; }
        public string NAICSCodeName { get; set; }
        public Guid? PSCCode { get; set; }
        public string PSCCodeName { get; set; }
        public bool CPAREligible { get; set; }
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevel { get; set; }
        public string AwardingAgencyOffice { get; set; }
        public string OfficeContractRepresentative { get; set; }
        public string OfficeContractTechnicalRepresent { get; set; }
        public string FundingAgencyOffice { get; set; }
        public string SetAside { get; set; }
        public decimal SelfPerformancePercent { get; set; }
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
        public string AppWageDetermineDavisBaconAct { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }
        public string Description { get; set; }
        public string AppWageDetermineServiceContractAct { get; set; }
        public Guid? FundingOfficeContractRepresentative { get; set; }
        public string FundingOfficeContractRepresentativeName { get; set; }
        public Guid? FundingOfficeContractTechnicalRepresent { get; set; }
        public string FundingOfficeContractTechnicalRepresentName { get; set; }
        public bool IsContract { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveStatus { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}