using System;
using System.Collections.Generic;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class CustomerTypeService : ICustomerTypeService
    {
        private readonly ICustomerTypeRepository _customerTypeRepository;
        public CustomerTypeService(ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeRepository = customerTypeRepository;
        }
        public IEnumerable<CustomerType> GetCustomerTypeList()
        {
            return _customerTypeRepository.GetCustomerTypeList();
        }

        public string GetCustomerTypeByGuid(Guid customerTypeGuid)
        {
            return _customerTypeRepository.GetCustomerTypeByGuid(customerTypeGuid);
        }

        public Guid GetCustomerTypeByName(string customerTypeName)
        {
            return _customerTypeRepository.GetCustomerTypeByName(customerTypeName);
        }
    }
}
