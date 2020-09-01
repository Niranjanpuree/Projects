using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IContractImportService
    {
        void ImportContractData(object configuration, Guid userGuid, string errorLogPath,bool isDelete);
    }
}
