using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class YearViewModel
    {
        public int Id { get; set; }
        public int BaseVehicleCount { get; set; }
        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingYearViewModel : ReviewViewModel
    {
       
        public YearViewModel EntityStaging { get; set; }
        public YearViewModel EntityCurrent { get; set; }
       
        public IList<CommentsStagingViewModel> Comments { get; set; }
        
    }
}