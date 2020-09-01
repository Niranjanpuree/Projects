using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.ViewModels
{
    public class VehicleToBodyStyleConfigSearchResultViewModel
    {
        public int BodyStyleConfigId { get; set; }
        public int VehicleId { get; set; }
        public List<BodyStyleConfigViewModel> BodyStyleConfigs { get; set; }
        public List<VehicleToBodyStyleConfigViewModel> VehicleToBodyStyleConfigs { get; set; }
    }
}