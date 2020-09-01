using System;

using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using Northwind.Core.Interfaces;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Models;

namespace Northwind.Web.Infrastructure
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly INotificationService _notificationService;
        private readonly Logger _eventLogger;
        public HomeController(IConfiguration config, INotificationService notificationService)
        {
            _config = config;
            _notificationService = notificationService;
            _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
        }
        public IActionResult Index()
        {
            var loggedUser = new Guid(HttpContext.User.Identity.Name);
            return RedirectToAction("Index", "Contract");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            string path = exceptionDetails.Path.Substring(1, exceptionDetails.Path.Length - 1);
            string action = "";
            try
            {
                string controller = path.Substring(0, path.IndexOf("/"));
                int length = path.Length;
                int remlen = path.Length - controller.Length;
                string newstring = path.Substring(controller.Length + 1, remlen - 1);
                if (newstring.Contains("/"))
                {
                    action = newstring.Substring(0, newstring.IndexOf("/"));
                }
                else if (newstring.Contains("?"))
                {
                    action = newstring.Substring(0, newstring.IndexOf("?") + 1);
                }
                else
                {
                    action = newstring;
                }
                EventLogHelper.Error(_eventLogger, new Infrastructure.Models.EventLog
                {

                    EventGuid = Guid.NewGuid(),
                    Action = action,
                    Application = "ESS",
                    EventDate = DateTime.UtcNow,
                    Message = exceptionDetails.Error.Message,
                    Resource = controller,
                    StackTrace = exceptionDetails.Error.StackTrace,
                    UserGuid = UserHelper.CurrentUserGuid(HttpContext)
                });
                return View(new { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            }
            catch
            {
                EventLogHelper.Error(_eventLogger, new Models.EventLog
                {
                    EventGuid = Guid.NewGuid(),
                    Action = "Index",
                    Application = "ESS",
                    EventDate = DateTime.UtcNow,
                    Message = exceptionDetails.Error.Message,
                    Resource = path,
                    StackTrace = exceptionDetails.Error.StackTrace,
                    UserGuid = UserHelper.CurrentUserGuid(HttpContext)
                });
                return View();
            }
        }
    }
}
