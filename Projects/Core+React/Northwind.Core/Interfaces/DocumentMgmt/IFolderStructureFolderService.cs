using Northwind.Core.Entities.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IFolderStructureFolderService
    {
        IEnumerable<FolderStructureFolder> GetFolders(Guid? ParentGuid);
        FolderStructureFolder GetFolderTree(Guid FolderStructureMasterGuid);
        FolderStructureFolder GetParentFolder(Guid folderStructureFolderGuid);
    }
}
