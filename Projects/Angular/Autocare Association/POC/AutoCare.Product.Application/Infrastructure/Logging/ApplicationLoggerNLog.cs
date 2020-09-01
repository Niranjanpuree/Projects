using NLog;
using System;

namespace AutoCare.Product.Application.Infrastructure.Logging
{
    public class ApplicationLoggerNLog : IApplicationLogger
    {
        public void WriteLog(string message, Type context, EnumWarehouse.LogType logType = EnumWarehouse.LogType.Info, Exception exception = null)
        {
            if (String.IsNullOrWhiteSpace(message))
            {
                throw new Exception("Message is required");
            }
            ILogger logger;
            if (context == null)
            {
                logger = LogManager.GetCurrentClassLogger();
            }
            else
            {
                logger = LogManager.GetLogger(context.Name);
            }
            LogLevel logLevel = LogLevel.Info;

            switch (logType)
            {
                case EnumWarehouse.LogType.None:
                case EnumWarehouse.LogType.Trace:
                    logLevel = LogLevel.Trace;
                    break;
                case EnumWarehouse.LogType.Debug:
                    logLevel = LogLevel.Debug;
                    break;
                case EnumWarehouse.LogType.Info:
                    logLevel = LogLevel.Info;
                    break;
                case EnumWarehouse.LogType.Warn:
                    logLevel = LogLevel.Warn;
                    break;
                case EnumWarehouse.LogType.Error:
                    logLevel = LogLevel.Error;
                    break;
                case EnumWarehouse.LogType.Fatal:
                    logLevel = LogLevel.Fatal;
                    break;
                default:
                    break;
            }

            LogEventInfo logEvent = new LogEventInfo(logLevel, context.Name, message);
            logEvent.Exception = exception;

            logger.Log(typeof(ApplicationLoggerNLog), logEvent);

        }
    }
}
