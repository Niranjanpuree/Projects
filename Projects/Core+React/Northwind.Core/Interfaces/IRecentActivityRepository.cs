﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Interfaces
{
    public interface IRecentActivityRepository
    {
        void InsertRecentActivity(Core.Entities.RecentActivity recentActivity);

        void UpdateRecentActivity(Core.Entities.RecentActivity recentActivity);

        IEnumerable<Core.Entities.RecentActivity> GetUserRecentActivity(Guid userGuid, string entityType, string userActionType);

        void RemoveFromActivity(Guid entityGuid,string entity);

        bool IsFavouriteActivity(Guid entityGuid, string entity, Guid userGuid);

        Core.Entities.RecentActivity CheckIfExistInActivity(string entity, Guid entityGuid, Guid userGuid, string userAction);

        Core.Entities.RecentActivity GetRecentActivityByEntityGuid(Guid contractGuid, Guid userGuid, string entityType, string userActionType);
    }
}