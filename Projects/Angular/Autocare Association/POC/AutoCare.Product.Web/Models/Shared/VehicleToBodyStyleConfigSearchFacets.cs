namespace AutoCare.Product.Web.Models.Shared
{
    public class VehicleToBodyStyleConfigSearchFacets
    {
        // vehicle specific
        public string[] Makes { get; set; }
        public string[] Models { get; set; }
        public string[] SubModels { get; set; }
        public string[] Regions { get; set; }
        public string[] Years { get; set; }
        public string[] VehicleTypes { get; set; }
        public string[] VehicleTypeGroups { get; set; }
        // body specific
        public string[] BodyNumDoors { get; set; }
        public string[] BodyTypes { get; set; }
    }
}