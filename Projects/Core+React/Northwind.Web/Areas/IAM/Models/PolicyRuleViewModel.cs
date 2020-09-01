using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Entities;

namespace Northwind.Web.Areas.IAM.Models
{
    public class PolicyRuleViewModel
    {
        public List<Resource> Resources { get; set; }
        public List<ResourceAction> Actions { get; set; }
        public string SelectedEffect { get; set; }

    }
}
