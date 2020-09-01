
namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleTypeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int VehicleTypeGroupId { get; set; }
        public int ModelCount { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
    }
}