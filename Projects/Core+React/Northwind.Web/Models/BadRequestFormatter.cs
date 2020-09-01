using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models
{
    public static class BadRequestFormatter
    {
        public static BadRequestObjectResult BadRequest(this Controller controller,object error)
        {
            return controller.BadRequest(error);
        }
    }
}
