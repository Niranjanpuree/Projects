using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class POCP
    {
        public string ProjectNumber { get; set; }
        [JsonProperty(PropertyName = "poIssueDate")]
        public DateTime POIssueDate { get; set; }
        [JsonProperty(PropertyName = "po_id")]
        public int POID { get; set; }
        public string VendorName { get; set; }
        public string PaymentTerms { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VoucheredAmount { get; set; }
        public decimal Balance { get; set; }
        public string Status { get; set; }
    }
}
