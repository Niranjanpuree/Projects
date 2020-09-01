using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Northwind.Core.Import.Service
{
    public class ExportCSVService : IExportCSVService
    {
        //private void CreateHeader(Dictionary<string, string> header, StreamWriter sw)
        //{
        //    foreach (var item in header)
        //    {
        //        sw.Write(item.Value + ",");
        //    }
        //    sw.Write(sw.NewLine);
        //}

        private void CreateHeader(Dictionary<string, string> header, StreamWriter sw )
        {
            //foreach (var item in keyValueOject as IDictionary<string, object>)
            //{
            //    sw.Write(header[item.Key] + ",");
            //}
            //sw.Write(sw.NewLine);

            foreach (var item in header)
            {
                sw.Write(item.Value + ",");
            }
            sw.Write(sw.NewLine);
        }


        private bool CreateRows<T>(List<T> list, StreamWriter sw)
        {
            foreach (var item in list)
            {
                //PropertyInfo[] properties = typeof(T).GetProperties();
                //for (int i = 0; i < properties.Length - 1; i++)
                //{
                //    var prop = properties[i];
                //    var value = prop.GetValue(item);
                //    if (value == null)
                //        value = "";
                //    //if (prop.GetValue(item).ToString().Contains(","))
                //    //    sw.Write(String.Format("\"{0}\",", prop.GetValue(item).ToString()));
                //    if (value.ToString().Contains(","))
                //        sw.Write(String.Format("\"{0}\",", value.ToString()));
                //    else
                //        sw.Write(prop.GetValue(item) + ",");

                //}
                foreach (var i in item as IDictionary<string, object>)
                {
                    var prop = i.Key;
                    var value = i.Value;
                    if (value == null)
                        value = " ";
                    //if (value.ToString().Contains(","))
                    //    sw.Write(String.Format("\"{0}\",", value.ToString()));
                    if (value.ToString().Contains(","))
                        sw.Write(String.Format("\"{0}\",", value.ToString()));
                    else
                        sw.Write(value + ",");

                }
                //var lastProp = properties[properties.Length - 1];
                sw.Write(sw.NewLine);
            }
            sw.Close();
            return true;
        }

        public bool SaveCSVWithStatus<T>(List<T> dataList, string path, string fileNameWithExtension, Dictionary<string, string> header)
        {
            try
            {
                var fullPath = Path.Combine(path, fileNameWithExtension);
                if (Directory.Exists(path))
                {
                    using (StreamWriter sw = new StreamWriter(fullPath))
                    {
                        CreateHeader(header, sw);
                        return CreateRows(dataList, sw);
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }

        public Dictionary<string, string> GetExportCSVHeader(Dictionary<string, string> header)
        {
            if (!header.ContainsKey("ImportStatus"))
                header.Add("ImportStatus", "Import Status");
            if (!header.ContainsKey("Reason"))
                header.Add("Reason", "Reason");
            return header;
        }
                
        public string GetExportFileName(string originalFileName, string fileName, string extension)
        {
            var formattedName = "{" + originalFileName + "}" + '-' + fileName + "_{" + DateTime.UtcNow.ToString("MM-dd-yyyy-hh-mm-ss") + "}" + extension;
            return formattedName;
        }

        public void ErrorLog(string file, string filePath, string errorLogPath, string reason)
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("File", "File");
            dictionary.Add("FilePath", "FilePath");
            dictionary.Add("status", "Status");
            dictionary.Add("Reason", "Reason");
            dictionary.Add("ImportDate", "ImportDate");

            var data = new DMErrorLog
            {
                File = file,
                FilePath = filePath,
                Status = "Failed",
                Reason = reason,
                ImportDate = DateTime.UtcNow
            };
            var list = new List<DMErrorLog>();
            list.Add(data);
            SaveCSVWithStatus(list, errorLogPath, file, dictionary);
        }

    }
}
