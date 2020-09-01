using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels
{
    public class AuthorizeActionRequest
    {
        public string ResourceType { get; set; }
        public string ResourceAction { get; set; }
        public string ExtraParam { get; set; }
    }

    public class AuthorizeActionResponse
    {
        public string ResourceType { get; set; }
        public string ResourceAction { get; set; }
        public bool IsAuthorized { get; set; }
        public string ExtraParam { get; set; }
    }
}
