
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleSearchResultViewModel
    {
        public List<BaseVehicleViewModel> BaseVehicles { get; set; }
        public List<VehicleViewModel> Vehicles { get; set; }
    }
}