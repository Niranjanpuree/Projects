using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Models.ViewModels.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Northwind.Core.Entities.EnumGlobal;

namespace Northwind.Web.Models.ViewModels.RevenueRecognition
{
    public class RevenueRecognitionViewModel : BaseViewModel
    {
        public Guid RevenueRecognizationGuid { get; set; }
        public Guid? ContractGuid { get; set; }
        public Guid? ContractResourceFileGuid { get; set; }

        public string IsModAdministrative { get; set; }
        public string IsModAdministrativeStatus { get { return string.IsNullOrEmpty(IsModAdministrative) ? "N/A" : IsModAdministrative; } }

        public bool IsIDIQContract { get; set; }
        public bool IsASC606 { get; set; }

        public string IsASC606Status { get { return IsASC606 == true ? "Yes" : "No"; } }
        public bool IsCurrentFiscalYearOfNorthWind { get; set; }
        public string IsCurrentFiscalYearOfNorthWindStatus { get { return IsCurrentFiscalYearOfNorthWind == true ? "Yes" : "No"; } }
        public bool DoesScopeContractChange { get; set; }
        public string DoesScopeContractChangeStatus { get { return DoesScopeContractChange == true ? "Yes" : "No"; } }

        [DisplayName("OverView Notes")]
        public string OverviewNotes { get; set; }
        public string OverviewNotesStatus { get { return string.IsNullOrEmpty(OverviewNotes) ? "N/A" : OverviewNotes; } }

        public string IdentifyTerminationClause { get; set; }
        public string IdentifyTerminationClauseStatus { get { return string.IsNullOrEmpty(IdentifyTerminationClause) ? "N/A" : IdentifyTerminationClause; } }

        public string WarrantyTerms { get; set; }
        public string WarrantyTermsStatus { get { return string.IsNullOrEmpty(WarrantyTerms) ? "N/A" : WarrantyTerms; } }

        public string EstimateWarrantyExposure { get; set; }
        public string EstimateWarrantyExposureStatus { get { return string.IsNullOrEmpty(EstimateWarrantyExposure) ? "N/A" : EstimateWarrantyExposure; } }

        public string PricingExplanation { get; set; }
        public string PricingExplanationStatus { get { return string.IsNullOrEmpty(PricingExplanation) ? "N/A" : PricingExplanation; } }

        public string Approach { get; set; }
        public string ApproachStatus { get { return string.IsNullOrEmpty(Approach) ? "N/A" : Approach; } }

        public string EachMultipleObligation { get; set; }
        public string EachMultipleObligationStatus { get { return string.IsNullOrEmpty(EachMultipleObligation) ? "N/A" : EachMultipleObligation; } }

        public bool IsDiscountPurchase { get; set; }
        public string IsDiscountPurchaseStatus { get { return IsDiscountPurchase == true ? "Yes" : "No"; } }

        public string HasMultipleContractObligations { get; set; }
        public string HasMultipleContractObligationsStatus { get { return string.IsNullOrEmpty(HasMultipleContractObligations) ? "N/A" : HasMultipleContractObligations; } }

        [DisplayName("Step 1 Notes")]
        public string Step1Note { get; set; }
        public string Step1NoteStatus { get { return string.IsNullOrEmpty(Step1Note) ? "N/A" : Step1Note; } }

        [Required, DisplayName("Identity Contract")]
        public string IdentityContract { get; set; }
        public string IdentityContractStatus { get { return string.IsNullOrEmpty(IdentityContract) ? "N/A" : IdentityContract; } }

        [DisplayName("Termination clause(s)")]
        public string IsTerminationClauseGovernmentStandard { get; set; }
        public string IsTerminationClauseGovernmentStandardStatus { get { return string.IsNullOrEmpty(IsTerminationClauseGovernmentStandard) ? "N/A" : IsTerminationClauseGovernmentStandard; } }

        public bool IsContractTermExpansion { get; set; }
        public string IsContractTermExpansionStatus { get { return IsContractTermExpansion == true ? "Yes" : "No"; } }

        [DisplayName("Step 2 Notes")]
        public string Step2Note { get; set; }
        public string Step2NoteStatus { get { return string.IsNullOrEmpty(Step2Note) ? "N/A" : Step2Note; } }

        public string IdentityPerformanceObligation { get; set; }
        public string IdentityPerformanceObligationStatus { get { return string.IsNullOrEmpty(IdentityPerformanceObligation) ? "N/A" : IdentityPerformanceObligation; } }

        [DisplayName("Multi Revenue Stream")]
        public bool IsMultiRevenueStream { get; set; }
        public string IsMultiRevenueStreamStatus { get { return IsMultiRevenueStream == true ? "Yes" : "No"; } }
        public bool IsRepetativeService { get; set; }
        public string IsRepetativeServiceStatus { get { return IsRepetativeService == true ? "Yes" : "No"; } }
        public bool HasOptionToPurchageAdditionalGoods { get; set; }
        public string HasOptionToPurchageAdditionalGoodsStatus { get { return HasOptionToPurchageAdditionalGoods == true ? "Yes" : "No"; } }
        public bool IsNonRefundableAdvancePayment { get; set; }
        public string IsNonRefundableAdvancePaymentStatus { get { return IsNonRefundableAdvancePayment == true ? "Yes" : "No"; } }
        public bool HasDiscountProvision { get; set; }
        public string HasDiscountProvisionStatus { get { return HasDiscountProvision == true ? "Yes" : "No"; } }
        public bool HasWarrenty { get; set; }
        public string HasWarrentyStatus { get { return HasWarrenty == true ? "Yes" : "No"; } }

        public string Step3Note { get; set; }
        public string Step3NoteStatus { get { return string.IsNullOrEmpty(Step3Note) ? "N/A" : Step3Note; } }

        [Required, DisplayName("Contract Type")]
        public string ContractType { get; set; }
        public string ContractTypeStatus { get { return string.IsNullOrEmpty(ContractType) ? "N/A" : ContractType; } }

        public bool IsPricingVariation { get; set; }
        public string IsPricingVariationStatus { get { return IsPricingVariation == true ? "Yes" : "No"; } }
        [Required, DisplayName("BaseContract Price")]
        public decimal BaseContractPrice { get; set; }

        [Required, DisplayName("Additional Period Option")]
        public string AdditionalPeriodOption { get; set; }
        public string AdditionalPeriodOptionStatus { get { return string.IsNullOrEmpty(AdditionalPeriodOption) ? "N/A" : AdditionalPeriodOption; } }

        public string Step4Note { get; set; }
        public string Step4NoteStatus { get { return string.IsNullOrEmpty(Step4Note) ? "N/A" : Step4Note; } }

        public bool HasLicensingOrIntellectualProperty { get; set; }
        public string HasLicensingOrIntellectualPropertyStatus { get { return HasLicensingOrIntellectualProperty == true ? "Yes" : "No"; } }

        public string Step5Note { get; set; }
        public string Step5NoteStatus { get { return string.IsNullOrEmpty(Step5Note) ? "N/A" : Step5Note; } }

        public bool IsNotify { get; set; }
        public Guid AccountRepresentive { get; set; }

        public CrudType CrudType { get; set; }
        public BasicContractInfoModel BasicContractInfoModel { get; set; }
        public bool isTaskOrder { get; set; }
        public IDictionary<string, string> IdentifyContract { get; set; }
        public IDictionary<string, string> PriceArrangementtype { get; set; }
        public List<RevenueContractExtensionViewModel> ListContractExtension { get; set; }
        public List<RevenuePerformanceObligationViewModel> ListRevenuePerformanceObligation { get; set; }
        public List<ContractFileViewModel> ContractFileList { get; set; }

        public string Displayname { get; set; }
        public bool IsCsvWbs
        {
            get
            {
                if (ContractFileList == null)
                    return false;
                var contractFile = ContractFileList.Where(x => x.keys == Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString() && x.IsCsv == true).FirstOrDefault();
                if (contractFile == null)
                    return false;
                return true;
            }
        }
        public bool IsCsvLaborCategoryRate
        {
            get
            {
                if (ContractFileList == null)
                    return false;
                var contractFile = ContractFileList.Where(x => x.keys == Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString() && x.IsCsv == true).FirstOrDefault();
                if (contractFile == null)
                    return false;
                return true;
            }
        }
        public string LaborFilePath
        {
            get
            {
                if (ContractFileList == null)
                    return null;
                var contractFile = ContractFileList.Where(x => x.keys == Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString() && x.Isfile == true).FirstOrDefault();
                if (contractFile == null)
                    return null;
                var fileName = $"{contractFile.FilePath}/{contractFile.UploadFileName}";
                return fileName;
            }
        }
        public string WbsFilePath
        {
            get
            {
                if (ContractFileList == null)
                    return null;
                var contractFile = ContractFileList.Where(x => x.keys == Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString() && x.Isfile == true).FirstOrDefault();
                if (contractFile == null)
                    return null;
                var fileName = $"{contractFile.FilePath}/{contractFile.UploadFileName}";
                return fileName;
            }
        }

        public bool IsAccountRepresentive { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsRevenueCreated { get; set; }
        public Guid ResourceGuid { get; set; }
        public string CurrentStage { get; set; }


        public bool IsContractNumber { get; set; }
        public string Title { get; set; }
        public string ResourceNumber { get; set; }
        public string Number
        {
            get
            {
                return (ResourceNumber);
            }
        }
        public string UpdatedByName { get; set; }
        public string ProjectManagerName { get; set; }
        public string AccountingRepresentativeName { get; set; }

        public string Status { get; set; }
        public string UpdatedDate { get { return UpdatedOn != null ? UpdatedOn.ToString("MM/dd/yyyy") : ""; } }

        public bool isViewHistory { get; set; }

        public bool IsFile { get; set; }
        public string Filepath { get; set; }
        public string FileName { get; set; }
    }
}
