using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Policy
{
    public class PolicyViewModel
    {
        public Guid PolicyGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PolicyJson { get; set; }
    }
}
