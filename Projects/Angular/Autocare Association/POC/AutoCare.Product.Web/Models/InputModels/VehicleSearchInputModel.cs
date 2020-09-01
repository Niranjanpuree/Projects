namespace AutoCare.Product.Web.Models.InputModels
{
    public class VehicleSearchInputModel
    {
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string[] Regions { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
    }
}