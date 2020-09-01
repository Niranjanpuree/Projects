using System;
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class SubModelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> LastUpdateDate { get; set; }
        public int VehicleCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }


    public class ChangeRequestStagingSubModelViewModel : ReviewViewModel
    {
        
        public SubModelViewModel EntityStaging { get; set; }
        public SubModelViewModel EntityCurrent { get; set; }
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}