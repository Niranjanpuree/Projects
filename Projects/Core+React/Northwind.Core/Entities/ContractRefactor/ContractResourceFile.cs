using CsvHelper.Configuration.Attributes;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities.ContractRefactor
{
    public class ContractResourceFile : IDocumentEntity
    {
        public Guid ContractResourceFileGuid { get; set; }
        public Guid MasterStructureGuid { get; set; }
        public Guid MasterFolderGuid { get; set; }
        public Guid ResourceGuid { get; set; }
        public string Keys { get; set; }
        public string UploadFileName { get; set; }
        public string UploadUniqueFileName { get; set; }
        public string MimeType { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsCsv { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public Guid ParentId { get; set; }
        public bool IsFile { get; set; }
        public bool IsReadOnly { get; set; }
        public List<IDocumentEntity> Subfolders { get; set; }
        public string Type { get; set; }
        public string ResourceType { get; set; }
        public Guid ContentResourceGuid { get; set; }
        public string UpdatedByName { get; set; }
        public int folderfilecounts { get ; set ; }
    }
}
