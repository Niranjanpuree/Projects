using Northwind.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IADGroupSyncService
    {
        void SyncGroupsFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID);
        void SyncGroupUsersAndManagerFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID);
    }
}
