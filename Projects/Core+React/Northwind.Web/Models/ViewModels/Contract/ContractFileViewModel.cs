using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Contract
{
    public class ContractFileViewModel
    {
        public Guid ContractResourceFileGuid { get; set; }
        public Guid ResourceGuid { get; set; }
        public Guid MasterFolderGuid { get; set; }
        public string UploadFileName { get; set; }
        public string UploadUniqueFileName { get; set; }
        public string keys { get; set; }
        public string ResourceType { get; set; }
        public string MimeType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsCsv { get; set; }
        public IFormFile FileToUpload { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public bool IsReadonly { get; set; }
        public bool Isfile { get; set; }
        public Guid? ParentId { get; set; }
        public Guid ContentResourceGuid { get; set; }
    }
}
