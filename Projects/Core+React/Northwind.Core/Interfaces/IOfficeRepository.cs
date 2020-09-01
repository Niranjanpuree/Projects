using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IOfficeRepository
    {
        IEnumerable<Office> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(string searchValue);
        int CheckDuplicate(Office officeModel);
        int Add(Office officeModel);
        int Edit(Office officeModel);
        int Delete(Guid[] ids);
        int DeleteById(Guid id);
        Office GetById(Guid id);
        Office GetDetailById(Guid id);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);

        string GetOfficeCodeByContractGuid(Guid contractGuid);
        Office GetOfficeByCode(string officeCode);

        IEnumerable<Office> GetOfficeListForUser(string searchValue, string filterBy, int pageSize, int skip, string sortField, string sortDirection);

        int GetTotalCountForUser(string searchValue, string filterBy);

        Office GetOfficeByCodeOrName(string officeCode, string officeName);

        int CheckDuplicateOfficeByName(string officeName, Guid officeGuid);
    }
}
