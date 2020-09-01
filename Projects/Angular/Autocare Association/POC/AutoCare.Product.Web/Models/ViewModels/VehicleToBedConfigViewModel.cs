using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBedConfigViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int BedConfigId { get; set; }
        public int BedLengthId { get; set; }
        public string Length { get; set; }
        public string BedLengthMetric { get; set; }
        public int BedTypeId { get; set; }
        public string BedTypeName { get; set; }
        public BedConfigViewModel BedConfig { get; set; }
        public VehicleViewModel Vehicle { get; set; }
    }
    public class ChangeRequestStagingVehicleToBedConfigViewModel:ReviewViewModel
    {
        public VehicleToBedConfigViewModel EntityStaging { get; set; }
        public VehicleToBedConfigViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel>Comments { get; set; }
       
       
    }
}