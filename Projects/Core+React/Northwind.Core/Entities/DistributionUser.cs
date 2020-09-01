using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class DistributionUser : BaseModel
    {
        public Guid DistributionUserGuid { get; set; }
        public Guid DistributionListGuid { get; set; }
        public Guid UserGuid { get; set; }
    }
}
