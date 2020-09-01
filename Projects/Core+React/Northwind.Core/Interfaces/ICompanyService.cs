using Northwind.Core.Entities;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAll(string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        IEnumerable<Company> GetAllWithOrg(string searchValue, int pageSize, int skip, string sortField, string sortDirection);
        int TotalRecord(string searchValue);
        int CheckDuplicates(Company company);
        int Add(Company company);
        int Edit(Company company);
        int Delete(Guid[] ids);
        int DeleteById(Guid id);
        Company GetbyId(Guid id);
        Company GetDetailsById(Guid id);
        int Disable(Guid[] ids);
        int Enable(Guid[] ids);

        Company GetCompanyByCode(string companyCode);

        Company GetCompanyByName(string companyName);

        Company GetCompanyByCodeOrName(string code, string name);
        IEnumerable<Company> GetCompanyList();
        #region orgid
        Organization GetOrganizationByName(string orgIDName);
        #endregion orgid
    }
}
