namespace AutoCare.Product.Web.Models.InputModels
{
    public class DeleteBlobInputModel
    {
        public string FileName { get; set; }
        public string ContainerName { get; set; }
        public string DirectoryPath { get; set; }
    }
}