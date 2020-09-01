using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IImportFileService
    {
        Dictionary<string, object> GetFileUsingJToken(string json);
        ImportConfiguration GetConfigurationSetting(string jsonConfigData);
        string[] GetAllCsvFilesFromDirectory(string directoryPath, string errorLogPath);
        Dictionary<string, string> GetCSVHeader(string json);
        Dictionary<string,string> GetCSVHeaderFromFile(string filePath, Dictionary<string, string> headerDictionary);
        void MoveFile(string sourcePath, string destinationPath, string fileName, string extension, bool isDelete);

        FileConfiguration GetFileConfigurationSetting(string jsonConfig);
        List<string> GetAllFilesFromDirectory(string directoryPath, string errorLogPath);
        List<string> GetAllFolder(string directoryPath, string errorLogPath);
        string MoveAttachment(string sourcePath, string destinationPath,bool isDelete);
    }
}
