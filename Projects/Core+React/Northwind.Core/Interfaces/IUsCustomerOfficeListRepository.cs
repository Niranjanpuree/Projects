using Northwind.Core.Entities;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IUsCustomerOfficeListRepository
    {
        IEnumerable<UsCustomerOfficeList> GetUsCustomerOfficeDepartmentList();
        IEnumerable<UsCustomerOfficeList> GetUsCustomerOfficeDepartmentListBySixDigitCode(string sixDigitCode);
        IEnumerable<UsCustomerOfficeList> GetDistinctCustomerName();
        IEnumerable<UsCustomerOfficeList> GetDistinctCustomerNameByDepartment(string department);

        UsCustomerOfficeList GetCustomerOfficeByContractingOfficeName(string contractingOfficeName);
    }
}
