using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.Group
{
    public class GroupPolicyViewModel
    {
        public Guid GroupPolicyGuid { get; set; }
        public Guid GroupGuid { get; set; }
        public Guid PolicyGuid { get; set; }
    }
}
