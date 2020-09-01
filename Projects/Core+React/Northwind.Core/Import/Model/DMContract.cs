using CsvHelper.Configuration;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Import.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Northwind.Core.Import.Model
{
    public class DMContract
    {
        [Trim]
        public string ContractNumber { get; set; }
        public string TaskOrderNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string IsIDIQContract { get; set; }
        public string IsTaskOrder { get; set; }
        public string IsPrimeContract { get; set; }
        public string SubContractNumber { get; set; }
        public string OrganisationID { get; set; }
        public string ContractTitle { get; set; }
        public string Description { get; set; }
        public string AwardAmount { get; set; }
        public string FundingAmount { get; set; }
        public string POPStart { get; set; }
        public string POPEnd { get; set; }
        public string ContractCountry { get; set; }
        public string ContractState { get; set; }
        public string NAICSCodeValue { get; set; }
        public string PSCCodeValue { get; set; }
        public string CPAREligible { get; set; }
        public string QualityLevel { get; set; }
        public string SetAside { get; set; }
        public string Competition { get; set; }
        public string ContractType { get; set; }
        public string Currency { get; set; }
        public string BillingAddress { get; set; }
        public string BillingFrequency { get; set; }
        public string OverHead { get; set; }
        public string GAPercent { get; set; }
        public string FeePercent { get; set; }
        public string InvoiceSubmissionMethod { get; set; }
        public string PaymentTerms { get; set; }
        public string ApplicableWageDetermination { get; set; }
        public string BillingFormula { get; set; }
        public string RevenueFormula { get; set; }
        public string RevenueRecognitionEACPercent { get; set; }
        public string OHonsite { get; set; }
        public string OHoffsite { get; set; }
        public string ContractRepresentatives { get; set; }
        public string ProjectControlRepresentative { get; set; }
        public string ContractProjectManager { get; set; }
        public string AccountingRepresentative { get; set; }
        public string ContractSubContractAdministrator { get; set; }
        public string ContractPurchasingRepresentative { get; set; }
        public string ContractHumanResourceRepresentative { get; set; }
        public string ContractQualityRepresentative { get; set; }
        public string ContractSafetyOfficer { get; set; }
        public string AwardingAgency { get; set; }
        public string AwardAgencyContractRepresentative { get; set; }
        public string AwardingAgencyTechnicalRepresentative { get; set; }
        public string FundingAgency { get; set; }
        public string FundingAgencyContractRepresentative { get; set; }
        public string FundingAgencyTechnicalRepresentative { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string TaskNodeID { get; set; }
        public string MasterTaskNodeID { get; set; }
        public bool IsImported { get; set; } = true;

        public string ImportStatus { get; set; }
        public string Reason { get; set; }
        public bool IsValid { get; set; }
        public bool IsPartialValid { get; set; } = true;

        public string QualityLevelValue { get; set; }
        public string CurrencyValue { get; set; }
        public string BillingFormulaValue { get; set; }
        public string RevenueFormulaValue { get; set; }
        public string PaymentTermValue { get; set; }
        public string CompetitionValue { get; set; }
        public string ContractTypeValue { get; set; }
        public string InvoiceSubmissionMethodValue { get; set; }
        public string ApplicableWageDeterminationValue { get; set; }
        public string BillingFrequencyValue { get; set; }

        public string PlaceOfPerformance { get; set; }
        public Guid ContractGuid { get; set; }
        public Guid CountryOfPerformance { get; set; }
        public Guid NAICSCode { get; set; }
        public Guid PSCCode { get; set; }
        public Guid ORGID { get; set; }
        public Guid? ParentContractGuid { get; set; }
        public bool QualityLevelRequirements { get; set; }

        public Guid ProjectManagerGuid { get; set; }
        public Guid RegionalManagerGuid { get; set; }
        public Guid ProjectControlsGuid { get; set; }
        public Guid ContractRepresentativeGuid { get; set; }
        public Guid CompanyPresidentGuid { get; set; }
        public Guid AccountRepresentativeGuid { get; set; }
        public Guid SubContractAdministratorGuid { get; set; }
        public Guid PurchasingRepresentativeGuid { get; set; }
        public Guid HumanResourceRepresentativeGuid { get; set; }
        public Guid QualityRepresentativeGuid { get; set; }
        public Guid SafetyOfficerGuid { get; set; }

        public Guid FundingAgencyOffice { get; set; }
        public Guid FundingOfficeContractRepresentative { get; set; }
        public Guid FundingOfficeContractTechnicalRepresent { get; set; }
        public Guid AwardingAgencyOffice { get; set; }
        public Guid OfficeContractRepresentative { get; set; }
        public Guid OfficeContractTechnicalRepresent { get; set; }
        public List<ContractUserRole> ContractUserRole { get; set; }
    }

    public sealed class ContractHeaderMap : ClassMap<DMContract>
    {
        public ContractHeaderMap(Dictionary<string, string> headers)
        {
            //foreach (var header in headers)
            //{
            //    Map(m => header.Key).Name(headers[header.Key.ToString()]).Optional();
            //}
            if(headers.ContainsKey("ContractNumber"))
                Map(m => m.ContractNumber).Name(headers["ContractNumber"]).Optional();
            if (headers.ContainsKey("TaskOrderNumber"))
                Map(m => m.TaskOrderNumber).Name(headers["TaskOrderNumber"]).Optional();
            if (headers.ContainsKey("ProjectNumber"))
                Map(m => m.ProjectNumber).Name(headers["ProjectNumber"]).Optional();
            if (headers.ContainsKey("IsIDIQContract"))
                Map(m => m.IsIDIQContract).Name(headers["IsIDIQContract"]).Optional();
            if (headers.ContainsKey("IsTaskOrder"))
                Map(m => m.IsTaskOrder).Name(headers["IsTaskOrder"]).Optional();
            if (headers.ContainsKey("IsPrimeContract"))
                Map(m => m.IsPrimeContract).Name(headers["IsPrimeContract"]).Optional();
            if (headers.ContainsKey("SubContractNumber"))
                Map(m => m.SubContractNumber).Name(headers["SubContractNumber"]).Optional();
            if (headers.ContainsKey("OrganisationID"))
                Map(m => m.OrganisationID).Name(headers["OrganisationID"]).Optional();
            if (headers.ContainsKey("ContractTitle"))
                Map(m => m.ContractTitle).Name(headers["ContractTitle"]).Optional();
            if (headers.ContainsKey("Description"))
                Map(m => m.Description).Name(headers["Description"]).Optional();
            if (headers.ContainsKey("AwardAmount"))
                Map(m => m.AwardAmount).Name(headers["AwardAmount"]).Optional();
            if (headers.ContainsKey("FundingAmount"))
                Map(m => m.FundingAmount).Name(headers["FundingAmount"]).Optional();
            if (headers.ContainsKey("POPStart"))
                Map(m => m.POPStart).Name(headers["POPStart"]).Optional();
            if (headers.ContainsKey("POPEnd"))
                Map(m => m.POPEnd).Name(headers["POPEnd"]).Optional();
            if (headers.ContainsKey("ContractCountry"))
                Map(m => m.ContractCountry).Name(headers["ContractCountry"]).Optional();
            if (headers.ContainsKey("ContractState"))
                Map(m => m.ContractState).Name(headers["ContractState"]).Optional();
            if (headers.ContainsKey("NAICSCodeValue"))
                Map(m => m.NAICSCodeValue).Name(headers["NAICSCodeValue"]).Optional();
            if (headers.ContainsKey("PSCCodeValue"))
                Map(m => m.PSCCodeValue).Name(headers["PSCCodeValue"]).Optional();
            if (headers.ContainsKey("CPAREligible"))
                Map(m => m.CPAREligible).Name(headers["CPAREligible"]).Optional();
            if (headers.ContainsKey("QualityLevel"))
                Map(m => m.QualityLevel).Name(headers["QualityLevel"]).Optional();
            if (headers.ContainsKey("SetAside"))
                Map(m => m.SetAside).Name(headers["SetAside"]).Optional();
            if (headers.ContainsKey("Competition"))
                Map(m => m.Competition).Name(headers["Competition"]).Optional();
            if (headers.ContainsKey("ContractType"))
                Map(m => m.ContractType).Name(headers["ContractType"]).Optional();
            if (headers.ContainsKey("Currency"))
                Map(m => m.Currency).Name(headers["Currency"]).Optional();
            if (headers.ContainsKey("BillingAddress"))
                Map(m => m.BillingAddress).Name(headers["BillingAddress"]).Optional();
            if (headers.ContainsKey("BillingFrequency"))
                Map(m => m.BillingFrequency).Name(headers["BillingFrequency"]).Optional();
            if (headers.ContainsKey("OverHead"))
                Map(m => m.OverHead).Name(headers["OverHead"]).Optional();
            if (headers.ContainsKey("GAPercent"))
                Map(m => m.GAPercent).Name(headers["GAPercent"]).Optional();
            if (headers.ContainsKey("FeePercent"))
                Map(m => m.FeePercent).Name(headers["FeePercent"]).Optional();
            if (headers.ContainsKey("InvoiceSubmissionMethod"))
                Map(m => m.InvoiceSubmissionMethod).Name(headers["InvoiceSubmissionMethod"]).Optional();
            if (headers.ContainsKey("PaymentTerms"))
                Map(m => m.PaymentTerms).Name(headers["PaymentTerms"]).Optional();
            if (headers.ContainsKey("ApplicableWageDetermination"))
                Map(m => m.ApplicableWageDetermination).Name(headers["ApplicableWageDetermination"]).Optional();
            if (headers.ContainsKey("BillingFormula"))
                Map(m => m.BillingFormula).Name(headers["BillingFormula"]).Optional();
            if (headers.ContainsKey("RevenueFormula"))
                Map(m => m.RevenueFormula).Name(headers["RevenueFormula"]).Optional();
            if (headers.ContainsKey("RevenueRecognitionEACPercent"))
                Map(m => m.RevenueRecognitionEACPercent).Name(headers["RevenueRecognitionEACPercent"]).Optional();
            if (headers.ContainsKey("OHonsite"))
                Map(m => m.OHonsite).Name(headers["OHonsite"]).Optional();
            if (headers.ContainsKey("OHoffsite"))
                Map(m => m.OHoffsite).Name(headers["OHoffsite"]).Optional();
            if (headers.ContainsKey("ContractRepresentatives"))
                Map(m => m.ContractRepresentatives).Name(headers["ContractRepresentatives"]).Optional();
            if (headers.ContainsKey("ProjectControlRepresentative"))
                Map(m => m.ProjectControlRepresentative).Name(headers["ProjectControlRepresentative"]).Optional();
            if (headers.ContainsKey("ContractProjectManager"))
                Map(m => m.ContractProjectManager).Name(headers["ContractProjectManager"]).Optional();
            if (headers.ContainsKey("AccountingRepresentative"))
                Map(m => m.AccountingRepresentative).Name(headers["AccountingRepresentative"]).Optional();
            if (headers.ContainsKey("ContractSubContractAdministrator"))
                Map(m => m.ContractSubContractAdministrator).Name(headers["ContractSubContractAdministrator"]).Optional();
            if (headers.ContainsKey("ContractPurchasingRepresentative"))
                Map(m => m.ContractPurchasingRepresentative).Name(headers["ContractPurchasingRepresentative"]).Optional();
            if (headers.ContainsKey("ContractHumanResourceRepresentative"))
                Map(m => m.ContractHumanResourceRepresentative).Name(headers["ContractHumanResourceRepresentative"]).Optional();
            if (headers.ContainsKey("ContractQualityRepresentative"))
                Map(m => m.ContractQualityRepresentative).Name(headers["ContractQualityRepresentative"]).Optional();
            if (headers.ContainsKey("ContractSafetyOfficer"))
                Map(m => m.ContractSafetyOfficer).Name(headers["ContractSafetyOfficer"]).Optional();
            if (headers.ContainsKey("AwardingAgency"))
                Map(m => m.AwardingAgency).Name(headers["AwardingAgency"]).Optional();
            if (headers.ContainsKey("AwardAgencyContractRepresentative"))
                Map(m => m.AwardAgencyContractRepresentative).Name(headers["AwardAgencyContractRepresentative"]).Optional();
            if (headers.ContainsKey("AwardingAgencyTechnicalRepresentative"))
                Map(m => m.AwardingAgencyTechnicalRepresentative).Name(headers["AwardingAgencyTechnicalRepresentative"]).Optional();
            if (headers.ContainsKey("FundingAgency"))
                Map(m => m.FundingAgency).Name(headers["FundingAgency"]).Optional();
            if (headers.ContainsKey("FundingAgencyContractRepresentative"))
                Map(m => m.FundingAgencyContractRepresentative).Name(headers["FundingAgencyContractRepresentative"]).Optional();
            if (headers.ContainsKey("FundingAgencyTechnicalRepresentative"))
                Map(m => m.FundingAgencyTechnicalRepresentative).Name(headers["FundingAgencyTechnicalRepresentative"]).Optional();
            if (headers.ContainsKey("Status"))
                Map(m => m.Status).Name(headers["Status"]).Optional();
            if (headers.ContainsKey("TaskNodeID"))
                Map(m => m.TaskNodeID).Name(headers["TaskNodeID"]).Optional();
            if (headers.ContainsKey("MasterTaskNodeID"))
                Map(m => m.MasterTaskNodeID).Name(headers["MasterTaskNodeID"]).Optional();
            if (headers.ContainsKey("Action"))
                Map(m => m.Action).Name(headers["Action"]).Optional();
        }
    }
}
