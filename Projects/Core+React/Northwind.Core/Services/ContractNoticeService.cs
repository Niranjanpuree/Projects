
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Mvc;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Services
{
    public class ContractNoticeService : IContractNoticeService
    {
        private readonly IContractNoticeRepository contractNoticeRepository;
        public ContractNoticeService(IContractNoticeRepository contractNoticeRepository)
        {
            this.contractNoticeRepository = contractNoticeRepository;
        }
    
        public int Add(ContractNotice contractNoticeModel)
        {
            return contractNoticeRepository.Add(contractNoticeModel);
        }
        public ContractResourceFile GetParentId(string key, Guid ResourceId)
        {
            return contractNoticeRepository.GetParentId(key, ResourceId);
        }
        public IEnumerable<ContractNotice> GetContractNoticeByContractId(Guid contractGuid, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            var contract = contractNoticeRepository.GetContractNoticeByContractId(contractGuid,  searchValue,  pageSize,  skip,  take,  sortField,  dir);
            return contract;
        }

        public IEnumerable<ContractResourceFile> GetAttachmentsByResourceGuid(Guid ResourceGuid)
        {
            var contractResourceFile = contractNoticeRepository.GetAttachmentsByResourceGuid(ResourceGuid);
            return contractResourceFile;
        }
        public ContractNotice GetDetailById(Guid contractNoticeGuid)
        {
            return contractNoticeRepository.GetDetailById(contractNoticeGuid);
        }
        
        
        public IEnumerable<ContractNotice> GetDetailsByNoticeType(string NoticeType, Guid ResourceId, string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return contractNoticeRepository.GetDetailByNoticeType(NoticeType, ResourceId,  searchValue,  pageSize,  skip,  take,  sortField,  dir);
        }

       public int GetNoticeCount(Guid contractGuid)
        {
            return contractNoticeRepository.GetNoticeCount(contractGuid);
        }
       public int GetNoticeViewDetailsCount(string NoticeType, Guid ResourceId)
        {
            return contractNoticeRepository.GetNoticeDetailsCount(NoticeType, ResourceId);
        }


    }
}
