
namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleInputModel : ReferenceDataInputModel
    {
        public int BaseVehicleId { get; set; }
        public int SubModelId { get; set; }
        public int SourceId { get; set; }
        public int RegionId { get; set; }
        public int VehicleToBrakeConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
    }
}