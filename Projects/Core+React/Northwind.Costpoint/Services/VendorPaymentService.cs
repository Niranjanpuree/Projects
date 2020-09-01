using Northwind.CostPoint.Entities;
using Northwind.CostPoint.Interfaces;
using System.Collections.Generic;

namespace Northwind.CostPoint.Services
{
    public class VendorPaymentServiceCP : IVendorPaymentServiceCP
    {
        IVendorPaymentRepositoryCP _vendorPaymentRepository;
        public VendorPaymentServiceCP(IVendorPaymentRepositoryCP vendorPaymentRepository)
        {
            _vendorPaymentRepository = vendorPaymentRepository;
        }

        public IEnumerable<VendorPaymentCP> GetVendorPayments(long projectId, string searchValue, int skip, int take, string orderBy, string dir)
        {
            return _vendorPaymentRepository.GetVendorPayments(projectId, searchValue, skip, take, orderBy, dir);
        }

        public int GetCount(long projectId, string searchValue)
        {
            return _vendorPaymentRepository.GetCount(projectId, searchValue);
        }

        public VendorPaymentCP GetById(long Id)
        {
            return _vendorPaymentRepository.GetById(Id);
        }
    }
}
