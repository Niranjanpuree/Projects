using System;
using NLog;

namespace AutoCare.Product.Infrastructure.Logging
{
    public class ApplicationLoggerNLog : IApplicationLogger
    {
        public void WriteLog(string message, Type context, LogType logType = LogType.Info, Exception exception = null)
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
                case LogType.None:
                case LogType.Trace:
                    logLevel = LogLevel.Trace;
                    break;
                case LogType.Debug:
                    logLevel = LogLevel.Debug;
                    break;
                case LogType.Info:
                    logLevel = LogLevel.Info;
                    break;
                case LogType.Warn:
                    logLevel = LogLevel.Warn;
                    break;
                case LogType.Error:
                    logLevel = LogLevel.Error;
                    break;
                case LogType.Fatal:
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
