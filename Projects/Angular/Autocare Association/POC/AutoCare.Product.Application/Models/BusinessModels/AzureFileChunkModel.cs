namespace AutoCare.Product.Application.Models.BusinessModels
{
    public class AzureFileChunkModel
    {
        public string BlobName { get; set; }
        public string ContainerName { get; set; }
        public string BlockId { get; set; }
        public byte[] Blocks{ get; set; }
    }
}
