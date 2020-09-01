using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
   public class CustomerContactTypeService : ICustomerContactTypeService
    {
        private readonly ICustomerContactTypeRepository _customerContactTypeRepository;
        public CustomerContactTypeService(ICustomerContactTypeRepository customerContactTypeRepository)
        {
            _customerContactTypeRepository = customerContactTypeRepository;
        }

        public IEnumerable<CustomerContactType> GetCustomerContactList()
        {
           return _customerContactTypeRepository.GetCustomerContactList();
        }

        public Guid GetCustomerContactTypeGuidByTypeName(string customerContactTypeName)
        {
            if (string.IsNullOrWhiteSpace(customerContactTypeName))
                return Guid.Empty;
            return _customerContactTypeRepository.GetCustomerContactTypeGuidByTypeName(customerContactTypeName);
        }
    }
}
