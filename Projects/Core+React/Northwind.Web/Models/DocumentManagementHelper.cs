using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Northwind.Web.Models
{
    public class DocumentManagementHelper
    {

        public static string getValidFolderName(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str))
                {
                    return string.Empty;
                }
                var fileInfo = new FileInfo(str);
                var name = fileInfo.Name;
                //string protectedCharacters = "/:*?\"|";
                return str.Replace("\\", "_").Replace("/", "_").Replace(":", "_").Replace("*", "_").Replace("?", "_").Replace("\"", "_").Replace("|", "");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
