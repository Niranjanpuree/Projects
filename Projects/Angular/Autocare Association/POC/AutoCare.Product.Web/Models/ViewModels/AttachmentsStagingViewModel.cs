using System;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class AttachmentsStagingViewModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public string DirectoryPath { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string AttachedBy { get; set; }
        public string FileUri { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public bool CanDelete { get; set; }
        public string ContainerName { get; set; }
    }
}