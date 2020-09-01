using System;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class EmployeeBillingRatesService : IEmployeeBillingRatesService
    {
        private readonly IEmployeeBillingRatesRepository _employeeBillingRatesRepository;
        public EmployeeBillingRatesService(IEmployeeBillingRatesRepository EmployeeBillingRatesRepository)
        {
            _employeeBillingRatesRepository = EmployeeBillingRatesRepository;
        }
        public int AddEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            return _employeeBillingRatesRepository.AddEmployeeBillingRates(employeeBillingRates);
        }
        public int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates)
        {
            return _employeeBillingRatesRepository.UpdateEmployeeBillingRates(employeeBillingRates);
        }
        public EmployeeBillingRates GetEmployeeBillingRatesById(Guid id)
        {
            return _employeeBillingRatesRepository.GetEmployeeBillingRatesById(id);
        }
        public int DeleteEmployeeBillingRates(Guid id)
        {
            return _employeeBillingRatesRepository.DeleteEmployeeBillingRates(id);
        }
        public bool updateFileName(Guid employeeGuid, string fileName)
        {
            return _employeeBillingRatesRepository.UpdateFileName(employeeGuid, fileName);
        }
    }
}
