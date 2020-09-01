using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BedLengthViewModel
    {
        public int Id { get; set; }
        public string Length { get; set; }
        public string BedLengthMetric { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class BedLengthDetailViewModel : BedLengthViewModel
    {
        public int BedConfigCount { get; set; }
        public int vehicleToBedConfigCount { get; set; }
    }
    public class ChangeRequestStagingBedLengthViewModel : ReviewViewModel
    {
        public BedLengthDetailViewModel EntityStaging { get; set; }
        public BedLengthDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
      
        }
}