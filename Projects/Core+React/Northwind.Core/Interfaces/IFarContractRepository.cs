using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IFarContractRepository
    {
        void Add(FarContract farContract);
        void AddRequiredData(FarContract farContract);
        List<FarContractDetail> GetAvailableAndOptional(Guid contractGuid, Guid farContractTypeGuid);
        List<FarContractDetail> GetAvailableAndOptionalList(Guid contractGuid, Guid farContractType, int pageSize, int skip, string text);
        List<FarContractDetail> GetRequiredData(Guid contractGuid, Guid farContractTypeGuid);
        List<FarContractDetail> GetSelectedData(Guid contractGuid, Guid farContractTypeGuid);
        void Delete(Guid contractGuid);
        void SoftDelete(Guid contractGuid);
        FarContract GetAvailableFarContractByContractGuid(Guid contractGuid);
    }
}
