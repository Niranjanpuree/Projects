using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Infrastructure.Models.ViewModels.Login
{
    public class WindowsAuthRequest
    {
        public string Identity { get; set; }
        public string Secret { get; set; }
    }
}
