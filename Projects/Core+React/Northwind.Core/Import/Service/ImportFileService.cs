using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Import.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using Northwind.Core.Import.Model;
using CsvHelper;

namespace Northwind.Core.Import.Service
{
    public class ImportFileService : IImportFileService
    {
        private readonly IExportCSVService _exportCSVService;
        public ImportFileService(IExportCSVService exportCSVService)
        {
            _exportCSVService = exportCSVService;
        }

        private string GetFileNameToCopy(string fileName, string extension)
        {
            var formattedName = "{" + fileName + "}" + '-' + "Imported_file_" + "{" + DateTime.UtcNow.ToString("MM-dd-yyyy-hh-mm-ss") + "}" + extension;
            return formattedName;
        }

        public string[] GetAllCsvFilesFromDirectory(string directoryPath, string errorLogPath)
        {
            try
            {
                string[] csvfiles = Directory.GetFiles(directoryPath, "*.csv");
                return csvfiles;
            }
            catch (Exception e)
            {
                var error = e.Message;
                var reason = "Invalid File Path";
                _exportCSVService.ErrorLog("Region", directoryPath, errorLogPath, reason);
                return null;
            }

        }

        public Dictionary<string, string> GetCSVHeader(string json)
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

        public Dictionary<string, object> GetFileUsingJToken(string json)
        {
            JObject obj = JObject.Parse(json);
            var attributes = obj.ToList<JToken>();

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

        /// <summary>
        /// return configuration setting value based on json object
        /// </summary>
        /// <param name="jsonConfigData">json object</param>
        /// <returns></returns>
        public ImportConfiguration GetConfigurationSetting(string jsonConfigData)
        {
            var config = new ImportConfiguration();
            config = JsonConvert.DeserializeObject<ImportConfiguration>(jsonConfigData);
            return config;
        }

        public Dictionary<string,string> GetCSVHeaderFromFile(string filePath,Dictionary<string,string> headerDictionary)
        {
            try
            {
                var csv = new CsvReader(new StreamReader(filePath));
                csv.Configuration.HasHeaderRecord = true;
                csv.Read();
                csv.ReadHeader();
                string[] headerArray = ((CsvFieldReader)((CsvParser)csv.Parser).FieldReader).Context.HeaderRecord;

                var finalHeader = new Dictionary<string, string>();
                foreach (var item in headerArray)
                {
                    var key = headerDictionary.FirstOrDefault(x => x.Value == item).Key;
                    if (key != null)
                    {
                        var h = headerDictionary.SingleOrDefault(m => m.Key == key);
                        finalHeader.Add(key,headerDictionary[key]);
                    }
                }
                csv.Context.Reader.Close();
                return finalHeader;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void MoveFile(string sourcePath, string destinationPath, string fileName, string extension,bool isDelete)
        {
            var newFileName = GetFileNameToCopy(fileName, extension);
            var sourceFile = Path.Combine(sourcePath, fileName + extension);
            var destinationFile = Path.Combine(destinationPath, newFileName);
            if (Directory.Exists(sourcePath) && Directory.Exists(destinationPath))
            {
                File.Copy(sourceFile, destinationFile);
                if(isDelete)
                    File.Delete(sourceFile);
            }
        }

        #region folder
        public FileConfiguration GetFileConfigurationSetting(string jsonConfig)
        {
            var config = new FileConfiguration();
            config = JsonConvert.DeserializeObject<FileConfiguration>(jsonConfig);
            return config;
        }

        public List<string> GetAllFolder(string directoryPath, string errorLogPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var directoryList = Directory.GetDirectories(directoryPath).Select(Path.GetFileName).ToList();
                    return directoryList;
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var reason = "Invalid File Path";
                _exportCSVService.ErrorLog("Attachment", directoryPath, errorLogPath, reason);
                return new List<string>();
            }
        }

        public List<string> GetAllFilesFromDirectory(string directoryPath, string errorLogPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    var allFiles = Directory.GetFiles(directoryPath, "*").ToList();
                    return allFiles;
                }
                return new List<string>();
            }
            catch (Exception e)
            {
                var error = e.Message;
                var reason = "Invalid File Path";
                _exportCSVService.ErrorLog("Attachment", directoryPath, errorLogPath, reason);
                return new List<string>();
            }

        }

        public string MoveAttachment(string sourcePath, string destinationPath,bool isDelete)
        {
            try
            {
                if (Directory.Exists(destinationPath))
                {
                    var fileName = Path.GetFileName(sourcePath);
                    var destination = destinationPath + Path.GetFileName(fileName);
                    if (File.Exists(destination))
                    {
                        destination = destinationPath + Path.GetFileNameWithoutExtension(fileName) + "copy" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now) + Path.GetExtension(sourcePath);
                    }
                    File.Copy(sourcePath, destination,true);
                    if(isDelete)
                        File.Delete(sourcePath);
                    return destination;
                }
                return string.Empty;
                
            }
            catch (Exception)
            {
                return string.Empty;
            }
           
        }
        #endregion folder
    }
}
