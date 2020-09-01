using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IResourceAttributeValueService
    {
        //IDictionary<Guid, string> GetDropDown(Guid ResourceAttributeGuid);
        //IDictionary<Guid, string> GetDropDownFromAttributeValueGuid(Guid ChildId);

        IEnumerable<ResourceAttributeValue> GetResourceValuesByResourceType(string resourceType, string name);
        IDictionary<string, string> GetDropDownByValue(string value);
        IDictionary<string, string> GetDropDownByResourceType(string resourceType, string name);
        IEnumerable<ResourceAttributeValue> GetResourceAttributeOptionsByResourceAttributeGuid(Guid resourceAttributeGuid);
        ResourceAttributeValue GetResourceAttributeValueByValue(string value);
        ResourceAttributeValue GetResourceAttributeValueByName(string name);
        ResourceAttributeValue GetResourceAttributeValueByResourceTypeNameValue(string resourceType, string attributeName, string nameValue);
    }
}
