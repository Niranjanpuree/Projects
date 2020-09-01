using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class Resource
    {
        public Guid ResourceGuid { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Application { get; set; }
    }
}
