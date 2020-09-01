using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class AdUserGroupAndManager
    {
        public User User { get; set; }
        public string Manager { get; set; }
        public string MemberOf { get; set; }
    }
}
