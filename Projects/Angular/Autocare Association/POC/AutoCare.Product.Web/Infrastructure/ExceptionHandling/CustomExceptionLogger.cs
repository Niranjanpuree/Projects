using System.Diagnostics;
using System.Web.Http.ExceptionHandling;
using AutoCare.Product.Infrastructure.Logging;

namespace AutoCare.Product.Web.Infrastructure.ExceptionHandling
{
    public class CustomExceptionLogger : ExceptionLogger
    {
        private IApplicationLogger _applicationLogger;
        public CustomExceptionLogger(IApplicationLogger applicationLogger)
        {
            _applicationLogger = applicationLogger; 
        }

        public override void Log(ExceptionLoggerContext context)
        {           
            Trace.TraceError(context.ExceptionContext.Exception.ToString());

            _applicationLogger.WriteLog(context.Exception.Message,
                GetType(), LogType.Error,
                context.Exception);

        }
    }
}