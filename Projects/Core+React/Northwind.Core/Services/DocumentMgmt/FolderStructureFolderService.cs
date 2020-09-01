using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services.DocumentMgmt
{
    public class FolderStructureFolderService : IFolderStructureFolderService
    {
        IFolderStructureFolderRepository _repository;

        public FolderStructureFolderService(IFolderStructureFolderRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FolderStructureFolder> GetFolders(Guid? ParentGuid)
        {
            return _repository.GetFolders(ParentGuid);
        }

        public FolderStructureFolder GetFolderTree(Guid FolderStructureMasterGuid)
        {
            return _repository.GetFolderTree(FolderStructureMasterGuid);
        }

        public FolderStructureFolder GetParentFolder(Guid folderStructureFolderGuid)
        {
            return _repository.GetParentFolder(folderStructureFolderGuid);
        }
    }
}
