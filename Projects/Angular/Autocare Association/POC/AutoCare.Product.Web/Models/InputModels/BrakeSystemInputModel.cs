
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BrakeSystemInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int BrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }
}