using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class ResourceAttributeValueService : IResourceAttributeValueService
    {
        private readonly IResourceAttributeValueRepository _resourceAttributeValueRepository;
        public ResourceAttributeValueService(IResourceAttributeValueRepository IResourceAttributeValueRepository)
        {
            _resourceAttributeValueRepository = IResourceAttributeValueRepository;
        }
        public IEnumerable<ResourceAttributeValue> GetResourceValuesByResourceType(string resourceType, string name)
        {
            return _resourceAttributeValueRepository.GetResourceValuesByResourceType(resourceType, name);
        }
        public IDictionary<string, string> GetDropDownByResourceType(string resourceType, string name)
        {
            IDictionary<string, string> model = new Dictionary<string, string>();
            var result = _resourceAttributeValueRepository.GetResourceValuesByResourceType(resourceType, name);
            foreach (var item in result)
            {
                model.Add(new KeyValuePair<string, string>(item.Value, item.Name));
            }
            return model;
        }
        public IDictionary<string, string> GetDropDownByValue(string value)
        {
            IDictionary<string, string> model = new Dictionary<string, string>();
            var result = _resourceAttributeValueRepository.GetResourceValuesByValue(value);
            foreach (var item in result)
            {
                model.Add(new KeyValuePair<string, string>(item.Value, item.Name));
            }
            return model;
        }

        public IEnumerable<ResourceAttributeValue> GetResourceAttributeOptionsByResourceAttributeGuid(Guid resourceAttributeGuid)
        {
            return _resourceAttributeValueRepository.GetResourceAttributeOptionsByResourceAttributeGuid(resourceAttributeGuid);
        }
        public ResourceAttributeValue GetResourceAttributeValueByValue(string value)
        {
            var result = _resourceAttributeValueRepository.GetResourceAttributeValueByValue(value);
            return result;
        }

        public ResourceAttributeValue GetResourceAttributeValueByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            var result = _resourceAttributeValueRepository.GetResourceAttributeValueByName(name.Trim());
            return result;
        }

        public ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue)
        {
            if (string.IsNullOrWhiteSpace(resourceType) || string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(nameValue))
                return null;
            return _resourceAttributeValueRepository.GetResourceAttributeValueByResourceTypeNameValue(resourceType,attributeName,nameValue);
        }
    }
}
