using Northwind.Core.Entities.DocumentMgmt;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services.DocumentMgmt
{
    public class FolderStructureMasterService : IFolderStructureMasterService
    {
        IFolderStructureMasterRepository _repository;
        public FolderStructureMasterService(IFolderStructureMasterRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FolderStructureMaster> GetActive(string module, string resourceType)
        {
            return _repository.GetActive(module, resourceType);
        }

        public IEnumerable<FolderStructureMaster> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
