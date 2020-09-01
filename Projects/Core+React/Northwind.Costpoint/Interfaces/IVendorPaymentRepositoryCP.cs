using Northwind.CostPoint.Entities;
using System.Collections.Generic;

namespace Northwind.CostPoint.Interfaces
{
    public interface IVendorPaymentRepositoryCP
    {
        IEnumerable<VendorPaymentCP> GetVendorPayments(long projectId, string searchValue, int skip, int take, string orderBy, string dir);
        int GetCount(long projectId, string searchValue);
        VendorPaymentCP GetById(long Id);
    }
}
