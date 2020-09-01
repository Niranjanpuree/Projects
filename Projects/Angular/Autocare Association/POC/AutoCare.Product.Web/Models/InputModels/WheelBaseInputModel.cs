
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class WheelBaseInputModel : ReferenceDataInputModel
    {
        public string Base { get; set; }
        public string WheelBaseMetric { get; set; }
        public List<VehicleToWheelBaseInputModel> VehicleToWheelBases { get; set; }
        public int VehicleToWheelBaseCount { get; set; }
    }
}