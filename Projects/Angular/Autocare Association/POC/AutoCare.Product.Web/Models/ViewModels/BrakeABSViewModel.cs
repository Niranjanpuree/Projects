using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BrakeABSViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BrakeABSDetailViewModel : BrakeABSViewModel
    {
        public int BrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }
    public class ChangeRequestStagingBrakeABSViewModel : ReviewViewModel
    {
        public BrakeABSDetailViewModel EntityStaging { get; set; }
        public BrakeABSDetailViewModel EntityCurrent { get; set; }
        //public IList<CommentsStagingViewModel> RequestorComments { get; set; }
        //public IList<CommentsStagingViewModel> ReviewerComments { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
    }
        
}