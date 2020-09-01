using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Models
{
    public class AuthorizeActionRequest
    {
        public string ResourceType { get; set; }
        public string ResourceAction { get; set; }
    }
}
