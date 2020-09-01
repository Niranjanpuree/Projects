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
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class CustomerImportService : ICustomerImportService
    {
        private Interface.IImportFileService _importFileService;
        private ICustomerService _customerService;
        private IStateService _stateService;
        private ICountryService _countryService;
        private IUsCustomerOfficeListService _usCustomerOfficeListService;
        private IUserService _userService;
        private IExportCSVService _exportCsvService;
        private ICustomerTypeService _customerTypeService;

        private string inputFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private Core.Entities.Customer customerEntity;
        private string errorFile = "Customer.csv";
        private string fileNameToAppend = "Customer_Import_Log";
        //string defaultCustomerCode = "000000";
        string federalType = "federal";

        public CustomerImportService(ICustomerService CustomerService,
            IUserService userService,
            IImportFileService importFileService,
            IExportCSVService exportCsvService,
            IUsCustomerOfficeListService usCustomerOfficeListService,
            IStateService stateService,
            ICountryService countryService,
            ICustomerTypeService customerTypeService)
        {
            _importFileService = importFileService;
            _customerService = CustomerService;
            _userService = userService;
            _stateService = stateService;
            _usCustomerOfficeListService = usCustomerOfficeListService;
            _countryService = countryService;
            _exportCsvService = exportCsvService;
            _customerTypeService = customerTypeService;
        }

        #region internal method
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

        //insert new customer to DB
        private bool InsertCustomerToDb(DMCustomer customer, Guid userGuid)
        {
            var entity = MapCustomerToCoreCustomer(customer, userGuid);
            _customerService.Add(entity);
            return true;
        }

        //Update customer to DB
        private bool UpdateCustomerToDb(Core.Entities.Customer customer)
        {
            _customerService.Edit(customer);
            return true;
        }

        private DMCustomer EnableDisableDeleteCustomer(DMCustomer customer, Guid customerGuid)
        {
            var action = customer.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteCustomer(customerGuid);
                customer.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableCustomer(true, customerGuid);
                customer.Reason = "Enable Successfully";
            }
            else
            {
                var result = EnableDisableCustomer(false, customerGuid);
                customer.Reason = "Disable Successfully";
            }

            customer.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return customer;
        }

        //enable/disable customer to DB
        private bool EnableDisableCustomer(bool isEnable, Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = customerGuid;

            if (isEnable)
                _customerService.EnableCustomer(guid);
            else
                _customerService.DisableCustomer(guid);
            return true;
        }

        //delete customer from db
        private bool DeleteCustomer(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = customerGuid;
            _customerService.Delete(guid);
            return false;
        }

        //map DMCustomer to Entity customer
        private Core.Entities.Customer MapCustomerToCoreCustomer(DMCustomer customer, Guid userGuid)
        {
            customerEntity = new Entities.Customer();
            customerEntity.CustomerGuid = Guid.NewGuid();
            customerEntity.CustomerName = customer.CustomerName.Trim();
            customerEntity.CustomerTypeGuid = customer.CustomerTypeGuid;
            customerEntity.Department = customer.Department;
            customerEntity.Agency = customer.Agency.Trim();
            customerEntity.CustomerDescription = customer.CustomerDescription;
            customerEntity.Address = customer.Address;
            customerEntity.AddressLine1 = customer.AddressLine1;
            customerEntity.City = customer.City;
            customerEntity.ZipCode = customer.ZipCode;
            customerEntity.PrimaryPhone = customer.PrimaryPhone;
            customerEntity.PrimaryEmail = customer.PrimaryEmail;
            customerEntity.Abbreviations = customer.Abbreviations;
            customerEntity.Tags = customer.Tags;
            customerEntity.CustomerCode = customer.CustomerCode;
            customerEntity.Url = customer.Url;
            customerEntity.StateId = customer.StateId;
            customerEntity.CountryId = customer.CountryId;
            customerEntity.CreatedOn = DateTime.UtcNow;
            customerEntity.IsActive = true;
            customerEntity.IsDeleted = false;
            customerEntity.CreatedBy = userGuid;
            customerEntity.UpdatedBy = userGuid;
            customerEntity.UpdatedOn = DateTime.UtcNow;

            customerEntity.CustomerTypeName = customer.CustomerType;
            customerEntity.Department = customer.Department;
            customerEntity.Agency = customer.Agency;
            customerEntity.CustomerDescription = customer.CustomerDescription;
            return customerEntity;
        }

        //Get customer list from csv file
        private List<DMCustomer> GetCustomerListFromCsvFile<DMCustomer>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {

                    var mapper = new CustomerHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);

                    var lbrRecord = csv.GetRecords<DMCustomer>().ToList();
                    return lbrRecord;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                return null;
            }

        }

        private DMCustomer CheckDuplicateCustomer(DMCustomer customer)
        {
            var customerCount = _customerService.CheckDuplicateForImport(customer.CustomerName, customer.CustomerGuid);
            if (customerCount == 1)
                customer.Reason = "Duplicate code";
            return customer;
        }

        private DMCustomer CustomerInputValidation(DMCustomer customer)
        {
            customer.IsValid = false;
            var mandatoryField = new
            {
                CustomerName = customer.CustomerName,
                Address = customer.Address,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                State = customer.State,
                CustomerType = customer.CustomerType
            };

            var isMandatoryValid = ValidationHelper.NullValidation(mandatoryField);
            if (!isMandatoryValid.IsValid)
            {
                customer.IsValid = false;
                customer.Reason += isMandatoryValid.Message;
                return customer;
            }

            customer.IsValid = true;
            return customer;
        }

        private DMCustomer DataValidation(DMCustomer customer)
        {
            customer.IsValid = true;
            var customerObject = MapperHelper.CreateDataObjectFromObject(customer);
            foreach (var item in customerObject)
            {
                switch (item.Key)
                {
                    case "State":
                        if (!string.IsNullOrEmpty(customer.State))
                        {
                            var state = _stateService.GetStateByStateName(customer.State);
                            if (state == null)
                            {
                                customer.Reason += "Invalid state.";
                                customer.IsValid = false;
                                customer.ImportStatus = ImportStatus.Fail.ToString();
                            }
                            else
                                customer.StateId = state.StateId;
                        }
                        break;
                    case "Country":
                        if (!string.IsNullOrEmpty(customer.Country))
                        {
                            var country = _countryService.GetCountryByCountryName(customer.Country);
                            if (country == null)
                            {
                                customer.Reason += "Invalid Country.";
                                customer.IsValid = false;
                                customer.ImportStatus = ImportStatus.Fail.ToString();
                            }
                            else
                                customer.CountryId = country.CountryId;
                        }
                        break;
                    case "CustomerType":
                        if (!string.IsNullOrWhiteSpace(customer.CustomerType))
                        {
                            var customerType = _customerTypeService.GetCustomerTypeByName(customer.CustomerType);
                            if (customerType == Guid.Empty)
                            {
                                customer.Reason += "Invalid customer type.";
                                customer.IsValid = false;
                                customer.ImportStatus = ImportStatus.Fail.ToString();
                            }
                            else
                                customer.CustomerTypeGuid = customerType;
                        }
                        break;
                }
            }
            return customer;
        }

        private void UpdateCustomerByOfficeForFederal(DMCustomer customer)
        {
            var usCustomerOffice = _usCustomerOfficeListService.GetCustomerOfficeByContractingOfficeName(customer.CustomerName);
            if (usCustomerOffice != null)
            {
                customer.Department = usCustomerOffice.DepartmentName;
                customer.CustomerName = usCustomerOffice.ContractingOfficeName;
                customer.Agency = usCustomerOffice.CustomerName;
                customer.City = usCustomerOffice.AddressCity;
                customer.ZipCode = usCustomerOffice.ZipCode;
                customer.Country = usCustomerOffice.CountryCode;
                customer.State = usCustomerOffice.AddressState;
                customer.CustomerCode = usCustomerOffice.ContractingOfficeCode;
            }
        }

        private IList<DMCustomer> ImportCustomer(List<DMCustomer> customerList, Guid userGuid)
        {
            var exportCustomerList = new List<DMCustomer>();
            var action = string.Empty;
            foreach (var customer in customerList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(customer.Action))
                        action = customer.Action.ToLower();
                    if (!string.IsNullOrWhiteSpace(customer.CustomerName))
                    {
                        var dbCustomer = _customerService.GetCustomerByName(customer.CustomerName.Trim());
                        switch (action.ToLower())
                        {
                            case "enable":
                            case "disable":
                            case "delete":
                                if (dbCustomer != null)
                                {
                                    var actionStatus = EnableDisableDeleteCustomer(customer, dbCustomer.CustomerGuid);
                                    customer.ImportStatus = actionStatus.ImportStatus;
                                    customer.Reason = actionStatus.Reason;
                                }
                                else
                                {
                                    customer.Reason = "Customer Not found.";
                                    customer.ImportStatus = ImportStatus.Fail.ToString();
                                }
                                break;
                            default:
                                var dataValidation = DataValidation(customer);
                                if (dataValidation.IsValid)
                                {
                                    if (dbCustomer == null)
                                    {
                                        if (dataValidation.IsValid)
                                        {
                                            if (!string.IsNullOrWhiteSpace(customer.CustomerType) && customer.CustomerType.ToLower().Trim() == federalType)
                                                UpdateCustomerByOfficeForFederal(customer);
                                            var isAdded = InsertCustomerToDb(customer, userGuid);
                                            if (isAdded)
                                            {
                                                customer.ImportStatus = ImportStatus.Success.ToString();
                                                customer.Reason = "Customer added successfully.";
                                            }
                                        }
                                        else
                                        {
                                            customer.Reason = dataValidation.Reason;
                                            customer.ImportStatus = ImportStatus.Fail.ToString();
                                        }
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrWhiteSpace(customer.CustomerType) && customer.CustomerType.ToLower().Trim() == federalType)
                                            UpdateCustomerByOfficeForFederal(customer);
                                        var updatedCustomer = MapperHelper.MapObjectToEntity(customer, dbCustomer);
                                        var parseCustomer = (Customer)updatedCustomer;
                                        parseCustomer.UpdatedBy = userGuid;
                                        parseCustomer.UpdatedOn = DateTime.Now;

                                        UpdateCustomerToDb(parseCustomer);
                                        customer.ImportStatus = ImportStatus.Success.ToString();
                                        customer.Reason = "Updated Successfully";
                                    }
                                }
                                else
                                {
                                    customer.ImportStatus = ImportStatus.Fail.ToString();
                                    customer.Reason = dataValidation.Reason;
                                }
                                break;
                        }
                        //end of switch case
                    }
                    else
                    {
                        customer.ImportStatus = ImportStatus.Fail.ToString();
                        customer.Reason = "Customer name is empty";
                    }
                    exportCustomerList.Add(customer);
                }
                catch (Exception ex)
                {
                    customer.Reason = "Error while updating in database i.e: " + ex.Message;
                    customer.ImportStatus = ImportStatus.Fail.ToString();
                }
            }
            return exportCustomerList;
        }

        private List<object> GetExportList(IList<DMCustomer> customertList, Dictionary<string, string> header)
        {
            var exportList = new List<object>();
            dynamic objectList = new List<dynamic>();
            foreach (var contract in customertList)
            {
                var prop = new ExpandoObject() as IDictionary<string, Object>;
                var contractObject = MapperHelper.CreateObjectWithValueFromObject(contract, header);

                IDictionary<string, string> dictionary = new Dictionary<string, string>();
                objectList.Add(new ExpandoObject());
                foreach (var item in contractObject)
                {
                    prop.TryAdd<string, Object>(item.Key, item.Value);
                }
                exportList.Add(prop);
            }
            return exportList;
        }
        #endregion internal method

        /// <summary>
        /// Import data from csv file to the system
        /// </summary>
        /// <param name="configuration"></param>
        public void ImportCustomerData(object configuration, Guid userGuid, string errorLogPath, bool isDelete)
        {
            var configDictionary = _importFileService.GetFileUsingJToken(configuration.ToString());
            var importConfigData = GetConfigurationSetting(configuration.ToString());
            if (importConfigData != null)
            {
                if (!string.IsNullOrWhiteSpace(importConfigData.InputFolderPath))
                {
                    this.inputFolderPath = importConfigData.InputFolderPath;
                    this.outputFolderPath = importConfigData.LogOutputPath;
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    //get list of csv file
                    var csvFile = _importFileService.GetAllCsvFilesFromDirectory(this.inputFolderPath, errorLogPath);
                    if (csvFile != null && csvFile.Length > 0)
                    {
                        var customerHeader = _importFileService.GetCSVHeader(importConfigData.CSVToAttributeMapping.ToString());
                        foreach (var file in csvFile)
                        {
                            var customerList = GetCustomerListFromCsvFile<DMCustomer>(file, customerHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, customerHeader);
                            if (customerList == null)
                            {
                                _exportCsvService.ErrorLog(this.errorFile, inputFolderPath, errorLogPath, "Invalid Header");
                                break;
                            }
                            else
                            {
                                //import Customer
                                var importedList = ImportCustomer(customerList, userGuid);
                                var fileName = Path.GetFileNameWithoutExtension(file);
                                var fileNameWithExtension = Path.GetFileName(file);
                                var fileExtension = Path.GetExtension(file);
                                var fileNameForExportWithStatus = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend,fileExtension);

                                var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                                var exportList = MapperHelper.GetExportList(importedList, exportHeader).ToList();
                                var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList, outputFolderPath, fileNameForExportWithStatus, exportHeader);
                                if (isFileSaved)
                                    _importFileService.MoveFile(inputFolderPath, this.outputFolderPath, fileName, fileExtension,isDelete);
                            }
                        }
                    }

                    watch.Stop();
                    var duration = watch.Elapsed.Duration();
                    Console.WriteLine(duration);
                }
            }
        }
    }
}
