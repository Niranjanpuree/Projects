namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBodyStyleConfigInputModel:ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public BodyStyleConfigInputModel BodyStyleConfig { get; set; }
        public int VehicleId { get; set; }
        public int BodyStyleConfigId { get; set; }
    }
}