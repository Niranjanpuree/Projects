using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class RevenueRecognitionViewModel : BaseViewModel
    {
        public Guid RevenueRecognizationGuid { get; set; }
        public Guid? ContractGuid { get; set; }
        public string IsModAdministrative { get; set; }
        public bool IsASC606 { get; set; }
        public string IsASC606Status { get { return IsASC606 == true ? "Yes" : "No"; } }
        public bool IsCurrentFiscalYearOfNorthWind { get; set; }
        public string IsCurrentFiscalYearOfNorthWindStatus { get { return IsCurrentFiscalYearOfNorthWind == true ? "Yes" : "No"; } }
        public bool DoesScopeContractChange { get; set; }
        public string DoesScopeContractChangeStatus { get { return DoesScopeContractChange == true ? "Yes" : "No"; } }

        [DisplayName("OverView Notes")]
        public string OverviewNotes { get; set; }

        public string IdentifyTerminationClause { get; set; }
        //public string ExtensionPeriod { get; set; }
        public string WarrantyTerms { get; set; }
        public string EstimateWarrantyExposure { get; set; }
        public string PricingExplanation { get; set; }
        public string Approach { get; set; }
        public string EachMultipleObligation { get; set; }
        public bool IsDiscountPurchase { get; set; }
        public string IsDiscountPurchaseStatus { get { return IsDiscountPurchase == true ? "Yes" : "No"; } }
        public string HasMultipleContractObligations { get; set; }
        [DisplayName("Step 1 Notes")]
        public string Step1Note { get; set; }
        [Required, DisplayName("Identity Contract")]
        public string IdentityContract { get; set; }
        [DisplayName("Termination clause(s)")]
        public string IsTerminationClauseGovernmentStandard { get; set; }
        public bool IsContractTermExpansion { get; set; }
        public string IsContractTermExpansionStatus { get { return IsContractTermExpansion == true ? "Yes" : "No"; } }
        [DisplayName("Step 2 Notes")]
        public string Step2Note { get; set; }
        public string IdentityPerformanceObligation { get; set; }
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
        [Required, DisplayName("Contract Type")]
        public string ContractType { get; set; }
        public bool IsPricingVariation { get; set; }
        public string IsPricingVariationStatus { get { return IsPricingVariation == true ? "Yes" : "No"; } }
        [Required, DisplayName("BaseContract Price")]
        public decimal BaseContractPrice { get; set; }
        [Required, DisplayName("Additional Period Option")]
        public string AdditionalPeriodOption { get; set; }
        public string Step4Note { get; set; }
        public bool HasLicensingOrIntellectualProperty { get; set; }
        public string HasLicensingOrIntellectualPropertyStatus { get { return HasLicensingOrIntellectualProperty == true ? "Yes" : "No"; } }
        public string Step5Note { get; set; }

        public bool IsNotify { get; set; }
        public Guid AccountRepresentive { get; set; }
    }
}
