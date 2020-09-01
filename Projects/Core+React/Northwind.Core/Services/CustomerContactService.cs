using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class CustomerContactService:ICustomerContactService
    {
        private readonly ICustomerContactRepository _customerContactRepository;
        public CustomerContactService(ICustomerContactRepository customerContactRepository)
        {
            _customerContactRepository = customerContactRepository;
        }

        public IEnumerable<CustomerContact> GetAll(string searchValue,Guid CustomerGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<CustomerContact> getall = _customerContactRepository.GetAll(searchValue,CustomerGuid, pageSize, skip, sortField, sortDirection);
            return getall;
        }
        public int Add(CustomerContact CustomerContact)
        {
            return _customerContactRepository.Add(CustomerContact);
        }
        public int Edit(CustomerContact CustomerContact)
        {
            return _customerContactRepository.Edit(CustomerContact);
        }
        public CustomerContact GetbyId(Guid id)
        {
            return _customerContactRepository.GetbyId(id);
        }
        public CustomerContact GetDetailsById(Guid id)
        {
            return _customerContactRepository.GetDetailsById(id);
        }
        public int Delete(Guid[] ids)
        {
            return _customerContactRepository.Delete(ids);
        }
        public int TotalRecord(Guid CustomerGuid)
        {
            int totalRecord = _customerContactRepository.TotalRecord(CustomerGuid);
            return totalRecord;
        }
        public int Disable(Guid[] ids)
        {
            return _customerContactRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _customerContactRepository.Enable(ids);
        }

        public IEnumerable<CustomerContact> GetCustomerContactList()
        {
            var result = _customerContactRepository.GetCustomerContactList();
            return result;
        }

        public CustomerContact GetCustomerContactByName(string firstName, string lastName, Guid customerGuid)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || customerGuid == Guid.Empty)
                return null;
            return _customerContactRepository.GetCustomerContactByName(firstName,lastName,customerGuid);
        }

        public CustomerContact GetCustomerContactByEmail(string email, Guid customerGuid)
        {
            if (string.IsNullOrWhiteSpace(email) || customerGuid == Guid.Empty)
                return null;
            return _customerContactRepository.GetCustomerContactByEmail(email,customerGuid);
        }

        public ICollection<KeyValuePairWithDescriptionModel<Guid, string, string>> GetAllContactByCustomer(Guid customerId, string contactType)
        {
            return _customerContactRepository.GetAllContactByCustomer(customerId,contactType);
        }
    }
}
