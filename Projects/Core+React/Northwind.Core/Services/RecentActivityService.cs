using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Services
{
    public class RecentActivityService : IRecentActivityService
    {
        private readonly IRecentActivityRepository _recentActivityRepo;

        public RecentActivityService(IRecentActivityRepository recentActivityRepo)
        {
            _recentActivityRepo = recentActivityRepo;
        }

        public void InsertRecentActivity(Core.Entities.RecentActivity recentActivity)
        {
            recentActivity.RecentActivityGuid = Guid.NewGuid();
            var activity = _recentActivityRepo.CheckIfExistInActivity(recentActivity.Entity,recentActivity.EntityGuid,recentActivity.UserGuid,recentActivity.UserAction);
            if (activity != null)
            {
                activity.UpdatedOn = DateTime.Now;
                activity.UpdatedBy = recentActivity.UpdatedBy;
                activity.IsDeleted = false;
                _recentActivityRepo.UpdateRecentActivity(activity);
            }
            else {
                _recentActivityRepo.InsertRecentActivity(recentActivity);
            }
            
        }

        public void UpdateRecentActivity(Core.Entities.RecentActivity recentActivity)
        {
            _recentActivityRepo.UpdateRecentActivity(recentActivity);
        }

        public IEnumerable<Core.Entities.RecentActivity> GetUserRecentActivity(Guid userGuid, string entityType, string userActionType)
        {
            return _recentActivityRepo.GetUserRecentActivity(userGuid, entityType, userActionType);
        }

        public void RemoveFromActivity(Guid entityGuid,string entity)
        {
            _recentActivityRepo.RemoveFromActivity(entityGuid,entity);
        }

        public bool IsFavouriteActivity(Guid entityGuid, string entity,Guid userGuid)
        {
            return _recentActivityRepo.IsFavouriteActivity(entityGuid,entity,userGuid);
        }

        public Core.Entities.RecentActivity GetRecentActivityByEntityGuid(Guid contractGuid, Guid userGuid, string entityType, string userActionType)
        {
            return _recentActivityRepo.GetRecentActivityByEntityGuid(contractGuid,userGuid,entityType,userActionType);
        }

    }
}
