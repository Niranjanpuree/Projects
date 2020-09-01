using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class GroupUser
    {
        public Guid GroupUserGUID { get; set; }
        public Guid GroupGUID { get; set; }
        public Guid UserGUID { get; set; }
        public string GroupName { get; set; }
    }
}
