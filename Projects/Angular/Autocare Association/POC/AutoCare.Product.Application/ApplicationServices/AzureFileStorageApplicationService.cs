using System;
using System.Threading.Tasks;
using AutoCare.Product.Application.BusinessServices;
using AutoCare.Product.Application.Models.BusinessModels;

namespace AutoCare.Product.Application.ApplicationServices
{
    public class AzureFileStorageApplicationService : IAzureFileStorageApplicationService
    {
        private readonly IAzureFileStorageBusinessService _azureFileStorageBusinessService;

        public AzureFileStorageApplicationService(IAzureFileStorageBusinessService azureFileStorageBusinessService)
        {
            _azureFileStorageBusinessService = azureFileStorageBusinessService;
        }

        public async Task SaveFileChunkAsync(AzureFileChunkModel azureFileChunkModel)
        {
            await _azureFileStorageBusinessService.SaveFileChunkAsync(azureFileChunkModel);
        }

        public async Task<Uri> SaveFileAsync(AzureFileModel azureFileModel)
        {
            return await _azureFileStorageBusinessService.SaveFileAsync(azureFileModel);
        }

        public Uri GetReadOnlyFileUrl(string containerName, string blobName, string directoryPath = null, int readAccessTimeOutInMinutes = 5)
        {
            return _azureFileStorageBusinessService.GetReadOnlyFileUrl(containerName, blobName, directoryPath, readAccessTimeOutInMinutes);
        }

        public async Task MoveFileAsync(string sourceContainerName, string targetContainerName, string blobName)
        {
            await
                _azureFileStorageBusinessService.MoveFileAsync(sourceContainerName, targetContainerName, blobName);
        }

        public async Task DeleteAsync(string containerName, string blobName = null, string directoryPath = null)
        {
            await
                _azureFileStorageBusinessService.DeleteAsync(containerName, blobName, directoryPath);
        }
    }
}
