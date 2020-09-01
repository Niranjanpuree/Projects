using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IFarContractTypeClauseService
    {
        IEnumerable<FarContractTypeClause> GetAll(string searchValue, int take, int skip, string sortField, string dir, string filterBy);
        IEnumerable<FarContractTypeClause> GetFarContractTypeByFarClauseId(Guid farClauseId);
        FarContractTypeClause GetById(Guid farContractTypeClauseGuid);
        int TotalRecord(string searchValue, string filterBy);
        void Add(FarContractTypeClause farContractTypeClause);
        void Edit(FarContractTypeClause farContractTypeClause);
        void Delete(Guid id);
        int CheckDuplicateByFarClauseAndFarContractTypeComposition(FarContractTypeClause farContractTypeClauseModel);
        FarContractTypeClause GetByFarClauseFarContractTypeGuid(Guid farClauseGuid, Guid farContractTypeGuid);
    }
}
