using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface ICompanyImportService
    {
        void ImportCompanyData(object configuration, Guid userGuid, string errorLogPath,bool isDelete);
        IList<DMCompany> ImportCompany(List<DMCompany> regionList, Guid userGuid);
    }
}
