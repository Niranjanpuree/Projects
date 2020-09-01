using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Infrastructure.Data.Notification
{
    public class NotificationBatchRepository : INotificationBatchRepository
    {
        public IDatabaseContext _context;

        public NotificationBatchRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public Guid Add(NotificationBatch model)
        {
            var sql = @"INSERT INTO NotificationBatch
                                                    (NotificationBatchGuid
                                                    ,NotificationTemplateGuid
                                                    ,ResourceId
                                                    ,ResourceType
                                                    ,ResourceAction
                                                    ,StartDate
                                                    ,AdditionalMessage
                                                    ,CreatedOn
                                                    ,CreatedBy)
                                            VALUES
                                                    (@NotificationBatchGuid
                                                    ,@NotificationTemplateGuid
                                                    ,@ResourceId
                                                    ,@ResourceType
                                                    ,@ResourceAction
                                                    ,@StartDate
                                                    ,@AdditionalMessage
                                                    ,@CreatedOn
                                                    ,@CreatedBy)";
            _context.Connection.Execute(sql, model);
            return model.NotificationBatchGuid;
        }

        public NotificationBatch GetByResourceId(Guid resourceId)
        {
            var sqlQuery = string.Format($@"select * from NotificationBatch where ResourceId =@ResourceId");
            var result = _context.Connection.QueryFirstOrDefault<NotificationBatch>(sqlQuery, new { ResourceId = resourceId });
            return result;
        }

        public Guid GetExistsGuid(Guid notificationTemplatesGuid, Guid resourceId)
        {
            var sqlQuery = string.Format($@"select NotificationBatchGuid from NotificationBatch where notificationTemplateGuid =@NotificationTemplatesGuid 
                                                and resourceId=@ResourceId");
            var result = _context.Connection.QueryFirstOrDefault<Guid>(sqlQuery,
                new { ResourceId = resourceId, NotificationTemplatesGuid = notificationTemplatesGuid });
            return result;
        }

        //public Guid GetResourceIdByNotificationMessageUserGuid(Guid userGuid)
        //{
        //    var sqlQuery = string.Format($@"select b.ResourceId from NotificationMessage m
        //                                    left join NotificationBatch b
        //                                    on b.NotificationBatchGuid = m.NotificationBatchGuid
        //                                    where m.UserGuid = @userGuid
        //                                    order by m.CreatedOn");
        //    var result= _context.Connection.QueryFirstOrDefault<Guid>(sqlQuery, new { userGuid = userGuid });
        //    return result;
        //}
    }
}
