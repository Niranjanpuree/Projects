using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class GroupPermission
    {
        public Guid GroupPermissionGuid { get; set; }
        public Guid GroupGuid { get; set; }
        public Guid ResourceGuid { get; set; }
        public Guid ResourceActionGuid { get; set; }
        [Ignore]
        public string Group { get; set; }
        [Ignore]
        public string Resource { get; set; }
        [Ignore]
        public string ResourceAction { get; set; }
    }
}
