using Northwind.Core.Entities;
using Northwind.Core.Interfaces.Sync;
using System;
using System.Collections.Generic;

namespace Northwind.Core.Interfaces
{
    public interface IActiveDirectoryGroupRepository
    {
        IEnumerable<Group> GetGroups();
        IEnumerable<Group> GetGroupByCN(string cn);
    }
}
