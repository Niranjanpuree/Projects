using Northwind.Core.Entities;
using Northwind.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces.Base
{
    public interface IResourceAttributeRepo
    {
        //ResourceAttribute Create(ResourceAttribute t);
        bool Delete(ResourceAttribute t);
        IEnumerable<ResourceAttribute> Get(BaseSearchSpec searchSpec, bool includeAll);
        bool HardDelete(ResourceAttribute t);
        ResourceAttribute Update(ResourceAttribute t);
        ResourceAttribute Update(ResourceAttribute t, List<ICriteria> Criteria);
    }
}
