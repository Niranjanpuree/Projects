using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure;
using AutoCare.Product.Application.Models.BusinessModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AutoCare.Product.Application.RepositoryServices
{
    public class AzureFileStorageRepositoryService : IAzureFileStorageRepositoryService
    {
        private readonly CloudBlobClient _cloudClient;
        public AzureFileStorageRepositoryService()
        {
            var blobStorageAccount = CloudStorageAccount.Parse(AppSettingConfiguration.Instance.StorageAccountConnectionStringName);
            _cloudClient = blobStorageAccount.CreateCloudBlobClient();
        }
        public async Task SaveFileChunkAsync(AzureFileChunkModel azureFileChunkModel)
        {
            //TODO: Validations
            try
            {
                var container = _cloudClient.GetContainerReference(azureFileChunkModel.ContainerName);
                await container.CreateIfNotExistsAsync();
                var blockBlock = container.GetBlockBlobReference(azureFileChunkModel.BlobName);
                using (var chunkStream = new MemoryStream(azureFileChunkModel.Blocks))
                {
                    try
                    {
                        blockBlock.PutBlock(azureFileChunkModel.BlockId, chunkStream, null);
                    }
                    catch (StorageException e)
                    {
                        throw new StorageException("File upload failed");
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Uri> SaveFileAsync(AzureFileModel azureFileModel)
        {
            //TODO: Validations
            try
            {
                var container = _cloudClient.GetContainerReference(azureFileModel.ContainerName);
                await container.CreateIfNotExistsAsync();
                var blockBlob = container.GetBlockBlobReference(azureFileModel.BlobName);
                blockBlob.Properties.ContentType = azureFileModel.ContentType;

                blockBlob.PutBlockList(azureFileModel.ChunkIdList);

                return GetReadOnlyFileUrl(azureFileModel.ContainerName, azureFileModel.BlobName);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public Uri GetReadOnlyFileUrl(string containerName, string blobName, string directoryPath = null,
            int readAccessTimeOutInMinutes = 5)
        {
            if (!String.IsNullOrWhiteSpace(directoryPath))
            {
                blobName = string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), directoryPath,
                    blobName);
            }

            var container = _cloudClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(blobName);
            var readPolicy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(readAccessTimeOutInMinutes)
            };

            return new Uri(blockBlob.Uri.AbsoluteUri + blockBlob.GetSharedAccessSignature(readPolicy));
        }

        public async Task MoveFilesAsync(string sourceContainerName, string targetContainerName, string targetDirectoryPath)
        {
            //Get Reference of source blob
            var sourceContainer = _cloudClient.GetContainerReference(sourceContainerName);
            if (sourceContainer == null)
            {
                throw new ArgumentException("Invalid source container");
            }

            var blobsName = sourceContainer.ListBlobs()
                .OfType<CloudBlockBlob>()
                .Select(b => b.Name)
                .ToList();

            foreach (var blobName in blobsName)
            {
                await MoveFileAsync(sourceContainer, targetContainerName, blobName, targetDirectoryPath);
            }
            
            //Delete source container
            await sourceContainer.DeleteAsync();
        }

        public async Task<Uri> MoveFileAsync(string sourceContainerName, string targetContainerName,
            string sourceBlobName, string targetDirectoryPath)
        {
            //Get Reference of source blob
            var sourceContainer = _cloudClient.GetContainerReference(sourceContainerName);
            return await MoveFileAsync(sourceContainer, targetContainerName, sourceBlobName, targetDirectoryPath);
        }

        private async Task<Uri> MoveFileAsync(CloudBlobContainer sourceContainer, string targetContainerName,
            string sourceBlobName, string targetDirectoryPath)
        {
            //Get Reference of source blob
            if (sourceContainer == null)
            {
                throw new ArgumentException("Invalid source container");
            }

            //Get Reference of target blob
            var targetContainer = _cloudClient.GetContainerReference(targetContainerName);
            await targetContainer.CreateIfNotExistsAsync();

            var sourceBlob = sourceContainer.GetBlockBlobReference(sourceBlobName);
            var targetBlobName = sourceBlobName;

            if (!String.IsNullOrWhiteSpace(targetDirectoryPath))
            {
                targetBlobName = string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                    targetDirectoryPath, targetBlobName);
            }

            var targetBlob = targetContainer.GetBlockBlobReference(targetBlobName);
            await targetBlob.StartCopyAsync(sourceBlob.Uri);

            while (targetBlob.CopyState.Status == CopyStatus.Pending)
            {
                await Task.Delay(100);
            }

            //using (var stream = new MemoryStream())
            //{
            //    sourceBlob.DownloadToStream(stream);
            //    stream.Seek(0, SeekOrigin.Begin);

            //    //Copy source blob to target blob
            //    targetBlob.UploadFromStream(stream);
            //}

            //Delete Source Blob
            await sourceBlob.DeleteIfExistsAsync();

            //Delete container if no item exist
            if (!sourceContainer.ListBlobs().Any())
            {
                await sourceContainer.DeleteIfExistsAsync();
            }

            return GetReadOnlyFileUrl(targetContainerName, targetBlobName);
        }

        public async Task DeleteAsync(string containerName, string blobName = null, string directoryPath = null)
        {
            var container = _cloudClient.GetContainerReference(containerName);

            if (String.IsNullOrWhiteSpace(blobName))
            {
                await container.DeleteIfExistsAsync();
                return;
            }

            if (!String.IsNullOrWhiteSpace(directoryPath))
            {
                blobName = string.Join(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture),
                    directoryPath, blobName);
            }

            var blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}
