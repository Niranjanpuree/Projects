using Dapper;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.RecentActivity
{
    public class RecentActivityRepository : IRecentActivityRepository
    {
        IDatabaseContext _context;
        public RecentActivityRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public void InsertRecentActivity(Core.Entities.RecentActivity recentActivity)
        {
            var insertSQL = @"INSERT INTO dbo.[RecentActivity](
                RecentActivityGuid,
                Entity,
                EntityGuid,
                UserGuid,
                UserAction,
                CreatedBy,
                CreatedOn,
                UpdatedBy,
                UpdatedOn,
                IsDeleted)
                VALUES(
                @RecentActivityGuid,
                @Entity,
                @EntityGuid,
                @UserGuid,
                @UserAction,
                @CreatedBy,
                @CreatedOn,
                @UpdatedBy,
                @UpdatedOn,
                @IsDeleted)";
            _context.Connection.Execute(insertSQL, recentActivity);
        }

        public void UpdateRecentActivity(Core.Entities.RecentActivity recentActivity)
        {
            var updateQuery = @"UPDATE [dbo].[RecentActivity]
                                SET UserAction = @UserAction,
                                    UpdatedBy = @UpdatedBy,
                                    UpdatedOn = @UpdatedOn,
                                    IsDeleted = @IsDeleted
                                WHERE RecentActivityGuid = @RecentActivityGuid
                                ";
            _context.Connection.Execute(updateQuery, recentActivity);
        }

        public IEnumerable<Core.Entities.RecentActivity> GetUserRecentActivity(Guid userGuid,string entityType,string userActionType)
        {
            var sql = @"SELECT TOP(10)* 
                        FROM RecentActivity
                        WHERE UserGuid = @userGuid
                        AND Entity = @entity
                        AND UserAction = @userAction
                        ORDER BY UpdatedOn DESC";
            var activityList = _context.Connection.Query<Core.Entities.RecentActivity>(sql, new { userGuid = userGuid, entity = entityType, userAction= userActionType });
            return activityList;
        }

        public bool SetContractAsFavourite(IList<Core.Entities.RecentActivity> activityList)
        {
            foreach (var activity in activityList)
            {
                string insertQuery = @"INSERT INTO [dbo].[RecentActiviity]( 
                                         RecentActivityGuid,
                                        Entity,
                                        EntityGuid,
                                        UserGuid,
                                        UserAction,
                                        CreatedBy,
                                        CreatedOn,
                                        UpdatedBy,
                                        UpdatedOn)
                                        VALUES(
                                        @RecentActivityGuid,
                                        @Entity,
                                        @EntityGuid,
                                        @UserGuid,
                                        @UserAction,
                                        @CreatedBy,
                                        @CreatedOn,
                                        @UpdatedBy,
                                        @UpdatedOn)";
                _context.Connection.Execute(insertQuery, activity);
            }
            return true;
        }

        public void RemoveFromActivity(Guid entityGuid,string entity)
        {
            var sql = @"UPDATE [dbo].[RecentActivity]
                        SET IsDeleted = 1
                        WHERE EntityGuid = @contractGuid
                        AND Entity = @entity";
            _context.Connection.Execute(sql, new { contractGuid = entityGuid, entity = entity });
                
        }

        public bool IsFavouriteActivity(Guid entityGuid,string entity, Guid userGuid)
        {
            var actionType = Core.Entities.EnumGlobal.ActivityType.MyFavorite.ToString();
            var sql = @"SELECT COUNT(1)
                        FROM RecentActivity
                        WHERE EntityGuid = @entityGuid
                        AND UserAction = @actionType
                        AND userGuid = @userGuid 
                        AND entity = @entity AND IsDeleted = 0";
            var count = _context.Connection.QueryFirstOrDefault<int>(sql, new { entityGuid = entityGuid,entity = entity,  actionType = actionType, userGuid = userGuid });
            if (count > 0)
                return true;
            return false;
        }

        public Core.Entities.RecentActivity CheckIfExistInActivity(string entity,Guid entityGuid,Guid userGuid, string userAction)
        {
            var sql = @"SELECT TOP(1)*
                        FROM RecentActivity
                        WHERE Entity = @entity
                        AND EntityGuid = @entityGuid
                        AND userGuid = @userGuid
                        AND UserAction = @userAction";
            var activity = _context.Connection.QueryFirstOrDefault<Core.Entities.RecentActivity>(sql, new { entity = entity, entityGuid = entityGuid, userGuid = userGuid, userAction = userAction });
            return activity;
        }

        public Core.Entities.RecentActivity GetRecentActivityByEntityGuid(Guid contractGuid, Guid userGuid, string entityType, string userActionType)
        {
            var sql = @"SELECT TOP(1)* 
                        FROM RecentActivity
                        WHERE EntityGuid = @entityGuid 
                        AND UserGuid = @userGuid
                        AND Entity = @entity
                        AND UserAction = @userAction";
            var activity = _context.Connection.QueryFirstOrDefault<Core.Entities.RecentActivity>(sql, new {entityGuid = contractGuid, userGuid = userGuid, entity = entityType, userAction = userActionType });
            return activity;
        }
    }
}
