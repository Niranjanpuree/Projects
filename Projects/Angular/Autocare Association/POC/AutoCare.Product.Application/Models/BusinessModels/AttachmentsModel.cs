using System;
using AutoCare.Product.Vcdb.Model;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class AttachmentsModel
    {
        public long AttachmentId { get; set; }
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public string DirectoryPath { get; set; }
        public string FileExtension { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public string AttachedBy { get; set; }
        public DateTime? CreatedDatetime { get; set; }
        public string ChunksIdList { get; set; }
        public string FileUri { get; set; }

        public FileStatus FileStatus { get; set; }
    }
}
