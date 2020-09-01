using System;

namespace Northwind.Core.Entities
{
    public class Organization
    {
        public Guid OrgIDGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
