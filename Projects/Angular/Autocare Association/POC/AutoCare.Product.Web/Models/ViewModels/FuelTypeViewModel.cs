using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class FuelTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingFuelTypeViewModel : ReviewViewModel
    {

        public FuelTypeViewModel EntityStaging { get; set; }
        public FuelTypeViewModel EntityCurrent { get; set; }

        public IList<CommentsStagingViewModel> Comments { get; set; }

    }
}