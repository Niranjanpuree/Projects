using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class AttachmentInputModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string ChunksIdList { get; set; }
        public string AttachedBy { get; set; }
        public FileStatus FileStatus { get; set; }
        public string DirectoryPath { get; set; }
    }
}