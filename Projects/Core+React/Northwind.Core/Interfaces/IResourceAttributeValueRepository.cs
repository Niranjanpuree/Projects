using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
   public interface IResourceAttributeValueRepository
    {
        //IDictionary<Guid, string> GetDropDown(Guid ResourceAttributeGuid);
        //IDictionary<Guid, string> GetDropDownFromAttributeValueGuid(Guid ResourceAttributeValueGuid);
        IEnumerable<ResourceAttributeValue> GetResourceValuesByResourceType(string resourceType, string name);
        ResourceAttributeValue GetResourceAttributeValueByName(string name);
        IEnumerable<ResourceAttributeValue> GetResourceValuesByValue(string value);
        IEnumerable<ResourceAttributeValue> GetResourceAttributeOptionsByResourceAttributeGuid(Guid resourceAttributeGuid);
        ResourceAttributeValue GetResourceAttributeValueByValue(string value);

        ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue);
    }
}
