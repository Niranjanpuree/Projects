
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToWheelBaseSearchResultViewModel
    {
        public int WheelBaseId { get; set; }
        public int VehicleId { get; set; }
        public List<WheelBaseViewModel> WheelBases { get; set; }
        public List<VehicleToWheelBaseViewModel> VehicleToWheelBases { get; set; }
    }
}