using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Interfaces;
using Northwind.Core.Entities;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class ResourceRepository : IResourceRepository
    {
        private IDatabaseContext _context;
        public ResourceRepository(IDatabaseContext context)
        {
            _context = context;

        }
        public IEnumerable<Resource> GetAll()
        {
            return _context.Connection.Query<Resource>("SELECT * FROM dbo.Resources ORDER BY Title ASC");
        }
    }
}
