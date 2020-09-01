using Microsoft.Extensions.Configuration;
using Northwind.Core.Import.Interface;
using Northwind.Core.Interfaces;using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleMigration
{
    
    public class Migration
    {
        private readonly IImportService _importService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private string importFileName;
        private string importFilePath;
        private string errorLogPath;
        private string attachmentFileName;
        private string attachmentFilePath;

        public Migration(IImportService importService, IConfiguration configuration,IUserService userService)
        {
            _importService = importService;
            _configuration = configuration;
            _userService = userService;

            importFileName = _configuration.GetSection("CSVImport").GetValue<string>("fileName");
            importFilePath = _configuration.GetSection("CSVImport").GetValue<string>("filePath");
            errorLogPath = _configuration.GetValue<string>("ImportLogPath");

            attachmentFileName = _configuration.GetSection("AttachmentImport").GetValue<string>("attachmentFileName");
            attachmentFilePath = _configuration.GetSection("AttachmentImport").GetValue<string>("attachmentFilePath");
        }

        public bool MigrateData(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                var user = _userService.GetUserByUsername(username.Trim());
                if (user == null)
                    return false;

                //import data
                var fullFilePath = importFilePath + importFileName;
                var loggedUserGuid = user.UserGuid;
                _importService.ImportData(fullFilePath, loggedUserGuid, this.errorLogPath);

                //import attachment
                var attachmentPath = attachmentFilePath + attachmentFileName;
                _importService.ImportAttachment(attachmentPath, loggedUserGuid, this.errorLogPath);
                return true;
            }
            return false;
        }

        public bool IsUsernameValid(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;
            var user = _userService.GetUserByUsername(username.Trim());
            if (user != null)
                return true;
            return false;
        }
    }
}
