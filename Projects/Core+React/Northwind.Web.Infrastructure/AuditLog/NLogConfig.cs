using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Web.Infrastructure.AuditLog
{
    public class NLogConfig
    {
        static public NLog.LogFactory AuditLogger = new LogFactory(new XmlLoggingConfiguration("NLog.config"));
        static public NLog.LogFactory EventLogger = new LogFactory(new XmlLoggingConfiguration("NLog-EventLog.config"));

    }
}
