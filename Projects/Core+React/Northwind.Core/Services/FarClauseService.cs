using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class FarClauseService : IFarClauseService
    {
        private IFarClauseRepository _farClauseRepository;
        public FarClauseService(IFarClauseRepository farClauseRepository)
        {
            _farClauseRepository = farClauseRepository;
        }

        public void Add(FarClause farClause)
        {
            _farClauseRepository.Add(farClause);
        }

        public int CheckDuplicateFarClauseNumber(FarClause farClause)
        {
            return _farClauseRepository.CheckDuplicateFarClauseNumber(farClause);
        }

        public void Delete(Guid id,Guid updatedBy)
        {
            _farClauseRepository.Delete(id, updatedBy);
        }

        public IEnumerable<Entities.FarClause> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir)
        {
            return _farClauseRepository.GetAll(searchValue, pageSize, skip, take, sortField, dir);
        }
        public IEnumerable<FarClause> GetAll()
        {
            return _farClauseRepository.GetAll();
        }
        public FarClause GetById(Guid FarClauseGuid)
        {
            return _farClauseRepository.GetById(FarClauseGuid);
        }

        public int TotalRecord(string searchValue)
        {
            return _farClauseRepository.TotalRecord(searchValue);
        }

        public void Edit(FarClause farClause)
        {
             _farClauseRepository.Edit(farClause);
        }

        public IEnumerable<FarClause> GetByAlphabetFilter(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            return _farClauseRepository.GetByAlphabetFilter(searchValue,take, skip, sortField, dir, filterBy);
        }

        public FarClause GetFarClauseByNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
                return null;
            return _farClauseRepository.GetFarClauseByNumber(number);
        }
    }
}
