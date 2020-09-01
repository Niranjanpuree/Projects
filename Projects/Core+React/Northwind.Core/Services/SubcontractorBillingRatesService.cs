using System;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class SubcontractorBillingRatesService : ISubcontractorBillingRatesService
    {
        private readonly ISubcontractorBillingRatesRepository _subcontractorBillingRatesRepository;
        public SubcontractorBillingRatesService(ISubcontractorBillingRatesRepository subcontractorBillingRatesRepository)
        {
            _subcontractorBillingRatesRepository = subcontractorBillingRatesRepository;
        }
        public int AddLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            return _subcontractorBillingRatesRepository.AddLaborCategoryRates(laborCategoryRates);
        }
        public int UpdateLaborCategoryRates(LaborCategoryRates laborCategoryRates)
        {
            return _subcontractorBillingRatesRepository.UpdateLaborCategoryRates(laborCategoryRates);
        }
        public LaborCategoryRates GetLaborCategoryRatesById(Guid id)
        {
            return _subcontractorBillingRatesRepository.GetLaborCategoryRatesById(id);
        }
        public int DeleteLaborCategoryRates(Guid id)
        {
            return _subcontractorBillingRatesRepository.DeleteLaborCategoryRates(id);
        }
        public bool UpdateFileName(Guid subcontractorGuid,string fileName)
        {
            return _subcontractorBillingRatesRepository.UpdateFileName(subcontractorGuid, fileName);
        }
    }
}
