using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class GroupPolicy
    {
        public Guid GroupPolicyGuid { get; set; }
        public Guid GroupGuid { get; set; }
        public Guid PolicyGuid { get; set; }
    }
}
