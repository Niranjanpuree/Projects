using System;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class ContractWBSService : IContractWBSService
    {
        private readonly IContractWBSRepository _contractWBSRepository;
        public ContractWBSService(IContractWBSRepository contractWBSRepository)
        {
            _contractWBSRepository = contractWBSRepository;
        }
        public int AddContractWBS(ContractWBS ContractWBS)
        {
            return _contractWBSRepository.AddContractWBS(ContractWBS);
        }
        public int UpdateContractWBS(ContractWBS ContractWBS)
        {
            return _contractWBSRepository.UpdateContractWBS(ContractWBS);
        }
        public ContractWBS GetContractWBSById(Guid id)
        {
            return _contractWBSRepository.GetContractWBSById(id);
        }
        public int DeleteContractWBS(Guid id)
        {
            return _contractWBSRepository.DeleteContractWBS(id);
        }
        public bool updateFileName(Guid wbsGuid,string fileName)
        {
            return _contractWBSRepository.updateFileName(wbsGuid,fileName);
        }
    }
}
