namespace AutoCare.Product.Web.Models.InputModels
{
    public class FuelTypeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }

    }
}