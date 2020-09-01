using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Services;
using Northwind.Core.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class OfficeImportService : IOfficeImportService
    {
        private readonly IImportFileService _importFileService;
        private readonly IOfficeService _officeService;
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly IStateService _stateService;
        private readonly IExportCSVService _exportCsvService;

        private string inputFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private Office officeEntity;
        private string errorFile = "Office.csv";
        private string fileNameToAppend = "Office_Import_Log";

        public OfficeImportService(IOfficeService officeService, IUserService userService, IImportFileService importFileService,
            ICountryService countryService, IStateService stateService, IExportCSVService exportCsvService)
        {
            _importFileService = importFileService;
            _officeService = officeService;
            _userService = userService;
            _exportCsvService = exportCsvService;
            _countryService = countryService;
            _stateService = stateService;
        }

        //Office internal method
        /// <summary>
        /// Return json file as dictionary
        /// </summary>
        /// <param name="json">json object</param>
        /// <returns></returns>

        /// <summary>
        /// return configuration setting value based on json object
        /// </summary>
        /// <param name="jsonConfigData">json object</param>
        /// <returns></returns>
        private ImportConfiguration GetConfigurationSetting(string jsonConfigData)
        {
            var config = new ImportConfiguration();
            config = JsonConvert.DeserializeObject<ImportConfiguration>(jsonConfigData);
            return config;
        }

        private bool InsertOfficeToDb(DMOffice Office, Guid userGuid)
        {
            var entity = MapOfficeToCoreOffice(Office, userGuid);
            _officeService.Add(entity);
            return true;
        }

        private bool UpdateOfficeToDb(Core.Entities.Office Office)
        {
            _officeService.Edit(Office);
            return true;
        }

        private bool EnableDisableOffice(bool isEnable, Guid officeGuid)
        {
            if (officeGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = officeGuid;

            if (isEnable)
                _officeService.Enable(guid);
            else
                _officeService.Disable(guid);
            return true;
        }

        private bool DeleteOffice(Guid officeGuid)
        {
            if (officeGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = officeGuid;
            _officeService.Delete(guid);
            return false;
        }

        private Office MapOfficeToCoreOffice(DMOffice office, Guid userGuid)
        {
            officeEntity = new Office();
            officeEntity.OfficeGuid = Guid.NewGuid();
            officeEntity.OfficeName = office.OfficeName;
            officeEntity.OfficeCode = office.OfficeCode;
            officeEntity.PhysicalAddress = office.PhysicalAddress;
            officeEntity.PhysicalAddressLine1 = office.PhysicalAddress1;
            officeEntity.PhysicalCountryId = office.PhysicalCountryGuid;
            officeEntity.PhysicalStateId = office.PhysicalStateGuid;
            officeEntity.PhysicalCity = office.PhysicalCity;
            officeEntity.PhysicalZipCode = office.PhysicalZipCode;
            officeEntity.MailingAddress = office.MailingAddress;
            officeEntity.MailingAddressLine1 = office.MailingAddress1;
            officeEntity.MailingCountryId = office.MailingCountryGuid;
            officeEntity.MailingStateId = office.MailingStateGuid;
            officeEntity.MailingCity = office.MailingCity;
            officeEntity.MailingZipCode = office.MailingZipCode;
            officeEntity.Phone = office.Phone;
            officeEntity.Fax = office.Fax;
            officeEntity.CreatedOn = DateTime.UtcNow;
            officeEntity.IsActive = true;
            officeEntity.IsDeleted = false;
            officeEntity.CreatedBy = userGuid;
            officeEntity.UpdatedBy = userGuid;
            officeEntity.UpdatedOn = DateTime.UtcNow;
            if(office.OperationManagerGuid != Guid.Empty)
                officeEntity.OperationManagerGuid = office.OperationManagerGuid;
            return officeEntity;
        }

        private List<DMOffice> GetOfficeListFromCsvFile<DMOffice>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {

                    var mapper = new OfficeHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var lbrRecord = csv.GetRecords<DMOffice>().ToList();
                    return lbrRecord;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                return null;
            }

        }

        private string GetExportFileName(string originalFileName, string fileName)
        {
            var formattedName = "{" + originalFileName + "}" + '-' + fileName + "_{" + DateTime.UtcNow.ToString("MM-dd-yyyy-hh-mm-ss") + "}";
            return formattedName;
        }

        private DMOffice EnableDisableDeleteOffice(DMOffice Office, Guid officeGuid)
        {
            var action = Office.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteOffice(officeGuid);
                Office.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableOffice(true, officeGuid);
                Office.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableOffice(false, officeGuid);
                Office.Reason = "Disable Successfully";
            }

            Office.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return Office;
        }

        private DMOffice CheckDuplicateOffice(DMOffice Office)
        {
            var r = _officeService.GetOfficeByCode(Office.OfficeCode);
            if (r != null)
                Office.Reason = "Duplicate code";
            return Office;
        }

        private DMOffice DataValidation(DMOffice office)
        {
            office.IsValid = true;
            var customerObject = MapperHelper.CreateDataObjectFromObject(office);
            foreach (var item in customerObject)
            {
                switch (item.Key)
                {
                    case "PhysicalCountry":
                        var physicalCountry = _countryService.GetCountryByCountryName(office.PhysicalCountry);
                        if (physicalCountry == null)
                        {
                            office.IsPartialValid = false;
                            office.Reason += "Physical country not found";
                        }
                        break;
                    case "PhysicalState":
                        var physicalState = _stateService.GetStateByStateName(office.PhysicalState);
                        if (physicalState == null)
                        {
                            office.IsPartialValid = false;
                            office.Reason += "Physical state not found";
                        }
                        break;
                    case "MailingCountry":
                        var mailingCountry = _countryService.GetCountryByCountryName(office.MailingCountry);
                        if (mailingCountry == null)
                        {
                            office.IsPartialValid = false;
                            office.Reason += "Mailing country not found";
                        }
                        else
                        {
                            office.MailingCountryGuid = mailingCountry.CountryId;
                        }
                        break;
                    case "MailingState":
                        var mailingState = _stateService.GetStateByStateName(office.MailingState);
                        if (mailingState == null)
                        {
                            office.IsPartialValid = false;
                            office.Reason += "Mailing state not found";
                        }
                        else
                        {
                            office.MailingStateGuid = mailingState.StateId;
                        }
                        break;
                    case "PhysicalCity":
                        if (string.IsNullOrEmpty(office.PhysicalCity))
                        {
                            office.IsPartialValid = false;
                            office.Reason = "PhysicalCity is empty";
                        }
                        break;
                    case "PhysicalAddress":
                        if (string.IsNullOrEmpty(office.PhysicalAddress))
                        {
                            office.IsPartialValid = false;
                            office.Reason = "PhysicalAddress is empty";
                        }
                        break;
                    case "":
                        if (string.IsNullOrEmpty(office.Phone))
                        {
                            office.IsPartialValid = false;
                            office.Reason = "Phone is empty";
                        }
                        break;
                }
            }
            return office;
        }

        public IList<DMOffice> ImportOffice(List<DMOffice> officeList, Guid userGuid)
        {
            var exportOfficeList = new List<DMOffice>();
            var action = string.Empty;
            foreach (var office in officeList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(office.Action))
                        action = office.Action.ToLower();
                    if (!string.IsNullOrWhiteSpace(office.OfficeCode) && !string.IsNullOrWhiteSpace(office.OfficeName))
                    {
                        var dbOffice = _officeService.GetOfficeByCodeOrName(office.OfficeCode, office.OfficeName);

                        switch (action)
                        {
                            case "enable":
                            case "disable":
                            case "delete":
                                if (dbOffice != null)
                                {
                                    var actionStatus = EnableDisableDeleteOffice(office, dbOffice.OfficeGuid);
                                    office.ImportStatus = actionStatus.ImportStatus;
                                    office.Reason = actionStatus.Reason;
                                }
                                break;
                            default:
                                var isCodeValid = ValidationHelper.IsNumeric(office.OfficeCode);
                                if (isCodeValid && office.OfficeCode.Length == 2)
                                {
                                    if (office.ImportStatus != ImportStatus.Fail.ToString())
                                    {
                                        if (!string.IsNullOrWhiteSpace(office.OperationManager))
                                        {
                                            var user = _userService.GetUserByWorkEmail(office.OperationManager);
                                            if (user != null)
                                                office.OperationManagerGuid = user.UserGuid;
                                        }
                                        if (dbOffice == null)
                                        {
                                            InsertOfficeToDb(office, userGuid);
                                            office.Reason = "Addedd Successfully";
                                            office.ImportStatus = ImportStatus.Success.ToString();
                                        }
                                        else
                                        {
                                            dbOffice.UpdatedBy = userGuid;
                                            dbOffice.UpdatedOn = DateTime.UtcNow;
                                            if (_officeService.CheckDuplicateOfficeByName(office.OfficeName, dbOffice.OfficeGuid) > 0)
                                            {
                                                office.Reason = "Duplicate Office name";
                                                office.ImportStatus = ImportStatus.Fail.ToString();
                                            }
                                            else
                                            {
                                                var updatedOffice = MapperHelper.MapObjectToEntity(office, dbOffice);
                                                var parseOffice = (Office)updatedOffice;
                                                UpdateOfficeToDb(parseOffice);
                                                office.Reason = "Updated Successfully";
                                                office.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    office.ImportStatus = ImportStatus.Fail.ToString();
                                    office.Reason = "Invalid Code";
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(office.OfficeCode))
                            office.Reason = "Office Code is empty";
                        else if (string.IsNullOrWhiteSpace(office.OfficeName))
                            office.Reason = "Office Name is empty";
                        office.ImportStatus = ImportStatus.Fail.ToString();
                    }
                    exportOfficeList.Add(office);
                }
                catch (Exception ex)
                {
                    office.Reason = "Error while updating in database i.e: " + ex.Message;
                    office.ImportStatus = ImportStatus.Fail.ToString();
                }
            }
            return exportOfficeList;
        }

        private static void CreateHeader(Dictionary<string, string> header, StreamWriter sw)
        {
            foreach (var item in header)
            {
                sw.Write(item.Value + ",");
            }
            sw.Write(sw.NewLine);
        }

        private static void CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                PropertyInfo[] properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length - 1; i++)
                {
                    var prop = properties[i];
                    if (prop.GetValue(item).ToString().Contains(","))
                        sw.Write(String.Format("\"{0}\",", prop.GetValue(item).ToString()));
                    else
                        sw.Write(prop.GetValue(item) + ",");
                }
                var lastProp = properties[properties.Length - 1];
                sw.Write(lastProp.GetValue(item) + sw.NewLine);
            }
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
        public void ImportOfficeData(object configuration, Guid userGuid, string errorLogPath, bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            var importConfigData = GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.outputFolderPath = importConfigData.LogOutputPath;


                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var officeHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var officeList = GetOfficeListFromCsvFile<DMOffice>(file, officeHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, officeHeader);
                            if (officeList == null)
                            {
                                _exportCsvService.ErrorLog(this.errorFile, inputFolderPath, errorLogPath, "Invalid Header");
                                break;
                            }
                            else
                            {
                                //import Office
                                var importedList = ImportOffice(officeList, userGuid);
                                var fileName = Path.GetFileNameWithoutExtension(file);
                                var fileNameWithExtension = Path.GetFileName(file);
                                var fileExtension = Path.GetExtension(file);
                                var fileNameForExportWithStatus = GetExportFileName(fileName, this.fileNameToAppend) + fileExtension;

                                var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                                var exportList = MapperHelper.GetExportList(importedList, exportHeader).ToList();
                                var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), outputFolderPath, fileNameForExportWithStatus, exportHeader);
                                if (isFileSaved)
                                    _importFileService.MoveFile(inputFolderPath, this.outputFolderPath, fileName, fileExtension, isDelete);
                            }
                        }
                    }
                }
            }
        }

    }
}
