using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IResourceAttributeService
    {
        IEnumerable<ResourceAttribute> GetByResource(string resource);
        IEnumerable<ResourceAttribute> GetByResourceToGrid(string resource);
        IEnumerable<ResourceAttribute> GetByResourceToExport(string resource);
        IEnumerable<ResourceAttribute> GetByResources(List<string> resourceName);
    }
}
