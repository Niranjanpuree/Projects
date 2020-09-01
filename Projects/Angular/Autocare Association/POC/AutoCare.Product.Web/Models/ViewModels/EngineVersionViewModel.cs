using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class EngineVersionViewModel
    {
        public int EngineVersionId { get; set; }
        public string EngineVersionName { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class EngineVersionDetailViewModel : EngineVersionViewModel
    {
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
    public class ChangeRequestStagingEngineVersionViewModel : ReviewViewModel
    {
       
        public EngineVersionDetailViewModel EntityStaging { get; set; }
        public EngineVersionDetailViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}