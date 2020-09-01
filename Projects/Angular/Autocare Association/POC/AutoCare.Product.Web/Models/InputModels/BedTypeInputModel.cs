namespace AutoCare.Product.Web.Models.InputModels
{
    public class BedTypeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int BedConfigCount { get; set; }
        public int VehicleToBedConfigCount { get; set; }
    }
}