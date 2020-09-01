using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class ModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }
        public string VehicleTypeName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
        public int BaseVehicleId { get; set; }  //NOTE: Required in base vehicle replace screen
    }

    public class ModelDetailViewModel : ModelViewModel
    {
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
    }

    public class ChangeRequestStagingModelViewModel : ReviewViewModel
    {
        
        public ModelDetailViewModel EntityStaging { get; set; }
        public ModelDetailViewModel EntityCurrent { get; set; }
        
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}