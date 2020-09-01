namespace AutoCare.Product.Web.Models.InputModels
{
    public class EngineDesignationInputModel : ReferenceDataInputModel
    {
        public string EngineDesignationName { get; set; }
        public int EngineConfigCount { get; set; }
        public int VehicleToEngineConfigCount { get; set; }
    }
}