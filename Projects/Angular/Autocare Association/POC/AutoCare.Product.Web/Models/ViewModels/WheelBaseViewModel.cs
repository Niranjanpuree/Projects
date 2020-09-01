using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class WheelBaseViewModel
    {
        public int Id { get; set; }
        public string Base { get; set; }
        public string WheelBaseMetric { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class WheelBaseDetailViewModel : WheelBaseViewModel
    {
        public int vehicleToWheelBaseCount { get; set; }
    }
    public class ChangeRequestStagingWheelBaseViewModel : ReviewViewModel
    {
        
        public WheelBaseDetailViewModel EntityStaging { get; set; }
        public WheelBaseDetailViewModel EntityCurrent { get; set; }
        public IList<VehicleToWheelBaseViewModel> ReplacementVehicleToWheelBases { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}