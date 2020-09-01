using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Northwind.PFS.Web.Models.ViewModels
{
    public class LaborPayrollViewModel
    {
        public string ProjectNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Wbs { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string AccountCode { get; set; }
        public string GA { get; set; }
        public long Hours { get; set; }
        public decimal Fringe { get; set; }
        public decimal Overhead { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
    }
}
