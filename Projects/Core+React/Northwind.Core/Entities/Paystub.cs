using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class Paystub
    {
        public string EmployeeName { get; set; }
        public DateTime PayPeriodStart { get; set; } 
        public DateTime PayPeriodEnd { get; set; }
        public string CheckNumber { get; set; }
        
    }
}
