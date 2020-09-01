using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class FarContractTypeClauseService : IFarContractTypeClauseService
    {
        private IFarContractTypeClauseRepository _farContractTypeClauseRepository;
        public FarContractTypeClauseService(IFarContractTypeClauseRepository farContractTypeClauseRepository)
        {
            _farContractTypeClauseRepository = farContractTypeClauseRepository;
        }

        public void Add(FarContractTypeClause farContractTypeClause)
        {
            _farContractTypeClauseRepository.Add(farContractTypeClause);
        }

        public void Delete(Guid id)
        {
            _farContractTypeClauseRepository.Delete(id);
        }


        public IEnumerable<FarContractTypeClause> GetAll(string searchValue, int take, int skip, string sortField, string dir, string filterBy)
        {
            return _farContractTypeClauseRepository.GetAll(searchValue, take, skip, sortField, dir, filterBy);
        }

        public FarContractTypeClause GetById(Guid farContractTypeClauseGuid)
        {
            return _farContractTypeClauseRepository.GetById(farContractTypeClauseGuid);
        }

        public IEnumerable<FarContractTypeClause> GetFarContractTypeByFarClauseId(Guid farClauseId)
        {
            return _farContractTypeClauseRepository.GetFarContractTypeByFarClauseId(farClauseId);
        }

        public int TotalRecord(string searchValue, string filterBy)
        {
            return _farContractTypeClauseRepository.TotalRecord(searchValue, filterBy);
        }

        public void Edit(FarContractTypeClause farContractTypeClause)
        {
            _farContractTypeClauseRepository.Edit(farContractTypeClause);
        }

        public int CheckDuplicateByFarClauseAndFarContractTypeComposition(FarContractTypeClause farContractTypeClauseModel)
        {
            return _farContractTypeClauseRepository.CheckDuplicateByFarClauseAndFarContractTypeComposition(farContractTypeClauseModel);
        }

        public FarContractTypeClause GetByFarClauseFarContractTypeGuid(Guid farClauseGuid, Guid farContractTypeGuid)
        {
            return _farContractTypeClauseRepository.GetByFarClauseFarContractTypeGuid(farClauseGuid, farContractTypeGuid);
        }
    }
}
