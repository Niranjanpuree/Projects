using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public interface IFarContractTypeService
    {
        IEnumerable<FarContractType> GetAllList();
        IEnumerable<FarContractType> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<FarContractType> GetAll();
        FarContractType GetById(Guid id);
        FarContractType GetByCode(string code);
        int CheckDuplicate(FarContractType farContractType);
        int TotalRecord(string searchValue);
        void Add(FarContractType farContractType);
        void Update(FarContractType farContractType);
        void Delete(Guid id);
    }
}
