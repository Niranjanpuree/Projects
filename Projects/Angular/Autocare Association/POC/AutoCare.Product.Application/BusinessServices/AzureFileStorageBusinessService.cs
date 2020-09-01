using System;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Application.RepositoryServices;

namespace AutoCare.Product.Application.BusinessServices
{
    public class AzureFileStorageBusinessService : IAzureFileStorageBusinessService
    {
        private readonly IAzureFileStorageRepositoryService _azureFileStorageRepositoryService;

        public AzureFileStorageBusinessService(IAzureFileStorageRepositoryService azureFileStorageRepositoryService)
        {
            _azureFileStorageRepositoryService = azureFileStorageRepositoryService;
        }

        public async Task SaveFileChunkAsync(AzureFileChunkModel azureFileChunkModel)
        {
            await _azureFileStorageRepositoryService.SaveFileChunkAsync(azureFileChunkModel);
        }

        public async Task<Uri> SaveFileAsync(AzureFileModel azureFileModel)
        {
            return await _azureFileStorageRepositoryService.SaveFileAsync(azureFileModel);
        }

        public Uri GetReadOnlyFileUrl(string containerName, string blobName, string directoryPath = null, int readAccessTimeOutInMinutes = 5)
        {
            return _azureFileStorageRepositoryService.GetReadOnlyFileUrl(containerName, blobName, directoryPath, readAccessTimeOutInMinutes);
        }

        public async Task MoveFileAsync(string sourceContainerName, string targetContainerName, string blobName)
        {
            await
                _azureFileStorageRepositoryService.MoveFilesAsync(sourceContainerName, targetContainerName, blobName);
        }

        public async Task DeleteAsync(string containerName, string blobName = null, string directoryPath = null)
        {
            await
                _azureFileStorageRepositoryService.DeleteAsync(containerName, blobName, directoryPath);
        }
    }
}
