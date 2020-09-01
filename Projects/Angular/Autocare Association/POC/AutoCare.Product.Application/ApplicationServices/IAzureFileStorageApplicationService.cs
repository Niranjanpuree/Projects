using System;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IAzureFileStorageApplicationService
    {
        Task SaveFileChunkAsync(AzureFileChunkModel azureFileChunkModel);
        Task<Uri> SaveFileAsync(AzureFileModel azureFileModel);
        Uri GetReadOnlyFileUrl(string containerName, string blobName, string directoryPath = null, int readAccessTimeOutInMinutes = 5);
        Task MoveFileAsync(string sourceContainerName, string targetContainerName, string blobName);
        Task DeleteAsync(string containerName, string blobName = null, string directoryPath = null);
    }
}
