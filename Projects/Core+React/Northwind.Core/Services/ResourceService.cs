using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

namespace Northwind.Core.Services
{
    public class ResourceService : IResourceService
    {
        private IResourceRepository _repo;
        private IResourceActionRepository _resourceActionRepo;
        private IResourceAttributeRepository _attributeRepo;
        private IQueryOperatorRepository _queryOperatorRepo;

        public ResourceService(IResourceRepository repo, IResourceActionRepository resourceActionRepo, IResourceAttributeRepository attributeRepo, IQueryOperatorRepository queryOperatorRepo)
        {
            _repo = repo;
            _resourceActionRepo = resourceActionRepo;
            _attributeRepo = attributeRepo;
            _queryOperatorRepo = queryOperatorRepo;
        }

        public IEnumerable<Resource> GetAll()
        {
            return _repo.GetAll();
        }

        public IEnumerable<ResourceAction> GetResourceAction(Guid resourceId)
        {
            return _resourceActionRepo.GetByResourceId(resourceId);
        }

        public IEnumerable<ResourceAttribute> GetResourceAttribute(Guid resourceId)
        {
            return _attributeRepo.GetResourceAttribute(resourceId);
        }

        public IEnumerable<ResourceAttribute> GetResourceAttributes(List<string> resourceName)
        {
            return _attributeRepo.GetByResources(resourceName);
        }

        public IEnumerable<QueryOperator> GetQueryOperators()
        {
            return _queryOperatorRepo.GetAll();
        }
    }
}
