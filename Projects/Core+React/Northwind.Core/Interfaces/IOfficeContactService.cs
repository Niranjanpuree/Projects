using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IOfficeContactService
    {
        IEnumerable<OfficeContact> GetAll(string searchValue,Guid OfficeGuid, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid OfficeGuid);
        int Add(OfficeContact OfficeContact);
        int Edit(OfficeContact OfficeContact);
        int Disable(Guid[] ids);
        int Delete(Guid[] ids);
        OfficeContact GetById(Guid id);
        OfficeContact GetDetailById(Guid id);
        int EnableOfficeContact(Guid[] ids);
        IDictionary<Guid, string> GetContactType();
    }
}
