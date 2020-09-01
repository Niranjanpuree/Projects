using System.Collections.Generic;

namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class AzureFileModel
    {
        public string BlobName { get; set; }
        public string ContainerName { get; set; }
        public string ContentType { get; set; }
        public List<string> ChunkIdList { get; set; }
    }
}
