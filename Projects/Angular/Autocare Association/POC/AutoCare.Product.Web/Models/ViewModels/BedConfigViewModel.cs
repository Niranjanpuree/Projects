using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BedConfigViewModel
    {
        public int Id { get; set; }
        public int BedLengthId { get; set; }
        public string Length { get; set; }
        public string BedLengthMetric { get; set; }
        public int BedTypeId { get; set; }
        public string BedTypeName { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }
    public class ChangeRequestStagingBedConfigViewModel :ReviewViewModel
    {
       
        public BedConfigViewModel EntityStaging { get; set; }
        public BedConfigViewModel EntityCurrent { get; set; }
        public IList<VehicleToBedConfigViewModel> ReplacementVehicleToBedConfigs { get; set; }
        //public IList<CommentsStagingViewModel> RequestorComments { get; set; }
        //public IList<CommentsStagingViewModel> ReviewerComments { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}