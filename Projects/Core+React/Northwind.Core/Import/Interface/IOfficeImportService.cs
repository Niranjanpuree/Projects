using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface IOfficeImportService
    {
        void ImportOfficeData(object configuration, Guid userGuid, string errorLogPath,bool isDelete);
        IList<DMOffice> ImportOffice(List<DMOffice> officeList, Guid userGuid);
    }
}
