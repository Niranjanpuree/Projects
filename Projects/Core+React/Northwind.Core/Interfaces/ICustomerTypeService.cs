using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface ICustomerTypeService
    {
        IEnumerable<CustomerType> GetCustomerTypeList();

        string GetCustomerTypeByGuid(Guid customerTypeGuid);
        Guid GetCustomerTypeByName(string customerTypeName);
    }
}
