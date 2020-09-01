using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class MakeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }

        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingMakeViewModel : ReviewViewModel
    {
        
        public MakeViewModel EntityStaging { get; set; }
        public MakeViewModel EntityCurrent { get; set; }
       
        public IList<CommentsStagingViewModel> Comments { get; set; }
       
    }
}