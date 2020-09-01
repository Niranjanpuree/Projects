using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System.Collections.Generic;

namespace Northwind.Core.Services
{
    public class UsCustomerOfficeListService : IUsCustomerOfficeListService
    {
        private readonly IUsCustomerOfficeListRepository _usCustomerOfficeListRepository;
        public UsCustomerOfficeListService(IUsCustomerOfficeListRepository usCustomerOfficeListRepository)
        {
            _usCustomerOfficeListRepository = usCustomerOfficeListRepository;
        }

        public IEnumerable<UsCustomerOfficeList> GetDistinctCustomerName()
        {
            return _usCustomerOfficeListRepository.GetDistinctCustomerName();
        }

        public IEnumerable<UsCustomerOfficeList> GetDistinctCustomerNameByDepartment(string department)
        {
            return _usCustomerOfficeListRepository.GetDistinctCustomerNameByDepartment(department);
        }

        public IEnumerable<UsCustomerOfficeList> GetUsCustomerOfficeDepartmentList()
        {
            return _usCustomerOfficeListRepository.GetUsCustomerOfficeDepartmentList();
        }

        public IEnumerable<UsCustomerOfficeList> GetUsCustomerOfficeDepartmentListBySixDigitCode(string sixDigitCode)
        {
            return _usCustomerOfficeListRepository.GetUsCustomerOfficeDepartmentListBySixDigitCode(sixDigitCode);
        }

        public UsCustomerOfficeList GetCustomerOfficeByContractingOfficeName(string contractingOfficeName)
        {
            if (string.IsNullOrWhiteSpace(contractingOfficeName))
                return null;
            return _usCustomerOfficeListRepository.GetCustomerOfficeByContractingOfficeName(contractingOfficeName);
        }
    }
}
