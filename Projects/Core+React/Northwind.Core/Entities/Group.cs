using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class Group
    {
        public Guid GroupGuid { get; set; }
        public Guid ParentGuid { get; set; }
        public string GroupName { get; set; }
        public string CN { get; set; }
        public string Description { get; set; }
        public IEnumerable<User> Members { get; set; }
    }
}
