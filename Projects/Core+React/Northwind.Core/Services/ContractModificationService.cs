
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class ContractModificationService : IContractModificationService
    {
        private readonly IContractModificationRepository contractModificationRepository;
        public ContractModificationService(IContractModificationRepository contractModificationRepository)
        {
            this.contractModificationRepository = contractModificationRepository;
        }
        public IEnumerable<ContractModification> GetAll(Guid contractGuid,bool isTaskModification, string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<ContractModification> getAll = contractModificationRepository.GetAll(contractGuid, isTaskModification, searchValue, pageSize, skip, sortField, sortDirection);
            return getAll;
        }
        public int TotalRecord(Guid contractGuid, bool isTaskModification)
        {
            int totalRecord = contractModificationRepository.TotalRecord(contractGuid,isTaskModification);
            return totalRecord;
        }
        public int Add(ContractModification contractModificationModel)
        {
            return contractModificationRepository.Add(contractModificationModel);
        }
        public int Edit(ContractModification contractModificationModel)
        {
            return contractModificationRepository.Edit(contractModificationModel);
        }
        public int Delete(Guid[] ids)
        {
            return contractModificationRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return contractModificationRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return contractModificationRepository.Enable(ids);
        }
        public ContractModification GetDetailById(Guid id)
        {
            return contractModificationRepository.GetDetailById(id);
        }
        public bool IsExistModificationNumber(Guid contractGuid, Guid contractModificationGuid, string modificationNumber)
        {
            return contractModificationRepository.IsExistModificationNumber(contractGuid, contractModificationGuid, modificationNumber);
        }
        public bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid)
        {
            return contractModificationRepository.InsertRevenueRecognitionGuid(revenueRecognition, contractGuid);
        }

        public bool UpdateRevenueRecognitionGuid(Guid modGuid, decimal? awardAmount, decimal? fundingAmount)
        {
            return contractModificationRepository.UpdateRevenueRecognitionGuid(modGuid, awardAmount, fundingAmount);
        }

        public ContractModification getAwardAndFundingAmountbyId(Guid id)
        {
            return contractModificationRepository.getAwardAndFundingAmountbyId(id);
        }

        public ContractModification GetTotalAwardAmount(Guid id)
        {
            return contractModificationRepository.GetTotalAwardAmount(id);
        }

        public bool IsExistModificationTitle(Guid contractGuid, Guid modificationGuid,string modificationTitle)
        {
            return contractModificationRepository.IsExistModificationTitle(contractGuid, modificationGuid, modificationTitle);
        }

        public ContractModification GetModByContractGuidAndModNumber(Guid contractGuid, string modNumber)
        {
            if (contractGuid == Guid.Empty || string.IsNullOrWhiteSpace(modNumber))
                return null;
            return contractModificationRepository.GetModByContractGuidAndModNumber(contractGuid, modNumber);
        }
    }
}
