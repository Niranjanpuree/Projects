using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BrakeConfigViewModel
    {
        public int Id { get; set; }
        public int FrontBrakeTypeId { get; set; }
        public string FrontBrakeTypeName { get; set; }
        public int RearBrakeTypeId { get; set; }
        public string RearBrakeTypeName { get; set; }
        public int BrakeSystemId { get; set; }
        public string BrakeSystemName { get; set; }
        public int BrakeABSId { get; set; }
        public string BrakeABSName { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingBrakeConfigViewModel: ReviewViewModel
    {
      
        public BrakeConfigViewModel EntityStaging { get; set; }
        public BrakeConfigViewModel EntityCurrent { get; set; }
        public IList<VehicleToBrakeConfigViewModel> ReplacementVehicleToBrakeConfigs { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}