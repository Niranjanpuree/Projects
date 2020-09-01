
namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBrakeConfigInputModel:ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public BrakeConfigInputModel BrakeConfig { get; set; }
        public int VehicleId { get; set; }
        public int BrakeConfigId { get; set; }
    }
}
