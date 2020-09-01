using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public IEnumerable<Customer> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<Customer> getall = _customerRepository.GetAll(searchValue, pageSize, skip, sortField, sortDirection);
            return getall;
        }
        public int TotalRecord(string searchValue)
        {
            int totalRecord = _customerRepository.TotalRecord(searchValue);
            return totalRecord;
        }
        public int CheckDuplicates(Customer Customer)
        {
            int count = _customerRepository.CheckDuplicates(Customer);
            return count;
        }

        public int Add(Customer CustomerModel)
        {
            return _customerRepository.Add(CustomerModel);
        }
        public int Edit(Customer CustomerModel)
        {
            return _customerRepository.Edit(CustomerModel);
        }
        public Customer GetCustomerById(Guid id)
        {
            return _customerRepository.GetCustomerById(id);
        }
        public Customer GetCustomerDetailsById(Guid id)
        {
            return _customerRepository.GetCustomerDetailsById(id);
        }
        public int Delete(Guid[] ids)
        {
            return _customerRepository.Delete(ids);
        }
        public int DisableCustomer(Guid[] ids)
        {
            return _customerRepository.DisableCustomer(ids);
        }
        public int EnableCustomer(Guid[] ids)
        {
            return _customerRepository.EnableCustomer(ids);
        }
        public IEnumerable<Customer> Find(CustomerSearchSpec spec)
        {
            return _customerRepository.Find(spec);
        }

        public IEnumerable<Attribute> GetAttributeNameListByResource(string resourceName)
        {
            throw new NotImplementedException();
        }

        public string BuildSql(BaseSearchSpec spec, out Dictionary<string, object> o)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Attribute> GetAttributeListByResource(string resourceName)
        {
            return _customerRepository.GetAttributeNameListByResource(resourceName);
        }

        public ICollection<Customer> GetOfficeData(string searchText)
        {
            return _customerRepository.GetOfficeData(searchText);
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            var result = _customerRepository.GetCustomerList();
            return result;
        }

        public int CheckDuplicateForImport(string name, Guid customerGuid)
        {
            var result = _customerRepository.CheckDuplicateForImport(name, customerGuid);
            return result;
        }

        public Customer GetCustomerByCodeOrName(string name, string code)
        {
            var result = _customerRepository.GetCustomerByCodeOrName(name, code);
            return result;
        }

        public Customer GetCustomerByName(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return null;
            return _customerRepository.GetCustomerByName(customerName);
        }

        public int DeleteById(Guid id)
        {
            var result = _customerRepository.DeleteById(id);
            return result;
        }
    }
}
