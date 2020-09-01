using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToWheelBaseViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int WheelBaseId { get; set; }
        public string Source { get; set; }
        public string WheelBaseName { get; set; }
        public virtual VehicleViewModel Vehicle { get; set; }
        public virtual WheelBaseViewModel WheelBase { get; set; }
    }

    public class ChangeRequestStagingVehicleToWheelBaseViewModel : ReviewViewModel
    {
       
        public VehicleToWheelBaseViewModel EntityStaging { get; set; }
        public VehicleToWheelBaseViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        

    }
}