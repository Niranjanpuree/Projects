using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class DriveTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        public int VehicleToDriveTypeCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }
    public class ChangeRequestStagingDriveTypeViewModel :ReviewViewModel
    {
        public DriveTypeViewModel EntityStaging { get; set; }
        public DriveTypeViewModel EntityCurrent { get; set; }
        public IList<VehicleToDriveTypeViewModel> ReplacementVehicleToDriveTypes { get; set; }
      
        public IList<CommentsStagingViewModel> Comments { get; set; }
           }
}