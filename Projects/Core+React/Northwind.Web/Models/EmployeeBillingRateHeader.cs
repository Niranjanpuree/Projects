﻿using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class EmployeeBillingRateHeader
    {
        public string LaborCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal Rate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public sealed class EmployeeBillingRateHeaderMap : ClassMap<EmployeeBillingRateHeader>
    {
        public EmployeeBillingRateHeaderMap()
        {
            Map(m => m.LaborCode).Name("laborcode", "LaborCode", "laborCode");
                //.Validate(field => !string.IsNullOrEmpty(field));

            Map(m => m.EmployeeName).Name("employeeName", "EmployeeName");
            Map(m => m.Rate).Name("rate", "Rate");
            Map(m => m.StartDate).Name("startdate", "StartDate", "contractType");
            Map(m => m.EndDate).Name("enddate", "EndDate", "contractType");
        }
    }
}
