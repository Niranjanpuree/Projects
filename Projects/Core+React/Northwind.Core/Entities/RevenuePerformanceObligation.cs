using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class RevenuePerformanceObligation:BaseModel
    {
        public Guid RevenueGuid { get; set; }
        public Guid PerformanceObligationGuid { get; set; }
        public string RevenueStreamIdentifier { get; set; }
        public bool RightToPayment { get; set; }
        public bool RoutineService { get; set; }
        public string RevenueOverTimePointInTime { get; set; }
        public string SatisfiedOverTime { get; set; }
    }
}
