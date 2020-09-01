using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class CostCP
    {
        public long CostId { get; set; }
        public string ProjectNumber { get; set; }
        [JsonProperty(PropertyName = "wbs")]
        public string WBS { get; set; }
        public int FiscalYear { get; set; }
        public string Period { get; set; }
        public string AccountCode { get; set; }
        public string ChargeCode { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        [JsonProperty(PropertyName = "invoiceId")]
        public string InvoiceID { get; set; }
        public decimal Amount { get; set; }
        public string Fringe { get; set; }
        public string Overhead { get; set; }
        [JsonProperty(PropertyName = "ga")]
        public string GA { get; set; }
        [JsonProperty(PropertyName = "po_id")]
        public long PO_ID { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
