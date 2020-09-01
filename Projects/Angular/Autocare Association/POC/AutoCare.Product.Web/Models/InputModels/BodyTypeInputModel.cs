
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BodyTypeInputModel : ReferenceDataInputModel
    {
        public string Name { get; set; }
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
    }
}