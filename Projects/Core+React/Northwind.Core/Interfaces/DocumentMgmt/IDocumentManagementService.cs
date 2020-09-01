using Northwind.Core.Entities;
using Northwind.Core.Entities.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IDocumentManagementService
    {
        bool HasDefaultStructure(string resourceType, Guid resourceGuid);
        void ManageDefaultStructure(string parentFolder, string contractNumber, FolderStructureFolder templateFolder, string resourceType, Guid resourceGuid, Guid createdBy);
        IDocumentEntity GetFolderTree(string resourceType, Guid resourceGuid);
        IDocumentEntity GetFileOrFolder(Guid fileGuid);
        IDocumentEntity GetFolderByMasterFolderGuid(Guid masterFolderGuid, String resourceType, Guid resourceGuid);
        IEnumerable<IDocumentEntity> GetFilesAndFoldersByParentId(string resourceType, Guid parentId);
        IEnumerable<IDocumentEntity> GetFilesAndFoldersByGuid(string resourceType, Guid fileGuid);
        IEnumerable<IDocumentEntity> GetKeyFolder(string resourceType, Guid resourceGuid, string keys = "");
        IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, Guid parentId, string searchString);
        IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, string inRoute, string searchString);
        void DeleteFilesAndFoldersByGuid(string resourceType, Guid fileGuid);
        void CreateFileOrFolder(IDocumentEntity entity);
        void UpdateFileOrFolder(IDocumentEntity entity);
        IDocumentEntity GetFilesAndFolderTree(string resourceType, Guid resourceId);
        IDocumentEntity GetFolderByKey(string resourceType, Guid resourceId,string key);
        IDocumentEntity RenameRootFolder(string resourceType, string folderName, Guid resourceId, Guid userGuid);
    }
}
