using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IFolderStructureFolderRepository
    {
        IEnumerable<FolderStructureFolder> GetFolders(Guid? ParentGuid);
        FolderStructureFolder GetFolderTree(Guid FolderStructureMasterGuid);
        FolderStructureFolder GetParentFolder(Guid folderStructureFolderGuid);
    }
}
