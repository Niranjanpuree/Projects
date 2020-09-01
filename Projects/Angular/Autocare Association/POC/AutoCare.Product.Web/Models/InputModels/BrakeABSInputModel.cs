
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BrakeABSInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public int BrakeConfigCount { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
    }
}