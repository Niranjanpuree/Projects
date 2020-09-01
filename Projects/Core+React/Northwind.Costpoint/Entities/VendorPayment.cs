using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class VendorPaymentCP
    {
        public long VendorPaymentId { get; set; }
        public long ProjectId { get; set; }
        public string PO { get; set; }
        public int FiscalYear { get; set; }
        public string Period { get; set; }
        public string Vendor { get; set; }
        public decimal Amount { get; set; }
        public int InvoiceNo { get; set; }
        public int CheckNo { get; set; }
        public int VoucherNo { get; set; }
        public bool Status { get; set; }
    }
}
