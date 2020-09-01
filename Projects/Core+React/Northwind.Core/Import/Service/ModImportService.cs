using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class ModImportService : IModImportService
    {
        private readonly IContractModificationService _contractModificationService;
        private readonly IImportFileService _importFileService;
        private readonly IExportCSVService _exportCsvService;
        private readonly Core.Interfaces.ContractRefactor.IContractsService _contractsService;
        private ContractModification modsEntity;
        private string inputFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private string errorLogPath = string.Empty;
        private string errorFile = "Mods.csv";
        private string fileNameToAppend = "Mods_Import_Log";
        //private bool isValidAwardAmount;
        private string rootPath = string.Empty;

        public ModImportService(IContractModificationService contractModificationService, IImportFileService importFileService,
            IExportCSVService exportCSVService, IContractsService contractsService)
        {
            _contractModificationService = contractModificationService;
            _importFileService = importFileService;
            _exportCsvService = exportCSVService;
            _contractsService = contractsService;
        }

        private ImportConfiguration GetConfigurationSetting(string jsonConfigData)
        {
            var config = new ImportConfiguration();
            config = JsonConvert.DeserializeObject<ImportConfiguration>(jsonConfigData);
            return config;
        }

        private bool InsertModsToDb(DMMods mods, Guid userGuid)
        {
            var entity = MapModsToCoreMods(mods, userGuid);
            _contractModificationService.Add(entity);
            return true;
        }

        private bool UpdateModsToDb(Core.Entities.ContractModification mods)
        {
            _contractModificationService.Edit(mods);
            return true;
        }

        private bool EnableDisableMods(bool isEnable, Guid modsGuid)
        {
            if (modsGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = modsGuid;

            if (isEnable)
                _contractModificationService.Enable(guid);
            else
                _contractModificationService.Disable(guid);
            return true;
        }

        private bool DeleteMods(Guid modsGuid)
        {
            if (modsGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = modsGuid;
            _contractModificationService.Delete(guid);
            return false;
        }

        private ContractModification MapModsToCoreMods(DMMods mods, Guid userGuid)
        {
            modsEntity = new ContractModification();

            if (string.IsNullOrWhiteSpace(mods.POPStart))
                mods.POPStart = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(mods.POPEnd))
                mods.POPEnd = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(mods.AwardAmount))
                mods.AwardAmount = "0";
            if (string.IsNullOrWhiteSpace(mods.FundingAmount))
                mods.FundingAmount = "0";

            modsEntity.ContractModificationGuid = mods.ContractModificationGuid;
            modsEntity.ContractGuid = mods.ContractGuid;
            modsEntity.ProjectNumber = mods.ProjectNumber;
            modsEntity.ModificationNumber = mods.ModNumber;
            modsEntity.ModificationTitle = mods.ModTitle;
            if (!string.IsNullOrWhiteSpace(mods.POPStart))
                modsEntity.POPStart = DateTime.Parse(mods.POPStart);
            if (!string.IsNullOrWhiteSpace(mods.POPEnd))
                modsEntity.POPEnd = DateTime.Parse(mods.POPEnd);
            if (!string.IsNullOrWhiteSpace(mods.AwardAmount))
                modsEntity.AwardAmount = Decimal.Parse(mods.AwardAmount);
            if (!string.IsNullOrWhiteSpace(mods.FundingAmount))
                modsEntity.FundingAmount = Decimal.Parse(mods.FundingAmount);
            modsEntity.CreatedOn = DateTime.UtcNow;
            modsEntity.IsActive = true;
            modsEntity.IsDeleted = false;
            modsEntity.CreatedBy = userGuid;
            modsEntity.UpdatedBy = userGuid;
            modsEntity.UpdatedOn = DateTime.UtcNow;
            return modsEntity;
        }

        private List<DMMods> GetModsListFromCsvFile<DMMods>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {

                    var mapper = new ModsHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var modsRecord = csv.GetRecords<DMMods>().ToList();
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

        private string GetExportFileName(string originalFileName, string fileName)
        {
            var formattedName = "{" + originalFileName + "}" + '-' + fileName + "_{" + DateTime.UtcNow.ToString("MM-dd-yyyy-hh-mm-ss") + "}";
            return formattedName;
        }

        private DMMods EnableDisableDeleteMods(DMMods mods, Guid modsGuid)
        {
            var action = mods.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteMods(modsGuid);
                mods.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableMods(true, modsGuid);
                mods.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableMods(false, modsGuid);
                mods.Reason = "Disable Successfully";
            }

            mods.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return mods;
        }

        private DMMods CheckDuplicateMods(DMMods mods)
        {
            mods.IsValid = true;
            var isModNumberExist = _contractModificationService.IsExistModificationNumber(mods.ContractGuid, mods.ContractModificationGuid, mods.ModNumber);
            if (isModNumberExist)
            {
                mods.Reason = "Duplicate Mod Number";
                mods.IsValid = false;
                mods.ImportStatus = ImportStatus.Fail.ToString();
            }

            var isModTitleExist = _contractModificationService.IsExistModificationTitle(mods.ContractGuid, mods.ContractModificationGuid, mods.ModTitle);
            if (isModTitleExist)
            {
                mods.Reason = "Duplicate Mod Title";
                mods.IsValid = false;
                mods.ImportStatus = ImportStatus.Fail.ToString();
            }
            return mods;
        }

        private DMMods ModsValidation(DMMods mods)
        {
            mods.IsValid = true;

            if (string.IsNullOrWhiteSpace(mods.POPStart) || mods.POPStart == "NULL")
                mods.POPStart = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(mods.POPEnd) || mods.POPEnd == "NULL")
                mods.POPEnd = DateTime.UtcNow.ToShortDateString();
            if (string.IsNullOrWhiteSpace(mods.AwardAmount) || mods.AwardAmount == "NULL")
                mods.AwardAmount = "0";
            if (string.IsNullOrWhiteSpace(mods.FundingAmount) || mods.FundingAmount == "NULL")
                mods.FundingAmount = "0";

            if (!string.IsNullOrWhiteSpace(mods.POPStart.ToString()))
            {
                var isPOPStartValid = ValidationHelper.IsValidDate(mods.POPStart.ToString());
                if (!isPOPStartValid)
                {
                    mods.ImportStatus = ImportStatus.Fail.ToString();
                    mods.IsValid = false;
                    mods.Reason += "Invalid POPStart date";
                }
            }

            if (!string.IsNullOrWhiteSpace(mods.POPEnd.ToString()))
            {
                var isPOPEndValid = ValidationHelper.IsValidDate(mods.POPEnd.ToString());
                if (!isPOPEndValid)
                {
                    mods.ImportStatus = ImportStatus.Fail.ToString();
                    mods.IsValid = false;
                    mods.Reason += "Invalid POPEnd date";
                }
            }

            if (!string.IsNullOrWhiteSpace(mods.AwardAmount.ToString()))
            {
                var isValidAwardAmount = ValidationHelper.IsValidDecimal(mods.AwardAmount.ToString());
                if (!isValidAwardAmount)
                {
                    mods.ImportStatus = ImportStatus.Fail.ToString();
                    mods.IsValid = false;
                    mods.Reason += "Invalid Award Amount";
                }
            }

            if (!string.IsNullOrWhiteSpace(mods.FundingAmount.ToString()))
            {
                var isValidFundingAmount = ValidationHelper.IsValidDecimal(mods.FundingAmount.ToString());
                if (!isValidFundingAmount)
                {
                    mods.ImportStatus = ImportStatus.Fail.ToString();
                    mods.IsValid = false;
                    mods.Reason += "Invalid Funding Amount";
                }
            }
            return mods;
        }

        public IList<DMMods> ImportMods(List<DMMods> modsList, Guid userGuid)
        {
            var exportModsList = new List<DMMods>();
            var action = string.Empty;
            foreach (var mods in modsList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(mods.Action))
                        action = mods.Action.ToLower();
                    if (!string.IsNullOrWhiteSpace(mods.ProjectNumber) &&
                        !string.IsNullOrWhiteSpace(mods.ModTitle) &&
                        !string.IsNullOrWhiteSpace(mods.ModNumber))
                    {
                        var contractGuid = _contractsService.GetContractGuidByProjectNumber(mods.ProjectNumber);
                        if (contractGuid != null && contractGuid != Guid.Empty)
                        {
                            mods.ContractGuid = contractGuid;
                            var dbMod = _contractModificationService.GetModByContractGuidAndModNumber(contractGuid, mods.ModNumber);
                            switch (action)
                            {
                                case "enable":
                                case "disable":
                                case "delete":
                                    if (dbMod != null)
                                    {
                                        var d = EnableDisableDeleteMods(mods, dbMod.ContractModificationGuid);
                                        mods.ImportStatus = d.ImportStatus.ToString();
                                        mods.Reason = d.Reason;
                                    }
                                    else
                                    {
                                        mods.ImportStatus = ImportStatus.Fail.ToString();
                                        mods.Reason = "Mods not found";
                                    }
                                    break;
                                default:
                                    var modValidation = ModsValidation(mods);
                                    if (modValidation.IsValid)
                                    {
                                        if (dbMod == null)
                                        {
                                            mods.ContractGuid = contractGuid;
                                            mods.ContractModificationGuid = Guid.NewGuid();
                                            var checkMods = CheckDuplicateMods(mods);
                                            if (checkMods.IsValid == true)
                                            {
                                                InsertModsToDb(mods, userGuid);
                                                mods.ImportStatus = ImportStatus.Success.ToString();
                                                mods.Reason = "Added successfully";
                                            }
                                            else
                                            {
                                                mods.ImportStatus = checkMods.ImportStatus;
                                                mods.Reason = checkMods.Reason;
                                            }
                                        }
                                        else
                                        {
                                            mods.ContractModificationGuid = dbMod.ContractModificationGuid;
                                            var checkMods = CheckDuplicateMods(mods);
                                            if (checkMods.IsValid)
                                            {
                                                if (!string.IsNullOrWhiteSpace(mods.POPStart))
                                                    dbMod.POPStart = DateTime.Parse(mods.POPStart);
                                                if (!string.IsNullOrWhiteSpace(mods.POPEnd))
                                                    dbMod.POPEnd = DateTime.Parse(mods.POPEnd);

                                                var updatedValue = MapperHelper.MapObjectToEntity(mods, dbMod);
                                                var parsedMod = (ContractModification)updatedValue;
                                                parsedMod.UpdatedOn = DateTime.UtcNow;
                                                parsedMod.UpdatedBy = userGuid;
                                                dbMod.FundingAmount = Decimal.Parse(mods.FundingAmount);
                                                dbMod.AwardAmount = Decimal.Parse(mods.AwardAmount);
                                                UpdateModsToDb(parsedMod);
                                                mods.ImportStatus = ImportStatus.Success.ToString();
                                                mods.Reason = "Updated successfully";
                                            }
                                            else
                                            {
                                                mods.ImportStatus = checkMods.ImportStatus;
                                                mods.Reason = checkMods.Reason;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        mods.ImportStatus = ImportStatus.Fail.ToString();
                                        mods.Reason = modValidation.Reason;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            mods.ImportStatus = ImportStatus.Fail.ToString();
                            mods.Reason = "Contract doesn't exist";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(mods.ProjectNumber))
                            mods.Reason = "Project number is empty";
                        else if (string.IsNullOrWhiteSpace(mods.ModTitle))
                            mods.Reason = "Mod title is empty";
                        else if (string.IsNullOrWhiteSpace(mods.ModNumber))
                            mods.Reason = "Mod number is empty";
                        mods.ImportStatus = ImportStatus.Fail.ToString();
                    }

                }
                catch (Exception ex)
                {
                    mods.Reason = "Error while updating in database i.e: " + ex.Message;
                    mods.ImportStatus = ImportStatus.Fail.ToString();
                }
                exportModsList.Add(mods);
            }
            return exportModsList;
        }

        private Dictionary<string, string> GetOfficeHeader(string json)
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
        //endoffice internal method

        /// <summary>
        /// Import data from csv file to the system
        /// </summary>
        /// <param name="configuration"></param>
        public void ImportModsData(object configuration, Guid userGuid, string errorLogPath,bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            var importConfigData = GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.outputFolderPath = importConfigData.LogOutputPath;
                    this.errorLogPath = errorLogPath;

                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var modHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var modsList = GetModsListFromCsvFile<DMMods>(file, modHeader);
                            var fileHeaders = _importFileService.GetCSVHeaderFromFile(file, modHeader);
                            if (modsList == null)
                            {
                                _exportCsvService.ErrorLog(this.errorFile, inputFolderPath, errorLogPath, "Invalid Header");
                                break;
                            }
                            else
                            {
                                //import Office
                                var importedList = ImportMods(modsList, userGuid);
                                var fileName = Path.GetFileNameWithoutExtension(file);
                                var fileNameWithExtension = Path.GetFileName(file);
                                var fileExtension = Path.GetExtension(file);
                                var fileNameForExportWithStatus = GetExportFileName(fileName, this.fileNameToAppend) + fileExtension;

                                //list for exporting to csv with status

                                var exportHeader = _exportCsvService.GetExportCSVHeader(modHeader);
                                var exportList = MapperHelper.GetExportList(importedList, exportHeader).ToList();
                                var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), outputFolderPath, fileNameForExportWithStatus, exportHeader);
                                if (isFileSaved)
                                    _importFileService.MoveFile(inputFolderPath, this.outputFolderPath, fileName, fileExtension,isDelete);
                            }
                        }
                    }
                }
            }
        }
    }
}
