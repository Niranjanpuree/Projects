using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class BodyStyleConfigViewModel
    {
        public int Id { get; set; }
        public int BodyNumDoorsId { get; set; }
        public string NumDoors { get; set; }
        public int BodyTypeId { get; set; }
        public string BodyTypeName { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }
    public class ChangeRequestStagingBodyStyleConfigViewModel : ReviewViewModel
    {
       
        public BodyStyleConfigViewModel EntityStaging { get; set; }
        public BodyStyleConfigViewModel EntityCurrent { get; set; }
        public IList<VehicleToBodyStyleConfigViewModel> ReplacementVehicleToBodyStyleConfigs { get; set; }
         public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}