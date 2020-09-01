using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class ResourceAttributeService : IResourceAttributeService
    {
        private IResourceAttributeRepository _attributeRepo;

        public ResourceAttributeService(IResourceAttributeRepository attributeRepo)
        {
            _attributeRepo = attributeRepo;
        }

        public IEnumerable<ResourceAttribute> GetByResource(string resource)
        {
            return _attributeRepo.GetByResource(resource);
        }

        public IEnumerable<ResourceAttribute> GetByResources(List<string> resourceName)
        {
            return _attributeRepo.GetByResources(resourceName);
        }

        public IEnumerable<ResourceAttribute> GetByResourceToExport(string resource)
        {
            return _attributeRepo.GetByResourceToExport(resource);
        }

        public IEnumerable<ResourceAttribute> GetByResourceToGrid(string resource)
        {
            return _attributeRepo.GetByResourceToGrid(resource);
        }
    }
}
