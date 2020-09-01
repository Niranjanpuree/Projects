using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class EngineDesignationViewModel
    {
        public int EngineDesignationId { get; set; }
        public string EngineDesignationName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class EngineDesignationDetailViewModel : EngineDesignationViewModel
    {
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
    public class ChangeRequestStagingEngineDesignationViewModel : ReviewViewModel
    {
       
        public EngineDesignationDetailViewModel EntityStaging { get; set; }
        public EngineDesignationDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}