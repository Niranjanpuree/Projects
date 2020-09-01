using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class ContractListViewModel
    {
        public string ContractNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractName { get; set; }
        public string ContractDescription { get; set; }
        public DateTime PeriodOfPerformanceStart { get; set; }
        public DateTime PeriodOfPerformanceEnd { get; set; }
        public string OrgId { get; set; }
        public double ContractValue { get; set; }
        public string Type { get; set; }

    }
}
