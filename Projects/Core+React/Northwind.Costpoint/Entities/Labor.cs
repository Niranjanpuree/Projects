using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class LaborCP
    {
        public string ProjectNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Wbs { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string AccountCode { get; set; }
        public decimal GA { get; set; }
        public long Hours { get; set; }        
        public decimal Fringe { get; set; }
        public decimal Overhead { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
