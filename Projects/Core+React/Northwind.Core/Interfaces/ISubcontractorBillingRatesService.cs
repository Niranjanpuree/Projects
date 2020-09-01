using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface ISubcontractorBillingRatesService
    {
        int AddLaborCategoryRates(LaborCategoryRates LaborCategoryRates);
        int UpdateLaborCategoryRates(LaborCategoryRates LaborCategoryRates);
        int DeleteLaborCategoryRates(Guid id);
        LaborCategoryRates GetLaborCategoryRatesById(Guid id);
        bool UpdateFileName(Guid subcontractorGuid, string fileName);
    }
}