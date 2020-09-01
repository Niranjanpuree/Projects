using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;


namespace Northwind.Core.Interfaces
{
    public interface IResourceService
    {
        IEnumerable<Resource> GetAll();
        IEnumerable<ResourceAction> GetResourceAction(Guid resourceId);
        IEnumerable<ResourceAttribute> GetResourceAttribute(Guid resourceId);
        IEnumerable<ResourceAttribute> GetResourceAttributes(List<string> resources);
        IEnumerable<QueryOperator> GetQueryOperators();
    }
}
