using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(string searchValue);
        int CheckDuplicates(Customer customerModel);
        int Add(Customer customerModel);
        int Edit(Customer customerModel);
        int Delete(Guid[] ids);
        int DeleteById(Guid id);
        Customer GetCustomerById(Guid id);
        Customer GetCustomerDetailsById(Guid id);
        int DisableCustomer(Guid[] ids);
        int EnableCustomer(Guid[] ids);
        IEnumerable<Customer> Find(CustomerSearchSpec spec);
        IEnumerable<Attribute> GetAttributeNameListByResource(string resourceName);
        string BuildSql(BaseSearchSpec spec, out Dictionary<string, object> o);
        ICollection<Customer> GetOfficeData(string searchText);
        IEnumerable<Customer> GetCustomerList();

        int CheckDuplicateForImport(string name, Guid customerGuid);
        Customer GetCustomerByCodeOrName(string name, string code);

        Customer GetCustomerByName(string customerName);
    }
}
