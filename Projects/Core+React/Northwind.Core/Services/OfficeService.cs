using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Northwind.Core.Interfaces.ContractRefactor;
using Northwind.Core.Entities.ContractRefactor;

namespace Northwind.Core.Services
{
    public class OfficeService :IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        public OfficeService(IOfficeRepository OfficeRepository)
        {
            _officeRepository = OfficeRepository;
        }

        public IEnumerable<Office> GetAll(string searchValue,int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<Office> getAll = _officeRepository.GetAll(searchValue, pageSize, skip, sortField, sortDirection);
            return getAll;
        }
        public int TotalRecord(string searchValue)
        {
            int totalRecord = _officeRepository.TotalRecord(searchValue);
            return totalRecord;
        }
        public int CheckDuplicate(Office office)
        {
            int count = _officeRepository.CheckDuplicate(office);
            return count;
        }
        public int Add(Office officeModel)
        {
            return _officeRepository.Add(officeModel);
        }
        public int Edit(Office officeModel)
        {
            return _officeRepository.Edit(officeModel); ;
        }
        public Office GetById(Guid id)
        {
            return _officeRepository.GetById(id);
        }
        public Office GetDetailById(Guid id)
        {
            return _officeRepository.GetDetailById(id);
        }
        public int Delete(Guid[] ids)
        {
            return _officeRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _officeRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _officeRepository.Enable(ids);
        }

        public string GetOfficeCodeByContractGuid(Guid contractGuid)
        {
            return _officeRepository.GetOfficeCodeByContractGuid(contractGuid);
        }

        public Office GetOfficeByCode(string officeCode)
        {
            return _officeRepository.GetOfficeByCode(officeCode);
        }

        public IEnumerable<Office> GetOfficeListForUser(string searchValue, string filterBy, int pageSize, int skip, string sortField, string sortDirection)
        {
            return _officeRepository.GetOfficeListForUser(searchValue,filterBy,pageSize,skip,sortField,sortDirection);
        }

        public int GetOfficeTotalCountForUser(string searchValue, string filterBy)
        {
            return _officeRepository.GetTotalCountForUser(searchValue,filterBy);
        }

        public Office GetOfficeByCodeOrName(string officeCode, string officeName)
        {
            return _officeRepository.GetOfficeByCodeOrName(officeCode, officeName);
        }

        public int CheckDuplicateOfficeByName(string officeName, Guid officeGuid)
        {
            return _officeRepository.CheckDuplicateOfficeByName(officeName, officeGuid);
        }

        public int DeleteById(Guid id)
        {
            return _officeRepository.DeleteById(id);
        }
    }
}
