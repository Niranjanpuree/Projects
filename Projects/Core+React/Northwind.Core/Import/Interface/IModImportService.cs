using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IModImportService
    {
        void ImportModsData(object configuration, Guid userGuid, string errorLogPath,bool isDelete);
    }
}
