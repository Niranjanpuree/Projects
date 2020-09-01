using System;

namespace AutoCare.Product.Infrastructure.Logging
{
    public interface IApplicationLogger
    {
        /// <summary>
        /// Writes Log       
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="context">instance of the class</param>
        /// <param name="logType">
        /// Trace [Begin method X, end method X etc],
        /// Debug [Executed queries, user authenticated, session expired]
        /// Info [Normal behaviour like mail sent, user updated profile etc]
        /// Warn [Incorrect behaviour but the application can continue]
        /// Error[For example, application crashes / exceptions]
        /// Fatal [ Highest level, important stuff down]
        /// </param>
        /// <param name="exception">exception</param>
        void WriteLog(string message, Type context, LogType logType = LogType.Info, Exception exception = null);
    }
}
