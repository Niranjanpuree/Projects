using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BrakeSystemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BrakeSystemDetailViewModel: BrakeSystemViewModel
    {
        public int BrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }
    public class ChangeRequestStagingBrakeSystemViewModel:ReviewViewModel
    {
        public BrakeSystemDetailViewModel EntityStaging { get; set; }
        public BrakeSystemDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}