using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
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
    public class RegionImportService : IRegionImportService
    {
        private readonly Interface.IImportFileService _importFileService;
        private readonly IRegionService _regionService;
        private readonly IUserService _userService;
        private readonly IExportCSVService _exportCsvService;

        private string inputFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private Core.Entities.Region regionEntity;
        private string errorFile = "Region.csv";
        private string fileNameToAppend = "Region_Import_Log";

        public RegionImportService(IRegionService regionService, IUserService userService, Interface.IImportFileService importFileService,
            IExportCSVService exportCsvService)
        {
            _importFileService = importFileService;
            _regionService = regionService;
            _userService = userService;
            _exportCsvService = exportCsvService;
        }

        #region internal method
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

        private bool InsertRegionToDb(DMRegion region, Guid userGuid)
        {
            var entity = MapRegionToCoreRegion(region, userGuid);
            _regionService.Add(entity);

            if (region.RegionalManagerGuid != Guid.Empty)
            {
                InsertRegionKeyPerson(region.RegionalManagerGuid,entity.RegionGuid,"Region",ContractUserRole._regionalManager);
            }
            return true;
        }

        private bool UpdateRegionToDb(Core.Entities.Region region)
        {
            _regionService.Edit(region);
            return true;
        }

        private bool EnableDisableRegion(bool isEnable, Guid regionGuid)
        {
            if (regionGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = regionGuid;

            if (isEnable)
                _regionService.Enable(guid);
            else
                _regionService.Disable(guid);
            return true;
        }

        private bool DeleteRegion(Guid regionGuid)
        {
            if (regionGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = regionGuid;
            _regionService.Delete(guid);
            return false;
        }

        private Core.Entities.Region MapRegionToCoreRegion(DMRegion region, Guid userGuid)
        {
            regionEntity = new Entities.Region();
            regionEntity.RegionGuid = Guid.NewGuid();
            regionEntity.RegionName = region.RegionName;
            regionEntity.RegionCode = region.RegionCode;
            //regionEntity.RegionalManager = region.RegionalManagerGuid;
            regionEntity.CreatedOn = DateTime.UtcNow;
            regionEntity.IsActive = true;
            regionEntity.IsDeleted = false;
            regionEntity.CreatedBy = userGuid;
            regionEntity.UpdatedBy = userGuid;
            regionEntity.UpdatedOn = DateTime.UtcNow;
            return regionEntity;
        }

        private bool InsertRegionKeyPerson(Guid userGuid, Guid regionGuid, string key, string roleType)
        {
            if (userGuid == Guid.Empty || regionGuid == Guid.Empty)
                return false;
            var manager = new RegionUserRoleMapping();
            manager.RegionUserRoleMappingGuid = Guid.NewGuid();
            manager.UserGuid = userGuid;
            manager.RegionGuid = regionGuid;
            manager.Keys = key;
            manager.RoleType = roleType;
            _regionService.AddUpdateManager(manager);
            return true;
        }

        private List<DIRegion> GetRegionListFromCsvFile<DIRegion>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {

                    var mapper = new RegionHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var lbrRecord = csv.GetRecords<DIRegion>().ToList();
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

        private DMRegion EnableDisableDeleteRegion(DMRegion region, Guid regionGuid)
        {
            var action = region.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteRegion(regionGuid);
                region.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableRegion(true, regionGuid);
                region.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableRegion(false, regionGuid);
                region.Reason = "Disable Successfully";
            }

            region.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return region;
        }

        private DMRegion CheckDuplicateRegion(DMRegion region)
        {
            var r = _regionService.GetRegionByCode(region.RegionCode);
            if (r != null)
                region.Reason = "Duplicate code";
            return region;
        }

        private DMRegion GetRegion(DMRegion region)
        {
            var regionObject = MapperHelper.CreateDataObjectFromObject(region);
            return region;
        }

        private IList<DMRegion> ImportRegion(List<DMRegion> regionList, Guid userGuid)
        {
            var exportRegionList = new List<DMRegion>();
            var action = string.Empty;
            foreach (var region in regionList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(region.Action))
                        action = region.Action.ToLower();
                    var isCodeValid = ValidationHelper.IsNumeric(region.RegionCode);
                    if (isCodeValid && region.RegionCode.Length == 2)
                    {
                        var dbRegion = _regionService.GetRegionByCodeOrName(region.RegionCode, region.RegionName);
                        switch (action)
                        {
                            case "enable":
                            case "disable":
                            case "delete":
                                if (dbRegion != null)
                                {
                                    var actionStatus = EnableDisableDeleteRegion(region, dbRegion.RegionGuid);
                                    region.ImportStatus = actionStatus.ImportStatus;
                                    region.Reason = actionStatus.Reason;
                                }
                                break;
                            default:
                                //for checking regional manager by display name
                                var manager = _userService.GetUserByWorkEmail(region.Manager);
                                if (manager == null)
                                {
                                    region.ImportStatus = Core.Entities.ImportStatus.Fail.ToString();
                                    region.Reason = "Manager not found";
                                }
                                else
                                {
                                    region.RegionalManagerGuid = manager.UserGuid;
                                    if (dbRegion == null)
                                    {
                                        InsertRegionToDb(region, userGuid);
                                        region.Reason = "Addedd Successfully";
                                        region.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
                                    }
                                    else
                                    {
                                        dbRegion.RegionName = region.RegionName;
                                        dbRegion.RegionalManager = region.RegionalManagerGuid;
                                        dbRegion.UpdatedBy = userGuid;
                                        dbRegion.UpdatedOn = DateTime.UtcNow;

                                        if (_regionService.CheckDuplicateRegionByName(region.RegionName, dbRegion.RegionGuid) > 0)
                                        {
                                            region.Reason = "Duplicate region name";
                                            region.ImportStatus = ImportStatus.Fail.ToString();
                                        }
                                        else
                                        {
                                            var updatedRegion = MapperHelper.MapObjectToEntity(region, dbRegion);
                                            var parseRegion = (Region)updatedRegion;
                                            UpdateRegionToDb(parseRegion);

                                            if (region.RegionalManagerGuid != Guid.Empty)
                                                InsertRegionKeyPerson(region.RegionalManagerGuid,dbRegion.RegionGuid,"Region", ContractUserRole._regionalManager);

                                            region.Reason = "Updated Successfully";
                                            region.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
                                        }
                                    }
                                }
                                break;
                        }
                        //end of switch case
                    }
                    else
                    {
                        region.ImportStatus = ImportStatus.Fail.ToString();
                        region.Reason = "Invalid Code";
                    }
                    exportRegionList.Add(region);
                }
                catch (Exception ex)
                {
                    region.Reason = "Error while updating in database i.e: " + ex.Message;
                    region.ImportStatus = ImportStatus.Fail.ToString();
                }
            }
            return exportRegionList;
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

        private Dictionary<string, string> GetRegionHeader(string json)
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
        #endregion internal method

        /// <summary>
        /// Import data from csv file to the system
        /// </summary>
        /// <param name="configuration"></param>
        public void ImportRegionData(object configuration, Guid userGuid, string errorLogPath,bool isDelete)
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
                        var regionHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var regionList = GetRegionListFromCsvFile<DMRegion>(file, regionHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, regionHeader);
                            if (regionList == null)
                            {
                                _exportCsvService.ErrorLog(this.errorFile, inputFolderPath, errorLogPath, "Invalid Header");
                                break;
                            }
                            else
                            {
                                //import region
                                var importedList = ImportRegion(regionList, userGuid);
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
