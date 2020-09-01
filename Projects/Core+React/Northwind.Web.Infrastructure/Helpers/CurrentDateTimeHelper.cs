using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Helpers
{
    public static class CurrentDateTimeHelper
    {
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}
