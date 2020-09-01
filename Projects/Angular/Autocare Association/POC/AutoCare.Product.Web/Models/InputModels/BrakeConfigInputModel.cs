
using System.Collections.Generic;

namespace AutoCare.Product.Web.Models.InputModels
{
    public class BrakeConfigInputModel : ReferenceDataInputModel
    {
        public int FrontBrakeTypeId { get; set; }
        public int RearBrakeTypeId { get; set; }
        public int BrakeSystemId { get; set; }
        public int BrakeABSId { get; set; }
        public List<VehicleToBrakeConfigInputModel> VehicleToBrakeConfigs { get; set; }     
        public int VehicleToBrakeConfigCount { get; set; }
    }
}