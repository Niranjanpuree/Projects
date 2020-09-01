using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Web.Areas.IAM.Models.IAM.ViewModels;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Models.ViewModels;
using Northwind.Web.Models.ViewModels.Contract;

namespace Northwind.Web.Helpers
{
    public static class ContractsMapper
    {
        public static Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel MapUserToUserViewModel(User user)
        {
            var userViewModel = new Northwind.Web.Infrastructure.Models.ViewModels.UserViewModel();
            userViewModel.Firstname = user.Firstname;
            userViewModel.Lastname = user.Lastname;
            userViewModel.JobTitle = user.JobTitle ?? "N/A";
            userViewModel.WorkPhone = user.WorkPhone;
            userViewModel.WorkEmail = user.WorkEmail ?? "N/A";
            userViewModel.Company = user.Company;
            userViewModel.Department = user.Department;
            userViewModel.DisplayName = user.DisplayName;
            return userViewModel;
        }

        public static Contracts MapModelToEntity(ContractViewModel contractViewModel)
        {
            Contracts contract = new Contracts();

            contract.ContractGuid = contractViewModel.ContractGuid;
            contract.ParentContractGuid = contractViewModel.ParentContractGuid;
            contract.ContractTitle = contractViewModel.BasicContractInfo.ContractTitle;
            contract.ContractNumber = contractViewModel.BasicContractInfo.ContractNumber;
            contract.POPStart = contractViewModel.BasicContractInfo.POPStart;
            contract.ORGID = contractViewModel.BasicContractInfo.ORGID;
            contract.POPEnd = contractViewModel.BasicContractInfo.POPEnd;
            contract.QualityLevel = contractViewModel.BasicContractInfo.QualityLevel;
            contract.QualityLevelRequirements = contractViewModel.BasicContractInfo.QualityLevelRequirements;
            contract.IsIDIQContract = contractViewModel.BasicContractInfo.IsIDIQContract;
            contract.Description = contractViewModel.BasicContractInfo.Description;
            contract.ProjectNumber = contractViewModel.BasicContractInfo.ProjectNumber;
            contract.IsPrimeContract = contractViewModel.BasicContractInfo.IsPrimeContract;
            contract.SubContractNumber = contractViewModel.BasicContractInfo.SubContractNumber;
            contract.NAICSCode = contractViewModel.BasicContractInfo.NAICSCode;
            contract.CountryOfPerformance = contractViewModel.BasicContractInfo.CountryOfPerformance;
            contract.PlaceOfPerformanceSelectedIds = contractViewModel.BasicContractInfo.PlaceOfPerformanceSelectedIds;
            contract.CPAREligible = contractViewModel.BasicContractInfo.CPAREligible;
            contract.PSCCode = contractViewModel.BasicContractInfo.PSCCode;
            contract.OfficeName = contractViewModel.BasicContractInfo.OfficeName;
            contract.RegionName = contractViewModel.BasicContractInfo.RegionName;
            contract.CompanyName = contractViewModel.BasicContractInfo.CompanyName;
            contract.Status = contractViewModel.Status;

            contract.AddressLine1 = contractViewModel.BasicContractInfo.AddressLine1;
            contract.AddressLine2 = contractViewModel.BasicContractInfo.AddressLine2;
            contract.AddressLine3 = contractViewModel.BasicContractInfo.AddressLine3;
            contract.City = contractViewModel.BasicContractInfo.City;
            contract.Province = contractViewModel.BasicContractInfo.Province;
            contract.County = contractViewModel.BasicContractInfo.County;
            contract.PostalCode = contractViewModel.BasicContractInfo.PostalCode;
            contract.FarContractTypeGuid = contractViewModel.BasicContractInfo.FarContractTypeGuid;

            contract.Competition = contractViewModel.FinancialInformation.Competition;
            contract.SelfPerformancePercent = contractViewModel.FinancialInformation.SelfPerformancePercent;
            contract.FundingAmount = contractViewModel.FinancialInformation.FundingAmount ?? 0;
            contract.FeePercent = contractViewModel.FinancialInformation.FeePercent ?? 0;
            contract.BillingFrequency = contractViewModel.FinancialInformation.BillingFrequency;
            contract.BillingFormula = contractViewModel.FinancialInformation.BillingFormula;
            contract.RevenueFormula = contractViewModel.FinancialInformation.RevenueFormula;
            contract.RevenueRecognitionEACPercent = contractViewModel.FinancialInformation.RevenueRecognitionEACPercent;
            contract.PaymentTerms = contractViewModel.FinancialInformation.PaymentTerms;
            contract.SetAside = contractViewModel.FinancialInformation.SetAside;
            contract.Currency = contractViewModel.FinancialInformation.Currency;
            contract.ApplicableWageDetermination = FormatHelper.ConcatTwoString(contractViewModel.FinancialInformation.AppWageDetermineDavisBaconAct, contractViewModel.FinancialInformation.AppWageDetermineServiceContractAct);
            contract.InvoiceSubmissionMethod = contractViewModel.FinancialInformation.InvoiceSubmissionMethod;
            contract.BlueSkyAwardAmount = contractViewModel.FinancialInformation.BlueSkyAwardAmount ?? 0;
            contract.SBA = contractViewModel.FinancialInformation.SBA;
            contract.GAPercent = contractViewModel.FinancialInformation.GAPercent ?? 0;
            contract.ContractType = contractViewModel.FinancialInformation.ContractType;
            contract.AwardAmount = contractViewModel.FinancialInformation.AwardAmount ?? 0;
            contract.BillingAddress = contractViewModel.FinancialInformation.BillingAddress;
            contract.OHonsite = contractViewModel.FinancialInformation.OHonsite;
            contract.OHoffsite = contractViewModel.FinancialInformation.OHoffsite;
            contract.OverHead = contractViewModel.FinancialInformation.OverHead ?? 0;
            contract.ProjectCounter = contractViewModel.ProjectCounter ?? 0;
            contract.IsActive = contractViewModel.IsActive;

            contract.AwardingAgencyOffice = contractViewModel.CustomerInformation.AwardingAgencyOffice;
            contract.OfficeContractRepresentative = contractViewModel.CustomerInformation.OfficeContractRepresentative;
            contract.OfficeContractTechnicalRepresent = contractViewModel.CustomerInformation.OfficeContractTechnicalRepresent;

            var contractUserRoleList = new List<ContractUserRole>();

            if (contractViewModel.CustomerInformation.IsSameAsAwardingoffice)
            {
                contract.FundingAgencyOffice = contractViewModel.CustomerInformation.AwardingAgencyOffice;
                contract.FundingOfficeContractRepresentative = contractViewModel.CustomerInformation.OfficeContractRepresentative;
                contract.FundingOfficeContractTechnicalRepresent = contractViewModel.CustomerInformation.OfficeContractTechnicalRepresent;
            }
            else
            {
                contract.FundingAgencyOffice = contractViewModel.CustomerInformation.FundingAgencyOffice;
                contract.FundingOfficeContractRepresentative = contractViewModel.CustomerInformation.FundingOfficeContractRepresentative;
                contract.FundingOfficeContractTechnicalRepresent = contractViewModel.CustomerInformation.FundingOfficeContractTechnicalRepresent;
            }
            if (contractViewModel.KeyPersonnel.CompanyPresident != null && contractViewModel.KeyPersonnel.CompanyPresident == Guid.Empty)
            {
                var president = new ContractUserRole();
                president.UserGuid = contractViewModel.KeyPersonnel.CompanyPresident;
                president.UserRole = Contracts._companyPresident;
                contractUserRoleList.Add(president);
            }

            if (contractViewModel.KeyPersonnel.AccountingRepresentative != null)
            {
                var accountRepresentative = new ContractUserRole();
                accountRepresentative.UserGuid = contractViewModel.KeyPersonnel.AccountingRepresentative;
                accountRepresentative.UserRole = Contracts._accountRepresentative;
                contractUserRoleList.Add(accountRepresentative);
            }

            if (contractViewModel.KeyPersonnel.ProjectManager != null)
            {
                var projectManager = new ContractUserRole();
                projectManager.UserGuid = contractViewModel.KeyPersonnel.ProjectManager;
                projectManager.UserRole = Contracts._projectManager;
                contractUserRoleList.Add(projectManager);
            }

            if (contractViewModel.KeyPersonnel.RegionalManager != null && contractViewModel.KeyPersonnel.RegionalManager != Guid.Empty)
            {
                var regionalManager = new ContractUserRole();
                regionalManager.UserGuid = contractViewModel.KeyPersonnel.RegionalManager ?? Guid.Empty;
                regionalManager.UserRole = Contracts._regionalManager;
                contractUserRoleList.Add(regionalManager);
            }

            if (contractViewModel.KeyPersonnel.ProjectControls != null)
            {
                var projectControls = new ContractUserRole();
                projectControls.UserGuid = contractViewModel.KeyPersonnel.ProjectControls;
                projectControls.UserRole = Contracts._projectControls;
                contractUserRoleList.Add(projectControls);
            }

            if (contractViewModel.KeyPersonnel.ContractRepresentative != null)
            {
                var contractRepresentative = new ContractUserRole();
                contractRepresentative.UserGuid = contractViewModel.KeyPersonnel.ContractRepresentative;
                contractRepresentative.UserRole = Contracts._contractRepresentative;
                contractUserRoleList.Add(contractRepresentative);
            }

            if (contractViewModel.KeyPersonnel.SubContractAdministrator != null)
            {
                var subContractAdministrator = new ContractUserRole();
                subContractAdministrator.UserGuid = contractViewModel.KeyPersonnel.SubContractAdministrator;
                subContractAdministrator.UserRole = Contracts._subContractAdministrator;
                contractUserRoleList.Add(subContractAdministrator);
            }

            if (contractViewModel.KeyPersonnel.PurchasingRepresentative != null)
            {
                var purchasingRepresentative = new ContractUserRole();
                purchasingRepresentative.UserGuid = contractViewModel.KeyPersonnel.PurchasingRepresentative;
                purchasingRepresentative.UserRole = Contracts._purchasingRepresentative;
                contractUserRoleList.Add(purchasingRepresentative);
            }

            if (contractViewModel.KeyPersonnel.HumanResourceRepresentative != null)
            {
                var humanResourceRepresentative = new ContractUserRole();
                humanResourceRepresentative.UserGuid = contractViewModel.KeyPersonnel.HumanResourceRepresentative;
                humanResourceRepresentative.UserRole = Contracts._humanResourceRepresentative;
                contractUserRoleList.Add(humanResourceRepresentative);
            }

            if (contractViewModel.KeyPersonnel.QualityRepresentative != null)
            {
                var qualityRepresentative = new ContractUserRole();
                qualityRepresentative.UserGuid = contractViewModel.KeyPersonnel.QualityRepresentative;
                qualityRepresentative.UserRole = Contracts._qualityRepresentative;
                contractUserRoleList.Add(qualityRepresentative);
            }

            if (contractViewModel.KeyPersonnel.SafetyOfficer != null)
            {
                var safetyOfficer = new ContractUserRole();
                safetyOfficer.UserGuid = contractViewModel.KeyPersonnel.SafetyOfficer;
                safetyOfficer.UserRole = Contracts._safetyOfficer;
                contractUserRoleList.Add(safetyOfficer);
            }

            if (contractViewModel.KeyPersonnel.OperationManager != null)
            {
                var operationManager = new ContractUserRole();
                Guid userGuid = contractViewModel.KeyPersonnel.OperationManager ?? Guid.Empty;
                operationManager.UserGuid = userGuid;
                operationManager.UserRole = Contracts._operationManager;
                contractUserRoleList.Add(operationManager);
            }

            contract.ContractUserRole = contractUserRoleList;

            return contract;
        }

        public static ContractViewModel MapEntityToModel(Contracts contract, bool isCumulative)
        {
            ContractViewModel contractViewModel = new ContractViewModel();

            var basicInfo = new BasicContractInfoViewModel();
            var keyPersonnel = new KeyPersonnelViewModel();
            var customerInfo = new CustomerInformationViewModel();
            var financialInfo = new FinancialInformationViewModel();

            contractViewModel.ContractGuid = contract.ContractGuid;
            contractViewModel.ParentContractGuid = contract.ParentContractGuid;
            contractViewModel.UpdatedOn = contract.UpdatedOn;
            contractViewModel.IsActive = contract.IsActive;
            contractViewModel.Status = contract.Status;
            contractViewModel.ProjectCounter = contract.ProjectCounter;

            //edited
            contractViewModel.IsImported = contract.IsImported;
            contractViewModel.TaskNodeId = contract.TaskNodeID;
            //

            basicInfo.ContractTitle = contract.ContractTitle;
            basicInfo.FarContractTypeGuid = contract.FarContractTypeGuid;
            basicInfo.ContractNumber = contract.ContractNumber;
            basicInfo.POPStart = contract.POPStart;
            basicInfo.POPEnd = contract.POPEnd;
            basicInfo.QualityLevel = contract.BasicContractInfo.QualityLevel;
            basicInfo.QualityLevelName = contract.BasicContractInfo.QualityLevelName;
            basicInfo.QualityLevelRequirements = contract.QualityLevelRequirements;
            basicInfo.IsIDIQContract = contract.IsIDIQContract;
            basicInfo.Description = contract.Description;
            basicInfo.ProjectNumber = contract.ProjectNumber;
            basicInfo.IsPrimeContract = contract.IsPrimeContract;
            basicInfo.SubContractNumber = contract.SubContractNumber;
            if (contract.Organisation != null)
            {
                basicInfo.ORGID = contract.Organisation.OrgIDGuid;
                basicInfo.OrganizationName = contract.Organisation.Name;
            }

            basicInfo.CPAREligible = contract.CPAREligible;
            basicInfo.CountryOfPerformanceSelected = contract.Country.CountryName;
            basicInfo.PlaceOfPerformanceSelected = contract.PlaceOfPerformance;
            basicInfo.PlaceOfPerformanceSelectedIds = contract.PlaceOfPerformanceSelectedIds;
            basicInfo.ParentContractNumber = contract.ParentContractNumber;

            basicInfo.AddressLine1 = contract.AddressLine1;
            basicInfo.AddressLine2 = contract.AddressLine2;
            basicInfo.AddressLine3 = contract.AddressLine3;
            basicInfo.City = contract.City;
            basicInfo.Province = contract.Province;
            basicInfo.County = contract.County;
            basicInfo.PostalCode = contract.PostalCode;

            if (contract.NAICS != null)
            {
                basicInfo.NAICSCodeName = contract.NAICS.Code + " " + contract.NAICS.Title;
                basicInfo.NAICSCode = contract.NAICS.NAICSGuid;
            }

            if (contract.PSC != null)
            {
                basicInfo.PSCCodeName = contract.PSC.CodeDescription;
                basicInfo.PSCCode = contract.PSC.PSCGuid;
            }

            basicInfo.OfficeName = contract.OfficeName;
            basicInfo.RegionName = contract.RegionName;
            basicInfo.CompanyName = contract.CompanyName;

            customerInfo.AwardingAgencyOffice = contract.CustomerInformation.AwardingAgencyOffice;
            customerInfo.OfficeContractRepresentative = contract.CustomerInformation.OfficeContractRepresentative;
            customerInfo.OfficeContractTechnicalRepresent = contract.CustomerInformation.OfficeContractTechnicalRepresent;
            customerInfo.FundingAgencyOffice = contract.CustomerInformation.FundingAgencyOffice;
            customerInfo.FundingOfficeContractRepresentative = contract.CustomerInformation.FundingOfficeContractRepresentative;
            customerInfo.FundingOfficeContractTechnicalRepresent = contract.CustomerInformation.FundingOfficeContractTechnicalRepresent;
            financialInfo.CompetitionType = contract.FinancialInformation.CompetitionType;
            financialInfo.Competition = contract.FinancialInformation.Competition;
            financialInfo.FundingAmount = contract.FundingAmount ?? 0;
            financialInfo.FeePercent = contract.FeePercent ?? 0;
            financialInfo.BillingFrequencyName = contract.FinancialInformation.BillingFrequencyName;
            financialInfo.BillingFrequency = contract.BillingFrequency;
            financialInfo.SetAsideName = contract.FinancialInformation.SetAsideName;
            financialInfo.SetAside = contract.FinancialInformation.SetAside;
            financialInfo.Currency = contract.FinancialInformation.Currency;
            financialInfo.CurrencyName = contract.FinancialInformation.CurrencyName;

            financialInfo.InvoiceSubmissionMethod = contract.InvoiceSubmissionMethod;
            financialInfo.InvoiceSubmissionMethodName = contract.FinancialInformation.InvoiceSubmissionMethodName;
            financialInfo.BlueSkyAwardAmount = contract.BlueSkyAwardAmount ?? 0;
            financialInfo.SBA = contract.SBA;
            financialInfo.GAPercent = contract.GAPercent ?? 0;
            financialInfo.ContractType = contract.FinancialInformation.ContractType;
            financialInfo.ContractTypeName = contract.FinancialInformation.ContractTypeName;
            financialInfo.AwardAmount = contract.AwardAmount ?? 0;

            financialInfo.BillingAddress = contract.BillingAddress;
            financialInfo.BillingFormula = contract.FinancialInformation.BillingFormula;
            financialInfo.RevenueFormula = contract.FinancialInformation.RevenueFormula;
            financialInfo.BillingFormulaName = contract.FinancialInformation.BillingFormulaName;
            financialInfo.RevenueFormulaName = contract.FinancialInformation.RevenueFormulaName;
            financialInfo.RevenueRecognitionEACPercent = contract.RevenueRecognitionEACPercent;
            financialInfo.SelfPerformancePercent = contract.SelfPerformancePercent;
            financialInfo.PaymentTerms = contract.FinancialInformation.PaymentTerms;
            financialInfo.PaymentTermsName = contract.FinancialInformation.PaymentTermsName;
            financialInfo.BlueSkyAwardAmount = contract.BlueSkyAwardAmount;
            financialInfo.OHonsite = contract.OHonsite;
            financialInfo.OHoffsite = contract.OHoffsite;
            financialInfo.OverHead = contract.OverHead;

            customerInfo.AwardingAgencyOfficeName = contract.CustomerInformation.AwardingAgencyOfficeName;
            customerInfo.OfficeContractRepresentativeName = contract.CustomerInformation.OfficeContractRepresentativeName;
            customerInfo.OfficeContractTechnicalRepresentName = contract.CustomerInformation.OfficeContractTechnicalRepresentName;
            customerInfo.FundingAgencyOfficeName = contract.CustomerInformation.FundingAgencyOfficeName;
            customerInfo.FundingOfficeContractRepresentativeName = contract.CustomerInformation.FundingOfficeContractRepresentativeName;
            customerInfo.FundingOfficeContractTechnicalRepresentName = contract.CustomerInformation.FundingOfficeContractTechnicalRepresentName;

            var wageDeterminaton = new List<string>();
            financialInfo.AppWageDetermineDavisBaconActType = contract.FinancialInformation.AppWageDetermineDavisBaconActType;
            financialInfo.AppWageDetermineServiceContractActType = contract.FinancialInformation.AppWageDetermineServiceContractActType;
            financialInfo.AppWageDetermineDavisBaconAct = contract.FinancialInformation.AppWageDetermineDavisBaconAct;
            financialInfo.AppWageDetermineServiceContractAct = contract.FinancialInformation.AppWageDetermineServiceContractAct;

            if (contract.CompanyPresident != null)
            {
                keyPersonnel.CompanyPresident = contract.CompanyPresident.UserGuid;
                keyPersonnel.CompanyPresidentName = contract.CompanyPresident.Firstname + " " + contract.CompanyPresident.Lastname + " (" + contract.CompanyPresident.JobTitle + ")";
                keyPersonnel.CompanyPresidentModel = MapUserToUserViewModel(contract.CompanyPresident);
            }

            if (contract.AccountRepresentative != null)
            {
                keyPersonnel.AccountingRepresentative = contract.AccountRepresentative.UserGuid;
                keyPersonnel.AccountingRepresentativeName = contract.AccountRepresentative.Firstname + " " + contract.AccountRepresentative.Lastname + " (" + contract.AccountRepresentative.JobTitle + ")";
                keyPersonnel.AccountRepresentativeModel = MapUserToUserViewModel(contract.AccountRepresentative);
            }

            if (contract.ProjectManager != null)
            {
                keyPersonnel.ProjectManager = contract.ProjectManager.UserGuid;
                keyPersonnel.ProjectManagerName = contract.ProjectManager.Firstname + " " + contract.ProjectManager.Lastname + " (" + contract.ProjectManager.JobTitle + ")";
                keyPersonnel.ProjectManagerModel = MapUserToUserViewModel(contract.ProjectManager);
            }

            if (contract.RegionalManager != null)
            {
                keyPersonnel.RegionalManager = contract.RegionalManager.UserGuid;
                keyPersonnel.RegionalManagerName = contract.RegionalManager.Firstname + " " + contract.RegionalManager.Lastname + ((contract.RegionalManager.JobTitle == null) ? " " : " (" + contract.RegionalManager.JobTitle + ")");
                keyPersonnel.RegionalManagerModel = MapUserToUserViewModel(contract.RegionalManager);
            }

            if (contract.DeputyRegionalManager != null)
            {
                keyPersonnel.DeputyRegionalManager = contract.DeputyRegionalManager.UserGuid;
                keyPersonnel.DeputyRegionalManagerName = contract.DeputyRegionalManager.Firstname + " " + contract.DeputyRegionalManager.Lastname
                    + ((contract.DeputyRegionalManager.JobTitle == null) ? " " : " (" + contract.DeputyRegionalManager.JobTitle + ")");
                keyPersonnel.DeputyRegionalManagerModel = MapUserToUserViewModel(contract.DeputyRegionalManager);
            }
            if (contract.HealthAndSafetyRegionalManager != null)
            {
                keyPersonnel.HealthAndSafetyRegionalManager = contract.HealthAndSafetyRegionalManager.UserGuid;
                keyPersonnel.HealthAndSafetyRegionalManagerName = contract.HealthAndSafetyRegionalManager.Firstname + " " + contract.HealthAndSafetyRegionalManager.Lastname + ((contract.HealthAndSafetyRegionalManager.JobTitle == null) ? " " : " (" + contract.HealthAndSafetyRegionalManager.JobTitle + ")");
                keyPersonnel.HSRegionalManagerModel = MapUserToUserViewModel(contract.HealthAndSafetyRegionalManager);
            }
            if (contract.BusinessDevelopmentRegionalManager != null)
            {
                keyPersonnel.BusinessDevelopmentRegionalManager = contract.BusinessDevelopmentRegionalManager.UserGuid;
                keyPersonnel.BusinessDevelopmentRegionalManagerName = contract.BusinessDevelopmentRegionalManager.Firstname + " " + contract.BusinessDevelopmentRegionalManager.Lastname + ((contract.BusinessDevelopmentRegionalManager.JobTitle == null) ? " " : " (" + contract.BusinessDevelopmentRegionalManager.JobTitle + ")");
                keyPersonnel.BDRegionalManagerModel = MapUserToUserViewModel(contract.BusinessDevelopmentRegionalManager);
            }

            if (contract.ProjectControls != null)
            {
                keyPersonnel.ProjectControls = contract.ProjectControls.UserGuid;
                keyPersonnel.ProjectControlName = contract.ProjectControls.Firstname + " " + contract.ProjectControls.Lastname + " (" + contract.ProjectControls.JobTitle + ")";
                keyPersonnel.ProjectControlModel = MapUserToUserViewModel(contract.ProjectControls);
            }

            if (contract.ContractRepresentative != null)
            {
                keyPersonnel.ContractRepresentative = contract.ContractRepresentative.UserGuid;
                keyPersonnel.ContractRepresentativeName = contract.ContractRepresentative.Firstname + " " + contract.ContractRepresentative.Lastname + " (" + contract.ContractRepresentative.JobTitle + ")";
                keyPersonnel.ContractRepresentativeModel = MapUserToUserViewModel(contract.ContractRepresentative);
            }

            if (contract.SubContractAdministrator != null)
            {
                keyPersonnel.SubContractAdministrator = contract.SubContractAdministrator.UserGuid;
                keyPersonnel.SubContractAdministratorName = contract.SubContractAdministrator.Firstname + " " + contract.SubContractAdministrator.Lastname + " (" + contract.SubContractAdministrator.JobTitle + ")";
                keyPersonnel.SubContractAdministratorModel = MapUserToUserViewModel(contract.SubContractAdministrator);
            }

            if (contract.PurchasingRepresentative != null)
            {
                keyPersonnel.PurchasingRepresentative = contract.PurchasingRepresentative.UserGuid;
                keyPersonnel.PurchasingRepresentativeName = contract.PurchasingRepresentative.Firstname + " " + contract.PurchasingRepresentative.Lastname + " (" + contract.PurchasingRepresentative.JobTitle + ")";
                keyPersonnel.PurchasingRepresentativeModel = MapUserToUserViewModel(contract.PurchasingRepresentative);
            }

            if (contract.HumanResourceRepresentative != null)
            {
                keyPersonnel.HumanResourceRepresentative = contract.HumanResourceRepresentative.UserGuid;
                keyPersonnel.HumanResourceRepresentativeName = contract.HumanResourceRepresentative.Firstname + " " + contract.HumanResourceRepresentative.Lastname + " (" + contract.HumanResourceRepresentative.JobTitle + ")";
                keyPersonnel.HumanResourceRepresentativeModel = MapUserToUserViewModel(contract.HumanResourceRepresentative);
            }

            if (contract.QualityRepresentative != null)
            {
                keyPersonnel.QualityRepresentative = contract.QualityRepresentative.UserGuid;
                keyPersonnel.QualityRepresentativeName = contract.QualityRepresentative.Firstname + " " + contract.QualityRepresentative.Lastname + " (" + contract.QualityRepresentative.JobTitle + ")";
                keyPersonnel.QualityRepresentativeModel = MapUserToUserViewModel(contract.QualityRepresentative);
            }

            if (contract.SafetyOfficer != null)
            {
                keyPersonnel.SafetyOfficer = contract.SafetyOfficer.UserGuid;
                keyPersonnel.SafetyOfficerName = contract.SafetyOfficer.Firstname + " " + contract.SafetyOfficer.Lastname + " (" + contract.SafetyOfficer.JobTitle + ")";
                keyPersonnel.SafetyOfficeModel = MapUserToUserViewModel(contract.SafetyOfficer);
            }

            if (contract.OperationManager != null)
            {
                keyPersonnel.OperationManager = contract.OperationManager.UserGuid;
                keyPersonnel.OperationManagerName = contract.OperationManager?.Firstname + " " + contract.OperationManager?.Lastname + " (" + contract.OperationManager?.JobTitle + ")";
                keyPersonnel.OperationManagerModel = MapUserToUserViewModel(contract.OperationManager);
            }

            if (isCumulative)
            {
                financialInfo.AwardAmount = contract.FinancialInformation.CumulativeAwardAmount;
                financialInfo.FundingAmount = contract.FinancialInformation.CumulativeFundingAmount;
            }

            contractViewModel.BasicContractInfo = basicInfo;
            contractViewModel.KeyPersonnel = keyPersonnel;
            contractViewModel.CustomerInformation = customerInfo;
            contractViewModel.FinancialInformation = financialInfo;
            contractViewModel.CreatedBy = contract.CreatedBy;
            contractViewModel.CreatedOn = contract.CreatedOn;

            return contractViewModel;
        }

        public static ContractAndProjectView MapEntityWithContractView(Contracts contract)
        {
            var view = new ContractAndProjectView();
            view.ContractGuid = contract.ContractGuid;
            view.BillingFormula = contract.BillingFormula;
            view.RevenueFormula = contract.RevenueFormula;
            view.AwardAmount = contract.AwardAmount ?? 0;
            view.BillingAddress = contract.BillingAddress;
            view.ContractTitle = contract.ContractTitle;
            view.POPStart = contract.POPStart.Value.Date.ToShortDateString();
            view.POPEnd = contract.POPEnd.Value.ToShortDateString();
            view.UpdatedOn = contract.UpdatedOn.ToShortDateString();
            view.CreatedOn = contract.CreatedOn.ToShortDateString();
            view.ContractNumber = contract.ContractNumber;
            view.Competition = contract.Competition;
            view.FundingAmount = contract.FundingAmount ?? 0;
            view.FeePercent = contract.FeePercent ?? 0;
            view.BillingFrequency = contract.BillingFrequency;
            //view.ProjectControls = contract.ParentContractGuid;
            view.QualityLevel = contract.QualityLevel;
            view.SetAside = contract.SetAside;
            view.QualityLevelRequirements = contract.QualityLevelRequirements;
            view.Currency = contract.Currency;
            //view.AppWageDetermine_DavisBaconAct = contract.AppWageDetermineDavisBaconAct;
            view.IsIDIQContract = contract.IsIDIQContract;
            view.Description = contract.Description;
            view.ProjectNumber = contract.ProjectNumber;
            view.InvoiceSubmissionMethod = contract.InvoiceSubmissionMethod;
            view.InvoiceSubmissionMethodName = contract.InvoiceSubmissionMethod;
            view.BlueSkyAwardAmount = contract.BlueSkyAwardAmount ?? 0;
            view.SBA = contract.SBA;
            view.SubContractNumber = contract.SubContractNumber;
            view.GAPercent = contract.GAPercent ?? 0;
            view.ContractType = contract.ContractType;
            view.IsPrimeContract = contract.IsPrimeContract;
            view.QualityLevelRequirements = contract.QualityLevelRequirements;
            view.Currency = contract.Currency;
            view.PaymentTerms = contract.PaymentTerms;
            view.ApplicableWageDetemination = contract.ApplicableWageDetermination;
            view.IsActive = contract.IsActive;
            view.IsFavorite = contract.IsFavorite;
            view.PlaceOfPerformance = contract.PlaceOfPerformance;
            view.ContractType = contract.ContractType;
            view.OverHead = contract.OverHead ?? 0;
            view.Status = contract.Status;

            view.FundingAgencyOfficeName = contract.FundingAgencyName;
            view.FundingOffice_ContractRepresentativeName = contract.FundingAgencyContractRepresentativeName;
            view.FundingOffice_ContractTechnicalRepresentName = contract.FundingAgencyContractTechnicalRepresentativeName;

            view.AwardingAgencyOfficeName = contract.AwardingAgencyName;
            view.Office_ContractRepresentative = contract.AwardingAgencyContractRepresentativeName;
            view.Office_ContractTechnicalRepresent = contract.AwardingAgencyContractTechnicalRepresentativeName;

            if (contract.Country != null)
                view.CountryOfPerformanceName = contract.Country.CountryName;

            if (contract.PSC != null)
                view.PSCCodeName = contract.PSC.CodeDescription;
            if (contract.NAICS != null)
                view.NAICSCodeName = contract.NAICS.Code + " (" + contract.NAICS.Title + ")";

            if (contract.IsActive == true)
                view.IsActiveStatus = "Active";
            else
                view.IsActiveStatus = "Inactive";

            if (contract.CompanyPresident != null)
            {
                view.CompanyPresident = contract.CompanyPresident.UserGuid;
                view.CompanyPresidentName = contract.CompanyPresident.Firstname + " " + contract.CompanyPresident.Lastname;
            }

            if (contract.AccountRepresentative != null)
            {
                view.AccountingRepresentative = contract.AccountRepresentative.UserGuid;
                view.AccountingRepresentativeName = contract.AccountRepresentative.Firstname + " " + contract.AccountRepresentative.Lastname;
            }

            if (contract.Country != null)
                view.CountryOfPerformanceName = contract.Country.CountryName;

            if (contract.ProjectManager != null)
            {
                view.ProjectManager = contract.ProjectManager.UserGuid;
                view.ProjectManagerName = contract.ProjectManager.Firstname + " " + contract.ProjectManager.Lastname;
            }

            if (contract.RegionalManager != null)
            {
                view.RegionalManager = contract.RegionalManager.UserGuid;
                view.RegionalManagerName = contract.RegionalManager.Firstname + " " + contract.RegionalManager.Lastname;
            }

            if (contract.ProjectControls != null)
            {
                view.ProjectControls = contract.ProjectControls.UserGuid;
                view.ProjectControlsName = contract.ProjectControls.Firstname + " " + contract.ProjectControls.Lastname;
            }


            if (contract.ContractRepresentative != null)
            {
                view.ContractRepresentative = contract.ContractRepresentative.UserGuid;
                view.ContractRepresentativeName = contract.ContractRepresentative.Firstname + " " + contract.ContractRepresentative.Lastname;
            }

            if (contract.SubContractAdministrator != null)
            {
                view.SubContractAdministrator = contract.SubContractAdministrator.UserGuid;
                view.SubContractAdministratorName = contract.SubContractAdministrator.Firstname + " " + contract.SubContractAdministrator.Lastname;
            }

            if (contract.PurchasingRepresentative != null)
            {
                view.PurchasingRepresentative = contract.PurchasingRepresentative.UserGuid;
                view.PurchasingRepresentativeName = contract.PurchasingRepresentative.Firstname + " " + contract.PurchasingRepresentative.Lastname;
            }

            if (contract.HumanResourceRepresentative != null)
            {
                view.HumanResourceRepresentative = contract.HumanResourceRepresentative.UserGuid;
                view.HumanResourceRepresentativeName = contract.HumanResourceRepresentative.Firstname + " " + contract.HumanResourceRepresentative.Lastname;
            }

            if (contract.QualityRepresentative != null)
            {
                view.QualityRepresentative = contract.QualityRepresentative.UserGuid;
                view.QualityRepresentativeName = contract.QualityRepresentative.Firstname + " " + contract.QualityRepresentative.Lastname;
            }

            if (contract.SafetyOfficer != null)
            {
                view.SafetyOfficer = contract.SafetyOfficer.UserGuid;
                view.SafetyOfficerName = contract.SafetyOfficer.Firstname + " " + contract.SafetyOfficer.Lastname;
            }

            if (contract.OperationManager != null)
            {
                view.OperationManager = contract.OperationManager.UserGuid;
                view.OperationManagerName = contract.OperationManager.Firstname + " " + contract.OperationManager.Lastname;
            }

            if (contract.Organisation != null)
            {
                view.OrgName = contract.Organisation.Name;
                view.ORGIDName = contract.Organisation.Name;
                view.OrganizationName = contract.Organisation.Name;
            }
            if (!string.IsNullOrWhiteSpace(contract.CompanyName))
                view.CompanyCode = contract.CompanyName;
            return view;
        }

        public static JobRequestViewModel MapJobRequestToViewModel(JobRequest jobRequest)
        {
            JobRequestViewModel jobRequestViewModel = new JobRequestViewModel();

            var basicInfo = new BasicContractInfoViewModel();
            var keyPersonnel = new KeyPersonnelViewModel();
            var customerInfo = new CustomerInformationViewModel();
            var financialInfo = new FinancialInformationViewModel();

            jobRequestViewModel.ContractGuid = jobRequest.Contracts.ContractGuid;
            jobRequestViewModel.Parent_ContractGuid = jobRequest.Contracts.ParentContractGuid;
            jobRequestViewModel.UpdatedOn = jobRequest.Contracts.UpdatedOn;

            if (jobRequest.Contracts.JobRequest != null)
            {
                jobRequestViewModel.Status = jobRequest.Contracts.JobRequest.Status;
                jobRequestViewModel.Companies = jobRequest.Contracts.JobRequest.Companies;
                jobRequestViewModel.CompanySelected = jobRequest.Contracts.JobRequest.CompanySelected;
                jobRequestViewModel.IsIntercompanyWorkOrder = jobRequest.Contracts.JobRequest.IsIntercompanyWorkOrder;
                jobRequestViewModel.Notes = jobRequest.Contracts.JobRequest.Notes;
            }

            if (jobRequest.Contracts.ContractResourceFile.Count > 0)
            {
                var wbs = jobRequest.Contracts.ContractResourceFile.Where(x => x.Keys == Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString()).FirstOrDefault();
                jobRequestViewModel.ContractWBS = MapContractFilesToContractWBSViewModel(wbs);

                var employeeBillingRates = jobRequest.Contracts.ContractResourceFile.Where(x => x.Keys == Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString()).FirstOrDefault();
                jobRequestViewModel.EmployeeBillingRates = MapContractFilesToEmployeeBillingRatesViewModel(employeeBillingRates);

                var laborBillingRates = jobRequest.Contracts.ContractResourceFile.Where(x => x.Keys == Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString()).FirstOrDefault();
                jobRequestViewModel.LaborCategoryRates = MapContractFilesToSubcontractorBillingRatesViewModel(laborBillingRates);
            }

            basicInfo.ContractTitle = jobRequest.Contracts.BasicContractInfo.ContractTitle;
            basicInfo.ContractNumber = jobRequest.Contracts.BasicContractInfo.ContractNumber;
            basicInfo.POPStart = jobRequest.Contracts.BasicContractInfo.POPStart;
            basicInfo.POPEnd = jobRequest.Contracts.BasicContractInfo.POPEnd;
            basicInfo.QualityLevel = jobRequest.Contracts.BasicContractInfo.QualityLevel;
            basicInfo.QualityLevelName = jobRequest.Contracts.BasicContractInfo.QualityLevelName;
            basicInfo.QualityLevelRequirements = jobRequest.Contracts.BasicContractInfo.QualityLevelRequirements;
            basicInfo.IsIDIQContract = jobRequest.Contracts.BasicContractInfo.IsIDIQContract;
            basicInfo.ProjectNumber = jobRequest.Contracts.BasicContractInfo.ProjectNumber;
            basicInfo.SubContractNumber = jobRequest.Contracts.BasicContractInfo.SubContractNumber;
            basicInfo.ORGID = jobRequest.Contracts.BasicContractInfo.ORGID;
            basicInfo.OrganizationName = jobRequest.Contracts.BasicContractInfo.OrganizationName;
            basicInfo.CountryOfPerformanceSelected = jobRequest.Contracts.BasicContractInfo.CountryOfPerformanceSelected;
            basicInfo.PlaceOfPerformanceSelected = jobRequest.Contracts.BasicContractInfo.PlaceOfPerformanceSelected;
            basicInfo.PlaceOfPerformanceSelectedIds = jobRequest.Contracts.BasicContractInfo.PlaceOfPerformanceSelectedIds;
            basicInfo.NAICSCodeName = jobRequest.Contracts.BasicContractInfo.NAICSCodeName;
            basicInfo.NAICSCode = jobRequest.Contracts.BasicContractInfo.NAICSCode;
            basicInfo.OfficeName = jobRequest.Contracts.OfficeName;
            basicInfo.RegionName = jobRequest.Contracts.RegionName;
            basicInfo.CompanyName = jobRequest.Contracts.CompanyName;
            basicInfo.ParentContractNumber = jobRequest.Contracts.ParentContractNumber;

            customerInfo.AwardingAgencyOffice = jobRequest.Contracts.CustomerInformation.AwardingAgencyOffice;
            customerInfo.OfficeContractRepresentative = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentative;
            customerInfo.OfficeContractTechnicalRepresent = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresent;
            customerInfo.FundingAgencyOffice = jobRequest.Contracts.CustomerInformation.FundingAgencyOffice;
            customerInfo.FundingOfficeContractRepresentative = jobRequest.Contracts.CustomerInformation.FundingOfficeContractRepresentative;
            customerInfo.FundingOfficeContractTechnicalRepresent = jobRequest.Contracts.CustomerInformation.FundingOfficeContractTechnicalRepresent;

            customerInfo.AwardingAgencyOfficeName = jobRequest.Contracts.CustomerInformation.AwardingAgencyOfficeName;
            customerInfo.OfficeContractRepresentativeName = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeName;
            customerInfo.OfficeContractTechnicalRepresentName = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentName;
            customerInfo.FundingAgencyOfficeName = jobRequest.Contracts.CustomerInformation.FundingAgencyOfficeName;
            customerInfo.FundingOfficeContractRepresentativeName = jobRequest.Contracts.CustomerInformation.FundingOfficeContractRepresentativeName;
            customerInfo.FundingOfficeContractTechnicalRepresentName = jobRequest.Contracts.CustomerInformation.FundingOfficeContractTechnicalRepresentName;
            customerInfo.AwardingAgencyCustomerTypeName = jobRequest.Contracts.CustomerInformation.AwardingAgencyCustomerTypeName;
            customerInfo.FundingAgencyCustomerTypeName = jobRequest.Contracts.CustomerInformation.FundingAgencyCustomerTypeName;

            customerInfo.ContractRepresentativeContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.ContractRepresentativePhoneNumber, jobRequest.Contracts.CustomerInformation.ContractRepresentativeEmailAddress);
            customerInfo.ContractRepresentativeAltContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.ContractRepresentativeAltPhoneNumber, jobRequest.Contracts.CustomerInformation.ContractRepresentativeAltEmailAddress);
            customerInfo.ContractTechnicalRepresentativeContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.ContractTechnicalRepresentativePhoneNumber, jobRequest.Contracts.CustomerInformation.ContractTechnicalRepresentativeEmailAddress);
            customerInfo.ContractTechnicalRepresentativeAltContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.ContractTechnicalRepresentativeAltPhoneNumber, jobRequest.Contracts.CustomerInformation.ContractTechnicalRepresentativeAltEmailAddress);
            customerInfo.OfficeContractRepresentativeContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativePhoneNumber, jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeEmailAddress);
            customerInfo.OfficeContractRepresentativeAltContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeAltPhoneNumber, jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeAltEmailAddress);
            customerInfo.OfficeContractTechnicalRepresentativeContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativePhoneNumber, jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeEmailAddress);
            customerInfo.OfficeContractTechnicalRepresentativeAltContact = FormatHelper.ConcatTwoString(jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeAltPhoneNumber, jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeAltEmailAddress);

            customerInfo.OfficeContractRepresentativePhoneNumber = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativePhoneNumber;
            customerInfo.OfficeContractRepresentativeAltPhoneNumber = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeAltPhoneNumber;
            customerInfo.OfficeContractRepresentativeEmailAddress = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeEmailAddress;
            customerInfo.OfficeContractRepresentativeAltEmailAddress = jobRequest.Contracts.CustomerInformation.OfficeContractRepresentativeAltEmailAddress;
            customerInfo.OfficeContractTechnicalRepresentativePhoneNumber = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativePhoneNumber;
            customerInfo.OfficeContractTechnicalRepresentativeAltPhoneNumber = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeAltPhoneNumber;
            customerInfo.OfficeContractTechnicalRepresentativeEmailAddress = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeEmailAddress;
            customerInfo.OfficeContractTechnicalRepresentativeAltEmailAddress = jobRequest.Contracts.CustomerInformation.OfficeContractTechnicalRepresentativeAltEmailAddress;

            financialInfo.ContractType = jobRequest.Contracts.FinancialInformation.ContractType;
            financialInfo.ContractTypeName = jobRequest.Contracts.FinancialInformation.ContractTypeName;
            financialInfo.AwardAmount = jobRequest.Contracts.FinancialInformation.CumulativeAwardAmount;
            financialInfo.FundingAmount = jobRequest.Contracts.FinancialInformation.CumulativeFundingAmount;
            financialInfo.FeePercent = jobRequest.Contracts.FinancialInformation.FeePercent;
            financialInfo.BillingAddress = jobRequest.Contracts.FinancialInformation.BillingAddress;
            financialInfo.SetAside = jobRequest.Contracts.FinancialInformation.SetAside;
            financialInfo.SetAsideName = jobRequest.Contracts.FinancialInformation.SetAsideName;
            financialInfo.Currency = jobRequest.Contracts.FinancialInformation.Currency;

            if (jobRequest.Contracts.CompanyPresident != null)
            {
                keyPersonnel.CompanyPresident = jobRequest.Contracts.CompanyPresident.UserGuid;
                keyPersonnel.CompanyPresidentName = jobRequest.Contracts.CompanyPresident.DisplayName + " (" + jobRequest.Contracts.CompanyPresident.JobTitle + ")";
            }

            if (jobRequest.Contracts.AccountRepresentative != null)
            {
                keyPersonnel.AccountingRepresentative = jobRequest.Contracts.AccountRepresentative.UserGuid;
                keyPersonnel.AccountingRepresentativeName = jobRequest.Contracts.AccountRepresentative.DisplayName + " (" + jobRequest.Contracts.AccountRepresentative.JobTitle + ")";
            }

            if (jobRequest.Contracts.ProjectManager != null)
            {
                keyPersonnel.ProjectManager = jobRequest.Contracts.ProjectManager.UserGuid;
                keyPersonnel.ProjectManagerName = jobRequest.Contracts.ProjectManager.DisplayName + " (" + jobRequest.Contracts.ProjectManager.JobTitle + ")";
            }

            if (jobRequest.Contracts.RegionalManager != null)
            {
                keyPersonnel.RegionalManager = jobRequest.Contracts.RegionalManager.UserGuid;
                keyPersonnel.RegionalManagerName = jobRequest.Contracts.RegionalManager.Firstname + " " + jobRequest.Contracts.RegionalManager.Lastname + ((jobRequest.Contracts.RegionalManager.JobTitle == null) ? " " : " (" + jobRequest.Contracts.RegionalManager.JobTitle + ")");
                keyPersonnel.RegionalManagerModel = MapUserToUserViewModel(jobRequest.Contracts.RegionalManager);
            }

            if (jobRequest.Contracts.DeputyRegionalManager != null)
            {
                keyPersonnel.DeputyRegionalManager = jobRequest.Contracts.DeputyRegionalManager.UserGuid;
                keyPersonnel.DeputyRegionalManagerName = jobRequest.Contracts.DeputyRegionalManager.Firstname + " " + jobRequest.Contracts.DeputyRegionalManager.Lastname
                    + ((jobRequest.Contracts.DeputyRegionalManager.JobTitle == null) ? " " : " (" + jobRequest.Contracts.DeputyRegionalManager.JobTitle + ")");
                keyPersonnel.DeputyRegionalManagerModel = MapUserToUserViewModel(jobRequest.Contracts.DeputyRegionalManager);
            }
            if (jobRequest.Contracts.HealthAndSafetyRegionalManager != null)
            {
                keyPersonnel.HealthAndSafetyRegionalManager = jobRequest.Contracts.HealthAndSafetyRegionalManager.UserGuid;
                keyPersonnel.HealthAndSafetyRegionalManagerName = jobRequest.Contracts.HealthAndSafetyRegionalManager.Firstname + " " + jobRequest.Contracts.HealthAndSafetyRegionalManager.Lastname + ((jobRequest.Contracts.HealthAndSafetyRegionalManager.JobTitle == null) ? " " : " (" + jobRequest.Contracts.HealthAndSafetyRegionalManager.JobTitle + ")");
                keyPersonnel.HSRegionalManagerModel = MapUserToUserViewModel(jobRequest.Contracts.HealthAndSafetyRegionalManager);
            }
            if (jobRequest.Contracts.BusinessDevelopmentRegionalManager != null)
            {
                keyPersonnel.BusinessDevelopmentRegionalManager = jobRequest.Contracts.BusinessDevelopmentRegionalManager.UserGuid;
                keyPersonnel.BusinessDevelopmentRegionalManagerName = jobRequest.Contracts.BusinessDevelopmentRegionalManager.Firstname + " " + jobRequest.Contracts.BusinessDevelopmentRegionalManager.Lastname + ((jobRequest.Contracts.BusinessDevelopmentRegionalManager.JobTitle == null) ? " " : " (" + jobRequest.Contracts.BusinessDevelopmentRegionalManager.JobTitle + ")");
                keyPersonnel.BDRegionalManagerModel = MapUserToUserViewModel(jobRequest.Contracts.BusinessDevelopmentRegionalManager);
            }

            if (jobRequest.Contracts.ProjectControls != null)
            {
                keyPersonnel.ProjectControls = jobRequest.Contracts.ProjectControls.UserGuid;
                keyPersonnel.ProjectControlName = jobRequest.Contracts.ProjectControls.DisplayName + " (" + jobRequest.Contracts.ProjectControls.JobTitle + ")";
            }

            if (jobRequest.Contracts.ContractRepresentative != null)
            {
                keyPersonnel.ContractRepresentative = jobRequest.Contracts.ContractRepresentative.UserGuid;
                keyPersonnel.ContractRepresentativeName = jobRequest.Contracts.ContractRepresentative.DisplayName + " (" + jobRequest.Contracts.ContractRepresentative.JobTitle + ")";
            }

            if (jobRequest.Contracts.SubContractAdministrator != null)
            {
                keyPersonnel.SubContractAdministrator = jobRequest.Contracts.SubContractAdministrator.UserGuid;
                keyPersonnel.SubContractAdministratorName = jobRequest.Contracts.SubContractAdministrator.DisplayName + " (" + jobRequest.Contracts.SubContractAdministrator.JobTitle + ")";
                keyPersonnel.SubContractAdministratorModel = MapUserToUserViewModel(jobRequest.Contracts.SubContractAdministrator);
            }

            if (jobRequest.Contracts.PurchasingRepresentative != null)
            {
                keyPersonnel.PurchasingRepresentative = jobRequest.Contracts.PurchasingRepresentative.UserGuid;
                keyPersonnel.PurchasingRepresentativeName = jobRequest.Contracts.PurchasingRepresentative.DisplayName + " (" + jobRequest.Contracts.PurchasingRepresentative.JobTitle + ")";
                keyPersonnel.PurchasingRepresentativeModel = MapUserToUserViewModel(jobRequest.Contracts.PurchasingRepresentative);
            }

            if (jobRequest.Contracts.HumanResourceRepresentative != null)
            {
                keyPersonnel.HumanResourceRepresentative = jobRequest.Contracts.HumanResourceRepresentative.UserGuid;
                keyPersonnel.HumanResourceRepresentativeName = jobRequest.Contracts.HumanResourceRepresentative.DisplayName + " (" + jobRequest.Contracts.HumanResourceRepresentative.JobTitle + ")";
                keyPersonnel.HumanResourceRepresentativeModel = MapUserToUserViewModel(jobRequest.Contracts.HumanResourceRepresentative);
            }

            if (jobRequest.Contracts.QualityRepresentative != null)
            {
                keyPersonnel.QualityRepresentative = jobRequest.Contracts.QualityRepresentative.UserGuid;
                keyPersonnel.QualityRepresentativeName = jobRequest.Contracts.QualityRepresentative.DisplayName + " (" + jobRequest.Contracts.QualityRepresentative.JobTitle + ")";
                keyPersonnel.QualityRepresentativeModel = MapUserToUserViewModel(jobRequest.Contracts.QualityRepresentative);
            }

            if (jobRequest.Contracts.SafetyOfficer != null)
            {
                keyPersonnel.SafetyOfficer = jobRequest.Contracts.SafetyOfficer.UserGuid;
                keyPersonnel.SafetyOfficerName = jobRequest.Contracts.SafetyOfficer.DisplayName + " (" + jobRequest.Contracts.SafetyOfficer.JobTitle + ")";
                keyPersonnel.SafetyOfficeModel = MapUserToUserViewModel(jobRequest.Contracts.SafetyOfficer);
            }

            if (jobRequest.Contracts.OperationManager != null)
            {
                keyPersonnel.OperationManager = jobRequest.Contracts.OperationManager.UserGuid;
                keyPersonnel.OperationManagerName = jobRequest.Contracts.OperationManager?.DisplayName + " (" + jobRequest.Contracts.OperationManager?.JobTitle + ")";
                keyPersonnel.OperationManagerModel = MapUserToUserViewModel(jobRequest.Contracts.OperationManager);
            }

            jobRequestViewModel.Parent_ContractGuid = jobRequest.Contracts.ParentContractGuid;
            jobRequestViewModel.BasicContractInfo = basicInfo;
            jobRequestViewModel.KeyPersonnel = keyPersonnel;
            jobRequestViewModel.CustomerInformation = customerInfo;
            jobRequestViewModel.FinancialInformation = financialInfo;

            return jobRequestViewModel;
        }

        public static JobRequest MapViewModelToJobRequest(JobRequestViewModel jobRequestViewModel)
        {
            JobRequest jobRequest = new JobRequest();

            var contract = new Contracts();

            contract.ContractGuid = jobRequestViewModel.ContractGuid;
            jobRequest.ContractGuid = jobRequestViewModel.ContractGuid;
            jobRequest.IsIntercompanyWorkOrder = jobRequestViewModel.IsIntercompanyWorkOrder;
            jobRequest.Notes = jobRequestViewModel.Notes;
            if (!string.IsNullOrEmpty(jobRequestViewModel.Companies))
            {
                jobRequest.Companies = jobRequestViewModel.Companies;
                jobRequest.CompanySelected = jobRequestViewModel.CompanySelected;
            }
            contract.ProjectNumber = jobRequestViewModel.BasicContractInfo.ProjectNumber;
            var contractUserRoleList = new List<ContractUserRole>();

            if (jobRequestViewModel.KeyPersonnel != null)
            {
                if (jobRequestViewModel.KeyPersonnel.CompanyPresident != null && jobRequestViewModel.KeyPersonnel.CompanyPresident == Guid.Empty)
                {
                    var president = new ContractUserRole();
                    president.UserGuid = jobRequestViewModel.KeyPersonnel.CompanyPresident;
                    president.UserRole = Contracts._companyPresident;
                    contractUserRoleList.Add(president);
                }

                if (jobRequestViewModel.KeyPersonnel.AccountingRepresentative != null)
                {
                    var accountRepresentative = new ContractUserRole();
                    accountRepresentative.UserGuid = jobRequestViewModel.KeyPersonnel.AccountingRepresentative;
                    accountRepresentative.UserRole = Contracts._accountRepresentative;
                    contractUserRoleList.Add(accountRepresentative);
                }

                if (jobRequestViewModel.KeyPersonnel.ProjectManager != null)
                {
                    var projectManager = new ContractUserRole();
                    projectManager.UserGuid = jobRequestViewModel.KeyPersonnel.ProjectManager;
                    projectManager.UserRole = Contracts._projectManager;
                    contractUserRoleList.Add(projectManager);
                }

                if (jobRequestViewModel.KeyPersonnel.RegionalManager != null && jobRequestViewModel.KeyPersonnel.RegionalManager != Guid.Empty)
                {
                    var regionalManager = new ContractUserRole();
                    regionalManager.UserGuid = jobRequestViewModel.KeyPersonnel.RegionalManager ?? Guid.Empty;
                    regionalManager.UserRole = Contracts._regionalManager;
                    contractUserRoleList.Add(regionalManager);
                }

                if (jobRequestViewModel.KeyPersonnel.ProjectControls != null)
                {
                    var projectControls = new ContractUserRole();
                    projectControls.UserGuid = jobRequestViewModel.KeyPersonnel.ProjectControls;
                    projectControls.UserRole = Contracts._projectControls;
                    contractUserRoleList.Add(projectControls);
                }

                if (jobRequestViewModel.KeyPersonnel.ContractRepresentative != null)
                {
                    var contractRepresentative = new ContractUserRole();
                    contractRepresentative.UserGuid = jobRequestViewModel.KeyPersonnel.ContractRepresentative;
                    contractRepresentative.UserRole = Contracts._contractRepresentative;
                    contractUserRoleList.Add(contractRepresentative);
                }

                if (jobRequestViewModel.KeyPersonnel.SubContractAdministrator != null)
                {
                    var subContractAdministrator = new ContractUserRole();
                    subContractAdministrator.UserGuid = jobRequestViewModel.KeyPersonnel.SubContractAdministrator;
                    subContractAdministrator.UserRole = Contracts._subContractAdministrator;
                    contractUserRoleList.Add(subContractAdministrator);
                }

                if (jobRequestViewModel.KeyPersonnel.PurchasingRepresentative != null)
                {
                    var purchasingRepresentative = new ContractUserRole();
                    purchasingRepresentative.UserGuid = jobRequestViewModel.KeyPersonnel.PurchasingRepresentative;
                    purchasingRepresentative.UserRole = Contracts._purchasingRepresentative;
                    contractUserRoleList.Add(purchasingRepresentative);
                }

                if (jobRequestViewModel.KeyPersonnel.HumanResourceRepresentative != null)
                {
                    var humanResourceRepresentative = new ContractUserRole();
                    humanResourceRepresentative.UserGuid = jobRequestViewModel.KeyPersonnel.HumanResourceRepresentative;
                    humanResourceRepresentative.UserRole = Contracts._humanResourceRepresentative;
                    contractUserRoleList.Add(humanResourceRepresentative);
                }

                if (jobRequestViewModel.KeyPersonnel.QualityRepresentative != null)
                {
                    var qualityRepresentative = new ContractUserRole();
                    qualityRepresentative.UserGuid = jobRequestViewModel.KeyPersonnel.QualityRepresentative;
                    qualityRepresentative.UserRole = Contracts._qualityRepresentative;
                    contractUserRoleList.Add(qualityRepresentative);
                }

                if (jobRequestViewModel.KeyPersonnel.SafetyOfficer != null)
                {
                    var safetyOfficer = new ContractUserRole();
                    safetyOfficer.UserGuid = jobRequestViewModel.KeyPersonnel.SafetyOfficer;
                    safetyOfficer.UserRole = Contracts._safetyOfficer;
                    contractUserRoleList.Add(safetyOfficer);
                }

                contract.ContractUserRole = contractUserRoleList;
            }
            jobRequest.Contracts = contract;
            jobRequest.Status = jobRequestViewModel.Status;

            return jobRequest;
        }

        public static ContractWBSViewModel MapContractFilesToContractWBSViewModel(ContractResourceFile contractFiles)
        {
            ContractWBSViewModel contractWBSViewModel = new ContractWBSViewModel();
            if (contractFiles == null)
                return null;
            contractWBSViewModel.ContractGuid = contractFiles.ResourceGuid;
            contractWBSViewModel.ContractResourceFileGuid = contractFiles.ContractResourceFileGuid;
            contractWBSViewModel.CreatedBy = contractFiles.CreatedBy;
            contractWBSViewModel.CreatedOn = contractFiles.CreatedOn;
            contractWBSViewModel.IsActive = contractFiles.IsActive;
            contractWBSViewModel.IsCsv = contractFiles.IsCsv;
            contractWBSViewModel.IsDeleted = contractFiles.IsDeleted;
            contractWBSViewModel.UpdatedBy = contractFiles.UpdatedBy;
            contractWBSViewModel.UpdatedOn = contractFiles.UpdatedOn;
            contractWBSViewModel.UploadFileName = contractFiles.UploadFileName;
            contractWBSViewModel.FilePath = contractFiles.FilePath;
            contractWBSViewModel.FileSize = contractFiles.FileSize;

            return contractWBSViewModel;
        }

        public static ContractFileViewModel MapContractWBSToContractFilesViewModel(ContractWBS contractWBS)
        {
            ContractFileViewModel contractFileViewModel = new ContractFileViewModel();

            contractFileViewModel.ResourceGuid = contractWBS.ContractGuid;
            contractFileViewModel.CreatedBy = contractWBS.CreatedBy;
            contractFileViewModel.CreatedOn = contractWBS.CreatedOn;
            contractFileViewModel.IsActive = contractWBS.IsActive;
            contractFileViewModel.IsCsv = contractWBS.IsCsv;
            contractFileViewModel.IsDeleted = contractWBS.IsDeleted;
            contractFileViewModel.UpdatedBy = contractWBS.UpdatedBy;
            contractFileViewModel.UpdatedOn = contractWBS.UpdatedOn;
            contractFileViewModel.UploadFileName = contractWBS.UploadFileName;
            contractFileViewModel.keys = Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString();

            return contractFileViewModel;
        }

        public static ContractResourceFile MapContractWBSViewModelToContractResourceFile(ContractWBSViewModel contractWBSViewModel)
        {
            ContractResourceFile contractFiles = new ContractResourceFile();
            contractFiles.ResourceGuid = contractWBSViewModel.ContractGuid;
            contractFiles.ContentResourceGuid = contractWBSViewModel.ContractGuid;
            contractFiles.CreatedBy = contractWBSViewModel.CreatedBy;
            contractFiles.CreatedOn = contractWBSViewModel.CreatedOn;
            contractFiles.Keys = Core.Entities.EnumGlobal.ResourceType.WorkBreakDownStructure.ToString();
            contractFiles.IsActive = contractWBSViewModel.IsActive;
            contractFiles.IsCsv = contractWBSViewModel.IsCsv;
            contractFiles.IsDeleted = contractWBSViewModel.IsDeleted;
            contractFiles.UpdatedBy = contractWBSViewModel.UpdatedBy;
            contractFiles.UpdatedOn = contractWBSViewModel.UpdatedOn;
            contractFiles.UploadFileName = contractWBSViewModel.UploadFileName;
            contractFiles.FilePath = contractWBSViewModel.FilePath;
            contractFiles.FileSize = contractWBSViewModel.FileSize;
            contractFiles.IsFile = true;

            return contractFiles;
        }

        public static ContractWBS MapContractFilesViewModelToContractWBS(ContractFileViewModel contractFileViewModel)
        {
            ContractWBS contractWBS = new ContractWBS();

            contractWBS.ContractGuid = contractFileViewModel.ResourceGuid;
            contractWBS.CreatedBy = contractFileViewModel.CreatedBy;
            contractWBS.CreatedOn = contractFileViewModel.CreatedOn;
            contractWBS.IsActive = contractFileViewModel.IsActive;
            contractWBS.IsCsv = contractFileViewModel.IsCsv;
            contractWBS.IsDeleted = contractFileViewModel.IsDeleted;
            contractWBS.UpdatedBy = contractFileViewModel.UpdatedBy;
            contractWBS.UpdatedOn = contractFileViewModel.UpdatedOn;
            contractWBS.UploadFileName = contractFileViewModel.UploadFileName;

            return contractWBS;
        }

        public static ContractResourceFile MapEmployeeBillingRatesViewModelToContractFiles(EmployeeBillingRatesViewModel employeeBillingRatesViewModel)
        {
            ContractResourceFile contractFiles = new ContractResourceFile();

            contractFiles.ResourceGuid = employeeBillingRatesViewModel.ContractGuid;
            contractFiles.ContentResourceGuid = employeeBillingRatesViewModel.ContractGuid;
            contractFiles.CreatedBy = employeeBillingRatesViewModel.CreatedBy;
            contractFiles.CreatedOn = employeeBillingRatesViewModel.CreatedOn;
            contractFiles.Keys = Core.Entities.EnumGlobal.ResourceType.EmployeeBillingRates.ToString();
            contractFiles.IsActive = employeeBillingRatesViewModel.IsActive;
            contractFiles.IsCsv = employeeBillingRatesViewModel.IsCsv;
            contractFiles.IsDeleted = employeeBillingRatesViewModel.IsDeleted;
            contractFiles.UpdatedBy = employeeBillingRatesViewModel.UpdatedBy;
            contractFiles.UpdatedOn = employeeBillingRatesViewModel.UpdatedOn;
            contractFiles.UploadFileName = employeeBillingRatesViewModel.UploadFileName;
            contractFiles.FilePath = employeeBillingRatesViewModel.FilePath;
            contractFiles.FileSize = employeeBillingRatesViewModel.FileSize;
            contractFiles.IsFile = true;
            return contractFiles;
        }

        public static EmployeeBillingRatesViewModel MapContractFilesToEmployeeBillingRatesViewModel(ContractResourceFile contractFiles)
        {
            EmployeeBillingRatesViewModel employeeBillingRatesViewModel = new EmployeeBillingRatesViewModel();
            if (contractFiles == null)
                return null;
            employeeBillingRatesViewModel.ContractGuid = contractFiles.ResourceGuid;
            employeeBillingRatesViewModel.ContractResourceFileGuid = contractFiles.ContractResourceFileGuid;
            employeeBillingRatesViewModel.CreatedBy = contractFiles.CreatedBy;
            employeeBillingRatesViewModel.CreatedOn = contractFiles.CreatedOn;
            employeeBillingRatesViewModel.IsActive = contractFiles.IsActive;
            employeeBillingRatesViewModel.IsCsv = contractFiles.IsCsv;
            employeeBillingRatesViewModel.IsDeleted = contractFiles.IsDeleted;
            employeeBillingRatesViewModel.UpdatedBy = contractFiles.UpdatedBy;
            employeeBillingRatesViewModel.UpdatedOn = contractFiles.UpdatedOn;
            employeeBillingRatesViewModel.UploadFileName = contractFiles.UploadFileName;
            employeeBillingRatesViewModel.FilePath = contractFiles.FilePath;
            employeeBillingRatesViewModel.FileSize = contractFiles.FileSize;
            return employeeBillingRatesViewModel;
        }

        public static EmployeeBillingRates MapContractFilesViewModelToEmployeeBillingRates(ContractFileViewModel contractFileViewModel)
        {
            EmployeeBillingRates employeeBillingRates = new EmployeeBillingRates();

            employeeBillingRates.ContractGuid = contractFileViewModel.ResourceGuid;
            employeeBillingRates.CreatedBy = contractFileViewModel.CreatedBy;
            employeeBillingRates.CreatedOn = contractFileViewModel.CreatedOn;
            employeeBillingRates.IsActive = contractFileViewModel.IsActive;
            employeeBillingRates.IsCsv = contractFileViewModel.IsCsv;
            employeeBillingRates.IsDeleted = contractFileViewModel.IsDeleted;
            employeeBillingRates.UpdatedBy = contractFileViewModel.UpdatedBy;
            employeeBillingRates.UpdatedOn = contractFileViewModel.UpdatedOn;
            employeeBillingRates.UploadFileName = contractFileViewModel.UploadFileName;

            return employeeBillingRates;
        }

        public static ContractResourceFile MapSubcontractorBillingRatesViewModelToContractFiles(LaborCategoryRatesViewModel laborCategoryRatesViewModel)
        {
            ContractResourceFile contractFiles = new ContractResourceFile();

            contractFiles.ResourceGuid = laborCategoryRatesViewModel.ContractGuid;
            contractFiles.ContentResourceGuid = laborCategoryRatesViewModel.ContractGuid;
            contractFiles.CreatedBy = laborCategoryRatesViewModel.CreatedBy;
            contractFiles.CreatedOn = laborCategoryRatesViewModel.CreatedOn;
            contractFiles.Keys = Core.Entities.EnumGlobal.ResourceType.LaborCategoryRates.ToString();
            contractFiles.IsActive = laborCategoryRatesViewModel.IsActive;
            contractFiles.IsCsv = laborCategoryRatesViewModel.IsCsv;
            contractFiles.IsDeleted = laborCategoryRatesViewModel.IsDeleted;
            contractFiles.UpdatedBy = laborCategoryRatesViewModel.UpdatedBy;
            contractFiles.UpdatedOn = laborCategoryRatesViewModel.UpdatedOn;
            contractFiles.UploadFileName = laborCategoryRatesViewModel.UploadFileName;
            contractFiles.FilePath = laborCategoryRatesViewModel.FilePath;
            contractFiles.FileSize = laborCategoryRatesViewModel.FileSize;
            contractFiles.IsFile = true;
            return contractFiles;
        }

        public static LaborCategoryRatesViewModel MapContractFilesToSubcontractorBillingRatesViewModel(ContractResourceFile contractFiles)
        {
            LaborCategoryRatesViewModel laborCategoryRatesViewModel = new LaborCategoryRatesViewModel();
            if (contractFiles == null)
                return null;
            laborCategoryRatesViewModel.ContractGuid = contractFiles.ResourceGuid;
            laborCategoryRatesViewModel.ContractResourceFileGuid = contractFiles.ContractResourceFileGuid;
            laborCategoryRatesViewModel.CreatedBy = contractFiles.CreatedBy;
            laborCategoryRatesViewModel.CreatedOn = contractFiles.CreatedOn;
            laborCategoryRatesViewModel.IsActive = contractFiles.IsActive;
            laborCategoryRatesViewModel.IsCsv = contractFiles.IsCsv;
            laborCategoryRatesViewModel.IsDeleted = contractFiles.IsDeleted;
            laborCategoryRatesViewModel.UpdatedBy = contractFiles.UpdatedBy;
            laborCategoryRatesViewModel.UpdatedOn = contractFiles.UpdatedOn;
            laborCategoryRatesViewModel.UploadFileName = contractFiles.UploadFileName;
            laborCategoryRatesViewModel.FilePath = contractFiles.FilePath;
            laborCategoryRatesViewModel.FileSize = contractFiles.FileSize;
            return laborCategoryRatesViewModel;
        }

        public static LaborCategoryRates MapContractFilesToSubcontractorBillingRates(ContractFileViewModel contractFileViewModel)
        {
            LaborCategoryRates laborCategoryRates = new LaborCategoryRates();

            laborCategoryRates.ContractGuid = contractFileViewModel.ResourceGuid;
            laborCategoryRates.CreatedBy = contractFileViewModel.CreatedBy;
            laborCategoryRates.CreatedOn = contractFileViewModel.CreatedOn;
            laborCategoryRates.IsActive = contractFileViewModel.IsActive;
            laborCategoryRates.IsCsv = contractFileViewModel.IsCsv;
            laborCategoryRates.IsDeleted = contractFileViewModel.IsDeleted;
            laborCategoryRates.UpdatedBy = contractFileViewModel.UpdatedBy;
            laborCategoryRates.UpdatedOn = contractFileViewModel.UpdatedOn;
            laborCategoryRates.UploadFileName = contractFileViewModel.UploadFileName;

            return laborCategoryRates;
        }

    }
}
