using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class ResourceAction
    {
        public Guid ResourceActionGuid { get; set; }
        public Guid ResourceGuid { get; set; }
        public Guid ActionGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ActionType { get; set; }
        
    }
}
