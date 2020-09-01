using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.Web.Models.ViewModels.RevenueRecognition
{
    public class RevenuePerformanceObligationViewModel : BaseViewModel
    {
        public Guid RevenueGuid { get; set; }
        public Guid PerformanceObligationGuid { get; set; }

        public string RevenueStreamIdentifier { get; set; }
        public string RevenueStreamIdentifierStatus { get { return string.IsNullOrEmpty(RevenueStreamIdentifier) ? "N/A" : RevenueStreamIdentifier; } }

        public bool RightToPayment { get; set; }
        public string RightToPaymentStatus { get { return RightToPayment == true ? "Yes" : "No"; } }

        public bool RoutineService { get; set; }
        public string RoutineServiceStatus { get { return RoutineService == true ? "Yes" : "No"; } }

        public string RevenueOverTimePointInTime { get; set; }
        public string RevenueOverTimePointInTimeStatus { get { return string.IsNullOrEmpty(RevenueOverTimePointInTime) ? "N/A" : RevenueOverTimePointInTime; } }

        public string SatisfiedOverTime { get; set; }
        public string SatisfiedOverTimeStatus { get { return string.IsNullOrEmpty(SatisfiedOverTime) ? "N/A" : SatisfiedOverTime; } }

    }
}
