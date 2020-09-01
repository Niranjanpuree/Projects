using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Northwind.Infrastructure.Data.Notification
{
    public class NotificationMessageRepository : INotificationMessageRepository
    {
        public IDatabaseContext _context;

        public NotificationMessageRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public bool Add(NotificationMessage model)
        {
            var sql = @"INSERT INTO NotificationMessage
                                                               (NotificationBatchGuid
                                                               ,DistributionListGuid
                                                               ,UserGuid
                                                               ,Subject
                                                               ,Message
                                                               ,AdditionalMessage
                                                               ,Status
                                                               ,UserResponse
                                                               ,NextAction
                                                               ,CreatedOn
                                                               ,CreatedBy)
                                                        VALUES
                                                               (@NotificationBatchGuid
                                                               ,@DistributionListGuid
                                                               ,@UserGuid
                                                               ,@Subject
                                                               ,@Message
                                                               ,@AdditionalMessage
                                                               ,@Status
                                                               ,@UserResponse
                                                               ,@NextAction
                                                               ,@CreatedOn
                                                               ,@CreatedBy)";
            _context.Connection.Execute(sql, model);
            return true;
        }

        public IEnumerable<NotificationMessage> GetByNotificationBatchId(Guid notificationBatchGuid)
        {
            var sql = $@"Select * from NotificationMessage where NotificationBatchGuid =@notificationBatchGuid";
            return _context.Connection.Query<NotificationMessage>(sql, new { notificationBatchGuid = notificationBatchGuid });
        }

        public IEnumerable<NotificationMessage> GetByNotificationBatchIdAndNotificationType(Guid notificationBatchGuid, string notificationTypeName)
        {
            var notificationTypeNameUser = EnumGlobal.NotificationType.UserNotification.ToString();
            var notificationTypeNameEmail = EnumGlobal.NotificationType.EmailNotification.ToString();
            var sql = $@"Select a.* from NotificationMessage a join NotificationBatch b 
                            on a.NotificationBatchGuid = b.NotificationBatchGuid
                            join NotificationTemplate c 
                            on b.NotificationTemplateGuid  = c.NotificationTemplateGuid
                            join NotificationType d
                            on c.NotificationTypeGuid = d.NotificationTypeGuid
                            where (d.NotificationTypeName = @notificationTypeNameUser or d.NotificationTypeName = @notificationTypeNameEmail)
                            and b.NotificationBatchGuid   = @notificationBatchGuid";

            return _context.Connection.Query<NotificationMessage>(sql, new { notificationTypeNameUser = notificationTypeNameUser, notificationTypeNameEmail = notificationTypeNameEmail, notificationBatchGuid = notificationBatchGuid });
        }

        public IEnumerable<NotificationMessage> GetDesktopNotification(Guid userGuid, string searchValue)
        {
            string searchParam = "";
            string searchString = "";
            var notificationTypeNameUser = EnumGlobal.NotificationType.UserNotification.ToString();
            var notificationTypeNameEmail = EnumGlobal.NotificationType.EmailNotification.ToString();
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchString = "%" + searchValue + "%";
                searchParam = $@" AND  a.Subject   LIKE @searchValue or a.Message LIKE @searchValue";
            }
            var notificationTypeName = EnumGlobal.NotificationType.UserNotification.ToString();
            var sql = $@"Select a.* from NotificationMessage a join NotificationBatch b 
                            on a.NotificationBatchGuid = b.NotificationBatchGuid
                            join NotificationTemplate c 
                            on b.NotificationTemplateGuid  = c.NotificationTemplateGuid
                            join NotificationType d
                            on c.NotificationTypeGuid = d.NotificationTypeGuid
                            where (d.NotificationTypeName = @notificationTypeNameUser or d.NotificationTypeName = @notificationTypeNameEmail)
                            and a.UserGuid   = @userGuid
                            {searchParam}
                            order by a.CreatedOn desc";
            return _context.Connection.Query<NotificationMessage>(sql, new { notificationTypeNameUser = notificationTypeNameUser, notificationTypeNameEmail = notificationTypeNameEmail, searchValue = searchString, userGuid = userGuid });
        }

        public NotificationMessage GetByNotificationMessageId(Guid notificationMessageGuid)
        {
            var sql = $@"Select * from NotificationMessage where NotificationMessageGuid = @notificationMessageGuid";
            return _context.Connection.QueryFirstOrDefault<NotificationMessage>(sql, new { notificationMessageGuid = notificationMessageGuid });
        }

        public NotificationMessage GetLatestDesktopNotification(Guid userGuid)
        {
            var notificationTypeNameUser = EnumGlobal.NotificationType.UserNotification.ToString();
            var notificationTypeNameEmail = EnumGlobal.NotificationType.EmailNotification.ToString();
            var sql = $@"Select top 1 a.* from NotificationMessage a join NotificationBatch b 
                            on a.NotificationBatchGuid = b.NotificationBatchGuid
                            join NotificationTemplate c 
                            on b.NotificationTemplateGuid  = c.NotificationTemplateGuid
                            join NotificationType d
                            on c.NotificationTypeGuid = d.NotificationTypeGuid
                           where (d.NotificationTypeName = @notificationTypeNameUser or d.NotificationTypeName = @notificationTypeNameEmail)
                            and a.UserGuid   = @userGuid
                            order by a.CreatedOn desc";

            return _context.Connection.QueryFirstOrDefault<NotificationMessage>(sql, new { notificationTypeNameUser = notificationTypeNameUser, notificationTypeNameEmail = notificationTypeNameEmail, userGuid = userGuid });
        }

        public IEnumerable<NotificationMessage> GetUnReadGetDesktopNotification(Guid userGuid)
        {
            var notificationTypeNameUser = EnumGlobal.NotificationType.UserNotification.ToString();
            var notificationTypeNameEmail = EnumGlobal.NotificationType.EmailNotification.ToString();
            var sql = $@"Select a.* from NotificationMessage a join NotificationBatch b 
                            on a.NotificationBatchGuid = b.NotificationBatchGuid
                            join NotificationTemplate c 
                            on b.NotificationTemplateGuid  = c.NotificationTemplateGuid
                            join NotificationType d
                            on c.NotificationTypeGuid = d.NotificationTypeGuid
                            where (d.NotificationTypeName = @notificationTypeNameUser or d.NotificationTypeName = @notificationTypeNameEmail)
                            and a.UserResponse = 0
                            and a.UserGuid   = @userGuid
                            order by a.CreatedOn desc";
            return _context.Connection.Query<NotificationMessage>(sql, new { notificationTypeNameUser = notificationTypeNameUser, notificationTypeNameEmail = notificationTypeNameEmail, userGuid = userGuid });
        }

        public int EditUserResponseByNotificationMessageId(Guid notificationMessageGuid)
        {
            string updateQuery = @"Update NotificationMessage set 
                                          UserResponse  =  1
                                          where NotificationMessageGuid =@notificationMessageGuid ";
            return _context.Connection.Execute(updateQuery, new { notificationMessageGuid = notificationMessageGuid });
        }

        public bool GetByNotificationBatchIdAndUserId(Guid notificationBatchGuid, Guid userGuid)
        {
            var sql = $@"Select count(*) from NotificationMessage where NotificationBatchGuid =@notificationBatchGuid and UserGuid = UserGuid";
            var result = _context.Connection.QueryFirstOrDefault<int>(sql, 
                new { NotificationBatchGuid = notificationBatchGuid,
                    UserGuid= userGuid
                });
            return result > 0 ? true : false;
        }
    }
}
