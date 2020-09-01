using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.CustomerType
{
    public class CustomerTypeRepository : ICustomerTypeRepository
    {
        private readonly IDatabaseContext _context;

        public CustomerTypeRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<Core.Entities.CustomerType> GetCustomerTypeList()
        {
            var data = _context.Connection.Query<Core.Entities.CustomerType>("SELECT * FROM CustomerType order by CustomerTypeName");
            return data;
        }

        public string GetCustomerTypeByGuid(Guid customerTypeGuid)
        {
            var customerType = _context.Connection.QueryFirstOrDefault<string>("SELECT TOP(1) CustomerTypeName FROM CustomerType WHERE CustomerTypeGuid = @customerTypeGuid", new { customerTypeGuid = customerTypeGuid });
            return customerType;
        }

        public Guid GetCustomerTypeByName(string customerTypeName)
        {
            var customerType = _context.Connection.QueryFirstOrDefault<Guid>("SELECT TOP(1) CustomerTypeGuid FROM CustomerType WHERE CustomerTypeName = @CustomerTypeName", new { CustomerTypeName = customerTypeName });
            return customerType;
        }
    }
}
