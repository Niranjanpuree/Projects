using Northwind.Core.Import.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Import.Interface
{
    public interface ICustomerContactImportService
    {
        void ImportCustomerContactData(object configuration, Guid userGuid, string errorPath,bool isDelete);
    }
}
