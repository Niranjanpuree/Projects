using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Northwind.Core.Entities;
using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Northwind.Core.Services.DocumentMgmt
{
    public class DocumentManagementService : IDocumentManagementService
    {
        IDocumentManagementRepository _repository;
        IConfiguration _configuration;
        public DocumentManagementService(IDocumentManagementRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public void CreateFileOrFolder(IDocumentEntity entity)
        {
            _repository.CreateFileOrFolder(entity);
        }

        public void DeleteFilesAndFoldersByGuid(string resourceType, Guid fileGuid)
        {
            _repository.DeleteFilesAndFoldersByGuid(resourceType, fileGuid);
        }

        public IDocumentEntity GetFileOrFolder(Guid fileGuid)
        {
            return _repository.GetFileOrFolder(fileGuid);
        }

        public IEnumerable<IDocumentEntity> GetFilesAndFoldersByGuid(string resourceType, Guid fileGuid)
        {
            return _repository.GetFilesAndFoldersByGuid(resourceType, fileGuid);
        }

        public IEnumerable<IDocumentEntity> GetFilesAndFoldersByParentId(string resourceType, Guid parentId)
        {
            return _repository.GetFilesAndFoldersByParentId(resourceType, parentId);
        }

        public IDocumentEntity GetFolderByMasterFolderGuid(Guid masterFolderGuid, string resourceType, Guid resourceGuid)
        {
            return _repository.GetFolderByMasterFolderGuid(masterFolderGuid, resourceType, resourceGuid);
        }

        public IDocumentEntity GetFolderTree(string resourceType, Guid resourceGuid)
        {
            return _repository.GetFolderTree(resourceType, resourceGuid);
        }
        public IDocumentEntity GetFilesAndFolderTree(string resourceType, Guid resourceId)
        {
            return _repository.GetFilesAndFolderTree(resourceType, resourceId);
        }

        public bool HasDefaultStructure(string resourceType, Guid resourceGuid)
        {
            return _repository.HasDefaultStructure(resourceType, resourceGuid);
        }

        public void ManageDefaultStructure(string parentFolder, string projectNumber, FolderStructureFolder templateFolder, string resourceType, Guid resourceGuid, Guid createdBy)
        {
            int i = 0;
            var entity = templateFolder;
            var documentRoot = _configuration.GetSection("Document:DocumentRoot").Value;

            projectNumber = !string.IsNullOrWhiteSpace(projectNumber) ? projectNumber.Trim() : string.Concat("(NotProvided)", "-", parentFolder);
            var relativePath = "/" + projectNumber;


            var doc = new DocumentEntity
            {
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow,
                Description = entity.Description,
                FilePath = relativePath,
                FileSize = "0",
                IsActive = true,
                IsCsv = false,
                IsDeleted = false,
                IsFile = false,
                IsReadOnly = true,
                ResourceType = resourceType,
                Keys = resourceType,
                MimeType = "Folder",
                ResourceGuid = resourceGuid,
                UpdatedBy = createdBy,
                UpdatedOn = DateTime.UtcNow,
                UploadFileName = projectNumber,
                UploadUniqueFileName = projectNumber,
                ContractResourceFileGuid = Guid.NewGuid()
            };
            CreateFileOrFolder(doc);

            if (i == 0)
            {
                //Create base folder by resource guid..
                relativePath = "/" + projectNumber;
                i++;
            }
            else
            {

                relativePath = "/" + projectNumber;
                i++;
            }

            Directory.CreateDirectory(documentRoot + relativePath);
            ManageChildFolderStructure(doc.ContractResourceFileGuid, entity, resourceType, resourceGuid, createdBy, relativePath);
        }

        public void UpdateFileOrFolder(IDocumentEntity entity)
        {
            _repository.UpdateFileOrFolder(entity);
        }

        private void ManageChildFolderStructure(Guid ParentGuid, FolderStructureFolder entity, string resourceType, Guid resourceGuid, Guid createdBy, string parentFolder)
        {
            var documentRoot = _configuration.GetSection("Document:DocumentRoot").Value;

            foreach (var c in entity.Children)
            {
                var path = string.Concat(documentRoot, parentFolder, "/", c.Name);
                var relativePath = path.Replace(documentRoot, "");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var doc = new DocumentEntity
                {
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow,
                    Description = c.Description,
                    FilePath = relativePath,
                    FileSize = "0",
                    IsActive = true,
                    IsCsv = false,
                    IsDeleted = false,
                    IsFile = false,
                    IsReadOnly = true,
                    Keys = c.Keys,
                    ResourceType = resourceType,
                    MasterFolderGuid = c.FolderStructureFolderGuid,
                    MasterStructureGuid = c.FolderStructureMasterGuid,
                    MimeType = "Folder",
                    ParentId = ParentGuid,
                    ResourceGuid = resourceGuid,
                    Type = "Folder",
                    UpdatedBy = createdBy,
                    UpdatedOn = DateTime.UtcNow,
                    UploadFileName = c.Name,
                    UploadUniqueFileName = c.Name,
                    ContractResourceFileGuid = Guid.NewGuid()
                };
                CreateFileOrFolder(doc);
                Directory.CreateDirectory(documentRoot + doc.FilePath);
                ManageChildFolderStructure(doc.ContractResourceFileGuid, c, resourceType, resourceGuid, createdBy, relativePath);
            }
        }


        public IEnumerable<IDocumentEntity> GetKeyFolder(string resourceType, Guid resourceGuid, string keys = "")
        {
            return _repository.GetKeyFolder(resourceType, resourceGuid, keys);
        }

        public IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, Guid parentId, string searchString)
        {
            return _repository.SearchFilesAndFolders(resourceType, parentId, searchString);
        }

        public IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, string inRoute, string searchString)
        {
            return _repository.SearchFilesAndFolders(resourceType, inRoute, searchString);
        }

        public IDocumentEntity GetFolderByKey(string resourceType, Guid resourceId, string key)
        {
            return _repository.GetFolderByKey(resourceType, resourceId, key);
        }

        public IDocumentEntity RenameRootFolder(string resourceType, string folderName, Guid resourceId, Guid userGuid)
        {
            var result = _repository.GetFolders(resourceType, resourceId, Guid.Empty).ToList();
            if (result.Count > 0)
            {
                var documentRoot = _configuration.GetSection("Document").GetSection("DocumentRoot").Value;                
                var folder = result[0];
                var oldFolderPath = folder.FilePath;
                folder.UploadFileName = folderName;
                folder.UploadUniqueFileName = folder.UploadFileName;
                folder.FilePath= "/" + folderName;
                folder.UpdatedBy = userGuid;
                folder.UpdatedOn = DateTime.UtcNow;
                Directory.Move(Path.Combine(documentRoot, oldFolderPath.Replace("/","").Replace("\\","")), Path.Combine(documentRoot, folder.FilePath.Replace("/","").Replace("\\","")));
                _repository.UpdateFileOrFolder(folder);
                onRenameChildFoldersAndFiles(folder, userGuid);
                return folder;
            }
            return null;
        }

        private void onRenameChildFoldersAndFiles(IDocumentEntity entity, Guid userGuid)
        {
            var result = _repository.GetFilesAndFoldersByParentId(entity.ResourceType, entity.ContractResourceFileGuid);
            foreach(var folder in result)
            {
                folder.FilePath = entity.FilePath + "/" + folder.UploadFileName;
                folder.UploadUniqueFileName = folder.UploadFileName;
                folder.UpdatedBy = userGuid;
                folder.UpdatedOn = DateTime.UtcNow;
                _repository.UpdateFileOrFolder(folder);
                if (folder.IsFile == false)
                {
                    onRenameChildFoldersAndFiles(folder, userGuid);
                }
            }
        }
    }
}
