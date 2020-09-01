﻿using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;

namespace Northwind.Core.Interfaces
{
    public interface IEmployeeBillingRatesRepository
    {

        int AddEmployeeBillingRates(EmployeeBillingRates employeeBillingRates);
        int UpdateEmployeeBillingRates(EmployeeBillingRates employeeBillingRates);
        int DeleteEmployeeBillingRates(Guid id);
        EmployeeBillingRates GetEmployeeBillingRatesById(Guid id);
        bool UpdateFileName(Guid employeeGuid, string fileName);
    }
}