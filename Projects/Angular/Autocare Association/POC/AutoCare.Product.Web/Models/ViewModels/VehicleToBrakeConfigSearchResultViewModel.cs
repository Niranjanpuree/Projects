
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBrakeConfigSearchResultViewModel
    {
        public int BrakeConfigId { get; set; }
        public int VehicleId { get; set; }
        public List<BrakeConfigViewModel> BrakeConfigs { get; set; }
        public List<VehicleToBrakeConfigViewModel> VehicleToBrakeConfigs { get; set; }
    }
}