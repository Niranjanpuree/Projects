using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class RecentActivity
    {
        public Guid RecentActivityGuid { get; set; }
        public string Entity { get; set; }
        public Guid EntityGuid { get; set; }
        public Guid UserGuid { get; set; }
        public string UserAction { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

    }
}
