using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BedTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BedTypeDetailViewModel : BedTypeViewModel
    {
        public int BedConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
    }
    public class ChangeRequestStagingBedTypeViewModel : ReviewViewModel
    {
       
        public BedTypeDetailViewModel EntityStaging { get; set; }
        public BedTypeDetailViewModel EntityCurrent { get; set; }
        //public IList<CommentsStagingViewModel> RequestorComments { get; set; }
        //public IList<CommentsStagingViewModel> ReviewerComments { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}