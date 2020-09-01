using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IFarClauseService
    {
        IEnumerable<Entities.FarClause> GetAll(string searchValue, int pageSize, int skip, int take, string sortField, string dir);
        IEnumerable<FarClause> GetAll();
        IEnumerable<Entities.FarClause> GetByAlphabetFilter(string searchValue, int take, int skip, string sortField, string dir, string filterBy);
        int TotalRecord(string searchValue);
        FarClause GetById(Guid FarClauseGuid);
        void Add(FarClause farClause);
        void Edit(FarClause farClause);
        void Delete(Guid id, Guid updatedBy);
        int CheckDuplicateFarClauseNumber(FarClause farClause);
        FarClause GetFarClauseByNumber(string number);
    }
}
