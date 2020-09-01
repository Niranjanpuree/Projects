using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IContractModificationRepository
    {
        IEnumerable<ContractModification> GetAll(Guid contractGuid, bool isTaskModification, string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid contractGuid, bool isTaskModification);
        int Add(ContractModification contractModificationModel);
        int Edit(ContractModification contractModificationModel);
        int Delete(Guid[] ids);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);
        ContractModification GetDetailById(Guid id);
        ContractModification GetTotalAwardAmount(Guid id);
        bool IsExistModificationNumber(Guid contractGuid, Guid contractModificationGuid, string modificationNumber);
        bool IsExistModificationTitle(Guid contractGuid, Guid modificationGuid, string modificationTitle);
        bool InsertRevenueRecognitionGuid(Guid revenueRecognition, Guid contractGuid);
        bool UpdateRevenueRecognitionGuid(Guid modGuid, decimal? awardAmount, decimal? fundingAmount);
        ContractModification getAwardAndFundingAmountbyId(Guid id);

        ContractModification GetModByContractGuidAndModNumber(Guid contractGuid, string modNumber);
    }
}