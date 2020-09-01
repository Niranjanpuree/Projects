using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Entities;


namespace Northwind.Web.Helpers
{
    public static class MapContractModel
    {
        public static Contract Map(Northwind.Web.Models.ViewModels.Contract.ContractViewModel contractViewModel)
        {
            Contract ContractModel = new Contract();

            ContractModel.ParentContractGuid = contractViewModel.ParentContractGuid;
            ContractModel.ContractGuid = contractViewModel.ContractGuid;
            ContractModel.IsIDIQContract = contractViewModel.BasicContractInfo.IsIDIQContract;
            ContractModel.IsPrimeContract = contractViewModel.BasicContractInfo.IsPrimeContract;
            ContractModel.ContractNumber = contractViewModel.BasicContractInfo.ContractNumber;
            ContractModel.SubContractNumber = contractViewModel.BasicContractInfo.SubContractNumber;
            ContractModel.ORGID = contractViewModel.BasicContractInfo.ORGID;
            ContractModel.ProjectNumber = contractViewModel.BasicContractInfo.ProjectNumber;
            ContractModel.ContractTitle = contractViewModel.BasicContractInfo.ContractTitle;
            ContractModel.Description = contractViewModel.BasicContractInfo.Description;
            ContractModel.CountryOfPerformance = contractViewModel.BasicContractInfo.CountryOfPerformance.ToString();
            ContractModel.PlaceOfPerformanceSelectedIds = contractViewModel.BasicContractInfo.PlaceOfPerformanceSelectedIds;
            ContractModel.POPStart = contractViewModel.BasicContractInfo.POPStart;
            ContractModel.POPEnd = contractViewModel.BasicContractInfo.POPEnd;
            ContractModel.NAICSCode = contractViewModel.BasicContractInfo.NAICSCode;
            ContractModel.PSCCode = contractViewModel.BasicContractInfo.PSCCode;
            ContractModel.CPAREligible = contractViewModel.BasicContractInfo.CPAREligible;
            ContractModel.QualityLevelRequirements = contractViewModel.BasicContractInfo.QualityLevelRequirements;
            ContractModel.QualityLevel = contractViewModel.BasicContractInfo.QualityLevel;

            ContractModel.CompanyPresident = contractViewModel.KeyPersonnel.CompanyPresident;
            ContractModel.RegionalManager = contractViewModel.KeyPersonnel.RegionalManager;
            ContractModel.ProjectManager = contractViewModel.KeyPersonnel.ProjectManager;
            ContractModel.ProjectControls = contractViewModel.KeyPersonnel.ProjectControls;
            ContractModel.AccountingRepresentative = contractViewModel.KeyPersonnel.AccountingRepresentative;
            ContractModel.ContractRepresentative = contractViewModel.KeyPersonnel.ContractRepresentative;

            ContractModel.AwardingAgencyOffice = contractViewModel.CustomerInformation.AwardingAgencyOffice;
            ContractModel.OfficeContractRepresentative = contractViewModel.CustomerInformation.OfficeContractRepresentative;
            ContractModel.OfficeContractTechnicalRepresent =
                contractViewModel.CustomerInformation.OfficeContractTechnicalRepresent;
            ContractModel.FundingAgencyOffice = contractViewModel.CustomerInformation.FundingAgencyOffice;
            ContractModel.FundingOfficeContractRepresentative =
                contractViewModel.CustomerInformation.FundingOfficeContractRepresentative;
            ContractModel.FundingOfficeContractTechnicalRepresent =
                contractViewModel.CustomerInformation.FundingOfficeContractTechnicalRepresent;

            ContractModel.SetAside = contractViewModel.FinancialInformation.SetAside;
            ContractModel.SelfPerformancePercent = contractViewModel.FinancialInformation.SelfPerformancePercent;
            ContractModel.SBA = contractViewModel.FinancialInformation.SBA;
            ContractModel.Competition = contractViewModel.FinancialInformation.Competition;
            ContractModel.ContractType = contractViewModel.FinancialInformation.ContractType;
            ContractModel.OverHead = contractViewModel.FinancialInformation.OverHead;
            ContractModel.GAPercent = contractViewModel.FinancialInformation.GAPercent;
            ContractModel.FeePercent = contractViewModel.FinancialInformation.FeePercent;
            ContractModel.Currency = contractViewModel.FinancialInformation.Currency;
            ContractModel.BlueSkyAwardAmount = contractViewModel.FinancialInformation.BlueSkyAwardAmount;
            ContractModel.AwardAmount = contractViewModel.FinancialInformation.AwardAmount;
            ContractModel.FundingAmount = contractViewModel.FinancialInformation.FundingAmount;
            ContractModel.BillingAddress = contractViewModel.FinancialInformation.BillingAddress;
            ContractModel.BillingFrequency = contractViewModel.FinancialInformation.BillingFrequency;
            ContractModel.InvoiceSubmissionMethod = contractViewModel.FinancialInformation.InvoiceSubmissionMethod;
            ContractModel.PaymentTerms = contractViewModel.FinancialInformation.PaymentTerms;
            ContractModel.AppWageDetermineDavisBaconAct =
                contractViewModel.FinancialInformation.AppWageDetermineDavisBaconAct;
            ContractModel.AppWageDetermineServiceContractAct =
                contractViewModel.FinancialInformation.AppWageDetermineServiceContractAct;
            ContractModel.BillingFormula = contractViewModel.FinancialInformation.BillingFormula;
            ContractModel.RevenueFormula = contractViewModel.FinancialInformation.RevenueFormula;
            ContractModel.RevenueRecognitionEACPercent = contractViewModel.FinancialInformation.RevenueRecognitionEACPercent;
            ContractModel.OHonsite = contractViewModel.FinancialInformation.OHonsite;
            ContractModel.OHoffsite = contractViewModel.FinancialInformation.OHoffsite;
            ContractModel.ProjectCounter = contractViewModel.ProjectCounter;
            return ContractModel;
        }
    }
}
