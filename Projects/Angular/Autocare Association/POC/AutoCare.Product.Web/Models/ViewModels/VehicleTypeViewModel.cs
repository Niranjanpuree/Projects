using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleTypeGroupId { get; set; }
        public string VehicleTypeGroupName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
        public int ModelCount { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
    }

    public class ChangeRequestStagingVehicleTypeViewModel : ReviewViewModel
    {
       
        public VehicleTypeViewModel EntityStaging { get; set; }
        public VehicleTypeViewModel EntityCurrent { get; set; }
       
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }


}