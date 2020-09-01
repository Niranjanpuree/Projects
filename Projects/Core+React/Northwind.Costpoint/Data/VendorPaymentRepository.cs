using Northwind.Core.Models;
using System.Collections.Generic;
using Dapper;
using Northwind.CostPoint.Interfaces;
using Northwind.CostPoint.Entities;

namespace Northwind.Costpoint.Data
{
    public class VendorPaymentRepositoryCP : IVendorPaymentRepositoryCP
    {
        IPFSDBContext _context;
        public VendorPaymentRepositoryCP(IPFSDBContext context)
        {
            _context = context;
        }

        public IEnumerable<VendorPaymentCP> GetVendorPayments(long projectId, string searchValue, int skip, int take, string orderBy, string dir)
        {
            var sql = "select * from VendorPayments";
            return _context.Connection.Query<VendorPaymentCP>(sql);
        }

        public int GetCount(long projectId, string searchValue)
        {
            var sql = "select Count(1) from VendorPayments";
            return _context.Connection.ExecuteScalar<int>(sql);
        }

        public VendorPaymentCP GetById(long Id)
        {
            var sql = "select * from Costs Where VendorPaymentId=@Id";
            var result = _context.Connection.Query<VendorPaymentCP>(sql, new { Id = Id });
            if (result.AsList().Count > 0)
                return result.AsList<VendorPaymentCP>()[0];
            else
                return null;
        }
    }
}
