using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class FarContractService : IFarContractService
    {
        private IFarContractRepository _farContractRepository;
        public FarContractService(IFarContractRepository farContractRepository)
        {
            _farContractRepository = farContractRepository;
        }

        public void Add(FarContract farContract)
        {
            _farContractRepository.Add(farContract);
        }

        public void AddRequiredData(FarContract farContract)
        {
            _farContractRepository.AddRequiredData(farContract);
        }


        public List<FarContractDetail> GetAvailableAndOptional(Guid contractGuid, Guid farContractTypeGuid)
        {
            return _farContractRepository.GetAvailableAndOptional(contractGuid, farContractTypeGuid);
        }

        public List<FarContractDetail> GetRequiredData(Guid contractGuid, Guid farContractTypeGuid)
        {
            return _farContractRepository.GetRequiredData(contractGuid, farContractTypeGuid);
        }

        public List<FarContractDetail> GetSelectedData(Guid contractGuid, Guid farContractTypeGuid)
        {
            return _farContractRepository.GetSelectedData(contractGuid, farContractTypeGuid);
        }

        public void Delete(Guid contractGuid)
        {
            _farContractRepository.Delete(contractGuid);
        }

        public void SoftDelete(Guid contractGuid)
        {
            _farContractRepository.SoftDelete(contractGuid);
        }

        public List<FarContractDetail> GetAvailableAndOptionalList(Guid contractGuid, Guid farContractType, int pageSize, int skip, string text)
        {
            return _farContractRepository.GetAvailableAndOptionalList(contractGuid, farContractType, pageSize, skip, text);
        }

        public FarContract GetAvailableFarContractByContractGuid(Guid contractGuid)
        {
            if (contractGuid == Guid.Empty)
                return null;
            return _farContractRepository.GetAvailableFarContractByContractGuid(contractGuid);
        }
    }
}
