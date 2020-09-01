using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class UserInterfaceMenu
    {
        public Guid MenuGuid { get; set; }
        public string MenuNamespace { get; set; }
        public string MenuText { get; set; }
        public string MenuUrl { get; set; }
        public string MenuDescription { get; set; }
        public string ParentMenuNamespace { get; set; }
        public string MenuClass { get; set; }
    }
}
