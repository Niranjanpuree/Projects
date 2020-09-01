using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Core.Import.Interface;
using Northwind.Core.Import.Service;
using Northwind.Web.Infrastructure.Helpers;

namespace Northwind.Web.Controllers
{
    public class ImportController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IImportService _importService;
        private string importFileName;
        private string importFilePath;
        private string errorLogPath;
        private string attachmentFileName;
        private string attachmentFilePath;
        public ImportController(IConfiguration configuration, IImportService importService)
        {
            _configuration = configuration;
            _importService = importService;

            importFileName = _configuration.GetSection("CSVImport").GetValue<string>("fileName");
            importFilePath = _configuration.GetSection("CSVImport").GetValue<string>("filePath");
            errorLogPath = _configuration.GetValue<string>("ImportLogPath");

            attachmentFileName = _configuration.GetSection("AttachmentImport").GetValue<string>("attachmentFileName");
            attachmentFilePath = _configuration.GetSection("AttachmentImport").GetValue<string>("attachmentFilePath");
        }
        public IActionResult Index()
        {
            var fullFilePath = importFilePath + importFileName;
            var loggedUserGuid = UserHelper.CurrentUserGuid(HttpContext);
            _importService.ImportData(fullFilePath, loggedUserGuid, this.errorLogPath);

            var attachmentPath = attachmentFilePath + attachmentFileName;
            _importService.ImportAttachment(attachmentPath, loggedUserGuid, this.errorLogPath);
            return View();
        }


    }
}