using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToDriveTypeViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int DriveTypeId { get; set; }
        public string Name { get; set; }
        public DriveTypeViewModel DriveType { get; set; }
        public VehicleViewModel Vehicle { get; set; }
    }
    public class ChangeRequestStagingVehicleToDriveTypeViewModel:ReviewViewModel
    {
       
        public VehicleToDriveTypeViewModel EntityStaging { get; set; }
        public VehicleToDriveTypeViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel>Comments { get; set; }
      
       
    }
}