using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractWBSViewModel : BaseViewModel
    {
        public Guid ContractWBSGuid { get; set; }
        public Guid ContractResourceFileGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile FileToUpload { get; set; }
        public bool IsCsv { get; set; }
        public string Displayname { get; set; }

        public string WBSCode { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ContractType { get; set; }
        public string InvoiceAtThisLevel { get; set; }
        public int sn { get; set; }

        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string UpdatedByDisplayname { get; set; }
    }
}
