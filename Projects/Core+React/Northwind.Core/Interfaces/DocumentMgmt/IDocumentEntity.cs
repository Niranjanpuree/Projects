using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IDocumentEntity
    {
        Guid ContractResourceFileGuid { get; set; }
        Guid MasterStructureGuid { get; set; }
        Guid MasterFolderGuid { get; set; }
        Guid ResourceGuid { get; set; }
        string Keys { get; set; }
        string ResourceType { get; set; }
        string UploadFileName { get; set; }
        string UploadUniqueFileName { get; set; }
        string MimeType { get; set; }
        bool IsActive { get; set; }
        bool IsDeleted { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        string UpdatedByName { get; set; }
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
        bool IsCsv { get; set; }
        string Description { get; set; }
        string FilePath { get; set; }
        string FileSize { get; set; }
        Guid ParentId { get; set; }
        bool IsFile { get; set; }
        bool IsReadOnly { get; set; }
        [Ignore]
        List<IDocumentEntity> Subfolders { get; set; }
        string Type { get; set; }
        int folderfilecounts { get; set; }
    }
}
