using Northwind.Core.Entities.ContractRefactor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.ContractRefactor
{
    public interface IContractFileRepository
    {
        IEnumerable<ContractResourceFile> GetFileListByContractGuid(Guid contractGuid);

        ContractResourceFile GetFilesByContractGuid(Guid contractGuid);

        bool InsertContractFile(ContractResourceFile file);

        bool UpdateContractFile(ContractResourceFile file);
    }
}
