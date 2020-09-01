
namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToWheelBaseInputModel : ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public WheelBaseInputModel WheelBase { get; set; }
        public int VehicleId { get; set; }
        public int WheelBaseId { get; set; }
    }
}