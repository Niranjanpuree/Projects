using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBrakeConfigViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int BrakeConfigId { get; set; }
        public int FrontBrakeTypeId { get; set; }
        public string FrontBrakeTypeName { get; set; }
        public int RearBrakeTypeId { get; set; }
        public string RearBrakeTypeName { get; set; }
        public int BrakeSystemId { get; set; }
        public string BrakeSystemName { get; set; }
        public int BrakeABSId { get; set; }
        public string BrakeABSName { get; set; }
        public BrakeConfigViewModel BrakeConfig { get; set; }
        public VehicleViewModel Vehicle { get; set; }
    }
    public class ChangeRequestStagingVehicleToBrakeConfigViewModel : ReviewViewModel
    {
        public VehicleToBrakeConfigViewModel EntityStaging { get; set; }
        public VehicleToBrakeConfigViewModel EntityCurrent { get; set; }
        
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}