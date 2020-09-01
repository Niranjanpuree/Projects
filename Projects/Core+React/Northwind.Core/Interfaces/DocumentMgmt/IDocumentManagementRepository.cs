using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IDocumentManagementRepository
    {
        bool HasDefaultStructure(string resourceType, Guid resourceGuid);
        IEnumerable<IDocumentEntity> GetFolders(string resourceType, Guid resourceGuid, Guid parentGuid, string keys = "");
        IDocumentEntity GetFolderTree(string resourceType, Guid resourceGuid);
        IDocumentEntity GetFilesAndFolderTree(string resourceType, Guid resourceGuid);
        IDocumentEntity GetFileOrFolder(Guid fileGuid);
        IEnumerable<IDocumentEntity> GetFilesAndFoldersByParentId(string resourceType, Guid parentId, string keys = "");
        IEnumerable<IDocumentEntity> GetFilesAndFoldersByGuid(string resourceType, Guid fileGuid, string keys = "");
        IEnumerable<IDocumentEntity> GetKeyFolder(string resourceType, Guid resourceGuid, string keys = "");
        IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, Guid parentId, string searchString);
        IEnumerable<IDocumentEntity> SearchFilesAndFolders(string resourceType, string inRoute, string searchString);
        IDocumentEntity GetFolderByMasterFolderGuid(Guid masterFolderGuid, String resourceType, Guid resourceGuid);
        void DeleteFilesAndFoldersByGuid(string resourceType, Guid fileGuid);
        void CreateFileOrFolder(IDocumentEntity entity);
        void UpdateFileOrFolder(IDocumentEntity entity);
        IDocumentEntity GetFolderByKey(string resourceType, Guid resourceId, string key);
    }
}
