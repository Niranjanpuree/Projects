using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.Core.Interfaces;
using System;

namespace Northwind.Core.Services
{
    public class ContractResourceFileService : IContractResourceFileService
    {
        private readonly IContractResourceFileRepository _iContractResourceFileRepository;
        public ContractResourceFileService(IContractResourceFileRepository ContractResourceFileRepository)
        {
            _iContractResourceFileRepository = ContractResourceFileRepository;
        }

        public ContractResourceFile GetFilePathByResourceIdAndKeys(string resourceKey, Guid resourceId)
        {
            return _iContractResourceFileRepository.GetFilePathByResourceIdAndKeys(resourceKey, resourceId);
        }
    }
}
