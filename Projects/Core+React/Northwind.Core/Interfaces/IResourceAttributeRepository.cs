using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;


namespace Northwind.Core.Interfaces
{
    public interface IResourceAttributeRepository
    {
        IEnumerable<ResourceAttribute> GetResourceAttribute(Guid resourceId);
        IEnumerable<ResourceAttribute> GetByResource(string resource);
        IEnumerable<ResourceAttribute> GetByResourceToGrid(string resource);
        IEnumerable<ResourceAttribute> GetByResourceToExport(string resource);
        IEnumerable<ResourceAttribute> GetByResources(List<string> resourceName);
    }
}
