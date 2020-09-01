namespace AutoCare.Product.Web.Models.InputModels
{
    public class EngineVinInputModel : ReferenceDataInputModel
    {
        public string EngineVinName { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
}