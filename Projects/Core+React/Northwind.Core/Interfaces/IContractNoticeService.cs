using Northwind.Core.Entities;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Interfaces
{
    public interface IContractNoticeService
    {
       
        int Add(ContractNotice contractNoticeModel);
        ContractNotice GetDetailById(Guid ContractNotice);
        IEnumerable<ContractNotice> GetDetailsByNoticeType(string NoticeType, Guid ResourceId, string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<ContractNotice> GetContractNoticeByContractId(Guid contractGuid, string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<ContractResourceFile> GetAttachmentsByResourceGuid(Guid resourceGuid);
        ContractResourceFile GetParentId(string ContractGuid, Guid ResourceId);
        int GetNoticeCount(Guid contractGuid);
        int GetNoticeViewDetailsCount(string NoticeType, Guid ResourceId);


    }
}