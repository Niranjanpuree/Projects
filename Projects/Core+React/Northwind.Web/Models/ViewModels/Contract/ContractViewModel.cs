using Microsoft.AspNetCore.Http;
using Northwind.Core.Entities;
using Northwind.Core.Utilities;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Models.ViewModels.RevenueRecognition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractViewModel : BaseModel
    {
        //public List<IFormFile> files { get; set; }
        public IFormFileCollection files { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid? ParentContractGuid { get; set; }
        public BasicContractInfoViewModel BasicContractInfo { get; set; }
        public KeyPersonnelViewModel KeyPersonnel { get; set; }
        public CustomerInformationViewModel CustomerInformation { get; set; }
        public FinancialInformationViewModel FinancialInformation { get; set; }
        public CustomerAttributeValuesViewModel CustomerAttributeValuesModel { get; set; }
        public int idFilterList { get; set; }
        public bool ShowHideTaskOrder { get; set; }
        public bool IsPermissionJobRequest { get; set; }
        public bool IsJobEditable { get; set; } = false;
        public FilterByList FilterList { get; set; }
        public string FilterBy { get; set; }
        public bool IsAuthorizedForRevenue { get; set; }
        public bool IsValidForRevenueRecognitionRequest { get; set; }
        public bool IsFarContractAvailable { get; set; } = false;

        public bool IsContractRepresentative { get; set; }
        public bool IsProjectManager { get; set; }
        public bool IsProjectControls { get; set; }
        public bool IsAccountingRepresentative { get; set; }

        public Guid? PreviousProjectGuid { get; set; }
        public Guid? NextProjectGuid { get; set; }
        public int? ProjectCounter { get; set; }
        public ContractQuestionaireViewModel dbContractQuestionaire { get; set; }
        public string UpdatedByName { get; set; }
        public ContractQuestionaireViewModel ContractQuestionaire { get; set; }
        public QuestionaireViewModel Questionaire { get; set; }
        public ContractWBSViewModel ContractWBS { get; set; }
        public EmployeeBillingRatesViewModel EmployeeBillingRatesViewModel { get; set; }
        public LaborCategoryRatesViewModel LaborCategoryRates { get; set; }
        public ContractCloseOutDetail ContractCloseOutDetail { get; set; }
        public RevenueRecognitionViewModel RevenueRecognitionModel { get; set; }
        public JobRequestViewModel JobRequest { get; set; }
        public EnumGlobal.ModuleType ModuleType { get; set; }
        public bool IsAuthorizedForContractClose { get; set; }
        // Grid Columnn List

        //basic info..
        [DisplayName("Is IDIQ Contract")]
        public bool IsIDIQContract { get; set; }
        public string IDIQContract { get { return IsIDIQContract == true ? "Yes" : "No"; } }
        public string ContractNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        public decimal AwardAmount { get; set; }
        public DateTime Periodofperformancestart { get; set; }
        public DateTime Periodofperformanceend { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string ContractType { get; set; }
        public string IsContract { get; set; }
        public string Description { get; set; }
        public string SubContractNumber { get; set; }
        public string OrganizationName { get; set; }
        public string CountryOfPerformance { get; set; }
        public string PlaceOfPerformance { get; set; }
        public string NAICSCodeName { get; set; }
        public string PSCCodeName { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string Status { get; set; }

        //key personal info..
        public string CompanyPresidentName { get; set; }
        public string RegionalManagerName { get; set; }
        public string ProjectManagerName { get; set; }
        public string ProjectControlName { get; set; }
        public string AccountingRepresentativeName { get; set; }
        public string ContractRepresentativeName { get; set; }


        // customer info..
        public string AwardingAgencyOfficeName { get; set; }
        public string FundingAgencyOfficeName { get; set; }


        //financial info..
        public string ContractTypeName { get; set; }
        public string OverHead { get; set; }
        public string GAPercent { get; set; }
        public string FeePercent { get; set; }
        public string CurrencyName { get; set; }
        public decimal BlueSkyAwardAmount { get; set; }
        public virtual Dictionary<Guid, string> CountrySelectListItems { get; set; }
        public ContractFileViewModel ContractFileViewModel { get; set; }
        public List<ContractFileViewModel> lstContractFileViewModel { get; set; }
        public bool IsImported { get; set; }
        public int TaskNodeId { get; set; }

    }
    public class BasicContractInfoViewModel
    {
        [Required]
        [DisplayName("Is this an IDIQ Contract?")]
        public bool IsIDIQContract { get; set; }
        public string IDIQContract { get { return IsIDIQContract == true ? "Yes" : "No"; } }
        [Required]
        [DisplayName("Is this a prime contract?")]
        public bool IsPrimeContract { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PrimeContract { get { return IsPrimeContract == true ? "Yes" : "No"; } }
        [Required]
        [Display(Name = "Contract Number")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractNumber { get; set; }
        [Display(Name = "Contract Description")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string Description { get; set; }
        //        [Required]
        [Display(Name = "Subcontract Number")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SubContractNumber { get; set; }
        [Required]
        [Display(Name = "Organization Id")]
        public Guid ORGID { get; set; }

        [Required]
        [Display(Name = "Organization Id")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OrganizationName { get; set; }
        [Display(Name = "Project Number")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ProjectNumber { get; set; }
        [Display(Name = "Contract Title")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        [Required]
        public string ContractTitle { get; set; }
        [Required]
        [Display(Name = "Country Of Performance")]
        public Guid CountryOfPerformance { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string CountryOfPerformanceSelected { get; set; }
        [Required]
        [Display(Name = "Place Of Performance")]
        public List<string> PlaceOfPerformance { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PlaceOfPerformanceSelected { get; set; }

        private string _PlaceOfPerformanceSelectedIds;

        public string PlaceOfPerformanceSelectedIds
        {
            get { return PlaceOfPerformance != null ? string.Join(",", PlaceOfPerformance) : _PlaceOfPerformanceSelectedIds; }
            set { _PlaceOfPerformanceSelectedIds = value; }
        }
        [Required]
        [Display(Name = "Period of Performance Start")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? POPStart { get; set; }
        [Required]
        [Display(Name = "Period of Performance End")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? POPEnd { get; set; }
        [Required]
        [DisplayName("NAICS Code")]
        public Guid NAICSCode { get; set; }
        [Required]
        [Display(Name = "NAICS Code")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string NAICSCodeName { get; set; }
        [DisplayName("PSC Code")]
        public Guid PSCCode { get; set; }
        //        [Required]
        [Display(Name = "PSC Code")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PSCCodeName { get; set; }
        [DisplayName("CPAR Eligible")]
        public bool CPAREligible { get; set; }
        public string CPAREligibleString { get { return CPAREligible == true ? "Yes" : "No"; } }
        [DisplayName("Quality Level Requirement")]
        public bool QualityLevelRequirements { get; set; }
        public string QualityLevelRequirementsString { get { return QualityLevelRequirements == true ? "Yes" : "No"; } }
        [DisplayName("Quality Level")]
        //        [Required]
        public string QualityLevel { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string QualityLevelName { get; set; }
        [Display(Name = "Company Name")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string CompanyName { get; set; }
        [Display(Name = "Region Name")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string RegionName { get; set; }
        [Display(Name = "Office Name")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeName { get; set; }
        public string ParentContractNumber { get; set; }
        public string ParentProjectNumber { get; set; }

        [DisplayFormat(NullDisplayText = "Not Entered")]
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AddressLine2 { get; set; }
        [DisplayName("Address Line 3")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AddressLine3 { get; set; }
        [DisplayName("City")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string City { get; set; }
        [DisplayName("Province")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string Province { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        [DisplayName("County")]
        public string County { get; set; }
        [DisplayName("Postal Code")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PostalCode { get; set; }
        [DisplayName("FAR Contract Type")]
        public Guid FarContractTypeGuid { get; set; }

        [DisplayName("FAR Contract Type")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string FarContractType { get; set; }
    }
    public class KeyPersonnelViewModel
    {
        [Display(Name = "Company President")]
        public Guid CompanyPresident { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string CompanyPresidentName { get; set; }
        [Display(Name = "Regional Manager")]
        public Guid? RegionalManager { get; set; }
        [Display(Name = "Deputy Regional Manager")]
        public Guid? DeputyRegionalManager { get; set; }
        [Display(Name = "Health And Safety Regional Manager")]
        public Guid? HealthAndSafetyRegionalManager { get; set; }
        [Display(Name = "Business Development Regional Manager")]
        public Guid? BusinessDevelopmentRegionalManager { get; set; }
        [Display(Name = "Operation Manager")]
        public string OperationManagerName { get; set; }
        [Display(Name = "Operation Manager")]
        public Guid? OperationManager { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string RegionalManagerName { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string DeputyRegionalManagerName { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string HealthAndSafetyRegionalManagerName { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string BusinessDevelopmentRegionalManagerName { get; set; }
        [Display(Name = "Project Manager")]
        public Guid ProjectManager { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ProjectManagerName { get; set; }
        [Display(Name = "Project Controls")]
        public Guid ProjectControls { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ProjectControlName { get; set; }
        [Display(Name = "Accounting Representative")]
        public Guid AccountingRepresentative { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AccountingRepresentativeName { get; set; }
        [Required]
        [Display(Name = "Contract Representative")]
        public Guid ContractRepresentative { get; set; }
        [Required]
        [Display(Name = "Contract Representative")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeName { get; set; }

        [Display(Name = "SubContract Administrator")]
        public Guid SubContractAdministrator { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SubContractAdministratorName { get; set; }
        [Display(Name = "Purchasing Representative")]
        public Guid PurchasingRepresentative { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PurchasingRepresentativeName { get; set; }
        [Display(Name = "Human Resource Representative")]
        public Guid HumanResourceRepresentative { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string HumanResourceRepresentativeName { get; set; }
        [Display(Name = "Quality Representative")]
        public Guid QualityRepresentative { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string QualityRepresentativeName { get; set; }
        [Display(Name = "Safety Officer")]
        public Guid SafetyOfficer { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SafetyOfficerName { get; set; }

        public UserViewModel CompanyPresidentModel { get; set; }
        public UserViewModel RegionalManagerModel { get; set; }
        public UserViewModel DeputyRegionalManagerModel { get; set; }
        public UserViewModel BDRegionalManagerModel { get; set; }
        public UserViewModel HSRegionalManagerModel { get; set; }
        public UserViewModel ProjectManagerModel { get; set; }
        public UserViewModel ProjectControlModel { get; set; }
        public UserViewModel AccountRepresentativeModel { get; set; }
        public UserViewModel ContractRepresentativeModel { get; set; }
        public UserViewModel UserDetailModel { get; set; }

        public UserViewModel SubContractAdministratorModel { get; set; }
        public UserViewModel PurchasingRepresentativeModel { get; set; }
        public UserViewModel HumanResourceRepresentativeModel { get; set; }
        public UserViewModel QualityRepresentativeModel { get; set; }
        public UserViewModel SafetyOfficeModel { get; set; }
        public UserViewModel OperationManagerModel { get; set; }
        
    }
    public class CustomerInformationViewModel
    {
        [Required]
        [Display(Name = "Awarding Agency Office")]
        public Guid AwardingAgencyOffice { get; set; }
        [Required]
        [Display(Name = "Awarding Agency Office")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AwardingAgencyOfficeName { get; set; }
        [Display(Name = "Awarding Agency Office Type")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AwardingAgencyCustomerTypeName { get; set; }
        [Display(Name = "Contract Representative")]
        public Guid OfficeContractRepresentative { get; set; }

        public Guid? OfficeContractRepresentativeHiddenGuid
        {
            get { return OfficeContractRepresentative; }
        }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeName { get; set; }
        [Display(Name = "Contract Technical Representative")]
        public Guid OfficeContractTechnicalRepresent { get; set; }

        public Guid? OfficeContractTechnicalRepresentHiddenGuid
        {
            get { return OfficeContractTechnicalRepresent; }
        }

        public string OfficeContractTechnicalRepresentName { get; set; }
        [Display(Name = "Funding Agency Office")]
        public Guid FundingAgencyOffice { get; set; }
        [Display(Name = "Funding Agency Office")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string FundingAgencyOfficeName { get; set; }
        [Display(Name = "Funding Agency Office Type")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string FundingAgencyCustomerTypeName { get; set; }
        [Display(Name = "Contract Representative")]
        public Guid FundingOfficeContractRepresentative { get; set; }
        public Guid? FundingOfficeContractRepresentativeHiddenGuid
        {
            get { return FundingOfficeContractRepresentative; }
        }
        public string FundingOfficeContractRepresentativeName { get; set; }
        [Display(Name = "Contract Technical Representative")]
        public Guid FundingOfficeContractTechnicalRepresent { get; set; }
        public Guid? FundingOfficeContractTechnicalRepresentHiddenGuid
        {
            get { return FundingOfficeContractTechnicalRepresent; }
        }
        public string FundingOfficeContractTechnicalRepresentName { get; set; }

        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativePhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeAltEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeAltPhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativePhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativeAltPhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativeEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativeAltEmailAddress { get; set; }

        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativePhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeAltEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeAltPhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativePhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativeAltPhoneNumber { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativeEmailAddress { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativeAltEmailAddress { get; set; }

        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractRepresentativeAltContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractRepresentativeAltContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativeContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTechnicalRepresentativeAltContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativeContact { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OfficeContractTechnicalRepresentativeAltContact { get; set; }
        public bool IsSameAsAwardingoffice { get; set; }

    }
    public class FinancialInformationViewModel
    {
        [Display(Name = "Set Aside")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SetAside { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SetAsideName { get; set; }
        [Display(Name = "Self Performance %")]
        [ValidDecimal(ErrorMessage = "Only decimal numbers allowed")]
        public decimal? SelfPerformancePercent { get; set; }
        public string FormattedSelfPerformancePercent
        {
            get
            {
                return SelfPerformancePercent == null ? "Not Entered" : string.Format("{0:###,##0.00}", SelfPerformancePercent.Value);
            }
        }
        [Display(Name = "SBA")]
        public bool SBA { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string SBAString { get { return SBA == true ? "Yes" : "No"; } }
        [Display(Name = "Competition")]
        public string Competition { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string CompetitionType { get; set; }
        [Required]
        [Display(Name = "Contract Type")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractType { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string ContractTypeName { get; set; }
        [Display(Name = "OverHead %")]
        public decimal? OverHead { get; set; }
        public string FormattedOverHead
        {
            get
            {
                return OverHead == null ? "Not Entered" : string.Format("{0:###,##0.00}", OverHead.Value);
            }
        }
        [Display(Name = "G&A %")]
        public decimal? GAPercent { get; set; }
        public string FormattedGAPercent
        {
            get
            {
                return GAPercent == null ? "Not Entered" : string.Format("{0:###,##0.00}", GAPercent.Value);
            }
        }

        [Display(Name = "Fee %")]
        public decimal? FeePercent { get; set; }
        public string FormattedFeePercent
        {
            get
            {
                return FeePercent == null ? "Not Entered" : string.Format("{0:###,##0.00}", FeePercent.Value);
            }
        }

        [Display(Name = "Currency")]
        public string Currency { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string CurrencyName { get; set; }
        [Display(Name = "Blue Sky Award Amount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? BlueSkyAwardAmount { get; set; }
        public string FormattedBlueSkyAwardAmount
        {
            get
            {
                return BlueSkyAwardAmount == null ? "Not Entered" : Currency + " " + string.Format("{0:###,##0.00}", BlueSkyAwardAmount.Value);
            }
        }
        [Display(Name = "Award Amount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? AwardAmount { get; set; }
        public string FormattedAwardAmount
        {
            get
            {
                return AwardAmount == null ? "Not Entered" : Currency + " " + string.Format("{0:###,##0.00}", AwardAmount.Value);
            }
        }

        [Display(Name = "Funded Amount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? FundingAmount { get; set; }
        public string FormattedFundingAmount
        {
            get
            {
                return FundingAmount == null ? "Not Entered" : Currency + " " + string.Format("{0:###,##0.00}", FundingAmount.Value);
            }
        }

        [Display(Name = "Billing Address")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string BillingAddress { get; set; }
        [Display(Name = "Billing Frequency")]
        public string BillingFrequency { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string BillingFrequencyName { get; set; }
        [Display(Name = "Invoice Submission Method")]
        public string InvoiceSubmissionMethod { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string InvoiceSubmissionMethodName { get; set; }
        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string PaymentTermsName { get; set; }
        [Display(Name = "Applicable Wage Determination")]
        public string AppWageDetermineDavisBaconAct { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AppWageDetermineDavisBaconActType { get; set; }
        public string AppWageDetermineServiceContractAct { get; set; }
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string AppWageDetermineServiceContractActType { get; set; }
        [Display(Name = "Billing Formula")]
        public string BillingFormula { get; set; }
        [Display(Name = "Revenue Formula")]
        public string RevenueFormula { get; set; }
        [Display(Name = "Billing Formula")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string BillingFormulaName { get; set; }
        [Display(Name = "Revenue Formula")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string RevenueFormulaName { get; set; }

        [Display(Name = "Revenue Recognition EAC %")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true, NullDisplayText = "Not Entered")]
        public decimal? RevenueRecognitionEACPercent { get; set; }
        [Display(Name = "OH onsite")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OHonsite { get; set; }
        [Display(Name = "OH offsite")]
        [DisplayFormat(NullDisplayText = "Not Entered")]
        public string OHoffsite { get; set; }
    }
    public class CustomerAttributeValuesViewModel
    {
        public IDictionary<string, string> ddlSetAside { get; set; }
        public IDictionary<string, string> ddlContractType { get; set; }
        public IDictionary<string, string> ddlCurrency { get; set; }
        public IDictionary<string, string> ddlBillingFrequency { get; set; }
        public IDictionary<string, string> ddlInvoiceSubmissionMethod { get; set; }
        public IDictionary<string, string> ddlQualityLevel { get; set; }
        public IDictionary<string, string> ddlPaymentTerms { get; set; }
        public IDictionary<string, string> radioAppWageDetermine_ServiceContractAct { get; set; }
        public IDictionary<string, string> radioAppWageDetermine_DavisBaconAct { get; set; }
        public IDictionary<string, string> radioCompetition { get; set; }
        public IDictionary<bool, string> radioIsIDIQContract { get; set; }
        public IDictionary<bool, string> radioIsPrimeContract { get; set; }
        public IDictionary<bool, string> radioQualityLevelRequirements { get; set; }
        public IDictionary<bool, string> radioCPAREligible { get; set; }
        public IDictionary<bool, string> radioSBA { get; set; }
        public IDictionary<string, string> ddlBillingFormula { get; set; }
        public IDictionary<string, string> ddlRevenueFormula { get; set; }
        public IDictionary<string, string> ddlStatus { get; set; }
        public IDictionary<Guid, string> ddlFarContractType { get; set; }

        //project..
        public IDictionary<string, string> ddlProjectType { get; set; }
        public IDictionary<string, string> radioAppWageDetermine_ServiceProjectAct { get; set; }
        public IDictionary<bool, string> radioIsPrimeProject { get; set; }
    }
    public class AssociateUserList
    {
        public UserViewModel CompanyPresident { get; set; }
        public UserViewModel RegionManager { get; set; }
        public string CompanyName { get; set; }
        public string RegionName { get; set; }
        public string OfficeName { get; set; }
    }
    public class EntityCodeViewModel
    {
        public string CompanyCode { get; set; }
        public string RegionCode { get; set; }
        public string OfficeCode { get; set; }
    }
    public class AutoCompleteReturnViewModel
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class FilterByList
    {
        public IDictionary<string, string> FilterBy { get; set; }
    }
    public class ContractCloseOutDetail
    {
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
}
