using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IExportCSVService
    {
        bool SaveCSVWithStatus<T>(List<T> dataList, string path, string fileName, Dictionary<string, string> header);
        Dictionary<string, string> GetExportCSVHeader(Dictionary<string, string> header);
        string GetExportFileName(string originalFileName, string fileNameWithExtension, string extension);
        void ErrorLog(string file, string filePath, string errorLogPath, string reason);
    }
}
