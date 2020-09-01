
namespace AutoCare.Product.Web.Models.InputModels
{
    public class ModelInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int VehicleTypeId { get; set; }
        public int BaseVehicleCount { get; set; }
        public int VehicleCount { get; set; }
    }
}