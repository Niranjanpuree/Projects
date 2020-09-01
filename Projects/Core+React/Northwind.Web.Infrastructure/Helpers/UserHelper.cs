using Microsoft.AspNetCore.Http;
using Northwind.Web.Infrastructure.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Helpers
{
    public class UserHelper
    {
        public static Guid CurrentUserGuid(HttpContext context)
        {
            try
            {
                Guid userGuid = new Guid(context.User.Identity.Name);
                //var userGuid = new Guid("73526DCB-3D7E-48C0-B4A0-4BE20AE3F85C");
                return userGuid;
            }
            catch
            {
                return Guid.Empty;
            }
        }
        public static Guid GetNewGuid()
        {
            Guid userGuid = Guid.NewGuid();
            return userGuid;
        }

        public static bool IsAuthorizedRepresentative(HttpContext context, Guid userGuid)
        {
            Guid loggeduserGuid = new Guid(context.User.Identity.Name);
            if (loggeduserGuid == userGuid)
                return true;
            else return false;
        }
        public static UserViewModel GetLoggedUser(HttpContext context)
        {
            var user = new UserViewModel();
            var claim = context.User.Claims.Where(x => x.Type == context.User.Identities.FirstOrDefault().RoleClaimType).FirstOrDefault().Value.ToString();
            user.Role = claim;
            user.UserGuid = new Guid(context.User.Identity.Name);
            return user;
        }
        public static string GetHostedIp(HttpContext context)
        {
            try
            {
                var ip = context.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                return ip;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
