using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class FarContractTypeService : IFarContractTypeService
    {
        private IFarContractTypeRepository _farContractTypeRepository;
        public FarContractTypeService(IFarContractTypeRepository FarContractTypeRepository)
        {
            _farContractTypeRepository = FarContractTypeRepository;
        }

        public void Add(FarContractType farContractType)
        {
            _farContractTypeRepository.Add(farContractType);
        }

        public int CheckDuplicate(FarContractType farContractType)
        {
            return _farContractTypeRepository.CheckDuplicate(farContractType);
        }

        public void Delete(Guid id)
        {
            _farContractTypeRepository.Delete(id);
        }
       
        public IEnumerable<FarContractType> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return _farContractTypeRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir);
        }
       
        public IEnumerable<FarContractType> GetAll()
        {
            return _farContractTypeRepository.GetAll();
        }

        public IEnumerable<FarContractType> GetAllList()
        {
            return _farContractTypeRepository.GetAllList();
        }

        public FarContractType GetByCode(string code)
        {
            return _farContractTypeRepository.GetByCode(code);
        }

        public FarContractType GetById(Guid id)
        {
            return _farContractTypeRepository.GetById(id);
        }

        public int TotalRecord(string searchValue)
        {
            return _farContractTypeRepository.TotalRecord(searchValue);
        }

        public void Update(FarContractType farContractType)
        {
            _farContractTypeRepository.Update(farContractType);
        }
    }
}
