using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Core.Entities
{
    public class ContractWBS:BaseModel
    {
        public Guid ContractWBSGuid { get; set; }
        public Guid ContractGuid { get; set; }
        public string UploadFileName { get; set; }
        public IFormFile FileToUpload { get; set; }
        public bool IsCsv { get; set; }

        public string WBSCode { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ContractType { get; set; }
        public string InvoiceAtThisLevel { get; set; }
        public int sn { get; set; }
    }
}
