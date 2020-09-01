using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Northwind.Core.Entities;
using Northwind.Core.Import.Helper;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class CustomerContactImportService : ICustomerContactImportService
    {
        private readonly IImportFileService _fileReaderService;
        private readonly IExportCSVService _exportCsvService;
        private readonly IUserService _userService;
        private readonly IImportFileService _importFileService;
        private readonly ICustomerContactService _customerContactService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerContactTypeService _customerContactTypeService;

        private Core.Entities.CustomerContact customerContactEntity;
        private string inputFolderPath = string.Empty;
        private string logOutputPath = string.Empty;
        private string fileExtension = string.Empty;
        private string errorLogPath = string.Empty;
        private string errorFile = "CustomerContact.csv";
        private string fileNameToAppend = "CustomerContact_Import_Log";
        //private string copyFileName = "CustomerContact_Original";

        public CustomerContactImportService(IImportFileService fileReaderService, IExportCSVService exportCsvService,
            ICustomerContactService customerContactService, IUserService userService, IImportFileService importFileService,
            ICustomerService customerService, ICustomerContactTypeService customerContactTypeService)
        {
            _fileReaderService = fileReaderService;
            _exportCsvService = exportCsvService;
            _customerContactService = customerContactService;
            _userService = userService;
            _importFileService = importFileService;
            _customerService = customerService;
            _customerContactTypeService = customerContactTypeService;
        }

        private List<DMCustomerContact> GetCustomerContactListFromCsvFile<DMCustomerContact>(string filePath, Dictionary<string, string> header)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader))
                {
                    var mapper = new CustomerContactHeaderMap(header);
                    csv.Configuration.RegisterClassMap(mapper);
                    var recordList = csv.GetRecords<DMCustomerContact>().ToList();
                    return recordList;
                }
            }
            catch (Exception e)
            {
                var errorMsg = e.Message;
                throw;
            }

        }

        private DMCustomerContact EnableDisableDeleteCustomerContact(DMCustomerContact customerContact, Guid customerContactGuid)
        {
            var action = customerContact.Action.ToLower();
            if (action == ImportAction.Delete.ToString().ToLower())
            {
                var result = DeleteCustomerContact(customerContactGuid);
                customerContact.Reason = "Deleted Successfully";
            }
            else if (action == ImportAction.Enable.ToString().ToLower())
            {
                var result = EnableDisableCustomerContact(true, customerContactGuid);
                customerContact.Reason = "Enabled Successfully";
            }
            else
            {
                var result = EnableDisableCustomerContact(false, customerContactGuid);
                customerContact.Reason = "Disabled Successfully";
            }

            customerContact.ImportStatus = Core.Entities.ImportStatus.Success.ToString();
            return customerContact;
        }

        private bool InsertCustomerContactToDb(DMCustomerContact customerContact, Guid userGuid)
        {
            var contact = (DMCustomerContact)MapperHelper.SetDefaultValueToNullProperty(customerContact);
            var entity = MapCustomerContactToCoreCustomerContact(contact, userGuid);
            _customerContactService.Add(entity);
            return true;
        }

        private bool UpdateCustomerContactToDb(Core.Entities.CustomerContact customerContact)
        {
            _customerContactService.Edit(customerContact);
            return true;
        }

        private bool EnableDisableCustomerContact(bool isEnable, Guid customerContactGuid)
        {
            if (customerContactGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = customerContactGuid;

            if (isEnable)
                _customerContactService.Enable(guid);
            else
                _customerContactService.Disable(guid);
            return true;
        }

        private bool DeleteCustomerContact(Guid customerContactGuid)
        {
            if (customerContactGuid == Guid.Empty)
                return false;
            Guid[] guid = new Guid[1];
            guid[0] = customerContactGuid;
            _customerContactService.Delete(guid);
            return false;
        }

        private Core.Entities.CustomerContact MapCustomerContactToCoreCustomerContact(DMCustomerContact customerContact, Guid userGuid)
        {
            customerContactEntity = new Entities.CustomerContact();
            customerContactEntity.ContactGuid = Guid.NewGuid();
            customerContactEntity.FirstName = customerContact.FirstName.Trim();
            customerContactEntity.MiddleName = customerContact.MiddleName.Trim();
            customerContactEntity.LastName = customerContact.LastName.Trim();
            customerContactEntity.PhoneNumber = customerContact.PhoneNumber;
            customerContactEntity.AltPhoneNumber = customerContact.AltPhoneNumber;
            customerContactEntity.EmailAddress = customerContact.EmailAddress;
            customerContactEntity.AltEmailAddress = customerContact.AltEmailAddress;
            customerContactEntity.CustomerGuid = customerContact.CustomerGuid;
            customerContactEntity.ContactTypeGuid = customerContact.ContactTypeGuid;
            customerContactEntity.CreatedOn = DateTime.UtcNow;
            customerContactEntity.IsActive = true;
            customerContactEntity.IsDeleted = false;
            customerContactEntity.CreatedBy = userGuid;
            customerContactEntity.UpdatedBy = userGuid;
            customerContactEntity.UpdatedOn = DateTime.UtcNow;
            return customerContactEntity;
        }

        private Guid GetCustomerGuidByCustomerName(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return Guid.Empty;
            var customer = _customerService.GetCustomerByName(customerName);
            if (customer != null)
                return customer.CustomerGuid;
            return Guid.Empty;
        }

        private Guid GetContactTypeGuidByName(string contactTypeName)
        {
            if (string.IsNullOrWhiteSpace(contactTypeName))
                return Guid.Empty;
            return _customerContactTypeService.GetCustomerContactTypeGuidByTypeName(contactTypeName);
        }

        public void ImportCustomerContactData(object configuration, Guid userGuid, string errorPath,bool isDelete)
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
                            var customerContactList = GetCustomerContactListFromCsvFile<DMCustomerContact>(file, regionHeader);
                            var fileHeader = _importFileService.GetCSVHeaderFromFile(file, regionHeader);
                            //import customer contact
                            var importedList = ImportCustomerContact(customerContactList, userGuid);
                            var fileName = Path.GetFileNameWithoutExtension(file);
                            this.fileExtension = Path.GetExtension(file);
                            var exportFileName = _exportCsvService.GetExportFileName(fileName, this.fileNameToAppend, this.fileExtension);

                            //list for exporting to csv with status
                            var exportHeader = _exportCsvService.GetExportCSVHeader(fileHeader);
                            var exportList = MapperHelper.GetExportList(importedList, exportHeader);
                            
                            var isFileSaved = _exportCsvService.SaveCSVWithStatus(exportList.ToList(), logOutputPath, exportFileName, exportHeader);
                            if (isFileSaved)
                                _importFileService.MoveFile(inputFolderPath, this.logOutputPath, fileName, fileExtension,isDelete);
                        }
                    }
                }
            }
        }

        public IList<DMCustomerContact> ImportCustomerContact(List<DMCustomerContact> customerContactList, Guid userGuid)
        {
            var exportCustomerContactList = new List<DMCustomerContact>();
            var action = string.Empty;
            foreach (var customerContact in customerContactList)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(customerContact.Action))
                        action = customerContact.Action.Trim().ToLower();

                    if (!string.IsNullOrWhiteSpace(customerContact.CustomerName)
                   && !string.IsNullOrWhiteSpace(customerContact.ContactName))
                    {
                        var customerGuid = GetCustomerGuidByCustomerName(customerContact.CustomerName.Trim());
                        if (customerGuid != Guid.Empty)
                        {
                            customerContact.CustomerGuid = customerGuid;
                            //split contact name to first and last name by space
                            var contact = customerContact.ContactName.Trim().Split(' ');
                            customerContact.FirstName = contact.First().Trim();
                            if(contact.Count() > 1)
                                customerContact.LastName = contact.Last().Trim();
                            var dbCustomerContact = _customerContactService.GetCustomerContactByName(customerContact.FirstName, customerContact.LastName, customerGuid);

                            switch (action)
                            {
                                case "delete":
                                case "enable":
                                case "disable":
                                    if (dbCustomerContact != null)
                                    {
                                        var boolOperation = EnableDisableDeleteCustomerContact(customerContact, dbCustomerContact.ContactGuid);
                                        customerContact.ImportStatus = boolOperation.ImportStatus;
                                        customerContact.Reason = boolOperation.Reason;
                                    }
                                    else
                                    {
                                        customerContact.ImportStatus = ImportStatus.Fail.ToString();
                                        customerContact.Reason = "Customer contact not found";
                                    }
                                    break;
                                default:
                                    //for customer type validation
                                    if (!string.IsNullOrWhiteSpace(customerContact.ContactType))
                                    {
                                        if (customerContact.ContactType.Trim() == "Contracting Officer")
                                            customerContact.ContactType = "Contract Representative";
                                        else if (customerContact.ContactType.Trim() == "Contract Technical Rep.")
                                            customerContact.ContactType = "Technical Contract Representative";

                                        var contactTypeGuid = GetContactTypeGuidByName(customerContact.ContactType.Trim());
                                        if (contactTypeGuid != Guid.Empty)
                                        {
                                            customerContact.ContactTypeGuid = contactTypeGuid;
                                        }
                                        else {
                                            customerContact.ImportStatus = ImportStatus.Fail.ToString();
                                            customerContact.Reason = "Invalid contact type";
                                            customerContact.IsValid = false;
                                        }
                                    }

                                    if (customerContact.IsValid) {
                                        if (dbCustomerContact == null)
                                        {
                                            InsertCustomerContactToDb(customerContact, userGuid);
                                            customerContact.ImportStatus = ImportStatus.Success.ToString();
                                            customerContact.Reason = "Contact added successfully";
                                        }
                                        else
                                        {
                                            var updatedCustomerContact = MapperHelper.MapObjectToEntity(customerContact, dbCustomerContact);
                                            var parsedContact = (CustomerContact)updatedCustomerContact;
                                            parsedContact.UpdatedBy = userGuid;
                                            parsedContact.UpdatedOn = DateTime.UtcNow;
                                            UpdateCustomerContactToDb(dbCustomerContact);
                                            customerContact.ImportStatus = ImportStatus.Success.ToString();
                                            customerContact.Reason = "Contact updated successfully";
                                        }
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            customerContact.Reason = "Invalid Customer";
                            customerContact.ImportStatus = ImportStatus.Fail.ToString();
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(customerContact.CustomerName))
                            customerContact.Reason = "Customer name cannot be empty";
                        else if (string.IsNullOrWhiteSpace(customerContact.ContactName))
                            customerContact.Reason = "Contact name cannot be empty";
                        customerContact.ImportStatus = ImportStatus.Fail.ToString();
                    }
                }
                catch (Exception ex)
                {
                    customerContact.Reason = "Error occured while updating in database i.e: " + ex.Message;
                    customerContact.ImportStatus = ImportStatus.Fail.ToString();
                }
                exportCustomerContactList.Add(customerContact);
            }
            return exportCustomerContactList;
        }
    }
}
