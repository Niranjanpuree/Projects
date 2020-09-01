using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Core.Import.Service
{
    public class ImportService : IImportService
    {
        private readonly IRegionImportService _regionImportService;
        private readonly ICompanyImportService _companyImportService;
        private readonly ICustomerContactImportService _customerContactImportService;
        private readonly ICustomerImportService _customerImportService;
        private readonly IOfficeImportService _officeImportService;
        private readonly IModImportService _modImportService;
        private readonly IContractImportService _contractImportService;
        private readonly IAttachmentImportService _attachmentImportService;
        private readonly IFarClauseImportService _farClauseImportService;
        private readonly IExportCSVService _exportCSVService;
        string region = "Region";
        string customer = "Customer";
        string company = "Company";
        string office = "Office";
        string customerContact = "CustomerContact";
        string mods = "Mods";
        string contract = "Contract";
        string farClause = "FarClause";
        string questionaire = "Questionaire";
        string deleteFile = "DeleteFileAfterProcessing";
        private string[] trueBooleanArray = { "1", "yes", "true", "y", "active" };

        public ImportService(IRegionImportService regionImportService, ICompanyImportService companyImportService,
            ICustomerContactImportService customerContactImportService, ICustomerImportService customerImportService,
            IOfficeImportService officeImportService, IModImportService modImportService, IContractImportService contractImportService,
            IAttachmentImportService attachmentImportService, IFarClauseImportService farClauseImportService,
            IExportCSVService exportCSVService)
        {
            _regionImportService = regionImportService;
            _companyImportService = companyImportService;
            _customerContactImportService = customerContactImportService;
            _customerImportService = customerImportService;
            _officeImportService = officeImportService;
            _modImportService = modImportService;
            _contractImportService = contractImportService;
            _attachmentImportService = attachmentImportService;
            _farClauseImportService = farClauseImportService;
            _exportCSVService = exportCSVService;
        }


        #region internal method
        private Dictionary<string, object> GetImportConfiguration(string json)
        {
            JObject obj = JObject.Parse(json);
            var attributes = obj["ImportConfiguration"].ToList<JToken>();

            var dictionary = new Dictionary<string, object>();
            foreach (JToken attribute in attributes)
            {
                JProperty jProperty = attribute.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                var propertyValue = jProperty.Value;
                dictionary.Add(propertyName, propertyValue);
            }
            return dictionary;
        }

        private FileConfiguration GetAttachmentConfiguration(string json)
        {
            JObject obj = JObject.Parse(json);
            var config = JsonConvert.DeserializeObject<FileConfiguration>(obj["Attachment"].ToString());
            return config;
        }


        private List<string> GetCsvField(object fieldObject)
        {
            IEnumerable<object> collection = (IEnumerable<object>)fieldObject;
            var fieldList = new List<string>();
            foreach (object item in collection)
            {
                fieldList.Add(item.ToString());
            }
            return fieldList;
        }
        #endregion internal method

        public void ImportData(string filePath, Guid userGuid, string errorLogPath)
        {
            try
            {
                bool isDeletable = false;
                var streamReader = new StreamReader(filePath);
                var json = streamReader.ReadToEnd();
                var configurationData = GetImportConfiguration(json);

                //check if file is movable or not
                if (configurationData.ContainsKey(this.deleteFile))
                {
                    var deleteAbleValue = configurationData[this.deleteFile].ToString();
                    if (!string.IsNullOrWhiteSpace(deleteAbleValue) && trueBooleanArray.Contains(deleteAbleValue.ToLower()))
                        isDeletable = true;
                }

                ///for region data import
                if (configurationData.ContainsKey(this.region))
                {
                    var regionConfigObject = configurationData[this.region];
                    if (regionConfigObject != null)
                        _regionImportService.ImportRegionData(regionConfigObject, userGuid, errorLogPath, isDeletable);

                }
                /////end of region

                //company imort
                if (configurationData.ContainsKey(this.company))
                {
                    var companyConfigObject = configurationData[this.company];
                    if (companyConfigObject != null)
                        _companyImportService.ImportCompanyData(companyConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //office import
                if (configurationData.ContainsKey(this.office))
                {
                    var officeConfigObject = configurationData[this.office];
                    if (officeConfigObject != null)
                        _officeImportService.ImportOfficeData(officeConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //customer import
                if (configurationData.ContainsKey(this.customer))
                {
                    var customerConfigObject = configurationData[this.customer];
                    if (customerConfigObject != null)
                        _customerImportService.ImportCustomerData(customerConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //customer contact import
                if (configurationData.ContainsKey(this.customerContact))
                {
                    var customerContactObject = configurationData[this.customerContact];
                    if (customerContactObject != null)
                        _customerContactImportService.ImportCustomerContactData(customerContactObject, userGuid, errorLogPath, isDeletable);
                }

                //contract import
                if (configurationData.ContainsKey(this.contract))
                {
                    var contractConfigObject = configurationData[this.contract];
                    if (contractConfigObject != null)
                        _contractImportService.ImportContractData(contractConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //mods import
                if (configurationData.ContainsKey(this.mods))
                {
                    var modsConfigObject = configurationData[this.mods];
                    if (modsConfigObject != null)
                        _modImportService.ImportModsData(modsConfigObject, userGuid, errorLogPath, isDeletable);
                }


                //farclause import
                if (configurationData.ContainsKey(this.farClause))
                {
                    var farClauseConfigObject = configurationData[this.farClause];
                    if (farClauseConfigObject != null)
                        _farClauseImportService.ImportFarClauseData(farClauseConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //questionaire import
                if (configurationData.ContainsKey(this.questionaire))
                {
                    var questionaireConfigObject = configurationData[this.questionaire];
                    if (questionaireConfigObject != null)
                        _farClauseImportService.ImportQuestionaireData(questionaireConfigObject, userGuid, errorLogPath, isDeletable);
                }

                //Task.Factory.StartNew(() =>
                //{
                //    ///for region data import
                //    if (configurationData.ContainsKey(this.region))
                //    {
                //        var regionConfigObject = configurationData[this.region];
                //        if (regionConfigObject != null)
                //            _regionImportService.ImportRegionData(regionConfigObject, userGuid, errorLogPath);

                //    }
                //    /////end of region
                //}).ContinueWith((company) =>
                //{
                //    //company imort
                //    if (configurationData.ContainsKey(this.company))
                //    {
                //        var companyConfigObject = configurationData[this.company];
                //        if (companyConfigObject != null)
                //            _companyImportService.ImportCompanyData(companyConfigObject, userGuid, errorLogPath);
                //    }
                //}).ContinueWith((office) =>
                //{
                //    //office import
                //    if (configurationData.ContainsKey(this.office))
                //    {
                //        var officeConfigObject = configurationData[this.office];
                //        if (officeConfigObject != null)
                //            _officeImportService.ImportOfficeData(officeConfigObject, userGuid, errorLogPath);
                //    }
                //}).ContinueWith((customer) =>
                //{
                //    //customer import
                //    if (configurationData.ContainsKey(this.customer))
                //    {
                //        var customerConfigObject = configurationData[this.customer];
                //        if (customerConfigObject != null)
                //            _customerImportService.ImportCustomerData(customerConfigObject, userGuid, errorLogPath);
                //    }
                //}).ContinueWith((customerContact) =>
                //{
                //    //customer contact import
                //    if (configurationData.ContainsKey(this.customerContact))
                //    {
                //        var customerContactObject = configurationData[this.customerContact];
                //        if (customerContactObject != null)
                //            _customerContactImportService.ImportCustomerContactData(customerContactObject, userGuid, errorLogPath);
                //    }
                //}).ContinueWith((contract) =>
                //{
                //    //contract import
                //    if (configurationData.ContainsKey(this.contract))
                //    {
                //        var contractConfigObject = configurationData[this.contract];
                //        if (contractConfigObject != null)
                //            _contractImportService.ImportContractData(contractConfigObject, userGuid, errorLogPath);
                //    }
                //}).ContinueWith((mod) =>
                //{
                //    //mods import
                //    if (configurationData.ContainsKey(this.mods))
                //    {
                //        var modsConfigObject = configurationData[this.mods];
                //        if (modsConfigObject != null)
                //            _modImportService.ImportModsData(modsConfigObject, userGuid, errorLogPath);
                //    }
                //});

            }
            catch (Exception ex)
            {
                _exportCSVService.ErrorLog("", "", errorLogPath, ex.Message);
            }

        }

        public void ImportAttachment(string filePath, Guid userGuid, string errorLogPath)
        {
            var isDeletable = false;
            var streamReader = new StreamReader(filePath);
            var json = streamReader.ReadToEnd();
            var configurationData = GetAttachmentConfiguration(json);

            if (configurationData != null && !string.IsNullOrWhiteSpace(configurationData.DeleteFileAfterProcessing) && trueBooleanArray.Contains(configurationData.DeleteFileAfterProcessing.ToLower()))
                isDeletable = true;
            ///import files
            _attachmentImportService.ImportAttachment(configurationData, userGuid, errorLogPath, isDeletable);
        }

    }


}
