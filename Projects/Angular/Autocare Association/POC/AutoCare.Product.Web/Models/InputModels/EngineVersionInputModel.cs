namespace AutoCare.Product.Web.Models.InputModels
{
    public class EngineVersionInputModel : ReferenceDataInputModel
    {
        public string EngineVersionName { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
}