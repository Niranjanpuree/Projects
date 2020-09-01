using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class WorkBreakDownHeader
    {
        public string WBSCode { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string ContractType { get; set; }
        public string InvoiceAtThisLevel { get; set; }
    }
    public sealed class WorkBreakDownHeaderMap : ClassMap<WorkBreakDownHeader>
    {
        public WorkBreakDownHeaderMap()
        {
            Map(m => m.WBSCode).Name("wbscode", "WBSCode", "wbsCode");

            Map(m => m.Description).Name("description", "Description");
            Map(m => m.Value).Name("value", "Value");
            Map(m => m.ContractType).Name("contracttype", "ContractType", "contractType");

            Map(m => m.InvoiceAtThisLevel)
               .Name("InvoiceAtThisLevel", "invoiceatthislevel", "invoiceAtThisLevel");
        }
    }
}
