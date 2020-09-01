using Dapper;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace Northwind.Infrastructure.Data.CustomerContactType
{
    public class CustomerContactTypeRepository: ICustomerContactTypeRepository
    {
        private readonly IDatabaseContext _context;

        public CustomerContactTypeRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<Northwind.Core.Entities.CustomerContactType> GetCustomerContactList()
        {
            var data = _context.Connection.Query<Northwind.Core.Entities.CustomerContactType>("SELECT * FROM CustomerContactType Where isActive =1 order by ContactTypeName");
            return data;
        }

        public Guid GetCustomerContactTypeGuidByTypeName(string customerContactTypeName)
        {
            var query = @"SELECT * 
                        FROM CustomerContactType
                        WHERE ContactTypeName = @contactTypeName";
            return _context.Connection.QueryFirstOrDefault<Guid>(query, new { contactTypeName = customerContactTypeName });
        }
    }
}
