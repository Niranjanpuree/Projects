using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface ICustomerRepository : IBaseRepository
    {
        IEnumerable<Customer> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(string searchValue);
        int CheckDuplicates(Customer CustomerModel);
        int Add(Customer CustomerModel);
        int Edit(Customer CustomerModel);
        int Delete(Guid[] ids);
        int DeleteById(Guid id);
        Customer GetCustomerById(Guid id);
        Customer GetCustomerDetailsById(Guid id);
        int DisableCustomer(Guid[] ids);
        int EnableCustomer(Guid[] ids);
        IEnumerable<Customer> Find(CustomerSearchSpec spec);
        IEnumerable<Attribute> GetAttributeNameListByResource(string  resourceName);
        ICollection<Customer> GetOfficeData(string searchText);
        IEnumerable<Customer> GetCustomerList();

        int CheckDuplicateForImport(string name, Guid customerGuid);
        Customer GetCustomerByCodeOrName(string name, string code);

        Customer GetCustomerByName(string customerName);
    }
}
