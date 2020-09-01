
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBedConfigSearchResultViewModel
    {
        public int BedConfigId { get; set; }
        public int VehicleId { get; set; }
        public List<BedConfigViewModel> BedConfigs { get; set; }
        public List<VehicleToBedConfigViewModel> VehicleToBedConfigs { get; set; }
    }
}