using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;
using Northwind.Core.Specifications;

namespace Northwind.Core.Interfaces
{
    public interface IBaseRepository
    {
        string BuildSql(BaseSearchSpec spec,out Dictionary<string, object> o);
    }
}
