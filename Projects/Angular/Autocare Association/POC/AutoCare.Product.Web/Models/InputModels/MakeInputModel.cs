namespace AutoCare.Product.Web.Models.InputModels
{
    public class MakeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int VehicleCount { get; set; }
        public int BaseVehicleCount { get; set; }

    }
}