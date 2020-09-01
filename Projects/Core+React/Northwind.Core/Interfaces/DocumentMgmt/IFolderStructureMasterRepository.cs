using Northwind.Core.Entities.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.DocumentMgmt
{
    public interface IFolderStructureMasterRepository
    {
        IEnumerable<FolderStructureMaster> GetAll();
        IEnumerable<FolderStructureMaster> GetActive(string module, string resourceType);
    }
}
