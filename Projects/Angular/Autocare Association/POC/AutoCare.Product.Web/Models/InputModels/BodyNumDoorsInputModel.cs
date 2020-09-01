
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BodyNumDoorsInputModel : ReferenceDataInputModel
    {
        public string NumDoors { get; set; }
        public int BodyStyleConfigCount { get; set; }
        public int VehicleToBodyStyleConfigCount { get; set; }
    }
}