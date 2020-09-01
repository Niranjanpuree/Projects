using System;

namespace AutoCare.Product.Vcdb.Model
{
    public class Attachments
    {
        public long AttachmentId { get; set; }
        public long ChangeRequestId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string AzureContainerName { get; set; }
        public string DirectoryPath { get; set; }
        public string AttachedBy { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public FileStatus FileStatus { get; set; }

        public ChangeRequestStore ChangeRequestStore { get; set; }
        
    }
}
