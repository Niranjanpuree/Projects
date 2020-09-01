using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public class ModListModel
    {
        public Guid Id { get; set; }
        public bool IsMod { get; set; }
        public string ContractNumber { get; set; }
        public string ProjectNumber { get; set; }
        public string Mod { get; set; }
        public string ModificationType { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EffectiveDate{ get; set; }
        public string DateEntered { get; set; }
        public decimal Amount { get; set; }
        public decimal FundingAmount { get; set; }
        public bool Status { get; set; }
        public string currency { get; set; }
    }
}
