using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Northwind.Core.Entities;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class CompanyImportService : ICompanyImportService
    {
        private readonly IImportFileService _fileReaderService;
        private readonly IExportCSVService _exportCsvService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly IImportFileService _importFileService;

        private Core.Entities.Company companyEntity;
        private string inputFolderPath = string.Empty;
        private string logOutputPath = string.Empty;
        private string fileExtension = string.Empty;
        private string errorLogPath = string.Empty;
        private string errorFile = "Company.csv";
        private string fileNameToAppend = "Company_Import_Log";
        //private string copyFileName = "Company_Original";

        public CompanyImportService(IImportFileService fileReaderService, IExportCSVService exportCsvService, ICompanyService companyService,
            IUserService userService, IImportFileService importFileService)
        {
            _fileReaderService = fileReaderService;
            _exportCsvService = exportCsvService;
            _companyService = companyService;
            _userService = userService;
            _importFileService = importFileService;
        }

        private List<DMCompany> GetCompanyListFromCsvFile<DICompany>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {

                    var mapper = new CompanyHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var lbrRecord = csv.GetRecords<DMCompany>().ToList();
                    return lbrRecord;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                throw;
            }

        }

        private DMCompany EnableDisableDeleteCompany(DMCompany company, Guid companyGuid)
        {
            var action = company.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteRegion(companyGuid);
                company.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableRegion(true, companyGuid);
                company.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableRegion(false, companyGuid);
                company.Reason = "Disable Successfully";
            }

            company.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return company;
        }

        private DMCompany CheckDuplicateRegion(DMCompany company)
        {
            var r = _companyService.GetCompanyByCode(company.Code);
            if (r != null)
                company.Reason = "Duplicate code";
            return company;
        }

        private bool InsertCompanyToDb(DMCompany company, Guid userGuid)
        {
            var entity = MapCompanyToCoreCompany(company, userGuid);
            _companyService.Add(entity);
            return true;
        }

        private bool UpdateRegionToDb(Core.Entities.Company company)
        {
            if (_companyService.Edit(company) > 0)
                return true;
            return false;
        }

        private bool EnableDisableRegion(bool isEnable, Guid companyGuid)
        {
            if (companyGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = companyGuid;

            if (isEnable)
                _companyService.Enable(guid);
            else
                _companyService.Disable(guid);
            return true;
        }

        private bool DeleteRegion(Guid regionGuid)
        {
            if (regionGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = regionGuid;
            _companyService.Delete(guid);
            return true;
        }

        private Core.Entities.Company MapCompanyToCoreCompany(DMCompany company, Guid userGuid)
        {
            companyEntity = new Entities.Company();
            companyEntity.CompanyGuid = Guid.NewGuid();
            companyEntity.CompanyName = company.Name;
            companyEntity.CompanyCode = company.Code;
            companyEntity.President = company.PresidentGuid;
            companyEntity.Abbreviation = company.Abbreviation;
            companyEntity.Description = company.Description;
            companyEntity.CreatedOn = DateTime.UtcNow;
            companyEntity.IsActive = true;
            companyEntity.IsDeleted = false;
            companyEntity.CreatedBy = userGuid;
            companyEntity.UpdatedBy = userGuid;
            companyEntity.UpdatedOn = DateTime.UtcNow;
            return companyEntity;
        }

        public void ImportCompanyData(object configuration, Guid userGuid, string errorPath,bool isDelete)
        {
            var configDictionary = _fileReaderService.GetFileUsingJToken(configuration.ToString());
            this.errorLogPath = errorPath + this.errorFile;
            var importConfigData = _fileReaderService.GetConfigurationSetting(configuration.ToString());

            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.logOutputPath = importConfigData.LogOutputPath;

                    //get list of csv file
                    var csvFile = _fileReaderService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var regionHeader = _fileReaderService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        
                        foreach (var file in csvFile)
                        {
                            var companyList = GetCompanyListFromCsvFile<DMCompany>(file, regionHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, regionHeader);
                            //import region
                            var importedList = ImportCompany(companyList, userGuid);
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            this.fileExtension = Path.GetExtension(file);
                            var exportFileName = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend, this.fileExtension);

                            
                            var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                            var exportList = MapperHelper.GetExportList(importedList, exportHeader).ToList();
                            var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), logOutputPath, exportFileName, exportHeader);
                            if (isFileSaved)
                                _importFileService.MoveFile(inputFolderPath, this.logOutputPath, fileName, fileExtension, isDelete);
                        }
                    }
                }
            }
        }
        public IList<DMCompany> ImportCompany(List<DMCompany> companyList, Guid userGuid)
        {
            var exportCompanyList = new List<DMCompany>();
            var action = string.Empty;
            foreach (var company in companyList)
            {
                if(!string.IsNullOrWhiteSpace(company.Action))
                    action = company.Action.ToLower();

                if (!string.IsNullOrWhiteSpace(company.Name) && !string.IsNullOrWhiteSpace(company.Code) && !string.IsNullOrWhiteSpace(company.Abbreviation))
                {
                    var dbCompany = _companyService.GetCompanyByCodeOrName(company.Code, company.Name);
                    switch (action)
                    {
                        case "enable":
                        case "disable":
                        case "delete":
                            if (dbCompany != null)
                            {
                                var actionStatus = EnableDisableDeleteCompany(company, dbCompany.CompanyGuid);
                                company.ImportStatus = actionStatus.ImportStatus;
                                company.Reason = actionStatus.Reason;
                            }
                            break;
                        default:
                            var inputValidate = InputValidation(company);
                            if (inputValidate.IsValid)
                            {
                                var companyValidation = ValidateCompanyData(company.Code, company.Name);
                                //for checking company president by username
                                var president = _userService.GetUserByWorkEmail(company.President);
                                if (president == null)
                                {
                                    company.ImportStatus = Core.Entities.ImportStatus.Fail.ToString();
                                    company.Reason = "President not found";
                                }
                                else
                                {
                                    company.PresidentGuid = president.UserGuid;
                                    if (dbCompany == null)
                                    {
                                        InsertCompanyToDb(company, userGuid);
                                        company.Reason = "Added Successfully";
                                        company.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
                                    }
                                    else
                                    {
                                        dbCompany.CompanyName = company.Name;
                                        dbCompany.President = company.PresidentGuid;
                                        dbCompany.UpdatedBy = userGuid;
                                        dbCompany.Abbreviation = company.Abbreviation;
                                        dbCompany.Description = company.Description;
                                        dbCompany.UpdatedOn = DateTime.UtcNow;
                                        UpdateRegionToDb(dbCompany);
                                        company.Reason = "Updated Successfully";
                                        company.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
                                    }
                                }
                            }
                            else
                            {
                                company.ImportStatus = ImportStatus.Fail.ToString();
                                company.Reason = inputValidate.Reason;
                            }
                            break;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(company.Name))
                        company.Reason = "Name is empty";
                    company.ImportStatus = ImportStatus.Fail.ToString();
                }
                exportCompanyList.Add(company);
            }
            return exportCompanyList;
        }

        private DMCompany InputValidation(DMCompany company)
        {
            company.IsValid = false;
            var isCodeNumeric = ValidationHelper.AlphaNum(company.Code);
            if (!isCodeNumeric || company.Code.Length != 2)
            {
                company.Reason += "Invalid Code";
                return company;
            }

            if (company.Name.Length > 255)
            {
                company.Reason += "Name must not exceed more than 255 character";
                company.ImportStatus = ImportStatus.Fail.ToString();
                return company;
            }

            company.IsValid = true;
            return company;
        }

        private DMCompany ValidateCompanyData(string code, string name)
        {
            var company = new DMCompany();
            company.IsValid = false;
            if (!string.IsNullOrWhiteSpace(code))
            {
                var companyByCode = _companyService.GetCompanyByCode(code);
                if (companyByCode != null)
                {
                    company.Reason += "Duplicate Code";
                    company.ImportStatus = ImportStatus.Fail.ToString();
                    return company;
                }
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                var companyByName = _companyService.GetCompanyByName(name);
                if (companyByName != null)
                {
                    company.Reason += "Duplicate Name";
                    company.ImportStatus = ImportStatus.Fail.ToString();
                    return company;
                }
            }

            company.IsValid = true;
            return company;
        }
    }
}
