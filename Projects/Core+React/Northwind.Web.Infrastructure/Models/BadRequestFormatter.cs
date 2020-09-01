using Microsoft.AspNetCore.Mvc;
using NLog;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Helpers;
using System;


namespace Northwind.Web.Infrastructure.Models
{
    public static class BadRequestFormatter
    {
        public static readonly Logger _eventLogger = NLogConfig.EventLogger.GetCurrentClassLogger();
        public static BadRequestObjectResult BadRequest(this Controller controller,Exception error)
        {
            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            string action = controller.ControllerContext.RouteData.Values["action"].ToString();

            EventLogHelper.Error(_eventLogger, new EventLog
            {
                EventGuid = Guid.NewGuid(),
                Action = action,
                Application = "ESS",
                EventDate = DateTime.UtcNow,
                Message = error.Message,
                Resource = controllerName,
                StackTrace = error.StackTrace,
                InnerException = error.InnerException,
                UserGuid = UserHelper.CurrentUserGuid(controller.HttpContext)
            });

            return controller.BadRequest(error);
        }

        public static BadRequestObjectResult BadRequest(this Controller controller, string errorMessage)
        {

            string controllerName = controller.ControllerContext.RouteData.Values["controller"].ToString();
            string action = controller.ControllerContext.RouteData.Values["action"].ToString();

            EventLogHelper.Error(_eventLogger, new EventLog
            {

                EventGuid = Guid.NewGuid(),
                Action = action,
                Application = "ESS",
                EventDate = DateTime.UtcNow,
                Message = errorMessage,
                Resource = controllerName,
                StackTrace = null,
                UserGuid = UserHelper.CurrentUserGuid(controller.HttpContext)
            });
            return controller.BadRequest(errorMessage);
        }

    }
}
