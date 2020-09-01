using Microsoft.AspNetCore.Http;
using NLog;
using Northwind.Core.AuditLog.Interfaces;
using Northwind.Core.AuditLog.Services;
using Northwind.Core.Interfaces;
using Northwind.Core.Utilities;
using Northwind.Infrastructure.AuditLog.Data;
using Northwind.Web.Infrastructure.Models;
using System;

namespace Northwind.Web.Infrastructure.AuditLog
{
    public static class AuditLogHandler
    {

        //call method by auditlog nlog config if method is set in config..
        public static void LogMethod(string level, string message, string data, string resourceName, string resourceId, string actor, string ipAddress, string action,
            string actionResult, string actionResultReason, string additonalInformation, string additionalInformationURL)
        {
            //todo call service..
            var auditLog = new Core.AuditLog.Entities.AuditLog()
            {
                RawData = data,
                TimeStamp = DateTime.Now,
                Actor = actor,
                IpAddress = ipAddress,
                Resource = resourceName,
                ResourceId = Guid.NewGuid(),
                Action = action,
                ActionResult = actionResult,
                AdditionalInformation = additonalInformation,
                AdditionalInformationURl = additionalInformationURL
            };
        }

        public static void InfoLog(Logger logger, string Actor, Guid ActorId, object Data, string Resource, Guid ResourceId, string IpAddress, string Action, Guid ActionId, string ActionResult, string ActionResultReason, string AdditionalInformation, string AdditionalInformationURl)
        {
            var auditLogModel = new Core.AuditLog.Entities.AuditLog()
            {
                TimeStamp = Helpers.CurrentDateTimeHelper.GetCurrentDateTime(),
                Actor = Actor,
                ActorId = ActorId,
                RawData = Helpers.FormatHelper.ObectToJson(Data),
                Resource = Resource,
                ResourceId = ResourceId,
                IpAddress = IpAddress,
                Action = Action,
                ActionId = ActionId,
                ActionResult = ActionResult,
                ActionResultReason = ActionResultReason,
                AdditionalInformation = AdditionalInformation,
                AdditionalInformationURl = AdditionalInformationURl
            };

            FillDataInGlobalDiagnosticsContext(auditLogModel);
            logger.Info(string.Empty);
        }

        public static void FillDataInGlobalDiagnosticsContext(Core.AuditLog.Entities.AuditLog auditLog)
        {
            GlobalDiagnosticsContext.Set("RawData", auditLog.RawData);
            GlobalDiagnosticsContext.Set("TimeStamp", auditLog.TimeStamp);
            GlobalDiagnosticsContext.Set("ResourceName", auditLog.Resource);
            GlobalDiagnosticsContext.Set("ResourceId", auditLog.ResourceId);
            GlobalDiagnosticsContext.Set("Actor", auditLog.Actor);
            GlobalDiagnosticsContext.Set("ActorId", auditLog.ActorId);
            GlobalDiagnosticsContext.Set("IPAddress", auditLog.IpAddress);
            GlobalDiagnosticsContext.Set("Action", auditLog.Action);
            GlobalDiagnosticsContext.Set("ActionId", auditLog.ActionId);
            GlobalDiagnosticsContext.Set("ActionResult", auditLog.ActionResult);
            GlobalDiagnosticsContext.Set("ActionResultReason", auditLog.ActionResultReason);
            GlobalDiagnosticsContext.Set("AdditionalInformation", auditLog.AdditionalInformation);
            GlobalDiagnosticsContext.Set("AdditionalInformationURl", auditLog.AdditionalInformationURl);
        }
    }

    public class EventLogHelper
    {
        public static void Debug(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Debug"), logger,eventLog);
        }

        public static void Info(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Info"), logger, eventLog);
        }
        
        public static void Error(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Error"), logger, eventLog);
        }

        public static void Trace(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Trace"), logger, eventLog);
        }

        public static void Fatal(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Fatal"), logger, eventLog);
        }

        public static void Warn(Logger logger, EventLog eventLog)
        {
            Log(LogLevel.FromString("Warn"), logger, eventLog);
        }

        public static void Log(LogLevel loglevel,Logger logger, EventLog eventLog)
        {
            logger.SetProperty("Action", eventLog.Action);
            logger.SetProperty("Application", eventLog.Application);
            logger.SetProperty("EventDate", eventLog.EventDate);
            logger.SetProperty("EventGuid", eventLog.EventGuid);
            logger.SetProperty("Message", eventLog.Message);
            logger.SetProperty("Resource", eventLog.Resource);
            logger.SetProperty("StackTrace", eventLog.StackTrace);
            logger.SetProperty("UserGuid", eventLog.UserGuid);
            logger.SetProperty("InnerException", eventLog.InnerException?.ToString());
            logger.Log(loglevel, eventLog.Message);
        }
    }
}
