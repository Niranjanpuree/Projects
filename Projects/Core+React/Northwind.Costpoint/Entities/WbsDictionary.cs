using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.CostPoint.Entities
{
    public class WbsDictionaryCP
    {
        public Guid WbsDictionaryGuid { get; set; }
        public string ProjectNumber { get; set; }
        public Guid WbsGuid { get; set; }
        public string WbsDictionaryTitle { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
