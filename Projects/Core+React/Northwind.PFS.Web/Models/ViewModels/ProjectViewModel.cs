using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.PFS.Web.Models.ViewModels
{
    public class ProjectViewModel
    {
        public long ProjectId { get; set; }
        public string ProjectNumber { get; set; }
        public string ContractNumber { get; set; } //new
        public string ProjectName { get; set; }
        public string Description { get; set; } //new
        public string OrgId { get; set; }
        public string RegionalManager { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectControl { get; set; } //new
        public string Company { get; set; }
        public DateTime? POPStartDate { get; set; }
        public DateTime? POPEndDate { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal FundedAmount { get; set; }
        public bool IsFavorite { get; set; }
        public Guid? ContractGuid { get; set; }
    }
}
