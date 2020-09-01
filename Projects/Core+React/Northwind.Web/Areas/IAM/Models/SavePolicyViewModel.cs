using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Areas.IAM.Models
{
    public class SavePolicyViewModel
    {
        public string PolicyName { get; set; }
        public string PolicyDesc { get; set; }
        public List<PolicyRuleViewModel> PolicyRules {get; set;}
    }
}
