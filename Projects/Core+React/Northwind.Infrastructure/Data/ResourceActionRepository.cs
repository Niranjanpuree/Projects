using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class ResourceActionRepository : IResourceActionRepository
    {
        private readonly IDatabaseContext _context;

        public ResourceActionRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public IEnumerable<ResourceAction> GetByResourceId(Guid resourceId)
        {
            return _context.Connection.Query<ResourceAction>("SELECT * FROM dbo.ResourceActions WHERE ResourceGuid=@resourceGuid", new { resourceGuid = resourceId });
        }
    }
}
