using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Helpers
{
    public class RouteUrlHelper
    {
        public static string GetAbsoluteAction(IUrlHelper url,string controller,string action,object param)
        {
            return url.Action(action,controller,param,url.ActionContext.HttpContext.Request.Scheme);
        }
    }
}
