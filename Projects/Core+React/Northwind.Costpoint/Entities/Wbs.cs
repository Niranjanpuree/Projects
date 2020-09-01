using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class WbsCP
    {
        public Guid WbsGuid { get; set; }
        public string ProjectNumber { get; set; }
        public string Wbs { get; set; }
        public string Description { get; set; }
        public bool AllowCharging { get; set; }
    }
}
