using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Dapper;

namespace Northwind.Infrastructure.Data
{
    public class ResourceAttributeRepository : IResourceAttributeRepository
    {
        private readonly IDatabaseContext _context;
        public ResourceAttributeRepository(IDatabaseContext context)
        {
            _context = context;
        }
        public IEnumerable<ResourceAttribute> GetByResource(string resource)
        {
            return _context.Connection.Query<ResourceAttribute>("SELECT * FROM ResourceAttribute WHERE ResourceType=@resourceType", new { resourceType = resource });
        }

        public IEnumerable<ResourceAttribute> GetByResources(List<string> resource)
        {
            return _context.Connection.Query<ResourceAttribute>("SELECT * FROM ResourceAttribute WHERE ResourceType IN @resourceType ORDER BY ResourceType ASC, Title ASC",  new { resourceType = resource } );

        }

        public IEnumerable<ResourceAttribute> GetByResourceToExport(string resource)
        {
            return _context.Connection.Query<ResourceAttribute>("SELECT * FROM ResourceAttribute WHERE ResourceType=@resourceType AND Exportable=1 Order by GridFieldOrder ASC", new { resourceType = resource });
        }

        public IEnumerable<ResourceAttribute> GetByResourceToGrid(string resource)
        {
            return _context.Connection.Query<ResourceAttribute>("SELECT * FROM ResourceAttribute WHERE ResourceType=@resourceType Order by GridFieldOrder ASC", new { resourceType = resource });
        }

        public IEnumerable<ResourceAttribute> GetResourceAttribute(Guid resourceId)
        {
            return _context.Connection.Query<ResourceAttribute>("SELECT * FROM ResourceAttribute WHERE ResourceAttributeGuid=@ResourceAttributeGuid", new { ResourceAttributeGuid = resourceId });
        }
    }
}
