using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class ProjectCP
    {
        public string ProjectNumber { get; set; }
        public string ContractNumber { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string OrgId { get; set; }
        public string RegionalManager { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectControl { get; set; }
        public string Company { get; set; }
        public DateTime POPStartDate { get; set; }
        public DateTime POPEndDate { get; set; }
        public decimal ContractAmount { get; set; }
        public decimal FundedAmount { get; set; }
        public bool IsFavorite { get; set; }
        public Guid? ContractGuid { get; set; }
        public string ContractType { get; set; }
        public decimal? EstimatedCost { get; set; }
        public decimal? EstimatedFee { get; set; }
    }
}
