using Northwind.Core.Entities;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IContractWBSRepository
    {
        int AddContractWBS(ContractWBS ContractWBS);
        int UpdateContractWBS(ContractWBS ContractWBS);
        int DeleteContractWBS(Guid id);
        ContractWBS GetContractWBSById(Guid id);
        bool updateFileName(Guid wbsGuid, string fileName);
    }
}