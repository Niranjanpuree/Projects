namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleToBedConfigInputModel:ReferenceDataInputModel
    {
        public VehicleInputModel Vehicle { get; set; }
        public BedConfigInputModel BedConfig { get; set; }
        public int VehicleId { get; set; }
        public int BedConfigId { get; set; }
    }
}