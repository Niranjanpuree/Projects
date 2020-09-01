
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BrakeTypeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int FrontBrakeConfigCount { get; set; }
        public int RearBrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }
}