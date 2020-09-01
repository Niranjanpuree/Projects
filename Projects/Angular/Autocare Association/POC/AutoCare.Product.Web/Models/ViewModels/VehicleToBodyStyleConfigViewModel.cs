using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBodyStyleConfigViewModel
    {
        public int Id { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleId { get; set; }
        public int BodyStyleConfigId { get; set; }
        public int BodyNumberDoorsId { get; set; }
        public string BodyNumDoors { get; set; }
        public int BodyTypeId { get; set; }
        public string BodyTypeName { get; set; }
        public BodyStyleConfigViewModel BodyStyleConfig { get; set; }
        public VehicleViewModel Vehicle { get; set; }
    }

    public class ChangeRequestStagingVehicleToBodyStyleConfigViewModel: ReviewViewModel
    {
        public VehicleToBodyStyleConfigViewModel EntityStaging { get; set; }
        public VehicleToBodyStyleConfigViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
             
    }
}