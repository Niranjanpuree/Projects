using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IContractCloseApprovalRepository
    {
        ContractCloseApproval GetByNormalizedValue(string normalizedValue);
    }
}
