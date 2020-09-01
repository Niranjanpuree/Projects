using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Entities.ContractRefactor
{
    public class Contracts
    {
        public static string _companyPresident = "company-president";
        public static string _projectManager = "project-manager";
        public static string _regionalManager = "regional-manager";
        public static string _deputyregionalManager = "deputy-regional-manager";
        public static string _hsregionalManager = "HS-regional-manager";
        public static string _bdregionalManager = "BD-regional-manager";
        public static string _accountRepresentative = "account-representative";
        public static string _contractRepresentative = "contract-representative";
        public static string _projectControls = "project-controls";
        public static string _subContractAdministrator = "subcontract-administrator";
        public static string _purchasingRepresentative = "purchasing-representative";
        public static string _humanResourceRepresentative = "human-resource-representative";
        public static string _qualityRepresentative = "quality-representative";
        public static string _safetyOfficer = "safety-officer";
        public static string _operationManager = "operation-manager";


        public Guid ContractGuid { get; set; }
        public bool IsIDIQContract { get; set; }
        public bool IsPrimeContract { get; set; }
        public string ContractNumber { get; set; }
        public string ParentContractNumber { get; set; }
        public string SubContractNumber { get; set; }
        public Guid ORGID { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractTitle { get; set; }
        
        public Guid? OfficeContractTechnicalRepresent { get; set; }
        public bool IsFavorite { get; set; }

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
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public decimal? RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }

        public string Description { get; set; }
        public string ApplicableWageDetermination { get; set; }
        public Guid? FundingOfficeContractRepresentative { get; set; }
        public Guid? FundingOfficeContractTechnicalRepresent { get; set; }
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
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
        public string CompanyName { get; set; }
        public string OfficeName { get; set; }

        public decimal CumulativeAwardAmount { get; set; }
        public decimal CumulativeFundingAmount { get; set; }

        public string FundingAgencyName { get; set; }
        public string FundingAgencyContractRepresentativeName { get; set; }
        public string FundingAgencyContractTechnicalRepresentativeName { get; set; }
        public string AwardingAgencyContractTechnicalRepresentativeName { get; set; }
        public string AwardingAgencyContractRepresentativeName { get; set; }
        public string AwardingAgencyName { get; set; }
        public string Status { get; set; }
        public bool IsImported { get; set; }
        public int TaskNodeID { get; set; }
        public int MasterTaskNodeID { get; set; }

        public Country Country { get; set; }
        public Naics NAICS { get; set; }
        public Psc PSC { get; set; }

        private JobRequest jobRequest;

        public JobRequest JobRequest
        {
            get { return jobRequest; }
            set { jobRequest = value; }
        }

        private ContractQuestionaire contractQuestionaire;

        public ContractQuestionaire ContractQuestionaire
        {
            get { return contractQuestionaire; }
            set { contractQuestionaire = value; }
        }

        private RevenueRecognition revenueRecognization;

        public RevenueRecognition RevenueRecognization
        {
            get { return revenueRecognization; }
            set { revenueRecognization = value; }
        }
        public Guid RevenueRecognitionGuid { get; set; }
        public BasicContractInfoModel BasicContractInfo { get; set; }
        public CustomerInformationModel CustomerInformation { get; set; }
        public FinancialInformationModel FinancialInformation { get; set; }
        public CustomerAttributeValuesModel CustomerAttributeValuesModel { get; set; }
        public ContractQuestionaire DbContractQuestionaire { get; set; }

        private List<ResourceAttributeValue> _ResourceAttributeValue = new List<ResourceAttributeValue>();


        public List<ResourceAttributeValue> ResourceAttributeValue
        {
            get { return _ResourceAttributeValue; }
            set { _ResourceAttributeValue = value; }
        }

        public List<ContractUserRole> ContractUserRole { get; set; }

        private List<ContractResourceFile> _contractResourceFile;

        public List<ContractResourceFile> ContractResourceFile
        {
            get { return _contractResourceFile; }
            set { _contractResourceFile = value; }
        }

        private List<ContractKeyPersonnel> _KeyPersonnel;

        public List<ContractKeyPersonnel> KeyPersonnel
        {
            get { return _KeyPersonnel; }
            set { _KeyPersonnel = value; }
        }

        public IDocumentEntity ContractWBS
        {
            get
            {
                if (ContractResourceFile != null)
                {
                    return ContractResourceFile.Where(m => m.Keys.Equals(EnumGlobal.ResourceType.WorkBreakDownStructure.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                return null;
            }
        }

        public IDocumentEntity EmployeeBillingRates
        {
            get
            {
                if (ContractResourceFile != null)
                {
                    return ContractResourceFile.Where(m => m.Keys.Equals(EnumGlobal.ResourceType.EmployeeBillingRates.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                return null;
            }
        }

        public IDocumentEntity LaborCategoryRates
        {
            get
            {
                if (ContractResourceFile != null)
                {
                    return ContractResourceFile.Where(m => m.Keys.Equals(EnumGlobal.ResourceType.LaborCategoryRates.ToString(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                }
                return null;
            }
        }

        private Organization _organisation;

        public Organization Organisation
        {
            get { return _organisation; }
            set { _organisation = value; }
        }

        public User RegionalManager { get; set; }
        public User DeputyRegionalManager { get; set; }
        public User BusinessDevelopmentRegionalManager { get; set; }
        public User HealthAndSafetyRegionalManager { get; set; }
        public User ProjectManager { get; set; }
        public User AccountRepresentative { get; set; }
        public User CompanyPresident { get; set; }
        public User ProjectControls { get; set; }
        public User ContractRepresentative { get; set; }
        public User SubContractAdministrator { get; set; }
        public User PurchasingRepresentative { get; set; }
        public User HumanResourceRepresentative { get; set; }
        public User QualityRepresentative { get; set; }
        public User SafetyOfficer { get; set; }
        public User OperationManager { get; set; }

        public void AddKeyPersonnel(ContractKeyPersonnel person)

        {
            KeyPersonnel.Add(person);

        }

        public void AddKeyPersonnel(string role, IUser user)
        {
            ContractKeyPersonnel c = new ContractKeyPersonnel();

        }
        public bool IsUpdated { get; set; }

        public class ContractBasicDetails
        {
            public Guid ContractGuid { get; set; }
            public bool IsIDIQContract { get; set; }
            public bool IsPrimeContract { get; set; }
            public string ContractNumber { get; set; }
            public string ParentContractNumber { get; set; }
            public Guid ParentContractGuid { get; set; }
            public string ProjectNumber { get; set; }
            public string ContractTitle { get; set; }
            public string ContractType { get; set; }
            public string Description { get; set; }
        }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string County { get; set; }
        public string PostalCode { get; set; }

        public Guid FarContractTypeGuid { get; set; }
    }
}
