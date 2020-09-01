using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Northwind.Infrastructure.Data.Notification
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        public IDatabaseContext _context;

        public NotificationTypeRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public NotificationType GetByNotificationTypeName(string notificationTypeName)
        {
            var sql = $@"Select * from NotificationType where NotificationTypeName =@notificationTypeName";
            return _context.Connection.QuerySingle<NotificationType>(sql, new { notificationTypeName = notificationTypeName });
        }
    }
}
