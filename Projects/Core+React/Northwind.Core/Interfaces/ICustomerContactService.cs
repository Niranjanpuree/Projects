using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface ICustomerContactService
    {
        IEnumerable<CustomerContact> GetAll(string searchValue,Guid CustomerGuid, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid CustomerGuid);
        int Add(CustomerContact CustomerContact);
        int Edit(CustomerContact CustomerContact);
        int Delete(Guid[] ids);
        CustomerContact GetbyId(Guid id);
        CustomerContact GetDetailsById(Guid id);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);
        IEnumerable<CustomerContact> GetCustomerContactList();

        CustomerContact GetCustomerContactByName(string firstName, string lastName,Guid customerGuid);
        CustomerContact GetCustomerContactByEmail(string email, Guid customerGuid);
        ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType);
    }
}
