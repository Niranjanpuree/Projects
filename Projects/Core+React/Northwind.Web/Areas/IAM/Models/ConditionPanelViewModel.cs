using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.Core.Entities;

namespace Northwind.Web.Areas.IAM.Models
{
    public class ConditionPanelViewModel
    {
        public List<ResourceAttribute> ResourceAttributes { get; set; }
        public List<OperatorName> Operators { get; set; }
        
        public ConditionPanelViewModel()
        {
            ResourceAttributes = new List<ResourceAttribute>();
            Operators = new List<OperatorName>();
        }

    }
}
