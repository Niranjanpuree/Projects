using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface ICustomerContactTypeRepository 
    {
        IEnumerable<CustomerContactType> GetCustomerContactList();

        Guid GetCustomerContactTypeGuidByTypeName(string custonerContactTypeName);
    }
}
