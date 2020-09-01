using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class FuelSystemDesignViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingFuelSystemDesignViewModel : ReviewViewModel
    {

        public FuelSystemDesignViewModel EntityStaging { get; set; }
        public FuelSystemDesignViewModel EntityCurrent { get; set; }

        public IList<CommentsStagingViewModel> Comments { get; set; }

    }
}