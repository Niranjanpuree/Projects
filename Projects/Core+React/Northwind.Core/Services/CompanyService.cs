using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IEnumerable<Company> GetAll(string searhValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            IEnumerable<Company> getall = _companyRepository.GetAll(searhValue, pageSize, skip, sortField, sortDirection);
            return getall;
        }
        public int TotalRecord(string searchValue)
        {
            int totalRecord = _companyRepository.TotalRecord(searchValue);
            return totalRecord;
        }
        public int CheckDuplicates(Company Company)
        {
            int count = _companyRepository.CheckDuplicates(Company);
            return count;
        }
        public int Add(Company Company)
        {
            return _companyRepository.Add(Company);
        }
        public int Edit(Company Company)
        {
            return _companyRepository.Edit(Company);
        }
        public Company GetbyId(Guid id)
        {
            return _companyRepository.GetbyId(id);
        }
        public Company GetDetailsById(Guid id)
        {
            return _companyRepository.GetDetailsById(id);
        }
        public int Delete(Guid[] ids)
        {
            return _companyRepository.Delete(ids);
        }
        public int Disable(Guid[] ids)
        {
            return _companyRepository.Disable(ids);
        }
        public int Enable(Guid[] ids)
        {
            return _companyRepository.Enable(ids);
        }

        public Company GetCompanyByCode(string companyCode)
        {
            if (string.IsNullOrWhiteSpace(companyCode))
                return null;
            return _companyRepository.GetCompanyByCode(companyCode);
        }

        public Company GetCompanyByName(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName))
                return null;
            return _companyRepository.GetCompanyByName(companyName);
        }

        public Company GetCompanyByCodeOrName(string code, string name)
        {
            return _companyRepository.GetCompanyByCodeOrName(code, name);
        }

        public IEnumerable<Company> GetAllWithOrg(string searchValue, int pageSize, int skip, string sortField, string sortDirection)
        {
            return _companyRepository.GetAllWithOrg(searchValue, pageSize, skip, sortField, sortDirection);
        }

        public IEnumerable<Company> GetCompanyList()
        {
            return _companyRepository.GetCompanyList();
        }

        #region organization
        public Organization GetOrganizationByName(string orgIDName)
        {
            if (string.IsNullOrWhiteSpace(orgIDName))
                return null;
            return _companyRepository.GetOrganizationByName(orgIDName);
        }

        public int DeleteById(Guid id)
        {
            return _companyRepository.DeleteById(id);
        }
        #endregion organization
    }
}
