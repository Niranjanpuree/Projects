using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class DistributionList : BaseModel
    {
        public Guid DistributionListGuid { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}
