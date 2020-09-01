using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IRegionImportService
    {
        void ImportRegionData(object configuration, Guid userGuid, string errorLogPath, bool isDelete);
    }
}
