using System;
using System.Collections.Generic;
using Dapper;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;

using System.Linq;
using System.Text;
using Northwind.Core.Specifications;
using Attribute = Northwind.Core.Entities.Attribute;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Northwind.Infrastructure.Data.Common
{
    public class CommonRepository : ICommonRepository
    {
        private readonly IDatabaseContext _context;
        public CommonRepository(IDatabaseContext context)
        {
            _context = context;
        }

        public string FilePost(string location, IFormFile fileToUpload)
        {
            if (!Directory.Exists(location))
            {
                DirectoryInfo di = Directory.CreateDirectory(location);
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), location, fileToUpload.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                fileToUpload.CopyToAsync(stream);
            }
            return path;
        }
        public bool CheckReviewStage(Guid moduleguid, Guid userGuid)
        {
            string query = $@"select 'HasValue' hasValue
                                                        from UserNotification usernotification left join 
                                                        UserNotificationMessage usernotificationmsg 
                                                        on usernotificationmsg.UserNotificationGuid = usernotification.NotificationGuid
                                                        where usernotification.ModuleGuid = @moduleguid
                                                        and usernotificationmsg.Status != 0
                                                        and usernotificationmsg.UserGuid =@userGuid";
            var result = _context.Connection.QueryFirstOrDefault<string>(query, new { moduleguid = moduleguid, userGuid = userGuid });
            return !string.IsNullOrEmpty(result) ? true : false;
        }
    }
}
