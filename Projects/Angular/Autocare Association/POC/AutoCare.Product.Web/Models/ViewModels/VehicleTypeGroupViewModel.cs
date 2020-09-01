using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleTypeGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
        public int VehicleTypeCount { get; set; }
    }

    public class ChangeRequestStagingVehicleTypeGroupViewModel : ReviewViewModel
    {
        
        public VehicleTypeGroupViewModel EntityStaging { get; set; }
        public VehicleTypeGroupViewModel EntityCurrent { get; set; }
        
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }

}