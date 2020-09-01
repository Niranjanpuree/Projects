using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class OfficeContactService : IOfficeContactService
    {
        private readonly IOfficeContactRepository _iOfficeContactRepository;
        public OfficeContactService(IOfficeContactRepository officeContactRepository)
        {
            _iOfficeContactRepository = officeContactRepository;
        }

        public IEnumerable<OfficeContact> GetAll(string searchValue, Guid officeGuid, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<OfficeContact> getOfficeContacts = _iOfficeContactRepository.GetAll(searchValue, officeGuid, pageSize, skip, sortField, sortDirection);
            return getOfficeContacts;
        }
        public int Add(OfficeContact officeContact)
        {
            return _iOfficeContactRepository.Add(officeContact);
        }
        public int Edit(OfficeContact officeContact)
        {
            return _iOfficeContactRepository.Edit(officeContact);
        }
        public int Delete(Guid[] ids)
        {
            return _iOfficeContactRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _iOfficeContactRepository.Disable(ids);
        }
        public int TotalRecord(Guid officeGuid)
        {
            int totalRecord = _iOfficeContactRepository.TotalRecord(officeGuid);
            return totalRecord;
        }

        public OfficeContact GetById(Guid id)
        {
            return _iOfficeContactRepository.GetById(id);
        }

        public OfficeContact GetDetailById(Guid id)
        {
            return _iOfficeContactRepository.GetDetailById(id);
        }

        public int EnableOfficeContact(Guid[] ids)
        {
            return _iOfficeContactRepository.Enable(ids);
        }

        public IDictionary<Guid, string> GetContactType()
        {
            return _iOfficeContactRepository.GetContactType();
        }
    }
}
