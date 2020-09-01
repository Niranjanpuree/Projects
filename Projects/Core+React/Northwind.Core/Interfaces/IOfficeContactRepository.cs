using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IOfficeContactRepository
    {
        IEnumerable<OfficeContact> GetAll(string searchValue,Guid officeGuid, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(Guid officeGuid);
        int Add(OfficeContact officeContact);
        int Edit(OfficeContact officeContact);
        int Delete(Guid[] ids);
        OfficeContact GetById(Guid id);
        OfficeContact GetDetailById(Guid id);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);
        IDictionary<Guid, string> GetContactType();
    }
}
