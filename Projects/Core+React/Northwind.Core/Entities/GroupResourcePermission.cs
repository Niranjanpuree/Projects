using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class GroupResourcePermission
    {
        public Guid ResourceGuid { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Application { get; set; }
        [Ignore]
        public IEnumerable<GroupResourceActionPermission> Actions { get; set; }
    }

    public class GroupResourcePermissionApplication
    {
        public string Application { get; set; }
        public IEnumerable<GroupResourcePermission> Resources { get; set; }
    }

    public class GroupResourceActionPermission
    {
        public Guid ActionGuid { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }

    public class Groups
    {
        public IEnumerable<GroupResourcePermissionApplication> GroupResourcePermission { get; set; }
        public IEnumerable<UserGroup> UserGroup { get; set; }
    }

    public class UserGroup
    {
       public string GroupName { get; set; }
       public Guid GroupGuid { get; set; }
       public Guid UserGuid { get; set; }

    }
}
