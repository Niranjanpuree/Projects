using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class EngineVinViewModel
    {
        public int EngineVinId { get; set; }
        public string EngineVinName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class EngineVinDetailViewModel : EngineVinViewModel
    {
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
    public class ChangeRequestStagingEngineVinViewModel : ReviewViewModel
    {
       
        public EngineVinDetailViewModel EntityStaging { get; set; }
        public EngineVinDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}