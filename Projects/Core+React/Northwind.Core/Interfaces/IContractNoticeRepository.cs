using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Interfaces
{
    public interface IContractNoticeRepository
    {
        
        int Add(ContractNotice contractNoticeModel);
        ContractNotice GetDetailById(Guid contractNoticeGuid);
        IEnumerable<ContractNotice> GetDetailByNoticeType(string NoticeType,Guid contractNoticeGuid, string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<ContractNotice> GetContractNoticeByContractId(Guid id,string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<ContractResourceFile> GetAttachmentsByResourceGuid(Guid id);
        ContractResourceFile GetParentId(string key, Guid ResourceId);
        int GetNoticeCount(Guid contractGuid);
        int GetNoticeDetailsCount(string NoticeType, Guid ResourceId);
    }
}