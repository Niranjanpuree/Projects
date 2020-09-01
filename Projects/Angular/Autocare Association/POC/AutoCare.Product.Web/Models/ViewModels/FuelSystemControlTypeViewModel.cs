using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class FuelSystemControlTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastUpdateDate { get; set; }
        public bool ChangeRequestExists { get; set; }
        public int FuelDeliveryConfigCount { get; set; }

        public long? ChangeRequestId { get; set; }
    }

    public class ChangeRequestStagingFuelSystemControlTypeViewModel : ReviewViewModel
    {

        public FuelSystemControlTypeViewModel EntityStaging { get; set; }
        public FuelSystemControlTypeViewModel EntityCurrent { get; set; }

        public IList<CommentsStagingViewModel> Comments { get; set; }

    }
}