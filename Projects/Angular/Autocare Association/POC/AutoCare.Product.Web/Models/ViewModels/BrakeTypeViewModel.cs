using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BrakeTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BrakeTypeDetailViewModel : BrakeTypeViewModel
    {
        public int FrontBrakeConfigCount { get; set; }
        public int RearBrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }

    public class ChangeRequestStagingBrakeTypeViewModel:ReviewViewModel
    {
        
        public BrakeTypeDetailViewModel EntityStaging { get; set; }
        public BrakeTypeDetailViewModel EntityCurrent { get; set; }
       
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}