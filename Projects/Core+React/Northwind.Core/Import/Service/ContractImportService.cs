using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Services;
using Northwind.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class ContractImportService : IContractImportService
    {
        private readonly IImportFileService _importFileService;
        private readonly IExportCSVService _exportCsvService;
        private readonly ICompanyService _companyService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly INaicsService _naicsService;
        private readonly IPscService _pscService;
        private readonly IResourceAttributeValueService _resourceAttributeValueService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerContactService _customerContactService;
        private readonly Core.Interfaces.ContractRefactor.IContractsService _contractsService;
        private readonly ICommonImportService _commonService;
        private readonly IFolderService _folderService;
        private readonly IFarContractTypeService _farContractTypeService; 
        
        private string inputFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private string errorLogPath = string.Empty;
        private string errorFile = "Contract.csv";
        private string fileNameToAppend = "Contract_Import_Log";
        private string rootPath = string.Empty;
        private string[] trueBooleanArray = { "1", "yes", "true", "y","active" };
        private string[] falseBooleanArray = { "0", "no", "false", "n","inactive" };
        private string costPlus = "costplus";
        private string firmFixedPrice = "FirmFixedPrice";
        Contracts contractEntity;

        public ContractImportService(IImportFileService importFileService,
            IExportCSVService exportCSVService, IContractsService contractsService,
            ICommonImportService commonService, ICompanyService companyService, ICountryService countryService, IStateService stateService,
            INaicsService naicsService, IPscService pscService, IResourceAttributeValueService resourceAttributeValueService,
            IUserService userService, ICustomerService customerService, ICustomerContactService customerContactService,IFolderService folderService,
            IFarContractTypeService farContractTypeService
            )
        {
            _importFileService = importFileService;
            _exportCsvService = exportCSVService;
            _contractsService = contractsService;
            _commonService = commonService;
            _companyService = companyService;
            _countryService = countryService;
            _stateService = stateService;
            _naicsService = naicsService;
            _pscService = pscService;
            _resourceAttributeValueService = resourceAttributeValueService;
            _userService = userService;
            _customerService = customerService;
            _customerContactService = customerContactService;
            _folderService = folderService;
            _farContractTypeService = farContractTypeService;
        }

        //get import configuration file
        private ImportConfiguration GetConfigurationSetting(string jsonConfigData)
        {
            var config = new ImportConfiguration();
            config = JsonConvert.DeserializeObject<ImportConfiguration>(jsonConfigData);
            return config;
        }

        //insert contract to db
        private bool InsertContractToDb(DMContract contract, Guid userGuid)
        {
            var entity = MapContractToCoreMContract(contract, userGuid);
            _contractsService.Save(entity);
            return true;
        }

        //update contract to db
        private bool UpdateContractToDb(Contracts contract)
        {
            _contractsService.Save(contract);
            return true;
        }

        //enable/disable contract based on contract guid
        private bool EnableDisableContract(bool isEnable, Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = contractGuid;

            if (isEnable)
                _contractsService.Enable(guid);
            else
                _contractsService.Disable(guid);
            return true;
        }

        //delete contract based on contract guid
        private bool DeleteContract(Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = contractGuid;
            _contractsService.Delete(guid);
            return false;
        }

        //map data migration contract model to core entity contract model
        private Contracts MapContractToCoreMContract(DMContract contract, Guid userGuid)
        {
            if (string.IsNullOrWhiteSpace(contract.AwardAmount))
                contract.AwardAmount = "0";
            if (string.IsNullOrWhiteSpace(contract.FundingAmount))
                contract.FundingAmount = "0";
            if (string.IsNullOrWhiteSpace(contract.POPStart))
                contract.POPStart = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(contract.POPEnd))
                contract.POPEnd = DateTime.UtcNow.ToShortDateString();

            contractEntity = new Contracts();
            contractEntity.ContractGuid = contract.ContractGuid;
            contractEntity.ProjectNumber = contract.ProjectNumber;
            contractEntity.ContractNumber = contract.ContractNumber;

            contractEntity.IsIDIQContract = trueBooleanArray.Contains(contract.IsIDIQContract.ToLower());
            contractEntity.IsPrimeContract = trueBooleanArray.Contains(contract.IsPrimeContract.ToLower());
            contractEntity.SubContractNumber = contract.SubContractNumber;
            contractEntity.ORGID = contract.ORGID;
            contractEntity.ContractTitle = contract.ContractTitle;
            contractEntity.Description = contract.Description;
            if (!string.IsNullOrEmpty(contract.AwardAmount))
                contractEntity.AwardAmount = Decimal.Parse(contract.AwardAmount);
            if (!string.IsNullOrWhiteSpace(contract.FundingAmount))
                contractEntity.FundingAmount = Decimal.Parse(contract.FundingAmount);
            contractEntity.POPStart = DateTime.Parse(contract.POPStart);
            contractEntity.POPEnd = DateTime.Parse(contract.POPEnd);
            contractEntity.CountryOfPerformance = contract.CountryOfPerformance;
            contractEntity.PlaceOfPerformance = contract.ContractState;
            contractEntity.NAICSCode = contract.NAICSCode;
            contractEntity.PSCCode = contract.PSCCode;
            if (!string.IsNullOrWhiteSpace(contract.CPAREligible))
                contractEntity.CPAREligible = trueBooleanArray.Contains(contract.CPAREligible.ToLower());
            contractEntity.QualityLevel = contract.QualityLevelValue;
            if (!string.IsNullOrWhiteSpace(contract.QualityLevel))
                contractEntity.QualityLevelRequirements = true;
            contractEntity.Competition = contract.CompetitionValue;
            contractEntity.ContractType = contract.ContractTypeValue;
            contractEntity.Currency = contract.CurrencyValue;
            contractEntity.BillingAddress = contract.BillingAddress;
            contractEntity.BillingFrequency = contract.BillingFrequencyValue;
            if (!string.IsNullOrWhiteSpace(contract.OverHead))
                contractEntity.OverHead = Decimal.Parse(contract.OverHead);
            if (!string.IsNullOrWhiteSpace(contract.GAPercent))
                contractEntity.GAPercent = Decimal.Parse(contract.GAPercent);
            if (!string.IsNullOrWhiteSpace(contract.FeePercent))
                contractEntity.FeePercent = Decimal.Parse(contract.FeePercent);
            contractEntity.InvoiceSubmissionMethod = contract.InvoiceSubmissionMethodValue;
            contractEntity.PaymentTerms = contract.PaymentTermValue;
            contractEntity.ApplicableWageDetermination = contract.ApplicableWageDeterminationValue;
            contractEntity.BillingFormula = contract.BillingFormulaValue;
            contractEntity.RevenueFormula = contract.RevenueFormulaValue;
            if (!string.IsNullOrWhiteSpace(contract.RevenueRecognitionEACPercent))
                contractEntity.RevenueRecognitionEACPercent = Decimal.Parse(contract.RevenueRecognitionEACPercent);
            contractEntity.OHonsite = contract.OHonsite;
            contractEntity.OHoffsite = contract.OHoffsite;
            contractEntity.AwardingAgencyOffice = contract.AwardingAgencyOffice;
            contractEntity.OfficeContractRepresentative = contract.OfficeContractRepresentative;
            contractEntity.OfficeContractTechnicalRepresent = contract.OfficeContractTechnicalRepresent;
            contractEntity.FundingAgencyOffice = contract.FundingAgencyOffice;
            contractEntity.FundingOfficeContractRepresentative = contract.FundingOfficeContractRepresentative;
            contractEntity.FundingOfficeContractTechnicalRepresent = contract.FundingOfficeContractTechnicalRepresent;
            contractEntity.PlaceOfPerformanceSelectedIds = contract.PlaceOfPerformance;
            contractEntity.Status = contract.Status;
            contractEntity.SetAside = contract.SetAside;
            if (contractEntity.ContractGuid == Guid.Empty)
            {
                contractEntity.CreatedOn = DateTime.UtcNow;
                contractEntity.CreatedBy = userGuid;
            }
            contractEntity.IsDeleted = false;
            contractEntity.IsActive = true;
            contractEntity.UpdatedOn = DateTime.UtcNow;
            contractEntity.UpdatedBy = userGuid;
            if (contract.ParentContractGuid == Guid.Empty)
                contract.ParentContractGuid = null;
            contractEntity.ParentContractGuid = contract.ParentContractGuid;
            contractEntity.IsImported = true;
            if(!string.IsNullOrWhiteSpace(contract.MasterTaskNodeID))
                contractEntity.MasterTaskNodeID = Int32.Parse(contract.MasterTaskNodeID);
            if(!string.IsNullOrWhiteSpace(contract.TaskNodeID))
                contractEntity.TaskNodeID = Int32.Parse(contract.TaskNodeID);

            var contractUserRoleList = new List<ContractUserRole>();
            if (contract.AccountRepresentativeGuid != Guid.Empty)
            {
                var accountRepresentative = new ContractUserRole();
                accountRepresentative.UserGuid = contract.AccountRepresentativeGuid;
                accountRepresentative.UserRole = Contracts._accountRepresentative;
                contractUserRoleList.Add(accountRepresentative);
            }
            if (contract.CompanyPresidentGuid != Guid.Empty)
            {
                var president = new ContractUserRole();
                president.UserGuid = contract.CompanyPresidentGuid;
                president.UserRole = Contracts._companyPresident;
                contractUserRoleList.Add(president);
            }

            if (contract.ProjectManagerGuid != Guid.Empty)
            {
                var projectManager = new ContractUserRole();
                projectManager.UserGuid = contract.ProjectManagerGuid;
                projectManager.UserRole = Contracts._projectManager;
                contractUserRoleList.Add(projectManager);
            }

            if (contract.RegionalManagerGuid != Guid.Empty)
            {
                var regionalManager = new ContractUserRole();
                regionalManager.UserGuid = contract.ProjectManagerGuid;
                regionalManager.UserRole = Contracts._regionalManager;
                contractUserRoleList.Add(regionalManager);
            }

            if (contract.ProjectControlsGuid != Guid.Empty)
            {
                var projectControls = new ContractUserRole();
                projectControls.UserGuid = contract.ProjectControlsGuid;
                projectControls.UserRole = Contracts._projectControls;
                contractUserRoleList.Add(projectControls);
            }

            if (contract.ContractRepresentativeGuid != Guid.Empty)
            {
                var contractRepresentative = new ContractUserRole();
                contractRepresentative.UserGuid = contract.ContractRepresentativeGuid;
                contractRepresentative.UserRole = Contracts._contractRepresentative;
                contractUserRoleList.Add(contractRepresentative);
            }

            if (contract.SubContractAdministratorGuid != Guid.Empty)
            {
                var subContractAdministrator = new ContractUserRole();
                subContractAdministrator.UserGuid = contract.SubContractAdministratorGuid;
                subContractAdministrator.UserRole = Contracts._subContractAdministrator;
                contractUserRoleList.Add(subContractAdministrator);
            }

            if (contract.PurchasingRepresentativeGuid != Guid.Empty)
            {
                var purchasingRepresentative = new ContractUserRole();
                purchasingRepresentative.UserGuid = contract.PurchasingRepresentativeGuid;
                purchasingRepresentative.UserRole = Contracts._purchasingRepresentative;
                contractUserRoleList.Add(purchasingRepresentative);
            }

            if (contract.HumanResourceRepresentativeGuid != Guid.Empty)
            {
                var humanResourceRepresentative = new ContractUserRole();
                humanResourceRepresentative.UserGuid = contract.HumanResourceRepresentativeGuid;
                humanResourceRepresentative.UserRole = Contracts._humanResourceRepresentative;
                contractUserRoleList.Add(humanResourceRepresentative);
            }

            if (contract.QualityRepresentativeGuid != Guid.Empty)
            {
                var qualityRepresentative = new ContractUserRole();
                qualityRepresentative.UserGuid = contract.QualityRepresentativeGuid;
                qualityRepresentative.UserRole = Contracts._qualityRepresentative;
                contractUserRoleList.Add(qualityRepresentative);
            }

            if (contract.SafetyOfficerGuid != Guid.Empty)
            {
                var safetyOfficer = new ContractUserRole();
                safetyOfficer.UserGuid = contract.SafetyOfficerGuid;
                safetyOfficer.UserRole = Contracts._safetyOfficer;
                contractUserRoleList.Add(safetyOfficer);
            }

            contractEntity.ContractUserRole = contractUserRoleList;

            //for default far contract type
            var farContractType = _farContractTypeService.GetByCode("other");
            if (farContractType != null)
                contractEntity.FarContractTypeGuid = farContractType.FarContractTypeGuid;

            return contractEntity;
        }

        private ContractModel MapContractToModel(DMContract contract)
        {
            var model = new ContractModel();
            model.ContractNumber = "123";
            model.ContractType = contract.ContractType;
            //if (Boolean.Parse(contract.IsTaskOrder))
            //    model.ContractNumber = contract.TaskOrderNumber;
            //model.IsIDIQ = Boolean.Parse(contract.IsIDIQ);
            //model.ProjectNumber = contract.ProjectNumber;
            //model.IsTaskOrder = Boolean.Parse(contract.IsTaskOrder);

            return model;
        }

        private List<ContractUserRole> GetContractUserRole(DMContract contract)
        {
            var contractUserRoleList = new List<ContractUserRole>();
            if (contract.AccountRepresentativeGuid != Guid.Empty)
            {
                var accountRepresentative = new ContractUserRole();
                accountRepresentative.UserGuid = contract.AccountRepresentativeGuid;
                accountRepresentative.UserRole = Contracts._accountRepresentative;
                contractUserRoleList.Add(accountRepresentative);
            }
            if (contract.CompanyPresidentGuid != Guid.Empty)
            {
                var president = new ContractUserRole();
                president.UserGuid = contract.CompanyPresidentGuid;
                president.UserRole = Contracts._companyPresident;
                contractUserRoleList.Add(president);
            }

            if (contract.ProjectManagerGuid != Guid.Empty)
            {
                var projectManager = new ContractUserRole();
                projectManager.UserGuid = contract.ProjectManagerGuid;
                projectManager.UserRole = Contracts._projectManager;
                contractUserRoleList.Add(projectManager);
            }

            if (contract.RegionalManagerGuid != Guid.Empty)
            {
                var regionalManager = new ContractUserRole();
                regionalManager.UserGuid = contract.ProjectManagerGuid;
                regionalManager.UserRole = Contracts._regionalManager;
                contractUserRoleList.Add(regionalManager);
            }

            if (contract.ProjectControlsGuid != Guid.Empty)
            {
                var projectControls = new ContractUserRole();
                projectControls.UserGuid = contract.ProjectControlsGuid;
                projectControls.UserRole = Contracts._projectControls;
                contractUserRoleList.Add(projectControls);
            }

            if (contract.ContractRepresentativeGuid != Guid.Empty)
            {
                var contractRepresentative = new ContractUserRole();
                contractRepresentative.UserGuid = contract.ContractRepresentativeGuid;
                contractRepresentative.UserRole = Contracts._contractRepresentative;
                contractUserRoleList.Add(contractRepresentative);
            }

            if (contract.SubContractAdministratorGuid != Guid.Empty)
            {
                var subContractAdministrator = new ContractUserRole();
                subContractAdministrator.UserGuid = contract.SubContractAdministratorGuid;
                subContractAdministrator.UserRole = Contracts._subContractAdministrator;
                contractUserRoleList.Add(subContractAdministrator);
            }

            if (contract.PurchasingRepresentativeGuid != Guid.Empty)
            {
                var purchasingRepresentative = new ContractUserRole();
                purchasingRepresentative.UserGuid = contract.PurchasingRepresentativeGuid;
                purchasingRepresentative.UserRole = Contracts._purchasingRepresentative;
                contractUserRoleList.Add(purchasingRepresentative);
            }

            if (contract.HumanResourceRepresentativeGuid != Guid.Empty)
            {
                var humanResourceRepresentative = new ContractUserRole();
                humanResourceRepresentative.UserGuid = contract.HumanResourceRepresentativeGuid;
                humanResourceRepresentative.UserRole = Contracts._humanResourceRepresentative;
                contractUserRoleList.Add(humanResourceRepresentative);
            }

            if (contract.QualityRepresentativeGuid != Guid.Empty)
            {
                var qualityRepresentative = new ContractUserRole();
                qualityRepresentative.UserGuid = contract.QualityRepresentativeGuid;
                qualityRepresentative.UserRole = Contracts._qualityRepresentative;
                contractUserRoleList.Add(qualityRepresentative);
            }

            if (contract.SafetyOfficerGuid != Guid.Empty)
            {
                var safetyOfficer = new ContractUserRole();
                safetyOfficer.UserGuid = contract.SafetyOfficerGuid;
                safetyOfficer.UserRole = Contracts._safetyOfficer;
                contractUserRoleList.Add(safetyOfficer);
            }

            return contractUserRoleList;
        }

        //Get contract list from csv file
        private List<DMContract> GetContractListFromCsvFile<DMContract>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    var mapper = new ContractHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var modsRecord = csv.GetRecords<DMContract>().ToList();
                    reader.Close();
                    return modsRecord;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                _exportCsvService.ErrorLog(this.errorFile, filePath, this.errorFile, errorMsg);
                return null;
            }

        }


        //enable or disable or delete contract
        private DMContract EnableDisableDeleteContract(DMContract contract, Guid contractGuid)
        {
            var action = contract.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteContract(contractGuid);
                contract.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableContract(true, contractGuid);
                contract.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableContract(false, contractGuid);
                contract.Reason = "Disable Successfully";
            }

            contract.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return contract;
        }

        //validation to delete, enable and disable contract
        private DMContract InputValidationForEnableDisableDelete(DMContract contract)
        {
            contract.IsValid = false;
            var dataToValidateMandatory = new
            {
                ContractNumber = contract.ContractNumber,
            };
            var mandatoryValidation = ValidationHelper.NullValidation(dataToValidateMandatory);
            if (!mandatoryValidation.IsValid)
            {
                contract.IsValid = mandatoryValidation.IsValid;
                contract.Reason = mandatoryValidation.Message;
                contract.ImportStatus = ImportStatus.Fail.ToString();
                return contract;
            }
            contract.IsValid = true;
            return contract;
        }

        //validate input data provided in csv file
        private DMContract ContractInputValidation(DMContract contract)
        {
            contract.IsValid = true;

            if (string.IsNullOrWhiteSpace(contract.AwardAmount))
                contract.AwardAmount = "0";
            if (string.IsNullOrWhiteSpace(contract.FundingAmount))
                contract.FundingAmount = "0";
            if (string.IsNullOrWhiteSpace(contract.POPStart))
                contract.POPStart = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(contract.POPEnd))
                contract.POPEnd = DateTime.UtcNow.ToShortDateString();
            //object for validating mandatory field
            var dataToValidateMandatory = new
            {
                //ContractNumber = contract.ContractNumber,
                TaskNodeID = contract.TaskNodeID,
                ProjectNumber = contract.ProjectNumber,
                IsIDIQ = contract.IsIDIQContract,
                IsPrime = contract.IsPrimeContract,
                Title = contract.ContractTitle,
                //OrgID = contract.OrganisationID,
                POPStart = contract.POPStart,
                POPEnd = contract.POPEnd,
                AwardAmount = contract.AwardAmount,
                FundedAmount = contract.FundingAmount,
            };
            var mandatoryValidation = ValidationHelper.NullValidation(dataToValidateMandatory);
            if (!mandatoryValidation.IsValid)
            {
                contract.IsValid = mandatoryValidation.IsValid;
                contract.Reason += mandatoryValidation.Message;
                contract.ImportStatus = ImportStatus.Fail.ToString();
                //return contract;
            }

            //object for decimal validation
            var dataToValidateDecimal = new
            {
                AwardAmount = contract.AwardAmount,
                FundedAmount = contract.FundingAmount,
                OverHeadFeePercent = contract.OverHead,
                GAFeePercent = contract.GAPercent,
                FeePercent = contract.FeePercent,
                RevenueRecognitionPercent = contract.RevenueRecognitionEACPercent,
                MasterTaskNodeID = contract.MasterTaskNodeID,
                TaskNodeID = contract.TaskNodeID
            };
            var decimalValidation = ValidationHelper.DecimalValidation(dataToValidateDecimal);
            if (!decimalValidation.IsValid)
            {
                contract.IsValid = decimalValidation.IsValid;
                contract.Reason += decimalValidation.Message;
                contract.ImportStatus = ImportStatus.Fail.ToString();
                //return contract;
            }

            //object for date validation
            var dataToValidateDate = new
            {
                POPStart = contract.POPStart,
                POPEnd = contract.POPEnd
            };
            var dateValidation = ValidationHelper.DateValidation(dataToValidateDate);
            if (!dateValidation.IsValid)
            {
                contract.IsValid = dateValidation.IsValid;
                contract.Reason += dateValidation.Message;
                contract.ImportStatus = ImportStatus.Fail.ToString();
                //return contract;
            }
            return contract;
        }

        //validate data logically and against db
        private DMContract ContractDataValidation(DMContract contract)
        {
            contract.IsValid = true;
            contract.ImportStatus = ImportStatus.Fail.ToString();

            //if(contract.ContractType == "")

            //start of conditional validation based on value
            if (!string.IsNullOrWhiteSpace(contract.IsTaskOrder) && trueBooleanArray.Contains(contract.IsTaskOrder.ToLower())
                && !string.IsNullOrWhiteSpace(contract.MasterTaskNodeID))
            {
                int masterTaskID;
                if (int.TryParse(contract.MasterTaskNodeID, out masterTaskID))
                {
                    var parentContractGuid = _contractsService.GetParentContractByMasterTaskNodeID(Int32.Parse(contract.MasterTaskNodeID.Trim()));
                    if (parentContractGuid == null || parentContractGuid == Guid.Empty)
                    {
                        contract.Reason += "The parent contract doesn't exist.";
                        contract.IsValid = false;
                        //return contract;
                    }
                    else
                    {
                        contract.ParentContractGuid = parentContractGuid;
                        contract.ContractNumber = contract.TaskOrderNumber;
                        contract.IsIDIQContract = "0";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(contract.IsPrimeContract) && falseBooleanArray.Contains(contract.IsPrimeContract.ToLower())
                && !string.IsNullOrWhiteSpace(contract.IsTaskOrder) && trueBooleanArray.Contains(contract.IsTaskOrder.ToLower()) && string.IsNullOrWhiteSpace(contract.SubContractNumber))
            {
                contract.Reason += "Subcontract Number is required.";
                contract.IsPartialValid = false;
                //return contract;
            }
            //end of conditional validaion

            if (string.IsNullOrWhiteSpace(contract.ContractNumber))
            {
                contract.Reason += "Contract number is empty.";
                contract.IsPartialValid = false;
            }

            if (!string.IsNullOrWhiteSpace(contract.OrganisationID))
            {
                var organization = _companyService.GetOrganizationByName(contract.OrganisationID.Trim());
                if (organization == null)
                {
                    contract.Reason += "Invalid OrgID.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                {
                    contract.ORGID = organization.OrgIDGuid;
                }
            }
            else
            {
                contract.Reason += "OrgID is empty.";
                contract.IsPartialValid = false;
            }
            

            //var org = contract.OrgID.Split('.');
            //var companyCode = org[0];
            //var regionCode = org[1];
            //var officeCode = org[2].Trim();

            if (!string.IsNullOrWhiteSpace(contract.ContractCountry))
            {
                var countryGuid = _countryService.GetCountryGuidBy3DigitCode(contract.ContractCountry.Trim());
                if (countryGuid == null)
                {
                    contract.Reason += "Invalid Country.";
                    contract.IsPartialValid = false;
                    contract.CountryOfPerformance = Guid.Empty;
                    //return contract;
                }
                else
                    contract.CountryOfPerformance = countryGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.NAICSCodeValue))
            {
                if (contract.NAICSCodeValue.ToString() == "none")
                    contract.NAICSCode = Guid.Empty;
                var naics = _naicsService.GetNaicsByCode(contract.NAICSCodeValue.Trim());
                if (naics == null)
                {
                    contract.Reason += "Invalid NAICS Code.";
                    contract.IsPartialValid = false;
                    contract.NAICSCode = Guid.Empty;
                }
                else
                    contract.NAICSCode = naics.NAICSGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractState))
            {
                var stateList = contract.ContractState.Trim().Split(',').ToList();
                var stateGuid = string.Empty;
                foreach (var state in stateList)
                {
                    var dbState = _stateService.GetStateByAbbreviations(state.Trim());
                    if (dbState == null)
                    {
                        contract.Reason += "Invalid State: " + state + ".";
                        contract.IsPartialValid = false;
                        contract.PlaceOfPerformance = string.Empty;
                    }
                    else
                        stateGuid += dbState.StateId + ",";
                }
                contract.PlaceOfPerformance = stateGuid.TrimEnd(',');
            }

            if (!string.IsNullOrWhiteSpace(contract.PSCCodeValue))
            {
                var psc = _pscService.GetPSCDetailByCode(contract.PSCCodeValue.Trim());
                if (psc == null)
                {
                    contract.Reason += "Invalid PSC Code.";
                    contract.IsPartialValid = false;
                    contract.PSCCode = Guid.Empty;
                    //return contract;
                }
                else
                    contract.PSCCode = psc.PSCGuid;
            }

            //attribute validation
            var validateAttribute = ValidateAttributeValue(contract);
            var isAttributeValid = validateAttribute.IsValid;
            //if (validateAttribute.IsValid == false)
            //    return validateAttribute;
            //contract = validateAttribute;

            contract.Reason += validateAttribute.Reason;

            //key personal validation
            var validateKeyPersonnal = ValidateKeyPersonal(contract);
            var isKeyPersonnalValid = validateKeyPersonnal.IsValid;
            contract = validateKeyPersonnal;

            if (!isAttributeValid || !isKeyPersonnalValid)
                contract.IsValid = false;

            if (!string.IsNullOrWhiteSpace(contract.AwardingAgency))
            {
                var awardingAgency = _customerService.GetCustomerByName(contract.AwardingAgency.Trim());
                if (awardingAgency == null)
                {
                    contract.Reason += "Invalid awarding agency.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                {
                    contract.AwardingAgencyOffice = awardingAgency.CustomerGuid;
                    var contractRep = contract.AwardAgencyContractRepresentative.Trim().Split(' ');
                    var firstName = contractRep.First();
                    var lastName = contractRep.Last();
                    var awardContractRepresentative = _customerContactService.GetCustomerContactByName(firstName, lastName, awardingAgency.CustomerGuid);
                    if (awardContractRepresentative == null)
                    {
                        contract.Reason += "Invalid award contract representative.";
                        contract.IsPartialValid = false;
                        //return contract;
                    }
                    else
                        contract.OfficeContractRepresentative = awardContractRepresentative.ContactGuid;

                    var technicalContractRep = contract.AwardingAgencyTechnicalRepresentative.Trim().Split(' ');
                    var techFirstName = technicalContractRep.First();
                    var techLastName = technicalContractRep.Last();
                    var awardTechnicalRepresentative = _customerContactService.GetCustomerContactByEmail(contract.AwardingAgencyTechnicalRepresentative.Trim(), awardingAgency.CustomerGuid);
                    if (awardTechnicalRepresentative == null)
                    {
                        contract.Reason = "Invalid award technical representative.";
                        contract.IsPartialValid = false;
                        //return contract;
                    }
                    else
                        contract.OfficeContractTechnicalRepresent = awardTechnicalRepresentative.ContactGuid;
                }
            }

            if (!string.IsNullOrWhiteSpace(contract.FundingAgency))
            {
                var fundingAgency = _customerService.GetCustomerByName(contract.FundingAgency);
                if (fundingAgency == null)
                {
                    contract.Reason += "Invalid funding agency.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                {
                    contract.FundingAgencyOffice = fundingAgency.CustomerGuid;
                    var contractRep = contract.FundingAgencyContractRepresentative.Trim().Split(' ');
                    var firstName = contractRep.First();
                    var lastName = contractRep.Last();
                    var fundingContractRepresentative = _customerContactService.GetCustomerContactByEmail(contract.FundingAgencyContractRepresentative, fundingAgency.CustomerGuid);
                    if (fundingContractRepresentative == null)
                    {
                        contract.Reason += "Invalid funding contract representative.";
                        contract.IsPartialValid = false;
                        //return contract;
                    }
                    else
                    {
                        contract.FundingOfficeContractRepresentative = fundingContractRepresentative.ContactGuid;
                    }


                    var technicalContractRep = contract.FundingAgencyTechnicalRepresentative.Trim().Split(' ');
                    var techFirstName = technicalContractRep.First();
                    var techLastName = technicalContractRep.Last();
                    var fundingTechnicalRepresentative = _customerContactService.GetCustomerContactByEmail(contract.FundingAgencyTechnicalRepresentative, fundingAgency.CustomerGuid);
                    if (fundingTechnicalRepresentative == null)
                    {
                        contract.Reason += "Invalid funding technical representative.";
                        contract.IsPartialValid = false;
                        //return contract;
                    }
                    else
                        contract.FundingOfficeContractTechnicalRepresent = fundingTechnicalRepresentative.ContactGuid;
                }
            }

            return contract;
        }

        private string GetStateGuid(string stateString)
        {
            var stateList = stateString.Trim().Split(',').ToList();
            var stateGuid = string.Empty;
            foreach (var state in stateList)
            {
                var dbState = _stateService.GetStateByAbbreviations(state.Trim());
                if (dbState != null)
                {
                    stateGuid += dbState.StateId + ",";
                }
            }
            stateGuid = stateGuid.TrimEnd(',');
            return stateGuid;
        }

        private DMContract DataValidationForUpdate(DMContract contract)
        {
            contract.IsValid = true;
            var contractUserRole = new List<ContractUserRole>();
           
            var contractObject = ContractHelper.CreateDataObjectFromObject(contract);
            foreach (var item in contractObject)
            {
                switch (item.Key)
                {
                    case "POPStart":
                    case "POPEnd":
                        var isValidDate = ValidationHelper.IsValidDate(item.Value);
                        if (!isValidDate)
                        {
                            contract.IsValid = false;
                            contract.Reason += item.Key + " is not valid.";
                        }
                        break;
                    case "AwardAmount":
                    case "FundingAmount":
                    case "OverHead":
                    case "GAPercent":
                    case "FeePercent":
                    case "RevenueRecognitionEACPercent":
                        var isValidaNumeric = ValidationHelper.IsValidDecimal(item.Value);
                        if (!isValidaNumeric)
                        {
                            contract.IsValid = false;
                            contract.Reason += item.Key + " is not valid.";
                        }
                        break;
                    case "ContractCountry":
                        var country = _countryService.GetCountryGuidBy3DigitCode(contract.ContractCountry.Trim());
                        if (country != null && country != Guid.Empty)
                        {
                            contract.CountryOfPerformance = country;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Country.";
                        }
                        break;
                    case "ContractState":
                        var stateGuid = GetStateGuid(contract.ContractState);
                        if (!string.IsNullOrWhiteSpace(stateGuid))
                        {
                            contract.PlaceOfPerformance = stateGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid State.";
                        }
                        break;
                    case "NAICSCodeValue":
                        var naics = _naicsService.GetNaicsByCode(contract.NAICSCodeValue.Trim());
                        if (naics != null)
                        {
                            contract.NAICSCode = naics.NAICSGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid NAICS Code.";
                        }
                        break;
                    case "PSCCodeValue":
                        var psc = _pscService.GetPscByCode(contract.PSCCodeValue.Trim());
                        if (psc.Count() > 0)
                        {
                            contract.PSCCode = psc.FirstOrDefault().PSCGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid PSC code.";
                        }
                        break;
                    case "OrganisationID":
                        var orgID = _companyService.GetOrganizationByName(contract.OrganisationID.Trim());
                        if (orgID != null)
                        {
                            contract.ORGID = orgID.OrgIDGuid;
                        }
                        else
                        {
                            contract.IsValid = false;
                            contract.Reason += "Invalid ORGID.";
                        }
                        break;
                    case "QualityLevel":
                        if (contract.QualityLevel == "1")
                            contract.QualityLevel = "Quality Level 1";
                        else if (contract.QualityLevel == "2")
                            contract.QualityLevel = "Quality Level 2";
                        var qualityLevel = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.QualityLevel.ToString(), contract.QualityLevel.Trim());
                        if (qualityLevel != null)
                        {
                            contract.QualityLevel = qualityLevel.Value;
                            contract.QualityLevelRequirements = true;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Quality Level.";
                        }
                        break;
                    case "Competition":
                        if (contract.Competition == "Non-Competed")
                            contract.Competition = "Non Competed";
                        var competition = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.Competition.ToString(), contract.Competition.Trim());
                        if (competition != null)
                        {
                            contract.Competition = competition.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Competition.";
                        }
                        break;
                    case "ContractType":
                        var contractType = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.ContractType.ToString(), contract.ContractType.Trim());
                        if (contractType != null)
                        {
                            contract.ContractType = contractType.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Contract Type.";
                        }
                        break;
                    case "InvoiceSubmissionMethod":
                        var invoiceMethod = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.InvoiceSubmissionMethod.ToString(), contract.InvoiceSubmissionMethod.Trim());
                        if (invoiceMethod != null)
                        {
                            contract.InvoiceSubmissionMethod = invoiceMethod.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Invoice Submission Method.";
                        }
                        break;
                    case "PaymentTerms":
                        var paymentTerms = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.PaymentTerms.ToString(), contract.PaymentTerms.Trim());
                        if (paymentTerms != null)
                        {
                            contract.PaymentTerms = paymentTerms.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Payment Terms.";
                        }
                        break;
                    case "ApplicableWageDetermination":
                        var wage = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.ApplicableWageDetemination.ToString(), contract.ApplicableWageDetermination.Trim());
                        if (wage != null)
                        {
                            contract.ApplicableWageDetermination = wage.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Applicable Wage Determination.";
                        }
                        break;
                    case "BillingFormula":
                        var billingFormula = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.BillingFormula.ToString(), contract.BillingFormula.Trim());
                        if (billingFormula != null)
                        {
                            contract.BillingFormula = billingFormula.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Billing Formula.";
                        }
                        break;
                    case "RevenueFormula":
                        var revenueFormula = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.RevenueFormula.ToString(), contract.RevenueFormula.Trim());
                        if (revenueFormula != null)
                        {
                            contract.RevenueFormula = revenueFormula.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Revenue Formula.";
                        }
                        break;
                    case "BillingFrequency":
                        var billingFrequency = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.BillingFrequency.ToString(), contract.BillingFrequency.Trim());
                        if (billingFrequency != null)
                        {
                            contract.BillingFrequency = billingFrequency.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Billing Frequency.";
                        }
                        break;
                    case "Currency":
                        if (string.IsNullOrWhiteSpace(contract.Currency))
                            contract.Currency = "USD";
                        var currency = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.Currency.ToString(), contract.Currency.Trim());
                        if (currency != null)
                        {
                            contract.Currency = currency.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Currency.";
                        }
                        break;
                    case "SetAside":
                        var setASide = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.SetAside.ToString(), contract.Currency.Trim());
                        if (setASide != null)
                        {
                            contract.SetAside = setASide.Value;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Acquisition Type.";
                        }
                        break;
                    case "ContractRepresentatives":
                        var contractRep = _userService.GetUserByWorkEmail(contract.ContractRepresentatives.Trim());
                        if (contractRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._contractRepresentative,
                                UserGuid = contractRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Contract Representative.";
                        }

                        break;
                    case "ProjectControlRepresentative":
                        var projectRep = _userService.GetUserByWorkEmail(contract.ContractRepresentatives.Trim());
                        if (projectRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._projectControls,
                                UserGuid = projectRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Project Controls.";
                        }
                        break;
                    case "ContractProjectManager":
                        var projectManager = _userService.GetUserByWorkEmail(contract.ContractRepresentatives.Trim());
                        if (projectManager != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._projectManager,
                                UserGuid = projectManager.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Project Manager";
                        }
                        break;
                    case "AccountingRepresentative":
                        var accountRep = _userService.GetUserByWorkEmail(contract.ContractRepresentatives.Trim());
                        if (accountRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._accountRepresentative,
                                UserGuid = accountRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Account Representative";
                        }
                        break;
                    case "ContractSubContractAdministrator":
                        var contractAdministrator = _userService.GetUserByWorkEmail(contract.ContractSubContractAdministrator.Trim());
                        if (contractAdministrator != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._subContractAdministrator,
                                UserGuid = contractAdministrator.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Subcontract Administrator.";
                        }
                        break;
                    case "ContractPurchasingRepresentative":
                        var purchasingRep = _userService.GetUserByWorkEmail(contract.ContractPurchasingRepresentative.Trim());
                        if (purchasingRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._purchasingRepresentative,
                                UserGuid = purchasingRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Purchasing Representative.";
                        }
                        break;
                    case "ContractHumanResourceRepresentative":
                        var humanRep = _userService.GetUserByWorkEmail(contract.ContractHumanResourceRepresentative.Trim());
                        if (humanRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._humanResourceRepresentative,
                                UserGuid = humanRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Human Resource Representative.";
                        }
                        break;
                    case "ContractQualityRepresentative":
                        var qualityRep = _userService.GetUserByWorkEmail(contract.ContractQualityRepresentative.Trim());
                        if (qualityRep != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._qualityRepresentative,
                                UserGuid = qualityRep.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Quality Representative.";
                        }
                        break;
                    case "ContractSafetyOfficer":
                        var safetyOfficer = _userService.GetUserByWorkEmail(contract.ContractSafetyOfficer.Trim());
                        if (safetyOfficer != null)
                        {
                            var userRole = new ContractUserRole
                            {
                                UserRole = Contracts._safetyOfficer,
                                UserGuid = safetyOfficer.UserGuid,
                                ContractGuid = contract.ContractGuid
                            };
                            contractUserRole.Add(userRole);
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Safety Officer.";
                        }
                        break;
                    case "AwardingAgency":
                        var awardAgency = _customerService.GetCustomerByName(contract.AwardingAgency.Trim());
                        if (awardAgency != null)
                        {
                            contract.AwardingAgencyOffice = awardAgency.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Agarding Agency";
                        }
                        break;
                    case "AwardAgencyContractRepresentative":
                        var awardContractRep = contract.AwardAgencyContractRepresentative.Trim().Split(' ');
                        var firstName = awardContractRep.First();
                        var lastName = awardContractRep.Last();
                        var awardRepresentative = _customerContactService.GetCustomerContactByName(firstName, lastName, contract.AwardingAgencyOffice);
                        if (awardRepresentative != null)
                        {
                            contract.OfficeContractRepresentative = awardRepresentative.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Award Agency Contract Representative";
                        }
                        break;
                    case "AwardingAgencyTechnicalRepresentative":
                        var technicalContractRep = contract.AwardingAgencyTechnicalRepresentative.Trim().Split(' ');
                        var techFirstName = technicalContractRep.First();
                        var techLastName = technicalContractRep.Last();
                        var awardTechnicalRepresentative = _customerContactService.GetCustomerContactByName(techFirstName, techLastName, contract.AwardingAgencyOffice);
                        if (awardTechnicalRepresentative != null)
                        {
                            contract.OfficeContractTechnicalRepresent = awardTechnicalRepresentative.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Award Technical Representative";
                        }
                        break;
                    case "FundingAgency":
                        var fundingAgency = _customerService.GetCustomerByName(contract.FundingAgency.Trim());
                        if (fundingAgency != null)
                        {
                            contract.FundingAgencyOffice = fundingAgency.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Funding Agency";
                        }
                        break;
                    case "FundingAgencyContractRepresentative":
                        var fundingContractRep = contract.FundingAgencyContractRepresentative.Trim().Split(' ');
                        var agencyFirstName = fundingContractRep.First();
                        var agencyLastName = fundingContractRep.Last();
                        var fundingRepresentative = _customerContactService.GetCustomerContactByName(agencyFirstName, agencyLastName, contract.FundingAgencyOffice);
                        if (fundingRepresentative != null)
                        {
                            contract.FundingOfficeContractRepresentative = fundingRepresentative.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Funding Contract Representative";
                        }
                        break;
                    case "FundingAgencyTechnicalRepresentative":
                        var fundingTechnicalRep = contract.FundingAgencyTechnicalRepresentative.Trim().Split(' ');
                        var agencyTechnicalFirstName = fundingTechnicalRep.First();
                        var agencyTechnicalLastName = fundingTechnicalRep.Last();
                        var fundingTechnialRepresentative = _customerContactService.GetCustomerContactByName(agencyTechnicalFirstName, agencyTechnicalLastName, contract.FundingAgencyOffice);
                        if (fundingTechnialRepresentative != null)
                        {
                            contract.FundingOfficeContractTechnicalRepresent = fundingTechnialRepresentative.CustomerGuid;
                        }
                        else
                        {
                            contract.IsPartialValid = false;
                            contract.Reason += "Invalid Funding Technical Representative";
                        }
                        break;
                    case "IsIDIQContract":
                        //start of conditional validation based on value
                        if (!string.IsNullOrWhiteSpace(contract.IsTaskOrder) && trueBooleanArray.Contains(contract.IsTaskOrder.ToLower()))
                        {
                            var parentContractGuid = _contractsService.GetParentContractByContractNumber(contract.ContractNumber.Trim());
                            if (parentContractGuid == null || parentContractGuid == Guid.Empty)
                            {
                                contract.Reason += "The parent contract number doesn't exist.";
                                contract.IsValid = false;
                                //return contract;
                            }
                            else
                            {
                                contract.ParentContractGuid = null;
                                contract.ContractNumber = contract.TaskOrderNumber;
                                contract.IsIDIQContract = "0";
                            }
                        }
                        break;
                }
            }
            contract.ContractUserRole = contractUserRole;
            return contract;
        }
        //Validate required fee percent based on contract type
        private DMContract ValidateFeePercentByContractType(DMContract contract)
        {
            contract.Reason = string.Empty;
            if (contract.ContractType.StartsWith(this.costPlus) &&
                (string.IsNullOrWhiteSpace(contract.OverHead) || string.IsNullOrWhiteSpace(contract.GAPercent)
                || string.IsNullOrWhiteSpace(contract.FeePercent)))
            {
                if (string.IsNullOrWhiteSpace(contract.OverHead))
                    contract.Reason += "OverHead Fee Percent is required.";
                else if (string.IsNullOrWhiteSpace(contract.GAPercent))
                    contract.Reason += "GAFeePercent is required.";
                else
                    contract.Reason += "FeePercent is required.";
                //return contract;
                contract.IsPartialValid = false;
            }

            if (contract.ContractType.Contains(this.firmFixedPrice) && string.IsNullOrWhiteSpace(contract.FeePercent))
            {
                contract.Reason += "Fee Percent is required for firm fixed price contract type.";
                contract.IsPartialValid = false;
                //return contract;
            }

            return contract;
        }

        //validate attribute value against db based on name
        private DMContract ValidateAttributeValue(DMContract contract)
        {
            if (!string.IsNullOrWhiteSpace(contract.QualityLevel))
            {
                if (contract.QualityLevel == "1")
                    contract.QualityLevel = "Quality Level 1";
                if (contract.QualityLevel == "2")
                    contract.QualityLevel = "Quality Level 2";
                var qualityLevel = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.QualityLevel.ToString(), contract.QualityLevel.Trim());
                if (qualityLevel == null)
                {
                    contract.Reason += "Invalid Quality Level.";
                    contract.IsPartialValid = false;
                    contract.QualityLevelValue = string.Empty;
                }
                else
                    contract.QualityLevelValue = qualityLevel.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.Competition))
            {
                if (contract.Competition == "Non-Competed")
                    contract.Competition = "Non Competed";
                var competition = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.Competition.ToString(), contract.Competition.Trim());
                if (competition == null)
                {
                    contract.Reason += "Invalid Competition.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.CompetitionValue = competition.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractType))
            {
                var contractType = _resourceAttributeValueService.GetResourceAttributeValueByName(contract.ContractType.Trim());
                if (contractType == null)
                {
                    contract.Reason += "Invalid contract type.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.ContractTypeValue = contractType.Value;

                //Fee(Overhead,GA and Fee percent) based on contract type
                if (contractType != null)
                {
                    var validateFee = ValidateFeePercentByContractType(contract);
                    //contract.IsValid = validateFee.IsValid;
                    contract.Reason += validateFee.Reason;
                    contract.IsPartialValid = validateFee.IsPartialValid;
                }
                //if (validateFee.IsValid == false)
                //    return validateFee;
            }

            if (!string.IsNullOrWhiteSpace(contract.InvoiceSubmissionMethod))
            {
                var submissionMethod = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.InvoiceSubmissionMethod.ToString(), contract.InvoiceSubmissionMethod.Trim());
                if (submissionMethod == null)
                {
                    contract.Reason += "Invalid submission method.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.InvoiceSubmissionMethodValue = submissionMethod.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.ApplicableWageDetermination))
            {
                var wageList = contract.ApplicableWageDetermination.Split(',').ToList();
                var wageValue = string.Empty;
                foreach (var wage in wageList)
                {
                    var applicableWage = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.ApplicableWageDetemination.ToString(), contract.ApplicableWageDetermination.Trim());
                    if (applicableWage == null)
                    {
                        contract.Reason += "Invalid Wage: " + wage + ".";
                        contract.IsPartialValid = false;
                    }
                    else
                        wageValue += applicableWage.Value + ",";
                }
                contract.ApplicableWageDeterminationValue = wageValue.TrimEnd(',');
            }

            if (!string.IsNullOrWhiteSpace(contract.BillingFormula))
            {
                var billingFormula = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.BillingFormula.ToString(), contract.BillingFormula.Trim());
                if (billingFormula == null)
                {
                    contract.Reason += "Invalid billing formula.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.BillingFormulaValue = billingFormula.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.RevenueFormula))
            {
                var revenueFormula = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.RevenueFormula.ToString(), contract.RevenueFormula.Trim());
                if (revenueFormula == null)
                {
                    contract.Reason += "Invalid revenue formula.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.RevenueFormulaValue = revenueFormula.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.BillingFrequency))
            {
                var billingFrequency = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.BillingFrequency.ToString(), contract.BillingFrequency.Trim());
                if (billingFrequency == null)
                {
                    contract.Reason += "Invalid Billing Frequency.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.BillingFrequencyValue = billingFrequency.Value;
            }

            if (!string.IsNullOrWhiteSpace(contract.Currency))
            {
                var currency = GetAttributeValueByName(EnumGlobal.ResourceType.Contract.ToString(), EnumGlobal.ResourceAttributeName.Currency.ToString(), contract.Currency.Trim());
                if (currency == null)
                {
                    contract.Reason += "Invalid Currency.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.CurrencyValue = currency.Value;
            }
            else
                contract.CurrencyValue = "USD";
            return contract;
        }

        //get attribute value detail by name
        private ResourceAttributeValue GetAttributeValueByName(string resourceType, string attributeName, string attributeValue)
        {
            if (string.IsNullOrWhiteSpace(resourceType) || string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(attributeValue))
                return null;
            var revenueAttributeValue = _resourceAttributeValueService.GetResourceAttributeValueByResourceTypeNameValue(resourceType, attributeName, attributeValue.Trim());
            if (revenueAttributeValue == null)
                return null;
            return revenueAttributeValue;
        }

        private User GetUserByEmail(string userEmail)
        {
            if (string.IsNullOrWhiteSpace(userEmail))
                return null;
            return _userService.GetUserByWorkEmail(userEmail);
        }

        //Validate key personnal
        private DMContract ValidateKeyPersonal(DMContract contract)
        {
            if (!string.IsNullOrWhiteSpace(contract.ContractRepresentatives))
            {
                var contractRep = _userService.GetUserByWorkEmail(contract.ContractRepresentatives.Trim());
                if (contractRep == null)
                {
                    contract.Reason += "Invalid Contract Representative.";
                    contract.IsPartialValid = false;
                }
                else
                    contract.ContractRepresentativeGuid = contractRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractProjectManager))
            {
                var projectManager = _userService.GetUserByWorkEmail(contract.ContractProjectManager.Trim());
                if (projectManager == null)
                {
                    contract.Reason += "Invalid Project Manager.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.ProjectManagerGuid = projectManager.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ProjectControlRepresentative))
            {
                var controlRep = _userService.GetUserByWorkEmail(contract.ProjectControlRepresentative.Trim());
                if (controlRep == null)
                {
                    contract.Reason += "Invalid Project Controls.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.ProjectControlsGuid = controlRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.AccountingRepresentative))
            {
                var accountRep = _userService.GetUserByWorkEmail(contract.AccountingRepresentative.Trim());
                if (accountRep == null)
                {
                    contract.Reason += "Invalid Account Representative.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.AccountRepresentativeGuid = accountRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractSubContractAdministrator))
            {
                var subContractAdministrator = _userService.GetUserByWorkEmail(contract.ContractSubContractAdministrator.Trim());
                if (subContractAdministrator == null)
                {
                    contract.Reason += "Invalid Subcontrat Administrator.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.SubContractAdministratorGuid = subContractAdministrator.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractPurchasingRepresentative))
            {
                var purchasingRep = _userService.GetUserByWorkEmail(contract.ContractPurchasingRepresentative.Trim());
                if (purchasingRep == null)
                {
                    contract.Reason += "Invalid Purchasing Representative.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.PurchasingRepresentativeGuid = purchasingRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractHumanResourceRepresentative))
            {
                var humanResourceRep = _userService.GetUserByWorkEmail(contract.ContractHumanResourceRepresentative.Trim());
                if (humanResourceRep == null)
                {
                    contract.Reason += "Invalid Human Resource Representative.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.HumanResourceRepresentativeGuid = humanResourceRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractQualityRepresentative))
            {
                var qualityRep = _userService.GetUserByWorkEmail(contract.ContractQualityRepresentative.Trim());
                if (qualityRep == null)
                {
                    contract.Reason += "Invalid Quality Representative.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.QualityRepresentativeGuid = qualityRep.UserGuid;
            }

            if (!string.IsNullOrWhiteSpace(contract.ContractSafetyOfficer))
            {
                var safetyOfficer = _userService.GetUserByWorkEmail(contract.ContractSafetyOfficer.Trim());
                if (safetyOfficer == null)
                {
                    contract.Reason += "Invalid Safety Officer.";
                    contract.IsPartialValid = false;
                    //return contract;
                }
                else
                    contract.SafetyOfficerGuid = safetyOfficer.UserGuid;
            }
            return contract;
        }

        public IList<DMContract> ImportContract(List<DMContract> contractList, Guid userGuid)
        {
            var exportContractList = new List<DMContract>();
            var action = string.Empty;
            foreach (var contract in contractList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(contract.Action))
                        action = contract.Action.ToLower();
                    if (!string.IsNullOrWhiteSpace(contract.ProjectNumber))
                    {
                        Contracts dbContract = null;
                        if (!string.IsNullOrWhiteSpace(contract.IsTaskOrder) && trueBooleanArray.Contains(contract.IsTaskOrder)
                            && !string.IsNullOrWhiteSpace(contract.TaskNodeID))
                            dbContract = _contractsService.GetContractByTaskNodeID(Int32.Parse(contract.TaskNodeID.Trim()));
                        else
                            dbContract = _contractsService.GetContractByProjectNumber(contract.ProjectNumber.Trim());
                        switch (action)
                        {
                            case "delete":
                            case "enable":
                            case "disable":
                                if (dbContract != null)
                                {
                                    var contractUpdate = EnableDisableDeleteContract(contract, dbContract.ContractGuid);
                                    contract.ImportStatus = contractUpdate.ImportStatus.ToString();
                                    contract.Reason = contractUpdate.Reason;
                                }
                                else
                                {
                                    contract.ImportStatus = ImportStatus.Fail.ToString();
                                    contract.Reason = "Contract not found";
                                }
                                break;
                            default:
                                if (dbContract == null)
                                {
                                    var inputValidation = ContractInputValidation(contract);
                                    if (inputValidation.IsValid)
                                    {
                                        var dataValidation = ContractDataValidation(contract);
                                        if (dataValidation.IsValid)
                                        {
                                            contract.ContractNumber = dataValidation.ContractNumber;
                                            contract.ParentContractGuid = dataValidation.ParentContractGuid;

                                            contractEntity = MapContractToCoreMContract(contract, userGuid);
                                            var saveContract = _contractsService.ImportContract(contractEntity);
                                            if (saveContract.StatusName.ToString().ToLower() == "success")
                                            {
                                               
                                                _folderService.CreateFolderTemplate(contractEntity.ContractGuid.ToString(),contractEntity.ProjectNumber,EnumGlobal.ResourceType.Contract.ToString(),contractEntity.ContractGuid,userGuid);
                                                if (!dataValidation.IsPartialValid)
                                                {
                                                    contract.ImportStatus = ImportStatus.PartialSuccess.ToString();
                                                    contract.Reason = dataValidation.Reason;
                                                }
                                                else
                                                    contract.ImportStatus = ImportStatus.Success.ToString();
                                            }
                                            else
                                            {
                                                contract.ImportStatus = ImportStatus.Fail.ToString();
                                            }
                                            foreach (KeyValuePair<string, object> msg in saveContract.Message)
                                            {
                                                var ob = (List<string>)msg.Value;
                                                foreach (var msg1 in ob)
                                                {
                                                    contract.Reason += msg1.ToString();
                                                }
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            contract.ImportStatus = dataValidation.ImportStatus;
                                            contract.Reason = dataValidation.Reason;
                                        }
                                    }
                                    else
                                    {
                                        contract.ImportStatus = inputValidation.ImportStatus;
                                        contract.Reason = inputValidation.Reason;
                                    }
                                }
                                else
                                {
                                    var dataValidation = DataValidationForUpdate(contract);
                                    if (dataValidation.IsValid)
                                    {
                                        dataValidation.ContractGuid = dbContract.ContractGuid;
                                        //start of conditional validation based on value
                                        if (!string.IsNullOrWhiteSpace(contract.IsTaskOrder) && trueBooleanArray.Contains(contract.IsTaskOrder.ToLower())
                                            && !string.IsNullOrWhiteSpace(contract.MasterTaskNodeID))
                                        {
                                            int masterTaskID;
                                            if (int.TryParse(contract.MasterTaskNodeID, out masterTaskID))
                                            {
                                                var parentContractGuid = _contractsService.GetParentContractByMasterTaskNodeID(Int32.Parse(contract.MasterTaskNodeID.Trim()));
                                                if (parentContractGuid == null || parentContractGuid == Guid.Empty)
                                                {
                                                    contract.Reason += "The parent contract doesn't exist.";
                                                    contract.IsValid = false;
                                                    //return contract;
                                                }
                                                else
                                                {
                                                    dataValidation.ParentContractGuid = parentContractGuid;
                                                    dataValidation.ContractNumber = contract.TaskOrderNumber;
                                                    dataValidation.IsIDIQContract = "0";
                                                }
                                            }
                                        }

                                        var updatedValue = MapperHelper.MapObjectToEntity(dataValidation, dbContract);
                                        var parsedContract = (Contracts)updatedValue;
                                        parsedContract.UpdatedOn = DateTime.UtcNow;
                                        parsedContract.UpdatedBy = userGuid;
                                        var updateContract = _contractsService.ImportContract(parsedContract);
                                        if (updateContract.StatusName.ToString().ToLower() == "success")
                                        {
                                            if (!dataValidation.IsPartialValid)
                                            {
                                                contract.ImportStatus = ImportStatus.PartialSuccess.ToString();
                                                contract.Reason = dataValidation.Reason;
                                            }
                                            else
                                                contract.ImportStatus = ImportStatus.Success.ToString();
                                        }
                                        else
                                        {
                                            contract.ImportStatus = ImportStatus.Fail.ToString();
                                        }
                                        foreach (KeyValuePair<string, object> msg in updateContract.Message)
                                        {
                                            var obj = (List<string>)msg.Value;
                                            foreach (var msg1 in obj)
                                            {
                                                contract.Reason += msg1.ToString();
                                            }
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        contract.ImportStatus = ImportStatus.Fail.ToString();
                                        contract.Reason = dataValidation.Reason;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        contract.ImportStatus = ImportStatus.Fail.ToString();
                        contract.Reason = "Project number is empty";
                    }

                }
                catch (Exception ex)
                {
                    contract.Reason = "Error while updating in database i.e: " + ex.Message;
                    contract.ImportStatus = ImportStatus.Fail.ToString();
                }
                exportContractList.Add(contract);
            }
            return exportContractList;
        }

        //get contract header
        private Dictionary<string, string> GetContracteHeader(string json)
        {
            JObject obj = JObject.Parse(json);
            var attributes = obj.ToList<JToken>();

            var dictionary = new Dictionary<string, string>();
            foreach (JToken attribute in attributes)
            {
                JProperty jProperty = attribute.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                var propertyValue = jProperty.Value.ToString();
                dictionary.Add(propertyName, propertyValue);
            }
            return dictionary;
        }
        //end contract internal method

        /// <summary>
        /// Import data from csv file to the system
        /// </summary>
        /// <param name="configuration"></param>
        public void ImportContractData(object configuration, Guid userGuid, string errorLogPath,bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            var importConfigData = GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.outputFolderPath = importConfigData.LogOutputPath;
                    this.errorLogPath = errorLogPath;

                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {

                        var contractHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var contractList = GetContractListFromCsvFile<DMContract>(file, contractHeader);
                            var fileHeaders = _importFileService.GetCSVHeaderFromFile(file, contractHeader);
                            if (contractList == null)
                            {
                                _exportCsvService.ErrorLog(this.errorFile, inputFolderPath, errorLogPath, "Invalid Header");
                                break;
                            }
                            else
                            {
                                //import Contract
                                var importedList = ImportContract(contractList, userGuid);
                                var fileName = Path.GetFileNameWithoutExtension(file);
                                var fileNameWithExtension = Path.GetFileName(file);
                                var fileExtension = Path.GetExtension(file);
                                var fileNameForExportWithStatus = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend, fileExtension);

                                //list for exporting to csv with status
                                var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeaders);
                                var exportList = MapperHelper.GetExportList(importedList, exportHeader).ToList();

                                var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), outputFolderPath, fileNameForExportWithStatus, exportHeader);
                                if (isFileSaved)
                                    _importFileService.MoveFile(inputFolderPath, this.outputFolderPath, fileName, fileExtension,isDelete);
                            }
                        }
                    }
                }

                watch.Stop();
                var duration = watch.Elapsed.Duration();
                Console.WriteLine("Contract migration finish at: " + duration);
            }
        }

        //private User Validation(string customerName)
        //{

        //}
    }
}
