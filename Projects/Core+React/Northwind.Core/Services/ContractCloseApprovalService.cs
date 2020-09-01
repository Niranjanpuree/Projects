using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
   public class ContractCloseApprovalService : IContractCloseApprovalService
    {
        private readonly IContractCloseApprovalRepository _contractCloseApprovalRepository;
        public ContractCloseApprovalService(IContractCloseApprovalRepository contractCloseApprovalRepository)
        {
            _contractCloseApprovalRepository = contractCloseApprovalRepository;
        }

        public ContractCloseApproval GetByNormalizedValue(string normalizedValue)
        {
            return _contractCloseApprovalRepository.GetByNormalizedValue(normalizedValue);
        }
    }
}
