
namespace AutoCare.Product.Web.Models.InputModels
{
    public class BedLengthInputModel : ReferenceDataInputModel
    {
        public string Length { get; set; }
        public string BedLengthMetric { get; set; }
        public int VehicleToBedConfigCount { get; set; }

    }
}