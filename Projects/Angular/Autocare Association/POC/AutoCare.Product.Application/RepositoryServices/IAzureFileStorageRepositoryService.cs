using System;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.RepositoryServices
{
    public interface IAzureFileStorageRepositoryService
    {
        Task SaveFileChunkAsync(AzureFileChunkModel azureFileChunkModel);
        Task<Uri> SaveFileAsync(AzureFileModel azureFileModel);
        Uri GetReadOnlyFileUrl(string containerName, string blobName, string directoryPath = null,
            int readAccessTimeOutInMinutes = 5);
        Task MoveFilesAsync(string sourceContainerName, string targetContainerName, string targetDirectoryPath);
        Task<Uri> MoveFileAsync(string sourceContainerName, string targetContainerName,
            string sourceBlobName, string targetDirectoryPath);
        Task DeleteAsync(string containerName, string blobName = null,string directoryPath = null);
    }
}
