﻿using System;
using System.Collections.Generic;
using System.Text;
using Northwind.Core.Entities;

namespace Northwind.Core.Interfaces
{
    public interface IUserSyncService
    {
        void SyncUsersFromActiveDirectory(IActiveDirectoryContext adContext, Guid syncBatchGUID); 
    }
}
