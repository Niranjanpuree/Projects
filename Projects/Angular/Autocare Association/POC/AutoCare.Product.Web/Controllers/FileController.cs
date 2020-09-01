using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoCare.Product.Application.ApplicationServices;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Web.Models.InputModels;

namespace AutoCare.Product.Web.Controllers
{
    public class FileController : Controller
    {
        private readonly IAzureFileStorageApplicationService _azureFileStorageApplicationService;

        public FileController(IAzureFileStorageApplicationService azureFileStorageApplicationService)
        {
            _azureFileStorageApplicationService = azureFileStorageApplicationService;
        }

        public string GetReadOnlyFileUri(string containerName, string fileName)
        {
            return _azureFileStorageApplicationService.GetReadOnlyFileUrl(containerName, fileName).ToString();
        }

        // POST api/<controller>
        public async Task<ActionResult> SaveChunk()
        {
            //var httpRequest = HttpContext.Current.Request;
            var httpRequest = Request;
            var azureFileChunkModel = new AzureFileChunkModel()
            {
                ContainerName = httpRequest.Form["tempContainerPath"] ?? Guid.NewGuid().ToString(),
                BlobName = httpRequest.Form["name"],
                BlockId = httpRequest.Form["chunkId"],
            };

            var isLastChunk = Convert.ToBoolean(httpRequest.Form["isLastChunk"]);
            var file = httpRequest.Files["fileChunk"];
            var chunkIdList = httpRequest.Form["chunkIdList"];
            var contentType = httpRequest.Form["contentType"];
            var autoCommitEnabled = Convert.ToBoolean(httpRequest.Form["autoCommitEnabled"]);

            if (file == null || String.IsNullOrWhiteSpace(azureFileChunkModel.BlobName) ||
                String.IsNullOrWhiteSpace(azureFileChunkModel.BlockId))
            {
                throw new ArgumentNullException("Name, ChunkId and File content all are required");
            }

            azureFileChunkModel.Blocks = new byte[file.ContentLength];
            file.InputStream.Read(azureFileChunkModel.Blocks, 0, Convert.ToInt32(file.ContentLength));

            await _azureFileStorageApplicationService.SaveFileChunkAsync(azureFileChunkModel);

            if (isLastChunk && autoCommitEnabled)
            {
                return await SaveBlob(azureFileChunkModel.ContainerName, azureFileChunkModel.BlobName, chunkIdList, contentType);
            }
            
            return Json(new { tempContainerPath = azureFileChunkModel.ContainerName });
        }

        

        [HttpPost]
        //[CustomAuthorize]
        public async Task<ActionResult> SaveBlob(string containerName, string fileName, string blockList, string contentType)
        {
            if (String.IsNullOrWhiteSpace(containerName) || String.IsNullOrWhiteSpace(fileName) ||
                String.IsNullOrWhiteSpace(blockList))
            {
                throw new ArgumentNullException("ContainerName, FileName and BlockList all are required");
            }

            var azureFileModel = new AzureFileModel()
            {
                ContainerName = containerName,
                BlobName = fileName,
                ChunkIdList = blockList.Split(',').ToList(),
                ContentType = contentType
            };

            var azureFileUri = await _azureFileStorageApplicationService.SaveFileAsync(azureFileModel);

            return
                Json(
                    new
                    {
                        tempContainerPath = containerName,
                        azureUri = azureFileUri
                    });
        }


        [HttpPost]
        //[CustomAuthorize]
        public async Task MoveBlob(string sourceContainerName, string targetContainerName, 
            string fileName)
        {
            await _azureFileStorageApplicationService.MoveFileAsync(sourceContainerName, targetContainerName, fileName);
        }

        [HttpPost]
        public async Task DeleteBlob(List<DeleteBlobInputModel> blobsToBeDeleted)
        {
            // if no item in blobsToBeDeleted
            if (blobsToBeDeleted == null || blobsToBeDeleted.Count == 0) return;

            var tasks = blobsToBeDeleted.Select(async item =>
            {
                await
                    _azureFileStorageApplicationService.DeleteAsync(item.ContainerName, item.FileName,
                        item.DirectoryPath);
            });

            await Task.WhenAll(tasks);
        }
    }
}